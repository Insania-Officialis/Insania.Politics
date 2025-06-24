#Использование образа 17.4 PostgreSQL
FROM postgres:17.4-bookworm

#Устновка расширений
RUN apt-get update && apt-get upgrade -y && apt-get install -y \
    postgresql-17-postgis-3 \
    && rm -rf /var/lib/apt/lists/*  #Очистка кеша

#Проброс порта
EXPOSE 5432

#Запуск PostgreSQL
CMD ["postgres"]