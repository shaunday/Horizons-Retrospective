#!/bin/bash

# Configurable values
DOMAIN="api.mywebthings.xyz"
EMAIL="shaun.d@live.com"
WEBROOT="./certbot-webroot"

# Run Certbot to obtain certificate
echo "Attempting to obtain SSL certificate for $DOMAIN"
sudo certbot certonly \
  --webroot -w "$WEBROOT" \
  -d "$DOMAIN" \
  --email "$EMAIL" \
  --agree-tos \
  --no-eff-email

# Check if Certbot was successful
if [ $? -ne 0 ]; then
  echo "Error: Certbot failed to obtain certificate."
  exit 1
fi

echo "SSL certificate successfully obtained."
