FROM quay.io/keycloak/keycloak:26.2.5 

COPY ./config/dev-realm-config.json /opt/keycloak/data/import/dev-realm-config.json 

CMD ["start-dev", "--import-realm"]