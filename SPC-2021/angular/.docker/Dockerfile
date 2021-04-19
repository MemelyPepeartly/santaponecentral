## arguments
ARG NGINX_VERSION=1.17
ARG NODE_VERSION=15.14.0

## stage - base
FROM node:${NODE_VERSION} as base
WORKDIR /angular
COPY package*.json ./
RUN npm ci

## stage - build
FROM base as build
COPY . ./
RUN npm run build

## stage - deploy
FROM nginx:${NGINX_VERSION} as deploy
COPY .docker/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /angular/dist/angular /usr/share/nginx/html/