// tradeElementTemplateLoader.ts
import fs from "fs/promises";
import path from "path";

export interface TradeElementTemplate {
  elementType: string;
  elements: unknown[];
}

const templates: Record<string, TradeElementTemplate> = {};

export async function loadTemplates(): Promise<void> {
  const basePath = process.cwd();
  const templatePath = path.join(
    basePath,
    "common",
    "trade-elements-templates"
  );

  let files: string[];
  try {
    files = await fs.readdir(templatePath);
  } catch (err) {
    throw new Error(
      `Failed to read template directory: ${templatePath}. ${err}`
    );
  }

  const jsonFiles = files.filter((f) => f.endsWith("-template.json"));

  await Promise.all(
    jsonFiles.map(async (file) => {
      const raw = await fs.readFile(path.join(templatePath, file), "utf-8");
      const parsed = JSON.parse(raw) as TradeElementTemplate;
      if (!parsed.elementType) {
        throw new Error(`Template missing 'elementType': ${file}`);
      }
      templates[parsed.elementType] = parsed;
    })
  );
}

export function getTemplate(type: string): TradeElementTemplate {
  const t = templates[type];
  if (!t) throw new Error(`Template not found: ${type}`);
  return t;
}
