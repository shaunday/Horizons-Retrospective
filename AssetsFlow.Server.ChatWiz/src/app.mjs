const API_VERSION = 'trade-wiz/api/v1';

import express from 'express';
import { httpLogger, baseLogger } from '#services/logger.js';
import tradeWizardRoutes from '#routes/tradeWizardRoutes.mjs';
import statusRoutes from '#routes/statusRoutes.mjs';
import cors from 'cors';

const app = express();
app.use(express.json());

// Determine CORS origin based on environment
const isDev = process.env.NODE_ENV === 'development';
let corsOrigin = isDev ? 'http://localhost:5173' : 'https://hsr.mywebthings.xyz';

if (corsOrigin) {
  baseLogger.info(`CORS policy set for frontend URL: ${corsOrigin}`);
}

// Configure CORS middleware
app.use(cors({ origin: corsOrigin }));

app.use(httpLogger);

app.use(`${API_VERSION}/status`, statusRoutes);
app.use(`${API_VERSION}/trade-wizard`, tradeWizardRoutes);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  baseLogger.info(`Server running on port ${PORT}`);
});
