# Tasks: Activity Log

**Feature**: Activity Log  
**Branch**: `004-activity-log`  
**Spec**: [spec.md](./spec.md) | **Plan**: [plan.md](./plan.md)

## Overview

This feature adds an activity log page that displays all dashboard operations (goal additions, mood updates, goal completions, goal deletions) in chronological order. Activity records are stored in SQLite database and displayed on a dedicated frontend page. Implemented using TDD (Test-Driven Development) approach.

## Dependencies & Execution Order

### User Story Dependencies

- **User Story 1 (P1)**: View Activity Log - **Independent** (can be implemented and tested alone)
- **User Story 2 (P1)**: Navigate to Activity Log - **Depends on US1** (requires activity log page to exist)
- **Activity Recording Integration**: **Depends on US1** (requires ActivityService and ActivityRepository to exist)

### Parallel Execution Opportunities

- T001 and T002 can run in parallel (different files)
- T003, T004, T005 can run in parallel (different files)
- Test tasks within a user story can run in parallel
- Backend and frontend model/type tasks can run in parallel

## Implementation Strategy

**MVP Scope**: User Stories 1 and 2 (P1) - View Activity Log and Navigate to Activity Log

**Incremental Delivery**:
1. **Phase 1**: Setup (database schema)
2. **Phase 2**: Foundational (models, types, repository)
3. **Phase 3**: User Story 1 - View Activity Log (TDD: tests first, then implementation)
4. **Phase 4**: User Story 2 - Navigate to Activity Log
5. **Phase 5**: Activity Recording Integration (FR-002 through FR-005)
6. **Phase 6**: Polish (error handling, edge cases)

Each phase is independently testable and can be validated before proceeding.

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Database schema update for activity records

- [x] T001 Update database schema to add ActivityRecords table in backend/src/DailyGoalTracker.Api/Data/Database/schema.sql

**Checkpoint**: Database schema ready for activity records.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core models, types, and repository that MUST be complete before user stories

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T002 [P] Create ActivityRecord model class in backend/src/DailyGoalTracker.Api/Models/ActivityRecord.cs
- [x] T003 [P] Create ActivityType enum in backend/src/DailyGoalTracker.Api/Models/ActivityRecord.cs
- [x] T004 [P] Create ActivityRecord TypeScript interface in frontend/src/types/index.ts
- [x] T005 Create ActivityRepository Dapper repository in backend/src/DailyGoalTracker.Api/Data/Dapper/ActivityRepository.cs

**Checkpoint**: Foundation ready - user story implementation can now begin.

---

## Phase 3: User Story 1 - View Activity Log (Priority: P1) 🎯 MVP

**Goal**: Display activity log page showing all activity records in chronological order (newest first)

**Independent Test**: Navigate to activity log page and verify it displays all activity records (goal additions, mood updates, goal completions, goal deletions) in a readable format. Works even with empty state (no activities).

### Tests for User Story 1 (TDD - Write First, Ensure They Fail)

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T006 [P] [US1] Unit test for ActivityRecord model in backend/src/DailyGoalTracker.Api.Tests/Unit/Models/ActivityRecordTests.cs
- [ ] T007 [P] [US1] Unit test for ActivityRepository GetAllAsync in backend/src/DailyGoalTracker.Api.Tests/Unit/Data/ActivityRepositoryTests.cs
- [ ] T008 [P] [US1] Unit test for ActivityService GetAllAsync in backend/src/DailyGoalTracker.Api.Tests/Unit/Services/ActivityServiceTests.cs
- [ ] T009 [P] [US1] Integration test for GET /api/activities endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/ActivitiesControllerTests.cs
- [ ] T010 [P] [US1] Unit test for useActivities composable in frontend/tests/unit/composables/useActivities.test.ts
- [ ] T011 [P] [US1] Unit test for ActivityLog component in frontend/tests/unit/components/ActivityLog.test.ts
- [ ] T012 [P] [US1] Unit test for ActivityLogView in frontend/tests/unit/views/ActivityLogView.test.ts

### Implementation for User Story 1

- [x] T013 [US1] Implement ActivityService with GetAllAsync method in backend/src/DailyGoalTracker.Api/Services/ActivityService.cs (depends on T005)
- [x] T014 [US1] Implement ActivitiesController with GET /api/activities endpoint in backend/src/DailyGoalTracker.Api/Controllers/ActivitiesController.cs (depends on T013)
- [x] T015 [US1] Register ActivityService and ActivityRepository in backend/src/DailyGoalTracker.Api/Program.cs
- [x] T016 [P] [US1] Create useActivities composable in frontend/src/composables/useActivities.ts
- [x] T017 [US1] Add getActivities method to API service in frontend/src/services/api.ts (depends on T016)
- [x] T018 [US1] Create ActivityLog component in frontend/src/components/ActivityLog.vue
- [x] T019 [US1] Create ActivityLogView in frontend/src/views/ActivityLogView.vue
- [x] T020 [US1] Add activity log route to frontend router in frontend/src/router/index.ts
- [x] T021 [US1] Implement empty state message in frontend/src/components/ActivityLog.vue
- [x] T022 [US1] Implement chronological ordering (newest first) in frontend/src/components/ActivityLog.vue
- [x] T023 [US1] Implement timestamp formatting in frontend/src/components/ActivityLog.vue
- [x] T024 [US1] Implement error state with retry button in frontend/src/components/ActivityLog.vue

**Checkpoint**: At this point, User Story 1 should be fully functional. Users can view the activity log page and see all activity records in chronological order.

---

## Phase 4: User Story 2 - Navigate to Activity Log (Priority: P1) 🎯 MVP

**Goal**: Provide navigation from dashboard to activity log page and back

**Independent Test**: Verify navigation link/button exists on dashboard that takes users to activity log page, and return navigation exists on activity log page.

### Tests for User Story 2 (TDD - Write First, Ensure They Fail)

- [ ] T025 [P] [US2] Integration test for navigation from dashboard to activity log in frontend/tests/integration/navigation.test.ts
- [ ] T026 [P] [US2] Unit test for navigation link in Dashboard component in frontend/tests/unit/views/Dashboard.test.ts

### Implementation for User Story 2

- [x] T027 [US2] Add navigation link/button to dashboard in frontend/src/views/Dashboard.vue
- [x] T028 [US2] Add return navigation to dashboard in frontend/src/views/ActivityLogView.vue

**Checkpoint**: At this point, User Stories 1 and 2 should both work. Users can navigate between dashboard and activity log page.

---

## Phase 5: Activity Recording Integration (FR-002 through FR-005)

**Purpose**: Integrate activity recording into existing controllers (GoalsController, TeamMembersController)

**Goal**: Record activities automatically when operations occur (add goal, update mood, complete goal, delete goal)

**Independent Test**: Perform operations (add goal, update mood, complete goal, delete goal) and verify activity records are created in database. Verify original operations still succeed even if activity recording fails.

### Tests for Activity Recording (TDD - Write First, Ensure They Fail)

- [ ] T029 [P] Unit test for ActivityService RecordActivityAsync with AddGoal type in backend/src/DailyGoalTracker.Api.Tests/Unit/Services/ActivityServiceTests.cs
- [ ] T030 [P] Unit test for ActivityService RecordActivityAsync with UpdateMood type in backend/src/DailyGoalTracker.Api.Tests/Unit/Services/ActivityServiceTests.cs
- [ ] T031 [P] Unit test for ActivityService RecordActivityAsync with CompleteGoal type in backend/src/DailyGoalTracker.Api.Tests/Unit/Services/ActivityServiceTests.cs
- [ ] T032 [P] Unit test for ActivityService RecordActivityAsync with DeleteGoal type in backend/src/DailyGoalTracker.Api.Tests/Unit/Services/ActivityServiceTests.cs
- [ ] T033 [P] Integration test for activity recording on POST /api/goals in backend/src/DailyGoalTracker.Api.Tests/Integration/GoalsControllerTests.cs
- [ ] T034 [P] Integration test for activity recording on PUT /api/goals/{id}/complete in backend/src/DailyGoalTracker.Api.Tests/Integration/GoalsControllerTests.cs
- [ ] T035 [P] Integration test for activity recording on DELETE /api/goals/{id} in backend/src/DailyGoalTracker.Api.Tests/Integration/GoalsControllerTests.cs
- [ ] T036 [P] Integration test for activity recording on PUT /api/teammembers/{id}/mood in backend/src/DailyGoalTracker.Api.Tests/Integration/TeamMembersControllerTests.cs
- [ ] T037 Integration test for best-effort recording (operation succeeds even if recording fails) in backend/src/DailyGoalTracker.Api.Tests/Integration/ActivityRecordingTests.cs

### Implementation for Activity Recording

- [x] T038 Implement ActivityService RecordActivityAsync method in backend/src/DailyGoalTracker.Api/Services/ActivityService.cs
- [x] T039 Implement ActivityRepository CreateAsync method in backend/src/DailyGoalTracker.Api/Data/Dapper/ActivityRepository.cs
- [x] T040 Add activity recording to GoalsController POST /api/goals in backend/src/DailyGoalTracker.Api/Controllers/GoalsController.cs
- [x] T041 Add activity recording to GoalsController PUT /api/goals/{id}/complete in backend/src/DailyGoalTracker.Api/Controllers/GoalsController.cs
- [x] T042 Add activity recording to GoalsController DELETE /api/goals/{id} in backend/src/DailyGoalTracker.Api/Controllers/GoalsController.cs
- [x] T043 Add activity recording to TeamMembersController PUT /api/teammembers/{id}/mood in backend/src/DailyGoalTracker.Api/Controllers/TeamMembersController.cs
- [x] T044 Implement best-effort error handling (non-blocking) in backend/src/DailyGoalTracker.Api/Controllers/GoalsController.cs
- [x] T045 Implement best-effort error handling (non-blocking) in backend/src/DailyGoalTracker.Api/Controllers/TeamMembersController.cs

**Checkpoint**: At this point, all activity types are being recorded automatically when operations occur. Original operations succeed even if activity recording fails.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Error handling, edge cases, and final validation

- [x] T046 Handle deleted team members gracefully (show stored name or "Unknown") in frontend/src/components/ActivityLog.vue
- [x] T047 Handle deleted goals gracefully (show stored description) in frontend/src/components/ActivityLog.vue
- [x] T048 Add loading state during API calls in frontend/src/components/ActivityLog.vue
- [x] T049 Add ARIA labels for accessibility in frontend/src/components/ActivityLog.vue
- [ ] T050 Verify performance target (<2 seconds for 1000 records) in backend/src/DailyGoalTracker.Api.Tests/Integration/ActivitiesControllerTests.cs
- [ ] T051 Run quickstart.md validation scenarios
- [ ] T052 Update documentation if needed

**Checkpoint**: All polish complete. Feature is production-ready.

---

## Task Summary

- **Total Tasks**: 52
- **Phase 1 (Setup)**: 1 task
- **Phase 2 (Foundational)**: 4 tasks
- **Phase 3 (US1 - View Activity Log)**: 19 tasks (7 tests + 12 implementation)
- **Phase 4 (US2 - Navigate)**: 4 tasks (2 tests + 2 implementation)
- **Phase 5 (Activity Recording)**: 17 tasks (9 tests + 8 implementation)
- **Phase 6 (Polish)**: 7 tasks

**Parallel Opportunities**: 
- T002, T003, T004 can run in parallel (different files)
- All test tasks within a phase marked [P] can run in parallel
- Backend and frontend tasks can often run in parallel

**MVP Scope**: Phases 1-4 (User Stories 1 and 2) - Complete activity log viewing and navigation functionality.

