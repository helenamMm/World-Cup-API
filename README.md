# 🏆 World Cup 2026 - API & Web Portal

![.NET](https://img.shields.io/badge/.NET_8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-4EA94B?style=for-the-badge&logo=mongodb&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap_5-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)

Plataforma integral (Backend + Frontend estático) para la gestión y visualización del **Mundial de la FIFA 2026**. Provee una API robusta y vistas web interactivas para seguir resultados en vivo, estadísticas, plantillas de equipos y recibir notificaciones por correo electrónico.

## 📖 Sobre el Proyecto

El proyecto está compuesto por dos partes principales:
1. **Backend:** Una API RESTful construida con ASP.NET Core 8 y MongoDB. Se encarga de toda la lógica de negocio, autenticación, gestión de la base de datos y envío de alertas por correo electrónico.
2. **Frontend:** Interfaz de usuario maquetada con HTML5, CSS3 y Bootstrap 5 que consume (o está lista para consumir) los endpoints de la API. Incluye paneles de administración y vistas para aficionados.

## ✨ Características Principales

### 👤 Para Usuarios (Aficionados):
* **⭐ Sistema de Favoritos:** Agrega tus equipos y partidos favoritos a tu perfil para un seguimiento personalizado.
* **🔔 Notificaciones Inteligentes:** Integración con `MailKit` (SMTP) para enviar correos electrónicos automáticamente cuando un partido marcado como favorito comienza o cuando se anota un gol.
* **📊 Estadísticas y Resultados:** Visualización de partidos en vivo, próximos encuentros y fase de grupos.

### 🛡️ Para Administradores:
* **⚙️ Panel de Control (CRUD):** Gestión completa de la base de datos a través de una interfaz dedicada.
* **👥 Gestión de Torneo:** Creación y actualización de Equipos, Jugadores y Partidos (control de fases, grupos y estados: *Programado, En Juego, Finalizado*).
* **🔐 Autenticación y Autorización:** Acceso seguro mediante **JWT Tokens** y validación de roles (`admin` vs `user`).

## 🛠️ Stack Tecnológico

* **Backend:** C# .NET 8, ASP.NET Core Web API.
* **Base de Datos:** MongoDB.
* **Frontend:** HTML5, CSS puro y Bootstrap 5.
* **Infraestructura:** Docker & Docker Compose.
