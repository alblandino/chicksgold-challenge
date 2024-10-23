# Water Bucket Challenge API

## Descripción

Esta es una API REST desarrollada en ASP.NET Core que resuelve el problema clásico de los cubos de agua. La API permite a los usuarios enviar las capacidades de dos cubos y la cantidad deseada de agua, y devuelve los pasos necesarios para alcanzar esa cantidad utilizando los cubos.

## Tabla de Contenidos

- [Requisitos Previos](#requisitos-previos)
- [Configuración del Entorno](#configuración-del-entorno)
- [Ejecutando la Aplicación](#ejecutando-la-aplicación)
- [Uso de la API](#uso-de-la-api)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Pruebas Unitarias](#pruebas-unitarias)
- [Contribuciones](#contribuciones)
- [Licencia](#licencia)

## Requisitos Previos

Antes de comenzar, asegúrate de tener instalados los siguientes componentes en tu máquina:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/) o cualquier cliente HTTP para probar la API.

## Configuración del Entorno

1. **Clonar el Repositorio**

   Clona este repositorio en tu máquina local usando el siguiente comando:

   ```bash
   git clone https://github.com/alblandino/chicksgold-challenge.git
   cd chicksgold-challenge
   ```

2. **Restaurar Dependencias**

   Asegúrate de estar en la carpeta del proyecto y ejecuta el siguiente comando para restaurar las dependencias:

   ```bash
   dotnet restore
   ```

3. **Compilar el Proyecto**

   Compila el proyecto para asegurarte de que todo esté configurado correctamente:

   ```bash
   dotnet build
   ```

## Ejecutando la Aplicación

Para ejecutar la API, usa el siguiente comando:

```bash
dotnet run
```

Esto iniciará el servidor y podrás acceder a la API en `http://localhost:5000` (o en el puerto que se muestre en la consola).

## Uso de la API

### Endpoint: `POST /challenge`

Envía una solicitud POST con el siguiente cuerpo JSON:

```json
{
    "x_capacity": 5,
    "y_capacity": 3,
    "z_amount_wanted": 4
}
```

#### Respuesta Exitosa

Si la solución es posible, recibirás una respuesta con los pasos a seguir:

```json
{
    "solution": [
        {
            "step": 1,
            "bucketX": 5,
            "bucketY": 0,
            "action": "Fill Bucket X - x:5,y:0",
            "status": null
        },
        {
            "step": 2,
            "bucketX": 2,
            "bucketY": 3,
            "action": "Transfer Bucket X to Bucket Y - x:2,y:3",
            "status": null
        },
        {
            "step": 3,
            "bucketX": 0,
            "bucketY": 3,
            "action": "Flush Bucket Y - x:0,y:3",
            "status": "Solved"
        }
    ]
}
```

#### Respuesta de Error

Si no hay solución, recibirás un estado 500 con el siguiente mensaje:

```json
{
    "solution": "No solution"
}
```

## Estructura del Proyecto

El proyecto tiene la siguiente estructura:

```
/TuSolucion
│
├── /Challenge
│   ├── ChallengeController.cs
│   └── WaterBucketChallenge.cs
│
├── /Challenge.Tests
│   ├── ChallengeControllerTests.cs
│   └── WaterBucketChallengeTests.cs
│
└── TuSolucion.sln
```

- **ChallengeController.cs**: Controlador que maneja las solicitudes HTTP.
- **WaterBucketChallenge.cs**: Lógica del negocio que resuelve el problema de los cubos de agua.
- **ChallengeControllerTests.cs**: Pruebas unitarias para el controlador.
- **WaterBucketChallengeTests.cs**: Pruebas unitarias para la lógica de negocio.

## Pruebas Unitarias

Para ejecutar las pruebas unitarias, asegúrate de estar en la carpeta del proyecto de pruebas y ejecuta el siguiente comando:

```bash
dotnet test
```

Esto ejecutará todas las pruebas y te dará un resumen de los resultados.