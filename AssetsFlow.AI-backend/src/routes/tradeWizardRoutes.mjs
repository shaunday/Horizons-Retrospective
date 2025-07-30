import express from 'express';
import { getCreationFlows, handleCreationStep } from './tradeWizardController.mjs';

const router = express.Router();

router.get('/creation-flows', getCreationFlows);
router.post('/creation-flow-step', handleCreationStep);

export default router;
