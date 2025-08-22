import express from 'express';
import cors from 'cors';

import { httpLogger, baseLogger } from './services/logger.js';
import tradeWizardRoutes from './routes/tradeWizardRoutes.js';
import statusRoutes from './routes/statusRoutes.js';
import { notFoundHandler, errorHandler } from './middleware/errorHandler.js';
import { config } from './config.js';

const app = express();
app.use(express.json());

// CORS
baseLogger.info(`CORS policy set for frontend URL: ${config.corsOrigin}`);
app.use(cors({ origin: config.corsOrigin }));

// Logging
app.use(httpLogger);

// Routes
app.use(`/${config.apiVersion}/status`, statusRoutes);
app.use(`/${config.apiVersion}/trade-wizard`, tradeWizardRoutes);

// Error handling
app.use(notFoundHandler);
app.use(errorHandler);

// Start
app.listen(config.port, () => {
  baseLogger.info(`Server running on port ${config.port}`);
});
