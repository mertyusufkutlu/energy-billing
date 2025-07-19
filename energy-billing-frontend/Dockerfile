# 1. Aşama: Angular projesini build et
FROM node:20-alpine as build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

# 2. Aşama: nginx üzerinden serve et
FROM nginx:alpine
COPY --from=build /app/dist/energy-billing /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
