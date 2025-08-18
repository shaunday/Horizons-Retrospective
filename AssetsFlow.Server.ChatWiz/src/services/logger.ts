import pino, { Logger } from 'pino';
import pinoHttp, { HttpLogger } from 'pino-http';

const isProd = process.env.NODE_ENV === 'production';

const baseLogger: Logger = pino(
  isProd
    ? {}
    : {
        transport: {
          target: 'pino-pretty',
          options: {
            colorize: true,
            translateTime: 'SYS:standard',
            singleLine: true,
            ignore: 'pid,hostname',
          },
        },
      }
);

// Express/Fastify middleware
const httpLogger: HttpLogger = pinoHttp({ logger: baseLogger });

export { baseLogger, httpLogger };
