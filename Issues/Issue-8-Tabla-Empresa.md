# Issue #8 - Tabla Empresa

## Descripción

Resumen: Crear la entidad y tabla para empresas, asociando cada empresa a uno o varios usuarios según la arquitectura de WaHub.

Contexto: En WaHub (ver https://uithub.com/jonasrrq/wahub), la gestión de empresas es clave para la organización de usuarios y recursos. Cada empresa debe poder tener múltiples usuarios asociados, permitiendo una administración flexible y escalable.

Qué se debe modificar:
- Modelos y migraciones para la tabla de empresas y su relación con usuarios.
- Lógica de negocio para la gestión de empresas y usuarios asociados.
- Interfaces de usuario para administración de empresas.
- Documentación técnica y de usuario.

Criterios de aceptación:
- Existe una tabla de empresas asociada a usuarios.
- Se pueden crear, editar y eliminar empresas y asociar usuarios.
- Pruebas automatizadas (unitarias y/o de integración) implementadas y documentadas para validar la gestión de empresas y usuarios.

## TODO List

- [ ] Definir modelos y migraciones para empresas y relaciones con usuarios.
- [ ] Implementar lógica de negocio para gestión de empresas y usuarios.
- [ ] Actualizar interfaces de usuario para administración de empresas.
- [ ] Documentar cambios en la arquitectura y uso.
- [ ] Implementar y documentar pruebas automatizadas.
- [ ] Validar funcionamiento y cobertura de pruebas.

## Development Notes

### 2025-06-18 - Inicio de la issue

- Se consultó el contexto global en https://uithub.com/jonasrrq/wahub.
- Se documentará el enfoque de testing y los archivos de prueba creados/modificados en esta sección.
