# SGCAE – Sistema de Gestión y Control de Acceso de Empleados

Backend con API REST para administrar empleados, credenciales y puntos de acceso, y para registrar y consultar las entradas y salidas de los empleados de una empresa de servicios logísticos y de almacenamiento.

El proyecto se construyó a partir de la estructura del proyecto **Inmoby** visto en clase, aplicando **Clean Architecture**, **DDD** y **CQRS**, con persistencia en **SQL Server** mediante **Entity Framework Core**.

## Tecnologías

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core 10 con SQL Server
- FluentValidation (validación de comandos y consultas)
- MSTest (pruebas unitarias)

## Estructura de la solución

```
Microservices/Access/
├── Access.slnx
├── ef.cmd                    Script para crear y aplicar migraciones
├── Access.Domain/            Entidades, objetos de valor y reglas del negocio (sin dependencias)
├── Access.Application/       Casos de uso (CQRS), contratos de repositorios, mediador y validaciones
├── Access.Persistence/       DataContext, configuraciones de EF Core, repositorios, seeders y migraciones
├── Access.Api/               Controladores REST, DTOs y formato de respuesta
└── Access.Tests/             Pruebas unitarias del dominio
docs/
└── SGCAE.postman_collection.json   Colección de Postman con las pruebas de la API
```

Las dependencias van hacia el dominio: `Api → Application ← Persistence`, y `Application → Domain`.

### Dominio (DDD)

| Entidad | Objetos de valor | Reglas principales |
|---|---|---|
| `Employee` | `DocumentNumber`, `Email`, `EmploymentStatus` | Identificación única que no cambia, correo único, no se modifica un empleado inactivo, se desactiva en lugar de eliminarse |
| `Area` | — | Área organizacional del empleado |
| `Credential` | `CredentialStatus` | Código único; solo una credencial vigente (`Active`) puede usarse; se puede bloquear (si está vigente) y revocar (definitivo) |
| `AccessPoint` | `AccessPointType` | Recepción, parqueadero, bodega o área restringida; puede estar habilitado o no |
| `AccessLog` | `MovementType`, `AccessResult` | Registro de cada intento de entrada o salida, autorizado o rechazado con su motivo |

### Casos de uso (CQRS)

Cada caso de uso tiene su comando o consulta, su validador y su manejador, y se ejecuta a través del mediador (`IMediator`):

- **Comandos:** `CreateEmployee`, `UpdateEmployee`, `DeactivateEmployee`, `AssignCredential`, `BlockCredential`, `RevokeCredential`, `CreateAccessPoint`, `RegisterAccess`.
- **Consultas:** `GetEmployeeById`, `GetEmployeesList`, `GetCredentialByCode`, `GetAccessPointById`, `GetAccessPointsList`, `GetAccessHistory`.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local o Express)
- Herramienta de EF Core: `dotnet tool install --global dotnet-ef`

## Configuración y ejecución

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/JulianHZ711/Seguimiento2-PD.git
   cd Seguimiento2-PD
   ```

2. Revisar la cadena de conexión en `Microservices/Access/Access.Api/appsettings.json`:
   ```json
   "MyConnection": "Server=localhost;Database=PDs2;Trusted_Connection=True;TrustServerCertificate=True"
   ```
   Si SQL Server es una instancia con nombre, cambiar el servidor, por ejemplo `localhost\SQLEXPRESS`.

3. Crear la base de datos aplicando la migración:
   ```bash
   cd Microservices/Access
   ./ef.cmd update
   ```

4. Ejecutar la API:
   ```bash
   dotnet run --project Access.Api
   ```
   La API queda en `http://localhost:5296`. Al iniciar se cargan los datos semilla:
   - 6 áreas organizacionales (Administración, Talento Humano, Logística, Almacenamiento, Seguridad, Tecnología).
   - 5 puntos de acceso (Recepción principal, Parqueadero, Bodega principal, Cuarto de servidores y Bodega de alto valor, este último deshabilitado).

5. El documento OpenAPI con todos los endpoints está en `http://localhost:5296/openapi/v1.json`.

## Endpoints

| Requerimiento | Método | Ruta | Descripción |
|---|---|---|---|
| RF-01 | POST | `/api/employees` | Registrar empleado |
| RF-01 | GET | `/api/employees?pageNumber=1&pageSize=15` | Consultar empleados (paginado) |
| RF-01 | GET | `/api/employees/{id}` | Consultar un empleado |
| RF-01 | PUT | `/api/employees` | Actualizar empleado |
| RF-01 | PATCH | `/api/employees/{id}/deactivate` | Desactivar empleado |
| RF-02 | POST | `/api/credentials` | Asignar credencial a un empleado activo |
| RF-02 | GET | `/api/credentials/{code}` | Consultar el estado de una credencial |
| RF-02 | PATCH | `/api/credentials/{code}/block` | Bloquear credencial |
| RF-02 | PATCH | `/api/credentials/{code}/revoke` | Revocar credencial |
| RF-03 | POST | `/api/access` | Registrar entrada o salida |
| RF-04 | GET | `/api/access/history` | Consultar el historial de accesos |
| RF-06 | POST | `/api/access-points` | Registrar punto de acceso |
| RF-06 | GET | `/api/access-points?isEnabled=true` | Consultar puntos de acceso (filtro opcional de habilitados) |
| RF-06 | GET | `/api/access-points/{id}` | Consultar un punto de acceso |

### RF-03: registro de entradas y salidas

Cuerpo de la petición (`movementType`: 1 = entrada, 2 = salida):

```json
{
  "credentialCode": "EMP-0001",
  "accessPointId": "00000000-0000-0000-0000-000000000000",
  "movementType": 1
}
```

El acceso se autoriza solo si la credencial existe y está vigente, el empleado tiene un vínculo laboral activo y el punto de acceso está habilitado. En cualquier otro caso el intento **queda registrado como rechazado** con su motivo, y la respuesta lo indica en `data.result` (`Authorized` o `Rejected`) y `data.rejectionReason`.

### RF-04: historial de accesos

Filtros opcionales: `employeeId`, `from`, `to`, `accessPointId`, `result` (`Authorized` o `Rejected`), además de `pageNumber` y `pageSize`. Las fechas se manejan en UTC; si `to` se envía sin hora, se incluye todo ese día.

```
GET /api/access/history?employeeId={id}&from=2026-10-01&to=2026-10-31&result=Rejected&pageNumber=1&pageSize=10
```

### Formato de respuesta

Todas las respuestas usan el mismo formato:

```json
{
  "isSuccess": true,
  "data": { },
  "message": null,
  "errors": null
}
```

| Código | Cuándo |
|---|---|
| 200 | Consulta o modificación exitosa |
| 201 | Registro creado (incluye los accesos rechazados, que también se registran) |
| 400 | Datos inválidos o regla de negocio incumplida (el detalle va en `errors`) |
| 404 | El recurso no existe |

## Pruebas

### Pruebas unitarias del dominio

```bash
dotnet test Microservices/Access/Access.slnx
```

Verifican las reglas e invariantes de `Employee`, `Email`, `DocumentNumber`, `Area`, `Credential`, `AccessPoint` y `AccessLog`.

### Pruebas de la API con Postman

1. Ejecutar la API.
2. En Postman, **Import** → seleccionar `docs/SGCAE.postman_collection.json`.
3. En la pestaña **Variables** de la colección, pegar en `areaId` el Id de un área:
   ```sql
   SELECT Id, Name FROM Areas
   ```
4. Ejecutar la colección completa (**Run collection**). Las carpetas siguen el orden de los requerimientos y los demás Ids se guardan automáticamente entre peticiones.

También se puede probar desde Visual Studio con el archivo `Microservices/Access/Access.Api/Access.Api.http`.
