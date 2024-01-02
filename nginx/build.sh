#!/bin/sh
docker stop nginx-demo
docker rmi nginx-demo
docker build -t nginx-demo .
docker run -p 8080:80 -p 5000:5000 -p 8000:8000 --rm -d --name nginx-demo nginx-demo
#docker exec -it nginx-demo sh
