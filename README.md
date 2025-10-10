# 🏨 Plataforma de Reservas de Hoteles

Plataforma diseñada para optimizar la gestión de habitaciones, clientes y reservas.
Incluye funcionalidades para **check-in / check-out**, **facturación automática** y **reportes de ocupación**, con una arquitectura moderna y escalable basada en **Clean Architecture**.

![Hotel Resort y Playa](https://github.com/user-attachments/assets/9f791e6c-8d5b-4ac0-9b98-ef1ec681e40d)

---

## 📚 Tabla de Contenidos
- [📦 Instalación](#-instalación)
- [🚀 Uso](#-uso)
- [🛠️ Tecnologías](#%EF%B8%8F-tecnologías)
- [👥 Autores](#-autores)
- [📄 Licencia](#-licencia)

---

## 📦 Instalación

Para configurar el proyecto y ejecutarlo localmente en **Visual Studio 2022**, sigue estos pasos:

### 1. Clonar el repositorio y Abrir la Solución
Abre una terminal, clona el repositorio y abre el archivo de solución:

```bash
git clone https://github.com/IsaacEffect/HotelReservation.git
cd HotelReservation
````

Abre la solución: `HotelReservation.sln` en Visual Studio 2022.

### 2\. Restaurar dependencias

Visual Studio lo hará automáticamente. Si necesitas hacerlo manualmente:

```bash
dotnet restore
```

### 3\. Configurar la Base de Datos (SQL Server)

1.  Abre el archivo `appsettings.json` dentro del proyecto **HotelReservation.API**.
2.  Edita la cadena de conexión de **SQL Server** con tus credenciales locales:

<!-- end list -->

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=HotelDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3.  **Opciones de Inicialización de DB:**
      * **Opción A (Recomendada si usas Migraciones):** Abre la Consola del Administrador de Paquetes y ejecuta: `Update-Database`.
      * **Opción B (Manual):** Crea la base de datos `NombreDB` desde **SQL Server Management Studio (SSMS)**.

### 4\. Compilar y Establecer Proyecto de Inicio

1.  Compila la solución para verificar que no haya errores:

    ```bash
    dotnet build
    ```

2.  Haz clic derecho sobre **HotelReservation.API** y selecciona **“Establecer como proyecto de inicio”**. (Esto es necesario para que el Frontend pueda consumir la API).

-----

## 🚀 Uso

El proyecto se compone de una **API (Backend)** y una **Capa de Presentación (Frontend)**. Ambos deben estar activos para el funcionamiento completo.

### 1\. Ejecutar el Backend (API)

Asegúrate de que **HotelReservation.API** sea el proyecto de inicio y presiona **Ctrl + F5** (o F5 para *debug*).

  * Se abrirá una ventana del navegador mostrando la documentación de la API (generalmente **Swagger** en `https://localhost:5001/swagger`).

### 2\. Ejecutar el Frontend (Presentación)

Una vez que la API esté corriendo:

1.  Haz clic derecho sobre **HotelReservation.Presentation** y selecciona **“Establecer como proyecto de inicio”**.
2.  Presiona **Ctrl + F5** (o F5).

<!-- end list -->

  * Se abrirá una nueva ventana del navegador mostrando la interfaz principal (ej: `index.html`).
  * El Frontend consumirá los datos desde la API (`https://localhost:5001/api/...`).

-----

## 🛠️ Tecnologías

El proyecto fue desarrollado utilizando el siguiente *stack*:

  * **Backend:** .NET 8, ASP.NET Core Web API, C#.
  * **Arquitectura:** **Clean Architecture**.
  * **Base de Datos:** **Entity Framework Core** con **SQL Server**.
  * **Frontend:** React.
  * **IDE:** Visual Studio 2022

-----

## 👥 Autores

Desarrollado por el equipo:

  * **Eduardo Alexander Ortiz Suncar**
  * **Eileen Abigail Valdéz Vargas**
  * **Charleny Contreras Ogando**
  * **Angel Isaac Mejia Martinez**
  * **Willy Gerson Alcantara Muñoz**

-----

## 📄 Licencia

Proyecto académico desarrollado con fines educativos.
No se permite uso comercial..

