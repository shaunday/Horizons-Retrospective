#!/bin/bash

# Update the system
sudo apt update

# Install Docker if it's not already installed
sudo apt install -y docker.io

# Verify Docker installation
docker --version

# Download the Docker Compose binary for Linux x86_64
curl -LO https://github.com/docker/compose/releases/download/v2.36.0/docker-compose-linux-x86_64

# Make it executable
chmod +x docker-compose-linux-x86_64

# Move it to /usr/local/bin for system-wide access
sudo mv docker-compose-linux-x86_64 /usr/local/bin/docker-compose

# Verify Docker Compose installation
docker-compose --version

# Install Certbot and Nginx plugin
sudo apt install -y certbot python3-certbot-nginx

# Pull the required images
sudo docker-compose pull

# Start the containers (assumes Nginx is one of them)
sudo docker-compose up -d

# Wait for Nginx to fully start before Certbot runs
echo "Waiting for Nginx to start..."
sleep 10

# Request SSL certificate (replace with your actual domain)
sudo certbot --nginx -d api.mywebthings.xyz

# Optional: Display container status
sudo docker-compose ps
