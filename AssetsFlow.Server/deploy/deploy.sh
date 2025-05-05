#!/bin/bash

# Update the system
sudo apt update

# Install Docker if it's not already installed
sudo apt install -y docker.io

# Verify Docker installation
docker --version

sudo curl -L "https://github.com/docker/compose/releases/download/latest/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Verify Docker Compose installation
docker-compose --version

# Pull the required images using docker-compose (without installing it)
docker-compose pull

# Start the containers defined in docker-compose.yml
docker-compose up -d

# (Optional) Check the status of the containers
docker-compose ps