# 🥗 NutriCheck – Backend

API REST para la gestión nutricional de pacientes y nutricionistas, desarrollada en C# con ASP.NET Core Web API.

---

## 📋 Tabla de contenidos

1. [Descripción](#descripción)  
2. [Códigos de respuesta HTTP](#códigos-de-respuesta-http)  
3. [Esquema de parámetros](#esquema-de-parámetros)  
4. [Autenticación (JWT)](#autenticación-jwt)  
5. [Tecnologías](#tecnologías)  
6. [Cómo correr el proyecto](#cómo-correr-el-proyecto)  

---

## Descripción

NutriCheck es una plataforma que ofrece:

- **Gestión de Pacientes**: CRUD (crear, listar, editar, eliminar).  
- **Nutricionistas**: modelo con precarga de usuario de prueba.  
- **Dietas**: asignar platos a pacientes por fecha y momento del día.  
- **Platos de comida**: crear y listar menús personalizados.  
- **Registro de Comidas**: pacientes registran ingestas diarias.  
- **Recordatorio de comidas faltantes**: detectar Desayuno/Almuerzo/Merienda/Cena no registrados.  
- **Preferencias alimenticias**: configurar opciones (vegano, sin TACC, etc.).  
- **Documentación interactiva** con Swagger (descripciones, parámetros y códigos de respuesta).

---

## Códigos de respuesta HTTP

| Código | Significado                                   |
|--------|-----------------------------------------------|
| 200    | OK – Operación exitosa.                       |
| 201    | Created – Recurso creado correctamente.       |
| 204    | No Content – Recurso eliminado correctamente. |
| 400    | Bad Request – Parámetros o datos inválidos.   |
| 401    | Unauthorized – Token JWT faltante o inválido. |
| 404    | Not Found – Recurso no encontrado.            |

---

## Esquema de parámetros

- **[FromBody]**  
  Datos JSON en el cuerpo de la petición (POST, PUT).

- **[FromQuery]**  
  Parámetros en la URL (`?fecha=2025-04-17&pacienteId=1`).

- **[FromRoute]**  
  Parámetros en la ruta (`/api/pacientes/{id}`).

---

## Autenticación (JWT)

- Usamos **Bearer JWT** para proteger endpoints.  
- En **Swagger UI**, clic en **Authorize** e ingresá:

## ⚙️ Tecnologías utilizadas
- C# / .NET 7
-ASP.NET Core Web API
-Entity Framework Core (InMemory para pruebas)
-Swagger (OpenAPI)
-QuestPDF (generación de PDF)
-JWT (Microsoft.AspNetCore.Authentication.JwtBearer)
-Git & GitHub

## COMO ORRER EL PROYECTO
git clone https://github.com/pablobarcala/nutricheck-back.git
cd nutricheck-back/NutriCheck.Backend
dotnet restore
dotnet build
dotnet run
