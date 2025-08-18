# Use official Node.js image
FROM node:20.6.1

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install only production dependencies
RUN npm ci --only=production

# Copy compiled TypeScript output
COPY ./dist ./dist

# Set environment
ENV NODE_ENV=production

# Run compiled app
CMD ["node", "dist/app.js"]
