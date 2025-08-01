const API_VERSION = 'trade-wiz/api/v1';

import express from 'express';
import { httpLogger, baseLogger } from './logger.mjs';
import tradeWizardRoutes from './routes/tradeWizardRoutes.mjs';
import statusRoutes from './routes/StatusRoutes.mjs';

const app = express();
app.use(express.json());

app.use(httpLogger);

app.use(`${API_VERSION}/status`, statusRoutes);
app.use(`${API_VERSION}/trade-wizard`, tradeWizardRoutes);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  baseLogger.info(`Server running on port ${PORT}`);
});
