# Ejemplo de Comunicación entre Microservicios con Azure Service Bus y MassTransit

Este repositorio contiene un ejemplo práctico en .NET 8 que ilustra cómo comunicar dos microservicios mediante Azure Service Bus utilizando MassTransit para la gestión de mensajes.

## 📲 Microservicio de Recepción de Órdenes

### Descripción
Este microservicio expone un endpoint `orders` que acepta peticiones para crear órdenes de pizzas. Las órdenes son enviadas al Azure Service Bus para ser procesadas.

### Tecnologías Utilizadas
- ASP.NET Core 8
- MassTransit
- Azure Service Bus

### Funcionalidades
- Recepción de peticiones para crear órdenes de pizzas.
- Envío asincrónico de órdenes al Azure Service Bus para su procesamiento.

## 👨🏻‍🍳 Microservicio de Procesamiento de Órdenes

### Descripción
Este microservicio está suscrito a una cola de órdenes en Azure Service Bus. Procesa las órdenes recibidas de manera asincrónica.

### Tecnologías Utilizadas
- ASP.NET Core 8
- MassTransit
- Azure Service Bus

### Funcionalidades
- Suscripción a una cola de órdenes para procesamiento asincrónico.
- Gestión y procesamiento de órdenes recibidas.

## Diagrama de Arquitectura



## Ejecución

### Requisitos Previos
- Visual Studio 2022
- .NET 8 SDK
- Cuenta de Azure con acceso a Azure Service Bus

### Configuración
1. Clona este repositorio.
2. Configura las credenciales de Azure Service Bus en ambos microservicios.
3. Ejecuta ambos proyectos simultáneamente.

### Contribuciones
Contribuciones y sugerencias son bienvenidas. Si deseas mejorar este ejemplo o reportar problemas, por favor abre un issue o envía un pull request.

### Licencia
Este proyecto está bajo la licencia MIT. Consulta el archivo `LICENSE` para más detalles.

🍕🚀 ¡Disfruta creando y procesando órdenes de pizzas con Azure Service Bus y MassTransit!
