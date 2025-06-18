# Issue #11 - Dividir archivos de recursos por componentes

## Descripción
Resumen: Separar los archivos de recursos de idiomas en archivos individuales por componente para mejorar la legibilidad y el mantenimiento del código.
Contexto: Actualmente, los recursos de idiomas están agrupados, lo que dificulta la localización y el mantenimiento. Separar los recursos por componente permitirá una gestión más eficiente y escalable, alineada con la arquitectura modular de WaHub (ver https://uithub.com/jonasrrq/wahub para contexto global).
Qué se debe modificar:
- Archivos de recursos de idiomas existentes en la carpeta `src/WaHub/Resources/` (o donde se encuentren actualmente).
- Componentes que consumen recursos de idioma, para que apunten a los nuevos archivos separados.
- Actualizar documentación si es necesario.
Criterios de aceptación:
- Cada componente tiene su propio archivo de recursos de idioma.
- El sistema de internacionalización sigue funcionando correctamente.
- La documentación refleja la nueva estructura.
- Se implementan y documentan pruebas automatizadas (unitarias y/o de integración) para validar la carga y uso de los recursos por componente.

## TODO List
- [ ] Identificar todos los archivos de recursos de idiomas existentes.
- [ ] Crear archivos de recursos individuales por componente.
- [ ] Actualizar los componentes para consumir los nuevos archivos.
- [ ] Actualizar la documentación relacionada.
- [ ] Implementar pruebas automatizadas para la carga y uso de recursos por componente.
- [ ] Validar el funcionamiento y cobertura de pruebas.

## Development Notes
### 2025-06-18 - Inicio de la issue
- Se sigue la instrucción de consultar el contexto global en https://uithub.com/jonasrrq/wahub.
- Se documentará el enfoque de testing y los archivos de prueba creados/modificados en esta sección.
