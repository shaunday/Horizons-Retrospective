export interface AppConfig {
  port: number;
  nodeEnv: 'development' | 'production' | 'test';
  corsOrigin: string;
  apiVersion: string;
}

const nodeEnv = (process.env.NODE_ENV as 'development' | 'production' | 'test') || 'development';

export const config: AppConfig = {
  port: Number(process.env.PORT) || 3000,
  nodeEnv,
  corsOrigin: nodeEnv === 'development'
    ? 'http://localhost:5173'
    : 'https://hsr.mywebthings.xyz',
  apiVersion: 'trade-wiz/api/v1',
};
