# Use the official Node.js 18 image
FROM node:18-alpine AS build
WORKDIR /app

# Accept build arguments
ARG API_URL
ARG NEXT_PUBLIC_API_URL

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install dependencies
RUN npm ci

# Copy the rest of the source code
COPY . .

# Create .env.production file with the environment variables
RUN echo "NEXT_PUBLIC_API_URL=$NEXT_PUBLIC_API_URL" > .env.production \
    && echo "API_URL=$API_URL" >> .env.production

# Build the Next.js application
RUN npm run build

# Production image
FROM node:18-alpine AS production
WORKDIR /app

# Copy package.json and package-lock.json from the build stage
COPY --from=build /app/package.json ./
COPY --from=build /app/package-lock.json ./

# Install production dependencies
RUN npm ci --only=production

# Copy the built application
COPY --from=build /app/.next ./.next

# Copy necessary files
COPY --from=build /app/next.config.mjs ./

# Expose port 3000
EXPOSE 3000

# Start the application
CMD ["npm", "run", "start"]