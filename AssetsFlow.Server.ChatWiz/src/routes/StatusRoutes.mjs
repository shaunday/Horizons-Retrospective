import express from 'express';
import { StatusController } from './StatusController.mjs';

const router = express.Router();

router.get('/health', StatusController.health);
router.get('/version', StatusController.version);

export default router;
