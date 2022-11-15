FROM node:latest

WORKDIR /app

COPY . .

EXPOSE 4200

RUN npm install

RUN npm install -g @angular/cli

CMD ng serve --host 0.0.0.0