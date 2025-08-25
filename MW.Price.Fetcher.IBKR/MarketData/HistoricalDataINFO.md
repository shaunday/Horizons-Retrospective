
REST endpoint /iserver/marketdata/history?conid={id}&period=1d&bar=5min.

Step Size
A step size is the permitted minimum and maximum bar size for any given period.


| Period | Step Size (min – max) | Default Bar |
|--------|----------------------|-------------|
| 1min   | 1min                 | 1min        |
| 1h     | 1min – 8h            | 1min        |
| 1d     | 1min – 8h            | 1min        |
| 1w     | 10min – 1w           | 15min       |
| 1m     | 1h – 1m              | 30min       |
| 3m     | 2h – 1m              | 1d          |
| 6m     | 4h – 1m              | 1d          |
| 1y     | 8h – 1m              | 1d          |
| 2y     | 1d – 1m              | 1d          |
| 3y     | 1d – 1m              | 1w          |
| 15y    | 1w – 1m              | 1w          |
