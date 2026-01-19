# SalvadoreX POS Desktop

Sistema de Punto de Venta para Windows con sincronizacion en la nube.

## Caracteristicas

- **Punto de Venta Completo**: Interfaz intuitiva para ventas rapidas
- **Funciona 100% Offline**: Base de datos SQLite local
- **Sincronizacion Automatica**: Cuando hay internet, sincroniza con Supabase
- **Inventario**: Gestion completa de productos y stock
- **Clientes**: Base de datos de clientes con credito y puntos
- **Historial de Ventas**: Reportes y reimpresion de tickets
- **Impresion de Tickets**: Compatible con impresoras termicas

## Requisitos

- Windows 10/11
- .NET 8.0 Runtime
- Visual Studio 2022 (para desarrollo)

## Instalacion para Desarrollo

1. Abrir `SalvadoreXDesktop.sln` en Visual Studio 2022
2. Restaurar paquetes NuGet
3. Compilar y ejecutar (F5)

## Estructura del Proyecto

```
SalvadoreXDesktop/
├── Models/           # Modelos de datos
├── Data/             # Repositorios y acceso a base de datos
├── Services/         # Servicios (sincronizacion, impresion)
├── Forms/            # Formularios de Windows Forms
├── Resources/        # Iconos y recursos
└── Properties/       # Configuracion del proyecto
```

## Sincronizacion

El sistema sincroniza automaticamente con Supabase cuando detecta conexion a internet:

1. **Cambios locales → Nube**: Las ventas, productos y clientes creados offline se suben automaticamente
2. **Cambios de la nube → Local**: Los cambios hechos en la web se descargan al escritorio

## Configuracion de Supabase

Editar `appsettings.json` con las credenciales de tu proyecto Supabase:

```json
{
  "Supabase": {
    "Url": "https://tu-proyecto.supabase.co",
    "AnonKey": "tu-anon-key"
  }
}
```

## Atajos de Teclado (POS)

- **F1**: Pago en Efectivo
- **F2**: Pago con Tarjeta
- **ESC**: Limpiar Carrito
- **Enter**: Buscar producto por codigo
- **+/-**: Aumentar/Disminuir cantidad
- **Delete**: Eliminar producto del carrito

## Base de Datos Local

La base de datos SQLite se guarda en:
```
%LOCALAPPDATA%\SalvadoreX\salvadorex_local.db
```

## Compilar para Produccion

```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

El ejecutable se generara en:
```
bin\Release\net8.0-windows\win-x64\publish\
```

## Licencia

Propietario - SalvadoreX
