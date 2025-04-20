#!/bin/bash

echo "Esperando a que SQL Server inicie..."
sleep 20

echo "Ejecutando script de inicialización..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "C0ntr@se#1" -i /init.sql

echo "Script ejecutado. Contenedor sigue activo."
tail -f /dev/null