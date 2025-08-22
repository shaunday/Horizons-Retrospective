import { Request, Response, NextFunction } from "express";
import { baseLogger } from "../services/logger.js";

// 404 handler
export function notFoundHandler(
  req: Request,
  res: Response,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  _next: NextFunction
) {
  res.status(404).json({ error: "Not Found" });
}

// Central error handler
export function errorHandler(
  err: unknown,
  req: Request,
  res: Response,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  _next: NextFunction
) {
  const error = err as { status?: number; message?: string };
  baseLogger.error(error);
  res
    .status(error.status ?? 500)
    .json({ error: error.message ?? "Internal Server Error" });
}
