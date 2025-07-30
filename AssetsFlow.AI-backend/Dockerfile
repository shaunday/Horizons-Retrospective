# Use an official Node.js runtime as a parent image
FROM node:20.6.1

# Set the working directory to /app
WORKDIR /app

# Copy the current directory contents into the container at /app
COPY ./src .

# Install any needed packages specified in package.json
RUN npm ci

# Define environment variable
ENV NODE_ENV=production

# Run index.js when the container launches
CMD [ "node", "server.mjs" ]