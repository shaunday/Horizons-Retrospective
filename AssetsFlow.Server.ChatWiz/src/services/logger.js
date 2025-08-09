import pino from 'pino';
import pinoHttp from 'pino-http';

const isProd = process.env.NODE_ENV === 'production';

const baseLogger = pino(
  isProd
    ? {}
    : {
        transport: {
          target: 'pino-pretty',
          options: {
            colorize: true,
            translateTime: 'SYS:standard',
            singleLine: true,
            ignore: 'pid,hostname'
          }
        }
      }
);

// Express/Fastify middleware
const httpLogger = pinoHttp({ logger: baseLogger });

export { baseLogger, httpLogger };
