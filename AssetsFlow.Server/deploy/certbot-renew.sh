#!/bin/bash

cron_job='0 0 * * * /usr/bin/certbot renew --quiet --deploy-hook "systemctl reload nginx"'

# Add it to root’s crontab if it isn’t already there
sudo crontab -l 2>/dev/null | grep -F "$cron_job" >/dev/null
if [ $? -ne 0 ]; then
    (sudo crontab -l 2>/dev/null; echo "$cron_job") | sudo crontab -
fi