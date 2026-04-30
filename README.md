# ☕ Brunch & Co — Sistema de Reservas

Sistema fullstack de reservas para café/brunch con panel de administración protegido por JWT.

## Stack

**Backend**
- ASP.NET Core 8
- Entity Framework Core + SQL Server
- Autenticación JWT
- Swagger / OpenAPI

**Frontend**
- Next.js 14
- TypeScript
- Tailwind CSS

## Funcionalidades

- Reservas públicas con validación de cupo por franja horaria
- Panel de administración con login y autenticación JWT
- Confirmar y cancelar reservas con modal de confirmación
- Badges de estado: pendiente / confirmada / cancelada
- Paginación y filtros por fecha en el panel admin
- Campos: nombre, email, teléfono, fecha, personas, notas

## Endpoints

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| POST | /api/Auth/login | — | Login admin, devuelve JWT |
| POST | /api/Reservas | — | Crear reserva (público) |
| GET | /api/Reservas | ✓ | Listar reservas con paginación |
| GET | /api/Reservas/{id} | ✓ | Obtener reserva por ID |
| PATCH | /api/Reservas/{id}/estado | ✓ | Cambiar estado |
| DELETE | /api/Reservas/{id} | ✓ | Eliminar reserva |

## Cómo correrlo localmente

### Backend

1. Clonar el repositorio
2. Copiar el archivo de configuración y completar los valores:

```bash
cp appsettings.json appsettings.Development.json
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CafeDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "tu-clave-secreta-de-al-menos-32-caracteres",
    "Issuer": "CafeApi",
    "Audience": "CafeApp"
  },
  "Admin": {
    "Usuario": "tu-usuario",
    "Password": "tu-password"
  }
}
```

3. Aplicar migraciones y correr:

```bash
dotnet ef database update
dotnet run
```

4. Swagger disponible en `https://localhost:7102/swagger`

### Frontend

```bash
cd cafe-frontend
cp .env.example .env.local
# Completar NEXT_PUBLIC_API_URL=http://localhost:5000
npm install
npm run dev
```

## Modelo de Reserva

```json
{
  "nombre": "Ana García",
  "email": "ana@email.com",
  "telefono": "11 1234-5678",
  "fecha": "2025-06-15T11:00:00",
  "personas": 3,
  "notas": "Cumpleaños, mesa cerca de la ventana"
}
```