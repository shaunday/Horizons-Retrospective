#!/bin/bash

# Stop and remove the running containers
echo "Stopping and removing existing containers..."
sudo docker-compose down --volumes --remove-orphans

# Check if any container is using port 80 and stop it
echo "Stopping any container using port 80..."
sudo lsof -t -i:80 | xargs -r sudo kill -9

# Run shared deployment steps
./common-deploy-steps.sh
