# Implementation Plan: Dashboard Layout Changes

**Branch**: `003-dashboard-layout` | **Date**: 2025-01-27 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/003-dashboard-layout/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Dashboard layout improvements and bug fix: Force light mode display, reorganize card layout (Team Statistics full-width at top, Add Goal and Update Mood side-by-side below), apply lighter button colors, and fix delete goal JSON parsing error. Frontend-only changes using Vue 3 + Tailwind CSS + DaisyUI. No backend changes required.

## Technical Context

**Language/Version**: 
- Frontend: TypeScript 5.x, Vue 3.x
- Backend: C# (.NET 8) - no changes required

**Primary Dependencies**: 
- Frontend: Vue 3, DaisyUI (Tailwind CSS component library), TypeScript, Tailwind CSS v4
- Backend: .NET 8 Web API (no changes)

**Storage**: SQLite (no changes)

**Testing**: 
- Frontend: Vitest (Vue 3 testing), Vue Test Utils
- Backend: No new tests required (bug fix only)

**Target Platform**: Desktop web browsers (Chrome, Firefox, Safari, Edge)

**Project Type**: web (frontend changes only)

**Performance Goals**: 
- Dashboard layout changes do not increase page load time by more than 10% (SC-006)
- All primary actions complete in same or less time than previous layout (SC-007)
- No performance regressions

**Constraints**: 
- Must maintain existing functionality (add goals, update mood, toggle completion)
- Must preserve responsive behavior for mobile devices
- Light mode enforcement must work regardless of browser settings
- Button colors must maintain accessibility contrast standards (4.5:1 minimum)

**Scale/Scope**: 
- Frontend UI/UX changes only
- No data model changes
- No API contract changes
- Single dashboard view modification

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Code Quality (MUST) ✅
- Vue 3 Composition API ensures maintainable component structure
- TypeScript provides type safety for layout changes
- Tailwind CSS utility classes promote consistent styling
- Existing code patterns will be followed

### Testing Standards (MUST) ✅
- Component tests for layout changes using Vitest + Vue Test Utils
- Visual regression testing for layout changes
- Integration tests for delete goal bug fix
- No new backend tests required (bug fix only)

### User Experience Consistency (MUST) ✅
- DaisyUI component library ensures consistent UI patterns
- Light mode enforcement provides consistent visual experience
- Responsive design maintained for mobile devices
- Accessibility standards maintained (contrast ratios)

### Performance Requirements (MUST) ✅
- Performance targets defined in spec (SC-006, SC-007)
- Layout changes must not degrade performance
- CSS-only changes (no JavaScript performance impact expected)

### MVP First (MUST) ✅
- P1 user story (Fix Delete Goal Error) delivers immediate bug fix value
- P2 user stories (Light Mode, Layout Reorganization) enhance UX
- P3 user story (Button Colors) provides polish
- Each story independently testable and deployable

### Do Not Overdesign (MUST) ✅
- CSS-only solution for light mode enforcement (no complex theme system)
- Tailwind CSS utility classes for layout (no custom CSS framework)
- Reuse existing DaisyUI components and design system colors
- Simple error handling fix (handle empty response body)

**Status**: ✅ All gates pass. No violations detected.

## Project Structure

### Documentation (this feature)

```text
specs/003-dashboard-layout/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command) - N/A (no data changes)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command) - N/A (no API changes)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
frontend/
├── src/
│   ├── components/
│   │   ├── StatsPanel.vue          # Modify: horizontal layout, full-width
│   │   ├── GoalForm.vue            # Modify: side-by-side layout
│   │   ├── MoodForm.vue            # Modify: side-by-side layout
│   │   └── TeamMemberCard.vue      # Modify: error display location
│   ├── views/
│   │   └── Dashboard.vue           # Modify: layout structure
│   ├── services/
│   │   └── api.ts                   # Modify: fix delete goal JSON parsing
│   ├── composables/
│   │   └── useGoals.ts              # Modify: error handling
│   ├── style.css                    # Modify: force light mode
│   └── App.vue                      # Modify: light mode enforcement

tests/
├── unit/
│   └── components/                  # Tests for layout changes
└── integration/                     # Tests for delete goal fix
```

**Structure Decision**: Frontend-only changes. Existing structure maintained. No new directories or files required.

## Complexity Tracking

> **No violations detected - all gates pass**

## Phase Completion Summary

### Phase 0: Research ✅

**Output**: `research.md`

**Research Topics Resolved**:
1. ✅ HTTP 204 No Content response handling in fetch API
2. ✅ CSS approach to force light mode regardless of browser preference
3. ✅ Tailwind CSS layout patterns for horizontal card arrangements
4. ✅ DaisyUI button color customization for light mode

**Status**: All research questions resolved. No blocking unknowns.

### Phase 1: Design & Contracts ✅

**Outputs**:
- ✅ `data-model.md` - Documented no data model changes required
- ✅ `contracts/README.md` - Documented no API contract changes required
- ✅ `quickstart.md` - Implementation guide with code examples
- ✅ Agent context updated (`.cursor/rules/specify-rules.mdc`)

**Design Decisions**:
- Frontend-only implementation (no backend changes)
- CSS-only light mode enforcement (no JavaScript theme switching)
- Tailwind CSS grid utilities for responsive layouts
- DaisyUI automatic light mode colors via `color-scheme`
- Simple response handling fix (check status before JSON parsing)

**Status**: Phase 1 complete. Ready for Phase 2 (task breakdown via `/speckit.tasks`).

## Next Steps

1. Run `/speckit.tasks` to generate task breakdown
2. Implement P1 user story (Fix Delete Goal Error)
3. Implement P2 user stories (Light Mode, Layout Reorganization)
4. Implement P3 user story (Button Colors)
5. Run tests and validate against success criteria
