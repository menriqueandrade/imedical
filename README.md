# 🩺 IMedical - Solución Completa

Este repositorio contiene una solución con dos proyectos que trabajan en conjunto para mostrar información del clima y noticias asociadas a distintas ciudades. La solución está dividida en:

- **IMedical.F (Frontend)**: Proyecto ASP.NET Web Forms (.NET Framework 4.7.2)
- **IMedicalB (Backend)**: API ASP.NET Core (.NET 9.0)
- **Swagger (Backend)**: [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)
---

## 📋 Requisitos Previos

- Visual Studio 2022 o superior
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Docker Desktop (opcional pero recomendado si no tienes SQL Server instalado)
---

## ⚙️ Configuración y Ejecución

### 🔁 Opción 1: Usar Docker para la base de datos (recomendado)

1. Instala y abre **Docker Desktop**.
2. Desde la raíz del proyecto, ejecuta el siguiente comando:

   ```bash
   docker-compose up

Esto ejecutara un contenedor de SQL Server

## ⚙️ Sin Docker (usar SQL Server local)
### 🔁 Opción 2: Usar Docker para la base de datos (recomendado)

1. Instala y abre proyecto.
2. Desde la raíz del proyecto, ejecuta el siguiente comando:

   ```bash
   🔧 En el proyecto IMedicalB (Backend)
   Modificar

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=IMedical;User Id=sa;Password=C0ntr@se#1;"
}       

 
   🔧 En el proyecto IMedical.F (Backend)
   Modificar string connectionString = "Server=localhost;Database=IMedical;User Id=sa;Password=C0ntr@se#1;";



   ## 🗃️ Inicialización de la Base de Datos

- El **backend** crea la base de datos automáticamente si no existe.
- El **frontend** contiene un método `InitializeDatabase` en `Weather.aspx.cs` que puede crear las tablas necesarias.

---

## 🧰 Tecnologías Utilizadas

- **ASP.NET Core 9.0** (Backend)
- **ASP.NET Web Forms 4.7.2** (Frontend)
- **SQL Server** (vía Docker o local)
- **Docker + Docker Compose**
- **Serilog**

---

## 🧪 Observaciones

- El proyecto está preparado para funcionar automáticamente si se usa Docker.
- Si decides no usar Docker, asegúrate de tener configuradas correctamente las cadenas de conexión tanto en el backend como en el frontend.
- El puerto por defecto del contenedor **SQL Server** es `1433`. Asegúrate de que esté disponible en tu sistema.

---

## 🧠 Tips Útiles

- Si experimentas problemas de conexión, prueba con:
  - `localhost`
  - `127.0.0.1`
  - `host.docker.internal`
  
- Revisa el log de Docker para confirmar que SQL Server arrancó correctamente.
- Puedes detener los contenedores con:

```bash
docker-compose down            
        
