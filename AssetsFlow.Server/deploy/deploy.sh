#!/bin/bash

# Update the system
sudo apt update

# Install Docker if it's not already installed
sudo apt install -y docker.io

# Install Certbot and Nginx plugin
sudo apt install -y certbot python3-certbot-nginx

# Verify Docker installation
docker --version

# Download the Docker Compose binary for Linux x86_64
curl -LO https://github.com/docker/compose/releases/download/v2.36.0/docker-compose-linux-x86_64

# Make it executable
chmod +x docker-compose-linux-x86_64

# Move it to /usr/local/bin for system-wide access
sudo mv docker-compose-linux-x86_64 /usr/local/bin/docker-compose

# Verify installation
docker-compose --version

# Optional: Navigate to the directory with docker-compose.yml
# cd /path/to/your/project

# Pull the required images
sudo docker-compose pull

# Start the containers
sudo docker-compose up -d

# Check container status
sudo docker-compose ps

# Request SSL certificate for your domain using Certbot
sudo certbot --nginx -d api.mywebthings.xyz

# Renew the certificate periodically (add this cron job to renew)
# You can add a cron job to renew SSL certificates automatically.
# To test renewal, you can run:
# sudo certbot renew --dry-run