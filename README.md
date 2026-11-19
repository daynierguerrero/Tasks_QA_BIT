# Tasks QA BIT API

API REST de tareas desarrollada en .NET 8 con arquitectura en capas y SQLite.

## Requisitos

- .NET 8 SDK.

## Ejecutar

Desde la carpeta de la solución:

```powershell
dotnet restore
dotnet run --project .\Tasks_QA_BIT\Tasks_QA_BIT.csproj
```

En desarrollo, Swagger queda disponible en la dirección mostrada por `dotnet run`, en `/swagger`. La base `tasks.db` se crea en el directorio de trabajo y la migración inicial se aplica automáticamente al iniciar.

## Tests

```powershell
dotnet test .\Tasks_QA_BIT.sln
```

## Endpoints

| Método | Ruta | Resultado |
|---|---|---|
| GET | `/api/tasks` | Lista tareas con filtros y paginación |
| GET | `/api/tasks/{id}` | Consulta una tarea |
| POST | `/api/tasks` | Crea una tarea |
| PUT | `/api/tasks/{id}` | Edita una tarea |
| DELETE | `/api/tasks/{id}` | Elimina una tarea |

Parámetros opcionales para listar: `status` (`Pendiente`, `EnCurso`, `Hecha`), `responsible`, `page` (por defecto 1) y `pageSize` (por defecto 10, máximo 100). La respuesta incluye `items`, `page`, `pageSize`, `totalCount` y `totalPages`.

Ejemplo para crear:

```json
{
  "title": "Preparar entrega",
  "description": "Revisar los pendientes del sprint",
  "status": "Pendiente",
  "responsible": "Ana",
  "dueDate": "2026-12-31"
}
```

Los campos son obligatorios; título y responsable tienen un máximo de 200 y 100 caracteres, respectivamente, y descripción un máximo de 2000. Las fechas usan formato ISO `yyyy-MM-dd`. Las validaciones devuelven `400`, los recursos inexistentes `404`, la creación `201` y la eliminación `204`.

## Capas

- `Tasks_QA_BIT.Domain`: entidades y estados del dominio.
- `Tasks_QA_BIT.Application`: contratos, reglas de aplicación y servicio.
- `Tasks_QA_BIT.Infrastructure`: persistencia SQLite, Entity Framework Core y migraciones.
- `Tasks_QA_BIT`: API HTTP y composición de dependencias.
- `Tasks_QA_BIT.Tests`: pruebas unitarias del servicio.
