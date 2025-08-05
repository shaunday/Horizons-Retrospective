$projectDir = Get-Location
$propsFile = Join-Path $projectDir "Directory.Packages.props"

Write-Host "Scanning for .csproj files under $projectDir ..." -ForegroundColor Cyan
$csprojFiles = Get-ChildItem -Path $projectDir -Recurse -Filter *.csproj

if ($csprojFiles.Count -eq 0) {
    Write-Error "No .csproj files found!"
    exit 1
}

$packageVersions = @{}
$conflicts = @{}

# Step 1: Read existing Directory.Packages.props packages (to avoid duplicates)
$existingPackages = @{}
if (Test-Path $propsFile) {
    try {
        [xml]$existingPropsXml = Get-Content $propsFile
        $existingRefs = $existingPropsXml.Project.ItemGroup.PackageVersion
        foreach ($pkg in $existingRefs) {
            $existingPackages[$pkg.Include] = $pkg.Version
        }
        Write-Host "Found $($existingPackages.Count) existing packages in Directory.Packages.props" -ForegroundColor Green
    } catch {
        Write-Warning "Failed to parse existing Directory.Packages.props - ignoring."
    }
}

# Step 2: Collect packages from .csproj files, remove Version attribute/element from them
foreach ($file in $csprojFiles) {
    $xml = [xml](Get-Content $file.FullName)
    $changed = $false

    # Get all PackageReference nodes (handle attribute and child element Version)
    $packageRefs = $xml.SelectNodes("//PackageReference")

    foreach ($pkgRef in $packageRefs) {
        $include = $pkgRef.Include
        if (-not $include) {
            continue
        }

        # Get version from attribute or child node
        $version = $null
        if ($pkgRef.Version) {
            $version = $pkgRef.Version
        } elseif ($pkgRef.HasAttribute("Version")) {
            $version = $pkgRef.GetAttribute("Version")
        }

        if ($version) {
            # Check for conflicts
            if ($packageVersions.ContainsKey($include)) {
                if ($packageVersions[$include] -ne $version) {
                    if (-not $conflicts.ContainsKey($include)) {
                        $conflicts[$include] = @($packageVersions[$include])
                    }
                    if (-not ($conflicts[$include] -contains $version)) {
                        $conflicts[$include] += $version
                    }
                }
            } else {
                $packageVersions[$include] = $version
            }

            # Remove Version attribute if present
            if ($pkgRef.HasAttribute("Version")) {
                $pkgRef.RemoveAttribute("Version")
                $changed = $true
            }

            # Remove Version child node if present
            $versionNode = $pkgRef.SelectSingleNode("Version")
            if ($versionNode) {
                $pkgRef.RemoveChild($versionNode) | Out-Null
                $changed = $true
            }
        }
    }

    if ($changed) {
        # Save the updated .csproj without versions in PackageReference
        $xml.Save($file.FullName)
        Write-Host "Updated (removed versions): $($file.FullName)" -ForegroundColor Yellow
    }
}

# Step 3: Report conflicts if any
if ($conflicts.Count -gt 0) {
    Write-Host "`n⚠️ Package version conflicts detected:" -ForegroundColor Red
    foreach ($key in $conflicts.Keys) {
        $versions = $conflicts[$key] + $packageVersions[$key]
        $uniqVersions = $versions | Sort-Object -Unique
        Write-Host "  $key : $($uniqVersions -join ', ')"
    }
    Write-Host "`nPlease resolve these version conflicts manually." -ForegroundColor Red
    exit 1
}

# Step 4: Combine new packages with existing props packages (avoid duplicates)
foreach ($key in $existingPackages.Keys) {
    if (-not $packageVersions.ContainsKey($key)) {
        $packageVersions[$key] = $existingPackages[$key]
    }
}

# Step 5: Generate Directory.Packages.props content
$propsContent = @()
$propsContent += '<?xml version="1.0" encoding="utf-8"?>'
$propsContent += '<Project>'
$propsContent += '  <ItemGroup>'

foreach ($pkg in $packageVersions.Keys | Sort-Object) {
    $escapedName = [System.Security.SecurityElement]::Escape($pkg)
    $escapedVersion = [System.Security.SecurityElement]::Escape($packageVersions[$pkg])
    $propsContent += "    <PackageVersion Include=`"$escapedName`" Version=`"$escapedVersion`" />"
}

$propsContent += '  </ItemGroup>'
$propsContent += '</Project>'

Set-Content -Path $propsFile -Value $propsContent -Encoding UTF8
Write-Host "`n✅ Generated Directory.Packages.props with $($packageVersions.Count) packages at:`n $propsFile" -ForegroundColor Green
