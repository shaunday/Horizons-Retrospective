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
