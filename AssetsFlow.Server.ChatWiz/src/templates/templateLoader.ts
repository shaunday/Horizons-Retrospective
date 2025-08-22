// tradeElementTemplateLoader.ts
import fs from "fs/promises";
import path from "path";

const templates: Record<string, unknown> = {};

async function loadTemplates() {
  const basePath = process.cwd();
  const templatePath = path.join(
    basePath,
    "common",
    "trade-elements-templates"
  );

  const files = await fs.readdir(templatePath);
  for (const file of files) {
    if (file.endsWith("-template.json")) {
      const raw = await fs.readFile(path.join(templatePath, file), "utf-8");
      const parsed = JSON.parse(raw);
      templates[parsed.elementType] = parsed;
    }
  }
}

await loadTemplates();

export function getTemplate(type: string) {
  const t = templates[type];
  if (!t) throw new Error(`Template not found: ${type}`);
  return t as { elementType: string; elements: unknown[] }; // type assertion on return
}
