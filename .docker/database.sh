#!/bin/bash

CONTAINER_NAME="rate-limit-database"
POSTGRES_USER="ratelimit"
POSTGRES_PASSWORD="12345678"
POSTGRES_DB="ratelimit"
POSTGRES_PORT=5432
POSTGRES_IMAGE="postgres:16"

CONTAINER_EXISTS=$(docker ps -a -q -f name="^${CONTAINER_NAME}$")
CONTAINER_RUNNING=$(docker ps -q -f name="^${CONTAINER_NAME}$")

if [ -n "$CONTAINER_RUNNING" ]; then
  echo "Container '$CONTAINER_NAME' já está rodando. Nada a fazer."
  exit 0
fi

if [ -n "$CONTAINER_EXISTS" ]; then
  echo "Container '$CONTAINER_NAME' existe mas está parado. Iniciando..."
  docker start $CONTAINER_NAME > /dev/null
else
    echo "Criando novo container PostgreSQL '$CONTAINER_NAME'..."
    docker run -d \
        --name $CONTAINER_NAME \
        -e POSTGRES_USER=$POSTGRES_USER \
        -e POSTGRES_PASSWORD=$POSTGRES_PASSWORD \
        -e POSTGRES_DB=$POSTGRES_DB \
        -p $POSTGRES_PORT:5432 \
        $POSTGRES_IMAGE
fi

echo "Aguardando PostgreSQL iniciar..."
until docker exec $CONTAINER_NAME pg_isready -U $POSTGRES_USER > /dev/null 2>&1; do
  sleep 1
done

echo "PostgreSQL rodando e pronto para uso."
echo "Acesso: user=$POSTGRES_USER | password=$POSTGRES_PASSWORD | db=$POSTGRES_DB"
echo "Porta local: $POSTGRES_PORT"
