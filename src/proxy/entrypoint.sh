#!/bin/sh
set -eu

tls_enabled=$(printf '%s' "${PROXY_TLS_ENABLED:-false}" | tr '[:upper:]' '[:lower:]')

case "$tls_enabled" in
  1|true|yes|on)
    cert_file="${PROXY_TLS_CERTIFICATE_FILE:-tls.crt}"
    key_file="${PROXY_TLS_CERTIFICATE_KEY_FILE:-tls.key}"
    cert_path="/etc/nginx/certs/$cert_file"
    key_path="/etc/nginx/certs/$key_file"

    if [ ! -f "$cert_path" ]; then
      echo "TLS is enabled but certificate file was not found: $cert_path" >&2
      exit 1
    fi

    if [ ! -f "$key_path" ]; then
      echo "TLS is enabled but certificate key file was not found: $key_path" >&2
      exit 1
    fi

    rm -f /etc/nginx/templates/http.conf.template /etc/nginx/templates/default.conf.template
    mv /etc/nginx/templates/https.conf.template /etc/nginx/templates/default.conf.template
    ;;
  *)
    rm -f /etc/nginx/templates/https.conf.template /etc/nginx/templates/default.conf.template
    mv /etc/nginx/templates/http.conf.template /etc/nginx/templates/default.conf.template
    ;;
esac
