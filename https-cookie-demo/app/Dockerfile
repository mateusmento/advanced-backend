FROM node:18.12.1-alpine AS build

WORKDIR /usr/app
COPY --chown=node:node package*.json ./
RUN npm ci
COPY --chown=node:node . ./
RUN npm run build
USER node

FROM nginx:1.23.3-alpine AS dist

WORKDIR /usr/share/nginx/html
COPY --from=build /usr/app/dist/ ./
