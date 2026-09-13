FROM node:22-alpine AS build
WORKDIR /app
COPY client/ecommerce-angular/package*.json ./
RUN npm ci
COPY client/ecommerce-angular/ ./
RUN npm run build

FROM nginx:1.27-alpine AS runtime
COPY devops/nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/ecommerce-angular/browser /usr/share/nginx/html
EXPOSE 80
