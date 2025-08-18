import express from 'express';
import { StatusController } from './statusController';

const router = express.Router();

router.get('/health', StatusController.health);
router.get('/version', StatusController.version);

export default router;
