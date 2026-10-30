# Sistema de Gestión y Control de Acceso de Empleados

Backend con API REST para administrar empleados, credenciales y puntos de acceso, y para registrar y consultar las entradas y salidas de los empleados de una empresa de servicios logísticos y de almacenamiento.

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
└── S2PDColeccion.postman_collection.json   Colección de Postman con las pruebas de la API
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

## Endpoints

| Requerimiento | Método | Ruta | Descripción |
|---|---|---|---|
| POST | `/api/employees` | Registrar empleado |
| GET | `/api/employees?pageNumber=1&pageSize=15` | Consultar empleados (paginado) |
| GET | `/api/employees/{id}` | Consultar un empleado |
| PUT | `/api/employees` | Actualizar empleado |
| PATCH | `/api/employees/{id}/deactivate` | Desactivar empleado |
| POST | `/api/credentials` | Asignar credencial a un empleado activo |
| GET | `/api/credentials/{code}` | Consultar el estado de una credencial |
| PATCH | `/api/credentials/{code}/block` | Bloquear credencial |
| PATCH | `/api/credentials/{code}/revoke` | Revocar credencial |
| POST | `/api/access` | Registrar entrada o salida |
| GET | `/api/access/history` | Consultar el historial de accesos |
| POST | `/api/access-points` | Registrar punto de acceso |
| GET | `/api/access-points?isEnabled=true` | Consultar puntos de acceso (filtro opcional de habilitados) |
| GET | `/api/access-points/{id}` | Consultar un punto de acceso |


| Código | Cuándo |
|---|---|
| 200 | Consulta o modificación exitosa |
| 201 | Registro creado (incluye los accesos rechazados, que también se registran) |
| 400 | Datos inválidos o regla de negocio incumplida (el detalle va en `errors`) |
| 404 | El recurso no existe |


## Evidencias

Capturas del funcionamiento de la API y de las pruebas, ubicadas en `docs/evidencias/`.

### Ejecución completa de la colección de Postman

![Ejecución de la colección de Postman](docs/evidencias/postman-run-collection.png)

### RF-01: Gestión de empleados

| Registrar | Consultar |
|---|---|
| ![Registrar empleado](docs/evidencias/registrar-empleado.png) | ![Consultar empleados](docs/evidencias/consultar-empleados.png) |

| Actualizar | Desactivar |
|---|---|
| ![Actualizar empleado](docs/evidencias/actualizar-empleado.png) | ![Desactivar empleado](docs/evidencias/desactivar-empleado.png) |

### RF-02: Gestión de credenciales

| Asignar | Consultar estado | Bloquear y revocar |
|---|---|---|
| ![Asignar credencial](docs/evidencias/asignar-credencial.png) | ![Consultar credencial](docs/evidencias/consultar-credencial.png) | ![Bloquear y revocar credencial](docs/evidencias/bloquear-revocar-credencial.png) |

### RF-03: Registro de entradas y salidas

| Acceso autorizado | Acceso rechazado |
|---|---|
| ![Acceso autorizado](docs/evidencias/acceso-autorizado.png) | ![Acceso rechazado](docs/evidencias/acceso-rechazado.png) |

### RF-04: Historial de accesos

![Historial de accesos con filtros y paginación](docs/evidencias/historial.png)

### RF-06: Puntos de acceso

| Registrar | Consultar habilitados |
|---|---|
| ![Registrar punto de acceso](docs/evidencias/registrar-punto-acceso.png) | ![Consultar puntos habilitados](docs/evidencias/puntos-habilitados.png) |

### Pruebas unitarias

![Resultado de dotnet test](docs/evidencias/dotnet-test.png)

### Base de datos

![Tablas y registros en SQL Server](docs/evidencias/base-de-datos.png)
