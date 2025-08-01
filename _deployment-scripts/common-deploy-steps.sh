#!/bin/bash

# Call Certbot script to get/renew certs
echo "Running certbot script to obtain/renew certificates..."
./get-cert.sh
if [ $? -ne 0 ]; then
  echo "Certbot failed. Aborting deployment."
  exit 1
fi

# Pull the latest image (optional if you rebuilt locally)
echo "Pulling the latest images..."
sudo docker-compose pull

# Recreate and start containers with the updated image
echo "Recreating and starting containers..."
sudo docker-compose up -d

# Show logs specifically for nginx
echo -e "\n--- NGINX logs ---"
sudo docker-compose logs --tail=50 nginx

# Optionally show logs for the web container too
echo -e "\n--- Web container logs ---"
sudo docker-compose logs --tail=50 web

# Optional: Display container status
sudo docker-compose ps

# Clean up unused Docker images
echo "Cleaning up unused Docker images..."
sudo docker image prune -f

echo "Deployment completed!"
echo "To monitor live logs, run: sudo docker-compose logs -f"
