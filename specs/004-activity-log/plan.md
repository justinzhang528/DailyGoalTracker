# Implementation Plan: Activity Log

**Branch**: `004-activity-log` | **Date**: 2025-01-27 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/004-activity-log/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Add an activity log page that displays all dashboard operations (goal additions, mood updates, goal completions, goal deletions) in chronological order. Activity records are stored in SQLite database and displayed on a dedicated frontend page accessible from the dashboard. Implemented using TDD (Test-Driven Development) approach with Vue 3 + TypeScript frontend and .NET 8 Web API backend.

## Technical Context

**Language/Version**: 
- Frontend: TypeScript 5.x, Vue 3.x
- Backend: C# (.NET 8)

**Primary Dependencies**: 
- Frontend: Vue 3, DaisyUI (Tailwind CSS component library), TypeScript
- Backend: .NET 8 Web API, Dapper ORM, Microsoft.Data.Sqlite

**Storage**: SQLite (same database as existing goals and team members)

**Testing**: 
- Frontend: Vitest (Vue 3 testing), Vue Test Utils
- Backend: xUnit (.NET testing framework)
- **TDD Approach**: Tests written before implementation

**Target Platform**: Desktop web browsers (Chrome, Firefox, Safari, Edge)

**Project Type**: web (frontend + backend)

**Performance Goals**: 
- Activity log page load: <2 seconds for up to 1000 records (SC-002)
- API response time: <200ms p95 for activity log endpoint
- Activity recording: non-blocking (best-effort, does not impact original operation performance)

**Constraints**: 
- Desktop-only (no mobile responsive design)
- SQLite file-based database (local deployment)
- Activity recording is best-effort (failures don't block original operations)
- No retroactive recording (only new operations after feature deployment)

**Scale/Scope**: 
- Small team (5-20 team members expected)
- Up to 1000 activity records displayed (performance target)
- Single SQLite database (shared with goals and team members)

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### I. Code Quality (MUST)
- ✅ **Readability**: TypeScript strict mode ensures type safety and code quality
- ✅ **Maintainability**: Vue 3 Composition API promotes clean, maintainable component structure
- ✅ **Consistency**: Follows existing project patterns (Dapper, repository pattern)
- ✅ **Error handling**: Best-effort activity recording with proper error handling
- ✅ **No technical debt**: Simple SQLite table structure, no complex abstractions

### II. Testing Standards (MUST)
- ✅ **Unit tests**: xUnit for backend, Vitest for frontend
- ✅ **Integration tests**: API integration tests for activity recording and retrieval
- ✅ **TDD approach**: Tests written before implementation (as specified by user)
- ✅ **Test maintainability**: Clear test structure following existing patterns
- ✅ **Test failures block deployment**: All tests must pass

### III. User Experience Consistency (MUST)
- ✅ **UI/UX patterns**: DaisyUI components for consistent styling
- ✅ **Error messages**: User-friendly error messages with retry option (FR-014)
- ✅ **Loading states**: Loading indicators during API calls
- ✅ **Accessibility**: Basic accessibility (keyboard navigation, ARIA labels)
- ✅ **Responsive design**: Desktop-only (consistent with existing dashboard)

### IV. Performance Requirements (MUST)
- ✅ **Performance goals**: <2 seconds for 1000 records (SC-002)
- ✅ **Performance regressions**: Activity recording is non-blocking (no impact on original operations)
- ✅ **Performance testing**: Load testing for activity log endpoint

### V. Minimum Viable Product (MVP) First (MUST)
- ✅ **MVP delivery**: P1 user stories (View Activity Log, Navigate to Activity Log) deliver standalone value
- ✅ **Incremental delivery**: Can be implemented and tested independently
- ✅ **Complete increments**: Each user story is independently testable

### VI. Do Not Overdesign (MUST)
- ✅ **Simplicity**: Simple SQLite table for activity records
- ✅ **No premature optimization**: No pagination/filtering initially (out of scope)
- ✅ **Justified complexity**: Minimal changes to existing codebase
- ✅ **Current problems**: Solves actual need for activity visibility

**Gate Status**: ✅ **PASS** - All constitution principles satisfied

**Post-Phase 1 Re-evaluation**: ✅ **PASS** - Design artifacts confirm simple, straightforward implementation. No complexity violations introduced.

## Project Structure

### Documentation (this feature)

```text
specs/004-activity-log/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
backend/
├── src/
│   ├── DailyGoalTracker.Api/
│   │   ├── Controllers/
│   │   │   └── ActivitiesController.cs    # New controller for activity log
│   │   ├── Models/
│   │   │   └── ActivityRecord.cs         # New model for activity records
│   │   ├── Services/
│   │   │   └── ActivityService.cs        # New service for activity business logic
│   │   ├── Data/
│   │   │   ├── Dapper/
│   │   │   │   └── ActivityRepository.cs # New repository for activity data access
│   │   │   └── Database/
│   │   │       └── schema.sql            # Updated with ActivityRecords table
│   │   └── Program.cs                    # Updated to record activities
│   └── DailyGoalTracker.Api.Tests/
│       ├── Unit/
│       │   └── ActivityServiceTests.cs   # Unit tests for activity service
│       └── Integration/
│           └── ActivitiesControllerTests.cs # Integration tests for API

frontend/
├── src/
│   ├── components/
│   │   └── ActivityLog.vue                # New component for activity log page
│   ├── views/
│   │   └── ActivityLogView.vue            # New view for activity log route
│   ├── composables/
│   │   └── useActivities.ts              # New composable for activity state management
│   ├── services/
│   │   └── api.ts                        # Updated with activity API methods
│   ├── types/
│   │   └── index.ts                      # Updated with ActivityRecord type
│   └── router/
│       └── index.ts                      # Updated with activity log route
└── tests/
    ├── unit/
    │   └── ActivityLog.test.ts            # Unit tests for activity log component
    └── integration/
        └── ActivityLog.integration.test.ts # Integration tests for activity log
```

**Structure Decision**: Web application structure with frontend and backend changes. This feature extends the existing dashboard application by adding a new page, API endpoint, and database table. Activity recording is integrated into existing controllers (GoalsController, TeamMembersController) without major refactoring.

## Complexity Tracking

No violations - this is a straightforward feature addition following existing patterns. Activity recording uses best-effort approach to maintain simplicity.
