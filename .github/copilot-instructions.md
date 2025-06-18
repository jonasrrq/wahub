## Issues
- If you find a bug or have a feature request, please open an issue in the repository. Provide as much detail as possible, including steps to reproduce the issue, expected behavior, and any relevant screenshots or logs.

- Al asignarse una issue, seguir estos pasos:
  1. Actualizar la rama master local:
     ```bash
     git checkout master
     git pull origin master
     ```
  2. Crear una nueva rama desde master para la issue:
     ```bash
     git checkout -b issue-[number]-[brief-title]
     ```
  3. Proceder con el desarrollo siguiendo los pasos a continuación

- For each issue in development, create a markdown file in the `/Issues` folder at the root of the project with the following name format: `Issue-[number]-[brief-title].md` using the following structure:
  ```markdown
  # Issue #[number] - [title]
  
  ## TODO List
  - [ ] Task 1 (Orden cronológico de implementación)
  - [ ] Task 2
  - [ ] Task 3
  (Marcar las tareas con [x] cuando se completen)
  
  ## Development Notes
  ### [date] - [brief description]
  - Detailed notes about implementation decisions
  - Problems encountered and solutions
  - Code changes and their rationale
  - References to related documentation
  ```

- Utilizar el archivo markdown de cada Issue para:
  1. Mantener una lista ordenada de tareas (TODO List) que refleje el orden específico en que deben implementarse
  2. Marcar el progreso actualizando las tareas completadas con [x]
  3. Documentar decisiones de implementación, problemas encontrados y soluciones aplicadas
  4. Registrar cambios significativos en el código y su justificación
  5. Mantener referencias a documentación relevante

---

## Descripción de Issues basada en el título

Para mantener la documentación y el contexto de las issues de manera consistente en el proyecto, sigue estos pasos para agregar la descripción de cada issue:

1. **Antes de redactar la descripción de la issue, consulta el siguiente enlace para obtener el contexto global y actualizado de toda la aplicación:** [https://uithub.com/jonasrrq/wahub](https://uithub.com/jonasrrq/wahub)
   - Este enlace contiene información esencial sobre la arquitectura, módulos, flujos y propósito general del proyecto WaHub.
   - Es obligatorio revisarlo para asegurar que la descripción de la issue sea precisa y relevante.
2. En base a ese contexto, redacta una descripción clara de lo que se debe hacer, especificando qué módulos, archivos o componentes se deben tocar o modificar para resolver la tarea, tomando como base el título de la issue.
3. Si el título no es suficientemente descriptivo, complementa con detalles relevantes sobre el objetivo, alcance y criterios de aceptación.

**Estructura recomendada:**
- Resumen: Explica brevemente el propósito de la issue.
- Contexto: Relaciona la issue con el flujo general del proyecto o módulos afectados.
- Qué se debe modificar: Enumera los archivos, módulos o componentes que probablemente deban ser modificados.
- Criterios de aceptación: Enumera los resultados esperados o condiciones para considerar la issue resuelta.

**Ejemplo:**

```markdown
# Issue #[número] - [Título]

## Descripción
Resumen: [Breve explicación de la tarea]
Contexto: [Cómo se relaciona con el proyecto]
Qué se debe modificar: [Lista de archivos, módulos o componentes]
Criterios de aceptación: [Resultados esperados]
```

- Mantén la descripción actualizada si la issue evoluciona.
- Estas instrucciones ayudan a mantener la trazabilidad y claridad en la gestión de issues del proyecto.

## Requerimiento de Pruebas Automatizadas

- **Toda issue que implique desarrollo de nuevas funcionalidades, corrección de bugs o refactorización debe incluir la especificación, implementación y documentación de pruebas automatizadas relevantes (unitarias y/o de integración) para validar el correcto funcionamiento de los cambios realizados.**
- En la descripción de la issue, especifica qué tipo de pruebas se deben implementar y los criterios de aceptación asociados a dichas pruebas.
- Documenta en el archivo markdown de la issue el enfoque de testing, los archivos de test creados o modificados, y los resultados esperados.
- No se considerará resuelta una issue hasta que las pruebas estén implementadas y documentadas.


# WaHub Theming Guidelines for New Components

When developing new UI components for WaHub, follow these guidelines to ensure proper integration with the existing light/dark theme system:

1.  **Use CSS Variables Exclusively for Colors & Themeable Attributes:**
    *   **Never use hardcoded colors** (e.g., `#FFF`, `black`, `rgb(0,0,0)`).
    *   Utilize the global `--wh-` prefixed CSS variables defined in `src/WaHub/wwwroot/css/components.css`.
    *   **Key Variable Categories:**
        *   Backgrounds: `var(--wh-bg-primary)`, `var(--wh-bg-secondary)`, `var(--wh-card-bg)`.
        *   Text: `var(--wh-text-primary)`, `var(--wh-text-secondary)`, `var(--wh-text-accent)`, `var(--wh-text-on-primary-bg)`.
        *   Borders: `var(--wh-border-primary)`, `var(--wh-border-secondary)`.
        *   Buttons: `var(--wh-btn-primary-bg)`, `var(--wh-btn-primary-text)`, etc. (and for secondary, success, warning, danger variants if available).
        *   Inputs: `var(--wh-input-bg)`, `var(--wh-input-text)`, `var(--wh-input-border)`.
        *   Semantic States (for alerts, badges, etc.): `var(--wh-success-bg)`, `var(--wh-success-text)`, etc.
        *   Shadows: `var(--wh-shadow-sm)`, `var(--wh-shadow-md)`.
        *   Accents: `var(--wh-accent-color)`.

2.  **Component-Specific CSS:**
    *   If the component requires unique styling, create a corresponding `.razor.css` file (e.g., `MyComponent.razor.css` for `MyComponent.razor`).
    *   Within this file, apply the `--wh-` variables. Avoid defining new local color variables unless absolutely necessary for a very specific, non-reusable purpose.

3.  **Interactive States:**
    *   Ensure `:hover`, `:focus`, `:active`, and `:disabled` states are styled using theme variables.
    *   Example: `background-color: var(--wh-btn-primary-hover-bg);`

4.  **Transitions:**
    *   Add CSS `transition` properties for themeable attributes (`background-color`, `color`, `border-color`, `box-shadow`) to ensure smooth visual changes when the theme toggles.
    *   Example: `transition: background-color 0.3s ease, color 0.3s ease;`
    *   The global `theme-transition` class on `document.documentElement` (added by `app.js`) helps manage initial load transitions.

5.  **JavaScript Interaction (If Any):**
    *   The theme is primarily controlled by the `data-theme` attribute on `document.documentElement`.
    *   If JS needs to be aware of the theme, it can read `window.waHubApp.theme.current`.
    *   To toggle the theme, use the existing `ThemeToggle.razor` component or, if imperative JS control is needed (rarely for components), invoke `window.waHubApp.theme.toggle()`.

6.  **Accessibility & Contrast:**
    *   When choosing variables, be mindful of text contrast against backgrounds. The defined theme variables aim for good contrast, but verify for your specific component.

7.  **Testing:**
    *   Test your new component in both **light** and **dark** modes.
    *   Verify all interactive states.

**Example Snippet (`MyComponent.razor.css`):**
```css
/* MyComponent.razor.css */
.my-component-card {
    background-color: var(--wh-card-bg);
    border: 1px solid var(--wh-border-primary);
    color: var(--wh-text-primary);
    padding: var(--wh-spacing-medium, 1rem); /* Assuming spacing variables exist or use fixed values */
    transition: background-color 0.3s ease, border-color 0.3s ease;
}

.my-component-button {
    background-color: var(--wh-btn-primary-bg);
    color: var(--wh-btn-primary-text);
    /* ... other button styles ... */
    transition: background-color 0.3s ease;
}

.my-component-button:hover {
    background-color: var(--wh-btn-primary-hover-bg);
}