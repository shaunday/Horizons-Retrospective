import express from "express";
import cors from "cors";

import { httpLogger, baseLogger } from "./services/logger.js";
import tradeWizardRoutes from "./routes/tradeWizardRoutes.js";
import statusRoutes from "./routes/statusRoutes.js";
import { notFoundHandler, errorHandler } from "./middleware/errorHandler.js";
import { config } from "./config.js";
import { loadTemplates } from "./templates/templateLoader.js";

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

// Startup
(async () => {
  try {
    baseLogger.info("Loading trade-element templates...");
    await loadTemplates();
    baseLogger.info("Templates loaded successfully.");

    app.listen(config.port, () => {
      baseLogger.info(`Server running on port ${config.port}`);
    });
  } catch (err) {
    // Throw instead of process.exit()
    throw new Error(
      `Failed to start server: ${err instanceof Error ? err.message : err}`
    );
  }
})();
