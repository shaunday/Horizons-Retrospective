// Express example
const express = require('express');
const pino = require('pino-http')();
const app = express();

app.use(pino);
app.get('/', (req, res) => {
  req.log.info('Request received');
  res.send('hello');
});
