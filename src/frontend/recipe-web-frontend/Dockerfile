FROM node:20 AS build

WORKDIR /app

COPY pnpm-lock.yaml* package.json* ./
RUN npm install -g pnpm
RUN pnpm install

COPY . .
RUN pnpm run build

FROM build AS development

ENV NODE_ENV=development
ENTRYPOINT [ "pnpm", "run", "dev", "--port", "80", "--host"]

FROM nginx:stable-alpine-slim AS production

COPY --from=build /app/dist /usr/share/nginx/html
COPY config/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]