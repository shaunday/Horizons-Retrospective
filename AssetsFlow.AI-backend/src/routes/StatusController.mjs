// StatusController.mjs
import pkg from '../package.json' assert { type: 'json' };

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