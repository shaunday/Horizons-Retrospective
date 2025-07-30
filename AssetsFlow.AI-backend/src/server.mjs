import express from 'express';
import { httpLogger, baseLogger } from './logger.mjs';
import { StatusController } from './StatusController.mjs';

const app = express();
app.use(express.json());

app.use(httpLogger);

// Health and version endpoints
app.get('/routes/health', StatusController.health);
app.get('/routes/version', StatusController.version);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  baseLogger.info(`Server running on port ${PORT}`);
});
