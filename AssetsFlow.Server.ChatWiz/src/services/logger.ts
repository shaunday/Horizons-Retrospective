import pino, { Logger } from "pino";
import pinoHttp, { HttpLogger } from "pino-http";
import fs from "fs";
import { join } from "path";
import { spawn } from "child_process";

const isProd = process.env.NODE_ENV === "production";

// File path for logs
const logFile = join(process.cwd(), "logs", "app.log");
// Ensure logs directory exists
fs.mkdirSync(join(process.cwd(), "logs"), { recursive: true });

let baseLogger: Logger;

if (isProd) {
  // In production: simple JSON file logging
  baseLogger = pino(pino.destination(logFile));
} else {
  // In development: pretty console + file logging
  // Create a child process that runs pino-pretty and writes to file
  const prettyStream = spawn("pino-pretty", [
    "--colorize",
    "--translateTime",
    "SYS:standard",
    "--singleLine",
    "--ignore",
    "pid,hostname",
  ]);

  // Pipe pretty output to console
  prettyStream.stdout.pipe(process.stdout);

  // Pipe same output to a file
  const fileStream = fs.createWriteStream(logFile, { flags: "a" });
  prettyStream.stdout.pipe(fileStream);

  // Base logger uses the prettyStream as its destination
  baseLogger = pino({}, prettyStream.stdin);
}

// Express/Fastify middleware
const httpLogger: HttpLogger = pinoHttp({ logger: baseLogger });

export { baseLogger, httpLogger };
