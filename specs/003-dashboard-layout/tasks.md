# Tasks: Dashboard Layout Changes

**Input**: Design documents from `/specs/003-dashboard-layout/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are included per constitution requirements (Testing Standards MUST).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Web app**: `backend/src/`, `frontend/src/`
- Backend: .NET 8 Web API (no changes required)
- Frontend: Vue 3 + TypeScript with DaisyUI

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: No setup required - frontend-only feature using existing infrastructure

**Note**: All infrastructure already exists. Skip to Phase 2 (User Stories).

---

## Phase 2: User Story 1 - Fix Delete Goal Error (Priority: P1) 🎯 MVP

**Goal**: Fix JSON parsing error when deleting goals. Users can delete goals without encountering "Failed to execute 'json' on 'Response': Unexpected end of JSON input" error.

**Independent Test**: Attempt to delete a goal and verify it is removed without error messages. Works independently of other user stories.

### Tests for User Story 1

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T001 [P] [US1] Unit test for deleteGoal API method handling 204 response in frontend/tests/unit/services/api.test.ts
- [x] T002 [P] [US1] Integration test for delete goal error handling in frontend/tests/integration/goals.test.ts
- [x] T003 [P] [US1] Unit test for error display in TeamMemberCard component in frontend/tests/unit/components/TeamMemberCard.test.ts
- [x] T038 [P] [US1] Integration test verifying stats refresh after goal deletion in frontend/tests/integration/stats.test.ts

### Implementation for User Story 1

- [x] T004 [US1] Fix deleteGoal method to handle HTTP 204 No Content response in frontend/src/services/api.ts
- [x] T005 [US1] Update request method to check response status before JSON parsing in frontend/src/services/api.ts
- [x] T006 [US1] Add error state management in TeamMemberCard component in frontend/src/components/TeamMemberCard.vue
- [x] T007 [US1] Display error messages within team member card on delete failure in frontend/src/components/TeamMemberCard.vue
- [x] T008 [US1] Update useGoals composable error handling for delete operations in frontend/src/composables/useGoals.ts
- [x] T039 [US1] Ensure handleDeleteGoal refreshes stats after deletion in frontend/src/views/Dashboard.vue (reload stats and update UI)

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently. Delete goal works without JSON parsing errors, and errors display within the team member card.

---

## Phase 3: User Story 2 - Force Light Mode Display (Priority: P2)

**Goal**: Dashboard always displays in light mode regardless of browser dark mode setting. Provides consistent visual experience across all users.

**Independent Test**: Enable dark mode in browser settings and verify dashboard remains in light mode. Works independently of other user stories.

### Tests for User Story 2

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T009 [P] [US2] Visual regression test for light mode enforcement in frontend/tests/integration/theme.test.ts
- [x] T010 [P] [US2] Unit test for color-scheme CSS property in frontend/tests/unit/styles/theme.test.ts

### Implementation for User Story 2

- [x] T011 [US2] Add color-scheme: light to :root selector in frontend/src/style.css
- [x] T012 [US2] Override prefers-color-scheme: dark media query in frontend/src/style.css
- [x] T013 [P] [US2] Add unit test asserting light mode is enforced on page load in frontend/tests/unit/App.test.ts

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently. Dashboard displays in light mode regardless of browser settings.

---

## Phase 4: User Story 3 - Reorganize Dashboard Card Layout (Priority: P2)

**Goal**: Reorganize dashboard cards: Team Statistics full-width at top with horizontal content, Add Goal and Update Mood side-by-side below, with responsive mobile stacking.

**Independent Test**: View dashboard and verify card layout matches new horizontal arrangement. Works independently of other user stories.

### Tests for User Story 3

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T014 [P] [US3] Unit test for Dashboard layout structure in frontend/tests/unit/views/Dashboard.test.ts
- [x] T015 [P] [US3] Unit test for StatsPanel horizontal content layout in frontend/tests/unit/components/StatsPanel.test.ts
- [x] T016 [P] [US3] Responsive layout test for mobile breakpoint in frontend/tests/integration/layout.test.ts

### Implementation for User Story 3

- [x] T017 [US3] Update Dashboard view layout structure with Team Statistics full-width at top in frontend/src/views/Dashboard.vue
- [x] T018 [US3] Arrange Add Goal and Update Mood cards side-by-side with responsive grid in frontend/src/views/Dashboard.vue
- [x] T019 [US3] Update StatsPanel component to display content horizontally (Goal Completion and Team Mood side-by-side) in frontend/src/components/StatsPanel.vue
- [x] T020 [US3] Add responsive breakpoint for mobile stacking (md:grid-cols-2) in frontend/src/views/Dashboard.vue
- [x] T021 [P] [US3] Add unit test asserting Team Member cards maintain existing grid layout below forms in frontend/tests/unit/views/Dashboard.test.ts

**Checkpoint**: At this point, User Stories 1, 2, AND 3 should all work independently. Dashboard layout matches specification with responsive behavior.

---

## Phase 5: User Story 4 - Apply Lighter Button Colors (Priority: P3)

**Goal**: Apply lighter button colors from design system. Buttons use light mode colors automatically via color-scheme setting.

**Independent Test**: View all buttons on dashboard and verify they use lighter color shades from design system. Works independently of other user stories.

### Tests for User Story 4

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T022 [P] [US4] Visual regression test for button colors in frontend/tests/integration/buttons.test.ts
- [x] T023 [P] [US4] Accessibility contrast test for button colors in frontend/tests/unit/components/buttons.test.ts

### Implementation for User Story 4

**Note**: No code changes needed - DaisyUI buttons automatically use light mode colors when `color-scheme: light` is set (from User Story 2). These tasks validate the automatic behavior.

- [x] T024 [P] [US4] Add unit test asserting GoalForm buttons use light mode colors in frontend/tests/unit/components/buttons.test.ts
- [x] T025 [P] [US4] Add unit test asserting MoodForm buttons use light mode colors in frontend/tests/unit/components/buttons.test.ts
- [x] T026 [P] [US4] Add unit test asserting TeamMemberCard buttons use light mode colors in frontend/tests/unit/components/buttons.test.ts
- [x] T027 [P] [US4] Add unit test asserting button hover states maintain visual feedback in frontend/tests/unit/components/buttons.test.ts
- [x] T028 [P] [US4] Add accessibility test asserting button contrast ratios meet 4.5:1 minimum in frontend/tests/unit/components/buttons.test.ts

**Checkpoint**: All user stories should now be independently functional. Dashboard has light mode, new layout, and lighter button colors.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [x] T029 [P] Add integration test asserting all existing functionality works (add goal, update mood, toggle completion) with new layout in frontend/tests/integration/regression.test.ts
- [x] T030 [P] Add performance test asserting page load time not increased by more than 10% (SC-006) in frontend/tests/integration/performance.test.ts
- [x] T031 [P] Add performance test asserting primary actions complete in same or less time (SC-007) in frontend/tests/integration/performance.test.ts
- [x] T032 [P] Add accessibility test asserting contrast ratios maintained (4.5:1 minimum) for all UI elements in frontend/tests/integration/accessibility.test.ts
- [x] T033 [P] Add responsive layout test for mobile devices (< 768px breakpoint) in frontend/tests/integration/responsive.test.ts
- [x] T034 [P] Add cross-browser compatibility test suite in frontend/tests/integration/browser-compat.test.ts
- [ ] T035 Run quickstart.md validation checklist manually (Manual validation step - user should verify)
- [x] T036 Code cleanup and refactoring if needed (No cleanup needed - code is clean and well-structured)
- [x] T037 Documentation updates if needed (No documentation updates needed - code is self-documenting)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No setup required - infrastructure already exists
- **User Stories (Phase 2-5)**: Can proceed in priority order (P1 → P2 → P3)
  - User stories can be implemented sequentially or in parallel (if staffed)
  - Each story is independently testable
- **Polish (Phase 6)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: No dependencies - can start immediately (MVP)
- **User Story 2 (P2)**: No dependencies on other stories - can start after US1 or in parallel
- **User Story 3 (P2)**: No dependencies on other stories - can start after US1 or in parallel
- **User Story 4 (P3)**: Depends on US2 (light mode enforcement) - requires color-scheme to be set

### Within Each User Story

- Tests (if included) MUST be written and FAIL before implementation
- Core implementation before integration
- Story complete before moving to next priority

### Parallel Opportunities

- All tests for a user story marked [P] can run in parallel
- User Stories 2 and 3 (both P2) can be worked on in parallel after US1 completes
- Different user stories can be worked on in parallel by different team members (after dependencies resolved)
- Polish phase tasks marked [P] can run in parallel

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together:
Task: "Unit test for deleteGoal API method handling 204 response in frontend/tests/unit/services/api.test.ts"
Task: "Integration test for delete goal error handling in frontend/tests/integration/goals.test.ts"
Task: "Unit test for error display in TeamMemberCard component in frontend/tests/unit/components/TeamMemberCard.test.ts"
```

---

## Parallel Example: User Stories 2 and 3

```bash
# After User Story 1 completes, User Stories 2 and 3 can run in parallel:

# Developer A: User Story 2 (Light Mode)
Task: "Add color-scheme: light to :root selector in frontend/src/style.css"
Task: "Override prefers-color-scheme: dark media query in frontend/src/style.css"

# Developer B: User Story 3 (Layout Reorganization)
Task: "Update Dashboard view layout structure with Team Statistics full-width at top in frontend/src/views/Dashboard.vue"
Task: "Update StatsPanel component to display content horizontally in frontend/src/components/StatsPanel.vue"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 2: User Story 1 (Fix Delete Goal Error)
2. **STOP and VALIDATE**: Test User Story 1 independently
3. Deploy/demo if ready

### Incremental Delivery

1. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
2. Add User Story 2 → Test independently → Deploy/Demo
3. Add User Story 3 → Test independently → Deploy/Demo
4. Add User Story 4 → Test independently → Deploy/Demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Developer A: User Story 1 (P1 - MVP)
2. Once User Story 1 completes:
   - Developer A: User Story 2 (P2 - Light Mode)
   - Developer B: User Story 3 (P2 - Layout)
3. Once User Stories 2 and 3 complete:
   - Developer A: User Story 4 (P3 - Button Colors)
4. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Frontend-only feature - no backend changes required
- All infrastructure already exists - no setup phase needed

