# Enable script debugging and verbose output
$DebugPreference = "Continue"
$VerbosePreference = "Continue"

# Set the path for output file (in current folder)
$propsFile = "Directory.Packages.props"

# Dictionary to hold package versions (key = package id, value = version string)
$packageVersions = @{}

# Get all csproj files recursively in the current directory
$csprojFiles = Get-ChildItem -Path . -Recurse -Filter *.csproj -ErrorAction SilentlyContinue

Write-Host "Found $($csprojFiles.Count) .csproj files." -ForegroundColor Cyan

foreach ($csproj in $csprojFiles) {
    Write-Verbose "Inspecting $($csproj.FullName)"
    try {
        # Load XML content
        [xml]$xml = Get-Content -Path $csproj.FullName -Raw

        if (-not $xml.Project) {
            Write-Warning "No <Project> node in $($csproj.Name), skipping."
            continue
        }

        # Find all PackageReference nodes under ItemGroup
        $itemGroups = $xml.Project.ItemGroup
        if (-not $itemGroups) {
            Write-Warning "No <ItemGroup> nodes in $($csproj.Name), skipping."
            continue
        }

        foreach ($itemGroup in $itemGroups) {
            $packageRefs = $itemGroup.PackageReference
            if (-not $packageRefs) { continue }

            foreach ($ref in $packageRefs) {
                $pkg = $ref.Include
                # Try to get Version attribute or Version child element text
                $ver = $ref.Version
                if (-not $ver) {
                    # Check for nested <Version> element
                    $verNode = $ref.SelectSingleNode("Version")
                    if ($verNode) {
                        $ver = $verNode.InnerText
                    }
                }

                if (-not $pkg) {
                    Write-Warning "⚠️ Skipped PackageReference with no Include in $($csproj.Name)"
                    continue
                }

                if (-not $ver) {
                    Write-Warning "⚠️ Incomplete PackageReference in $($csproj.Name) (Include='$pkg', Version missing)"
                    continue
                }

                # Compare and keep the highest version
                if ($packageVersions.ContainsKey($pkg)) {
                    try {
                        # Use [version] for proper comparison, fallback on string comparison if invalid version format
                        $currentVer = [version]$packageVersions[$pkg]
                        $newVer = [version]$ver
                        if ($newVer -gt $currentVer) {
                            Write-Verbose "Updating $pkg version from $($packageVersions[$pkg]) to $ver"
                            $packageVersions[$pkg] = $ver
                        }
                    } catch {
                        Write-Verbose "Could not compare versions for package ${pkg}: '${ver}' and '${packageVersions[$pkg]}'. Keeping existing version."
                    }
                } else {
                    $packageVersions[$pkg] = $ver
                    Write-Verbose "Added package $pkg version $ver"
                }
            }
        }
    }
    catch {
        Write-Warning "⚠️ Failed to parse $($csproj.FullName): $_"
    }
}

if ($packageVersions.Count -eq 0) {
    Write-Warning "No PackageReference entries found in any .csproj files."
    exit 1
}

Write-Host "Total unique packages found: $($packageVersions.Count)" -ForegroundColor Green

# Build Directory.Packages.props content
$propsLines = @()
$propsLines += '<?xml version="1.0" encoding="utf-8"?>'
$propsLines += '<Project>'
$propsLines += '  <ItemGroup>'

foreach ($pkg in $packageVersions.Keys | Sort-Object) {
    $ver = $packageVersions[$pkg]
    # Escape attributes properly
    $escapedPkg = [System.Security.SecurityElement]::Escape($pkg)
    $escapedVer = [System.Security.SecurityElement]::Escape($ver)
    $propsLines += "    <PackageVersion Include=""$escapedPkg"" Version=""$escapedVer"" />"
}

$propsLines += '  </ItemGroup>'
$propsLines += '</Project>'

# Write to the props file with UTF8 encoding
Set-Content -Path $propsFile -Value $propsLines -Encoding UTF8

Write-Host "✅ Generated $propsFile with $($packageVersions.Count) packages." -ForegroundColor Green
