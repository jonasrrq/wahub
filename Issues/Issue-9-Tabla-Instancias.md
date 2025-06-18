# Issue #9 - Tabla Instancias

## Descripción

Resumen: Crear una tabla de instancias asociadas a empresas, permitiendo que una empresa tenga múltiples instancias y gestionando el acceso de usuarios a dichas instancias.

Contexto: Según la arquitectura de WaHub (ver https://uithub.com/jonasrrq/wahub), la gestión de empresas y sus instancias es fundamental para la administración de recursos y usuarios. Se requiere una relación de muchos a muchos entre empresas e instancias, y un control granular de acceso de usuarios a instancias.

Qué se debe modificar:
- Modelos y migraciones para la nueva tabla de instancias y la relación muchos a muchos con empresas.
- Lógica de negocio para gestionar accesos de usuarios a instancias.
- Interfaces de usuario para administración de instancias y asignación de usuarios.
- Actualizar documentación técnica y de usuario.

Criterios de aceptación:
- Existe una tabla de instancias asociada a empresas.
- Se implementa la relación muchos a muchos entre empresas e instancias.
- El usuario principal de la empresa tiene acceso a todas las instancias; usuarios secundarios pueden tener acceso limitado.
- Pruebas automatizadas (unitarias y/o de integración) implementadas y documentadas para validar la gestión de instancias y accesos.

## TODO List

- [ ] Definir modelos y migraciones para instancias y relaciones.
- [ ] Implementar lógica de negocio para gestión de accesos.
- [ ] Actualizar interfaces de usuario para administración de instancias.
- [ ] Documentar cambios en la arquitectura y uso.
- [ ] Implementar y documentar pruebas automatizadas.
- [ ] Validar funcionamiento y cobertura de pruebas.

## Development Notes

### 2025-06-18 - Inicio de la issue

- Se consultó el contexto global en https://uithub.com/jonasrrq/wahub.
- Se documentará el enfoque de testing y los archivos de prueba creados/modificados en esta sección.
