#!/bin/bash

# Wait for the certificate to be issued (or check if it exists)
while [ ! -f /etc/letsencrypt/live/api.mywebthings.xyz/fullchain.pem ]; do
  echo "Waiting for SSL certificate..."
  sleep 5
done

# Once the certificate exists, add HTTPS config
echo "Certificate found, applying HTTPS configuration..."

# Start Nginx in foreground mode (Docker best practice)
nginx -g 'daemon off;'
