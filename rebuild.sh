#!/bin/bash

echo "Stopping and removing existing containers..."
docker-compose down -v

echo "Building and starting containers..."
docker-compose up --build
