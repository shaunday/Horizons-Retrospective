"use strict";
// import fs from 'fs/promises';
// import path from 'path';
// import { execSync } from 'child_process';
// function getElementTemplatePath() {
//   const isDev = process.env.NODE_ENV !== 'production';
//   const basePath = isDev
//     ? execSync('git rev-parse --show-toplevel', { encoding: 'utf-8' }).trim()
//     : path.resolve(new URL('.', import.meta.url).pathname);
//   return path.join(basePath, 'common', 'trade-elements-template');
// }
// const TEMPLATE_FILES = {
//   tradeOrigin: 'tradeOrigin-template.json',
// };
// export async function loadElementTemplates() {
//   const templatesDir = getElementTemplatePath();
//   const templates = {};
//   const errors = {};
//   for (const [key, fileName] of Object.entries(TEMPLATE_FILES)) {
//     const filePath = path.join(templatesDir, fileName);
//     try {
//       const raw = await fs.readFile(filePath, 'utf-8');
//       templates[key] = JSON.parse(raw);
//     } catch (err) {
//       errors[key] = `Failed to load: ${err.message}`;
//     }
//   }
//   return { templates, errors };
// }
