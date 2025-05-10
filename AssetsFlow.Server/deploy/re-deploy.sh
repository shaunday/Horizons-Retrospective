#!/bin/bash

# Stop and remove the running containers
echo "Stopping and removing existing containers..."
sudo docker-compose down

# Check if any container is using port 80 and stop it
echo "Stopping any container using port 80..."
sudo lsof -t -i:80 | xargs sudo kill -9

# Pull the latest image (optional if you rebuilt locally)
echo "Pulling the latest images..."
sudo docker-compose pull

# Recreate and start containers with the updated image
echo "Recreating and starting containers..."
sudo docker-compose up -d

# Clean up unused Docker images
echo "Cleaning up unused Docker images..."
sudo docker image prune -f

echo "Deployment completed successfully!"
