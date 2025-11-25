# Research: Dashboard Layout Changes

**Feature**: 003-dashboard-layout  
**Date**: 2025-01-27

## Research Questions

### 1. How to handle empty HTTP responses (204 No Content) in fetch API?

**Context**: The delete goal endpoint returns HTTP 204 No Content with an empty response body. The current `api.ts` implementation always calls `response.json()`, which fails when the response body is empty.

**Research Findings**:

**Decision**: Check response status and content type before attempting JSON parsing. For 204 No Content responses, skip JSON parsing entirely.

**Rationale**: 
- HTTP 204 No Content is a standard response for successful DELETE operations
- Attempting to parse JSON on an empty response body causes "Unexpected end of JSON input" error
- The fix is simple: check if response has content before parsing

**Implementation Approach**:
```typescript
// Check if response has content before parsing
if (response.status === 204 || response.headers.get('content-length') === '0') {
  return {} as ApiResponse<T>; // Return empty response
}
// Only parse JSON if response has content
const contentType = response.headers.get('content-type');
if (contentType && contentType.includes('application/json')) {
  return await response.json();
}
```

**Alternatives Considered**:
- Always return void for DELETE operations - Rejected: Inconsistent with other API methods that return ApiResponse
- Use response.text() then parse - Rejected: More complex, unnecessary for this use case
- Change backend to return 200 with empty JSON - Rejected: 204 is the correct HTTP status for DELETE

**References**:
- MDN: https://developer.mozilla.org/en-US/docs/Web/API/Response/json
- HTTP Status Codes: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/204

---

### 2. How to force light mode in CSS regardless of browser dark mode preference?

**Context**: The dashboard must always display in light mode, even when the user's browser is set to dark mode.

**Research Findings**:

**Decision**: Use CSS `color-scheme` property with `:root` selector to force light mode, and override any dark mode media queries.

**Rationale**:
- `color-scheme: light` tells the browser to use light mode color palette
- Overriding `@media (prefers-color-scheme: dark)` ensures dark mode styles never apply
- CSS-only solution is simple and performant
- Works with DaisyUI theme system

**Implementation Approach**:
```css
:root {
  color-scheme: light;
}

/* Override any dark mode media queries */
@media (prefers-color-scheme: dark) {
  :root {
    color-scheme: light;
    /* Force light mode colors */
  }
}
```

**Alternatives Considered**:
- JavaScript-based theme switching - Rejected: More complex, requires runtime logic, potential flash
- CSS custom properties with forced values - Rejected: More verbose, harder to maintain
- DaisyUI data-theme attribute - Rejected: Still respects browser preference by default

**References**:
- MDN: https://developer.mozilla.org/en-US/docs/Web/CSS/color-scheme
- Tailwind CSS: https://tailwindcss.com/docs/dark-mode

---

### 3. Tailwind CSS layout patterns for horizontal card layouts

**Context**: Need to arrange cards horizontally: Team Statistics full-width at top, Add Goal and Update Mood side-by-side below, with responsive stacking on mobile.

**Research Findings**:

**Decision**: Use Tailwind CSS grid utilities (`grid`, `grid-cols-1`, `md:grid-cols-2`) for responsive side-by-side layout, and `w-full` for full-width cards.

**Rationale**:
- Tailwind grid utilities provide clean, responsive layouts
- `grid-cols-1` for mobile (stacked), `md:grid-cols-2` for desktop (side-by-side)
- `w-full` ensures full-width cards
- Consistent with existing Tailwind usage in the project

**Implementation Approach**:
```html
<!-- Team Statistics: Full width -->
<div class="w-full">
  <StatsPanel />
</div>

<!-- Add Goal & Update Mood: Side-by-side on desktop, stacked on mobile -->
<div class="grid grid-cols-1 md:grid-cols-2 gap-4">
  <GoalForm />
  <MoodForm />
</div>
```

**Alternatives Considered**:
- Flexbox (`flex flex-row`) - Rejected: Less responsive, requires more breakpoint management
- CSS Grid with explicit columns - Rejected: More verbose, Tailwind utilities are cleaner
- Custom CSS classes - Rejected: Violates "Do Not Overdesign" principle, Tailwind utilities sufficient

**References**:
- Tailwind CSS Grid: https://tailwindcss.com/docs/grid-template-columns
- Tailwind CSS Responsive Design: https://tailwindcss.com/docs/responsive-design

---

### 4. DaisyUI button color customization for light mode

**Context**: Need to apply lighter button colors that are already defined in the design system.

**Research Findings**:

**Decision**: Use DaisyUI's existing light mode button classes (`btn-primary`, `btn-secondary`, etc.) which automatically use light mode colors when `color-scheme: light` is set.

**Rationale**:
- DaisyUI buttons automatically adapt to `color-scheme` setting
- No custom CSS needed if design system colors are already defined
- Consistent with DaisyUI component library usage
- Maintains accessibility contrast automatically

**Implementation Approach**:
- Ensure `color-scheme: light` is set (from research #2)
- Use existing DaisyUI button classes (`btn-primary`, `btn-outline`, etc.)
- DaisyUI will automatically use light mode colors

**Alternatives Considered**:
- Custom CSS button styles - Rejected: Unnecessary, DaisyUI already provides light mode colors
- CSS custom properties - Rejected: More complex, DaisyUI handles this automatically
- JavaScript-based color switching - Rejected: Overengineered, CSS solution is sufficient

**References**:
- DaisyUI Buttons: https://daisyui.com/components/button/
- DaisyUI Themes: https://daisyui.com/docs/themes/

---

## Summary

All research questions resolved. No blocking unknowns remain. Implementation approach is straightforward:

1. **Delete Goal Bug Fix**: Check response status/content-type before JSON parsing
2. **Light Mode Enforcement**: CSS `color-scheme: light` with media query override
3. **Layout Changes**: Tailwind CSS grid utilities for responsive horizontal layouts
4. **Button Colors**: DaisyUI automatic light mode colors via `color-scheme`

All solutions are simple, maintainable, and align with existing project patterns.

