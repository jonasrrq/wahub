# Issue #6 - Campañas, programar envíos masivos

## Descripción

Resumen: Crear el dashboard y la tabla para el manejo de campañas, asociando cada campaña a una instancia y a una lista de contactos.

Contexto: En WaHub (ver https://uithub.com/jonasrrq/wahub), la gestión de campañas es fundamental para la funcionalidad de envíos masivos. Es necesario que cada campaña esté vinculada a una instancia y a una lista de contactos para una administración eficiente y segmentada.

Qué se debe modificar:
- Modelos y migraciones para la tabla de campañas y sus relaciones con instancias y listas de contactos.
- Lógica de negocio para la programación y gestión de campañas.
- Interfaces de usuario para administración y programación de campañas.
- Documentación técnica y de usuario.

Criterios de aceptación:
- Existe una tabla de campañas asociada a instancias y listas de contactos.
- Se pueden crear, editar, eliminar y programar campañas.
- Pruebas automatizadas (unitarias y/o de integración) implementadas y documentadas para validar la gestión y programación de campañas.

## TODO List

- [ ] Definir modelos y migraciones para campañas y relaciones.
- [ ] Implementar lógica de negocio para gestión y programación de campañas.
- [ ] Actualizar interfaces de usuario para administración de campañas.
- [ ] Documentar cambios en la arquitectura y uso.
- [ ] Implementar y documentar pruebas automatizadas.
- [ ] Validar funcionamiento y cobertura de pruebas.

## Development Notes

### 2025-06-18 - Inicio de la issue

- Se consultó el contexto global en https://uithub.com/jonasrrq/wahub.
- Se documentará el enfoque de testing y los archivos de prueba creados/modificados en esta sección.
