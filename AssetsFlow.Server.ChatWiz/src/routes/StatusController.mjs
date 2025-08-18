import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

// Resolve __dirname in ESM
const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// Read package.json manually
const pkg = JSON.parse(fs.readFileSync(path.join(__dirname, '../../package.json'), 'utf-8'));

export class StatusController {
  static health(req, res) {
    res.status(200).json({ status: 'ok' });
  }

  static version(req, res) {
    res.status(200).json({
      name: pkg.name,
      version: pkg.version,
    });
  }
}
