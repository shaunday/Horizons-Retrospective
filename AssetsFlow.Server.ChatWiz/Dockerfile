# Use an official Node.js runtime as a parent image
FROM node:20.6.1

# Set the working directory to /app
WORKDIR /app

# Copy package.json and package-lock.json files
COPY package.json package-lock.json ./

# Install any needed packages specified in package.json
RUN npm ci

# Copy the rest of the application source code
COPY ./src ./src

# Define environment variable
ENV NODE_ENV=production

# Run app.mjs when the container launches
CMD ["node", "src/app.mjs"]