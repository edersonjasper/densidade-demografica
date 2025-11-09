#!/bin/bash


echo "Building Docker image..."

docker build -t densidade-demografica-api:latest .

echo "Build completed!"

# Mostrar tamanho da imagem
docker images densidade-demografica-api:latest

echo ""
echo "To run the container:"
echo "   docker run -p 5000:8080 densidade-demografica-api:latest"
echo ""
echo "Or use docker-compose:"
echo "   docker-compose up"