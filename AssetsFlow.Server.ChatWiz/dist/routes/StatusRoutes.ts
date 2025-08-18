import express, { Router } from 'express';
import { StatusController } from './StatusController';

const router: Router = express.Router();

router.get('/health', (req, res) => StatusController.health(req, res));
router.get('/version', (req, res) => StatusController.version(req, res));

export default router;
