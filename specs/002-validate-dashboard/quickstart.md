# Quickstart: Validate Frontend Dashboard Setup

**Feature**: Validate Frontend Dashboard Setup  
**Date**: 2025-01-27

## Prerequisites

- Node.js and npm installed
- Frontend project directory accessible
- Terminal access

## Setup

1. Navigate to frontend directory:
   ```bash
   cd frontend
   ```

2. Verify current dependencies:
   ```bash
   npm list tailwindcss daisyui
   ```

## Validation Steps

### Step 1: Check Package Dependencies

Check `package.json` for required packages:

```bash
grep -E "(@tailwindcss/vite|tailwindcss|daisyui)" package.json
```

**Expected**: All three packages should be listed in `devDependencies`.

**If missing `@tailwindcss/vite`**:
```bash
npm install -D @tailwindcss/vite
```

### Step 2: Check Vite Configuration

Check `vite.config.ts` for Tailwind plugin:

```bash
cat vite.config.ts
```

**Expected**: Should include:
```typescript
import tailwindcss from '@tailwindcss/vite'

export default defineConfig({
  plugins: [
    vue(),
    tailwindcss(),  // Should be present
  ],
})
```

**If missing**: Add the import and plugin to `vite.config.ts`.

### Step 3: Check CSS Configuration

Check `src/style.css` for required directives:

```bash
head -5 src/style.css
```

**Expected**: Should include:
```css
@import "tailwindcss";
@plugin "daisyui";
```

**If missing `@plugin "daisyui";`**: Add it after the Tailwind import.

### Step 4: Validate Build Process

Run the build to verify compilation:

```bash
npm run build
```

**Expected**: Build completes successfully without CSS errors.

**If errors occur**: Review error messages and fix configuration issues.

### Step 5: Validate Visual Rendering

Start the development server:

```bash
npm run dev
```

Open the dashboard in a browser and verify:

1. **Tailwind Utilities**: Check that classes like `container`, `mx-auto`, `p-6`, `text-4xl` apply correctly
2. **DaisyUI Components**: Check that classes like `btn`, `btn-primary`, `card`, `card-body`, `select`, `select-bordered` render with proper styling
3. **Overall Layout**: Verify dashboard components display with proper spacing, colors, and visual hierarchy

## Validation Checklist

- [ ] `@tailwindcss/vite` package installed in package.json
- [ ] `@tailwindcss/vite` plugin added to vite.config.ts
- [ ] `@import "tailwindcss";` present in style.css
- [ ] `@plugin "daisyui";` present in style.css
- [ ] `npm run build` completes successfully
- [ ] `npm run dev` starts without errors
- [ ] Dashboard displays with correct Tailwind styling
- [ ] Dashboard displays with correct DaisyUI component styling

## Troubleshooting

### Build Fails with CSS Errors

**Symptoms**: Build process fails with CSS compilation errors

**Solutions**:
1. Verify all packages are installed: `npm install`
2. Check vite.config.ts plugin configuration
3. Verify CSS import/plugin directives are correct
4. Clear node_modules and reinstall: `rm -rf node_modules package-lock.json && npm install`

### Styles Not Applying

**Symptoms**: Components render but without Tailwind/DaisyUI styling

**Solutions**:
1. Verify CSS file is imported in main.ts or App.vue
2. Check browser console for CSS loading errors
3. Verify build output includes compiled CSS
4. Check that class names match Tailwind/DaisyUI conventions

### Plugin Not Found Error

**Symptoms**: Vite reports `@tailwindcss/vite` plugin not found

**Solutions**:
1. Verify package is installed: `npm list @tailwindcss/vite`
2. Check import statement in vite.config.ts
3. Restart dev server after installing package

## Success Criteria

Validation is successful when:
- ✅ All configuration files are correct
- ✅ Build process completes without errors
- ✅ Dashboard renders with proper Tailwind utility class styling
- ✅ Dashboard renders with proper DaisyUI component class styling
- ✅ All components display with correct visual appearance

