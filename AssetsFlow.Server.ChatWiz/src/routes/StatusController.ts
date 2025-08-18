import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';
import { Request, Response } from 'express';

// Resolve __dirname in ESM
const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// Read package.json manually
const pkg = JSON.parse(fs.readFileSync(path.join(__dirname, '../../package.json'), 'utf-8')) as {
  name: string;
  version: string;
};

export class StatusController {
  static health(req: Request, res: Response): void {
    res.status(200).json({ status: 'ok' });
  }

  static version(req: Request, res: Response): void {
    res.status(200).json({
      name: pkg.name,
      version: pkg.version,
    });
  }
}
