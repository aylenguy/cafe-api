# Brunch & Co — API

API REST para gestión de reservas de Brunch & Co. Desarrollada con ASP.NET Core y Entity Framework Core.

## Stack

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI

## Endpoints

| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | /api/Reservas | Crear una reserva |
| GET | /api/Reservas | Obtener todas las reservas |

## Modelo

\```json
{
  "nombre": "string",
  "email": "string",
  "fecha": "2024-01-01T12:00:00",
  "cantidadPersonas": 2
}
\```

## Cómo correrlo

1. Clonar el repositorio
2. Configurar la cadena de conexión en `appsettings.json`

\```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=CafeDb;..."
}
\```

3. Aplicar migraciones

\```bash
dotnet ef database update
\```

4. Correr la API

\```bash
dotnet run
\```

5. Swagger disponible en [https://localhost:7102/swagger](https://localhost:7102/swagger)