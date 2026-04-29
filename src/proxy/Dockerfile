FROM nginx:alpine

COPY ./config/http.conf.template /config/http.conf.template
COPY ./config/https.conf.template /config/https.conf.template
COPY ./entrypoint.sh /docker-entrypoint.d/10-select-proxy-config.sh

RUN chmod +x /docker-entrypoint.d/10-select-proxy-config.sh
