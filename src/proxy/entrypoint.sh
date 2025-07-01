#!/bin/sh
envsubst '${PUBLIC_HOST} ${BACKEND_HOST} ${BACKEND_PORT} ${KEYCLOAK_HOST} ${KEYCLOAK_PORT} ${FRONTEND_HOST} ${FRONTEND_PORT}' < /etc/nginx/nginx.conf.template > /etc/nginx/conf.d/default.conf
cat /etc/nginx/conf.d/default.conf
nginx -g 'daemon off;'