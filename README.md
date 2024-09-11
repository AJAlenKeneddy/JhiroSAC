Jhiro Shop
Jhiro Shop es un proyecto de tienda de productos textiles, diseñado para gestionar productos y ventas de manera eficiente, proporcionando una interfaz amigable tanto para los administradores como para los usuarios finales.

Funcionalidades principales de Jhiro Shop:
CRUD de Productos:

Gestión completa de productos, permitiendo listar, crear, actualizar, eliminar y restaurar productos.
Implementación de Razor Pages para facilitar la gestión de productos en un ambiente controlado.
Visualización y Restauración de Productos Eliminados:

Se creó una página dedicada a listar productos eliminados y dar la opción de restaurarlos.
Redirección automática a la página de productos eliminados tras restaurar uno.
Interfaz de Usuario Responsiva:

Diseño responsivo tanto para la página de inicio como para las secciones de productos, incluyendo la funcionalidad de ver detalles de productos por categorías.
Integración de Bootstrap:

Uso de Bootstrap para asegurar una UI profesional y responsiva.
Tecnologías utilizadas:
Blazor Server: Desarrollo de componentes interactivos.
ASP.NET Core: Para la estructura general del back-end.
EF Core: Manejador de base de datos y consultas.
SQL Server: Base de datos relacional.
Bootstrap: Utilizado para asegurar que la interfaz de usuario sea responsiva y profesional.
Configuración del Proyecto:
Configuración de servicios:

Se ha agregado el servicio ProductoService en Program.cs para interactuar con los endpoints de productos y categorías desde el front-end de Blazor.
Uso de ApiBaseUrl para definir la base URL para las llamadas a la API.
Procedimientos almacenados (SP):

Implementación de varios SP para manejar categorías y productos en la base de datos.
