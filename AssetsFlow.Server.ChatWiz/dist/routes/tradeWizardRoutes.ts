import express from 'express';
import { getCreationFlows, handleCreationStep } from './tradeWizardController';
const router = express.Router();
// GET /creation-flows
router.get('/creation-flows', getCreationFlows);
// POST /creation-flow-step
router.post('/creation-flow-step', handleCreationStep);
export default router;
