#!/bin/bash

# Update the system
sudo apt update

# Install Docker if it's not already installed
sudo apt install -y docker.io

# Verify Docker installation
docker --version

# Verify Docker Compose installation
docker-compose --version

# Install Certbot and Nginx plugin
sudo apt install -y certbot python3-certbot-nginx

# Ensure your certbot script is executable
chmod +x ./get-cert.sh

# Run shared deployment steps
./common-deploy-steps.sh

# Define the cron job with Certbot logic inline
cron_job='0 0 * * * /usr/bin/certbot renew --quiet --deploy-hook "systemctl reload nginx"'

# Add it to root’s crontab if it isn’t already there
sudo crontab -l 2>/dev/null | grep -F "$cron_job" >/dev/null
if [ $? -ne 0 ]; then
    (sudo crontab -l 2>/dev/null; echo "$cron_job") | sudo crontab -
fi

echo "Deployment complete: inline Certbot renew cron job scheduled."