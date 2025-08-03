FROM node:20 AS build

WORKDIR /app

COPY pnpm-lock.yaml* package.json pnpm-workspace.yaml ./
RUN npm install -g pnpm
RUN pnpm fetch

COPY . .
RUN pnpm install --frozen-lockfile --prefer-offline 
RUN pnpm recursive run --sort build

FROM build AS development

ENV NODE_ENV=development
WORKDIR /app/recipe-web-frontend/
ENTRYPOINT [ "pnpm", "run", "dev", "--port", "80", "--host"]

FROM nginx:stable-alpine-slim AS production

COPY --from=build recipe-web-frontend/app/dist /usr/share/nginx/html
COPY config/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]