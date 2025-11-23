# Tasks: Team Goals Dashboard

**Input**: Design documents from `/specs/001-team-goals-dashboard/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Tests are included per constitution requirements (Testing Standards MUST).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Web app**: `backend/src/`, `frontend/src/`
- Backend: .NET 8 Web API with Dapper
- Frontend: Vue 3 + TypeScript with DaisyUI

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [ ] T001 Create backend project structure in backend/src/DailyGoalTracker.Api/
- [ ] T002 Create frontend project structure in frontend/ with Vue 3 + TypeScript
- [ ] T003 [P] Initialize .NET 8 Web API project in backend/src/DailyGoalTracker.Api/
- [ ] T004 [P] Initialize Vue 3 + TypeScript project in frontend/ with Vite
- [ ] T005 [P] Install and configure Dapper ORM in backend/src/DailyGoalTracker.Api/
- [ ] T006 [P] Install and configure DaisyUI and Tailwind CSS in frontend/
- [ ] T007 [P] Configure TypeScript strict mode in frontend/tsconfig.json
- [ ] T008 Create team-members.json configuration file in backend/team-members.json
- [ ] T009 [P] Setup xUnit test project in backend/src/DailyGoalTracker.Api.Tests/
- [ ] T010 [P] Setup Vitest and Vue Test Utils in frontend/tests/

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [ ] T011 Create SQLite database schema script in backend/src/DailyGoalTracker.Api/Data/Database/schema.sql
- [ ] T012 Implement database initialization service in backend/src/DailyGoalTracker.Api/Data/Database/DatabaseInitializer.cs
- [ ] T013 Create database connection configuration in backend/src/DailyGoalTracker.Api/Configuration/DatabaseConfig.cs
- [ ] T014 [P] Create TeamMember model class in backend/src/DailyGoalTracker.Api/Models/TeamMember.cs
- [ ] T015 [P] Create Goal model class in backend/src/DailyGoalTracker.Api/Models/Goal.cs
- [ ] T016 [P] Create API response wrapper models in backend/src/DailyGoalTracker.Api/Models/ApiResponse.cs
- [ ] T017 [P] Create TypeScript types/interfaces in frontend/src/types/index.ts
- [ ] T018 [P] Create API service base class in frontend/src/services/api.ts
- [ ] T019 Configure CORS middleware in backend/src/DailyGoalTracker.Api/Program.cs
- [ ] T020 Configure error handling middleware in backend/src/DailyGoalTracker.Api/Middleware/ErrorHandlingMiddleware.cs
- [ ] T021 Implement team member configuration loader in backend/src/DailyGoalTracker.Api/Services/TeamMemberConfigService.cs
- [ ] T022 [P] Create Dapper repository base class in backend/src/DailyGoalTracker.Api/Data/Dapper/BaseRepository.cs

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - View Team Goals Dashboard (Priority: P1) 🎯 MVP

**Goal**: Display dashboard showing all team members, their goals, moods, and team statistics

**Independent Test**: Load dashboard page and verify it displays team member cards with goals, mood indicators, and stats panel. Works even with empty state (no goals/moods).

### Tests for User Story 1

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [ ] T023 [P] [US1] Unit test for TeamMember model in backend/src/DailyGoalTracker.Api.Tests/Unit/Models/TeamMemberTests.cs
- [ ] T024 [P] [US1] Unit test for Goal model in backend/src/DailyGoalTracker.Api.Tests/Unit/Models/GoalTests.cs
- [ ] T025 [P] [US1] Integration test for GET /api/teammembers endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/TeamMembersControllerTests.cs
- [ ] T026 [P] [US1] Integration test for GET /api/goals endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/GoalsControllerTests.cs
- [ ] T027 [P] [US1] Integration test for GET /api/stats endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/StatsControllerTests.cs
- [ ] T028 [P] [US1] Unit test for Dashboard view component in frontend/tests/unit/views/Dashboard.test.ts
- [ ] T029 [P] [US1] Unit test for TeamMemberCard component in frontend/tests/unit/components/TeamMemberCard.test.ts
- [ ] T030 [P] [US1] Unit test for StatsPanel component in frontend/tests/unit/components/StatsPanel.test.ts

### Implementation for User Story 1

- [ ] T031 [P] [US1] Implement TeamMember Dapper repository in backend/src/DailyGoalTracker.Api/Data/Dapper/TeamMemberRepository.cs
- [ ] T032 [P] [US1] Implement Goal Dapper repository in backend/src/DailyGoalTracker.Api/Data/Dapper/GoalRepository.cs
- [ ] T033 [US1] Implement TeamMember service in backend/src/DailyGoalTracker.Api/Services/TeamMemberService.cs (depends on T031)
- [ ] T034 [US1] Implement Goal service in backend/src/DailyGoalTracker.Api/Services/GoalService.cs (depends on T032)
- [ ] T035 [US1] Implement Stats service in backend/src/DailyGoalTracker.Api/Services/StatsService.cs (depends on T033, T034)
- [ ] T036 [US1] Implement TeamMembersController with GET endpoint in backend/src/DailyGoalTracker.Api/Controllers/TeamMembersController.cs
- [ ] T037 [US1] Implement GoalsController with GET endpoint in backend/src/DailyGoalTracker.Api/Controllers/GoalsController.cs
- [ ] T038 [US1] Implement StatsController with GET endpoint in backend/src/DailyGoalTracker.Api/Controllers/StatsController.cs
- [ ] T039 [P] [US1] Create useGoals composable in frontend/src/composables/useGoals.ts
- [ ] T040 [P] [US1] Create useMoods composable in frontend/src/composables/useMoods.ts
- [ ] T041 [P] [US1] Create useStats composable in frontend/src/composables/useStats.ts
- [ ] T042 [US1] Implement API service methods for GET endpoints in frontend/src/services/api.ts (depends on T039, T040, T041)
- [ ] T043 [US1] Create Dashboard view component in frontend/src/views/Dashboard.vue
- [ ] T044 [US1] Create TeamMemberCard component in frontend/src/components/TeamMemberCard.vue
- [ ] T045 [US1] Create StatsPanel component in frontend/src/components/StatsPanel.vue
- [ ] T046 [US1] Integrate Dashboard view with API services in frontend/src/views/Dashboard.vue
- [ ] T047 [US1] Add empty state handling for no team members in frontend/src/views/Dashboard.vue
- [ ] T048 [US1] Add empty state handling for no goals in frontend/src/components/TeamMemberCard.vue
- [ ] T049 [US1] Add empty state handling for no mood in frontend/src/components/TeamMemberCard.vue

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently. Dashboard displays all team members, goals, moods, and stats.

---

## Phase 4: User Story 2 - Add Goals for Team Members (Priority: P2)

**Goal**: Enable users to add and delete goals for team members

**Independent Test**: Use "Add Goal" form to create goals for team members and verify they appear on dashboard. Delete goals and verify removal.

### Tests for User Story 2

- [ ] T050 [P] [US2] Integration test for POST /api/goals endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/GoalsControllerTests.cs
- [ ] T051 [P] [US2] Integration test for DELETE /api/goals/{id} endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/GoalsControllerTests.cs
- [ ] T052 [P] [US2] Unit test for goal validation (empty description) in backend/src/DailyGoalTracker.Api.Tests/Unit/Services/GoalServiceTests.cs
- [ ] T053 [P] [US2] Unit test for GoalForm component in frontend/tests/unit/components/GoalForm.test.ts
- [ ] T054 [P] [US2] Integration test for add goal user journey in frontend/tests/integration/goals.test.ts

### Implementation for User Story 2

- [ ] T055 [US2] Add POST endpoint to GoalsController in backend/src/DailyGoalTracker.Api/Controllers/GoalsController.cs
- [ ] T056 [US2] Add DELETE endpoint to GoalsController in backend/src/DailyGoalTracker.Api/Controllers/GoalsController.cs
- [ ] T057 [US2] Add CreateGoal method to GoalService in backend/src/DailyGoalTracker.Api/Services/GoalService.cs
- [ ] T058 [US2] Add DeleteGoal method to GoalService in backend/src/DailyGoalTracker.Api/Services/GoalService.cs
- [ ] T059 [US2] Add CreateGoal method to GoalRepository in backend/src/DailyGoalTracker.Api/Data/Dapper/GoalRepository.cs
- [ ] T060 [US2] Add DeleteGoal method to GoalRepository in backend/src/DailyGoalTracker.Api/Data/Dapper/GoalRepository.cs
- [ ] T061 [US2] Add goal validation (empty description check) in backend/src/DailyGoalTracker.Api/Services/GoalService.cs
- [ ] T062 [US2] Add POST /api/goals method to API service in frontend/src/services/api.ts
- [ ] T063 [US2] Add DELETE /api/goals/{id} method to API service in frontend/src/services/api.ts
- [ ] T064 [US2] Update useGoals composable with addGoal function in frontend/src/composables/useGoals.ts
- [ ] T065 [US2] Update useGoals composable with deleteGoal function in frontend/src/composables/useGoals.ts
- [ ] T066 [US2] Create GoalForm component in frontend/src/components/GoalForm.vue
- [ ] T067 [US2] Add form validation for empty goal description in frontend/src/components/GoalForm.vue
- [ ] T068 [US2] Add delete button/icon to goals in TeamMemberCard component in frontend/src/components/TeamMemberCard.vue
- [ ] T069 [US2] Integrate GoalForm with Dashboard view in frontend/src/views/Dashboard.vue
- [ ] T070 [US2] Update Dashboard to refresh after goal add/delete in frontend/src/views/Dashboard.vue

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently. Users can view dashboard and add/delete goals.

---

## Phase 5: User Story 3 - Mark Goals as Complete (Priority: P3)

**Goal**: Enable users to mark goals as complete or incomplete

**Independent Test**: Check/uncheck goal completion checkbox and verify goal status updates, completion counts update, and stats reflect changes.

### Tests for User Story 3

- [ ] T071 [P] [US3] Integration test for PUT /api/goals/{id} endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/GoalsControllerTests.cs
- [ ] T072 [P] [US3] Unit test for goal completion toggle logic in backend/src/DailyGoalTracker.Api.Tests/Unit/Services/GoalServiceTests.cs
- [ ] T073 [P] [US3] Unit test for completion count calculation in frontend/tests/unit/components/TeamMemberCard.test.ts
- [ ] T074 [P] [US3] Integration test for mark complete user journey in frontend/tests/integration/goals.test.ts

### Implementation for User Story 3

- [ ] T075 [US3] Add PUT endpoint to GoalsController in backend/src/DailyGoalTracker.Api/Controllers/GoalsController.cs
- [ ] T076 [US3] Add UpdateGoal method to GoalService in backend/src/DailyGoalTracker.Api/Services/GoalService.cs
- [ ] T077 [US3] Add UpdateGoal method to GoalRepository in backend/src/DailyGoalTracker.Api/Data/Dapper/GoalRepository.cs
- [ ] T078 [US3] Add PUT /api/goals/{id} method to API service in frontend/src/services/api.ts
- [ ] T079 [US3] Update useGoals composable with toggleGoalComplete function in frontend/src/composables/useGoals.ts
- [ ] T080 [US3] Add checkbox to each goal in TeamMemberCard component in frontend/src/components/TeamMemberCard.vue
- [ ] T081 [US3] Implement checkbox change handler in TeamMemberCard component in frontend/src/components/TeamMemberCard.vue
- [ ] T082 [US3] Update completion count calculation in TeamMemberCard component in frontend/src/components/TeamMemberCard.vue
- [ ] T083 [US3] Update StatsPanel to recalculate completion percentage in frontend/src/components/StatsPanel.vue
- [ ] T084 [US3] Add visual styling for completed goals (strikethrough) in frontend/src/components/TeamMemberCard.vue

**Checkpoint**: At this point, User Stories 1, 2, AND 3 should all work independently. Users can view, add, delete, and mark goals complete.

---

## Phase 6: User Story 4 - Update Mood for Team Members (Priority: P4)

**Goal**: Enable users to update team member moods

**Independent Test**: Use "Update Mood" form to set mood for team member and verify mood emoji appears on card and stats panel updates.

### Tests for User Story 4

- [ ] T085 [P] [US4] Integration test for PUT /api/teammembers/{id}/mood endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/TeamMembersControllerTests.cs
- [ ] T086 [P] [US4] Unit test for mood validation in backend/src/DailyGoalTracker.Api.Tests/Unit/Services/TeamMemberServiceTests.cs
- [ ] T087 [P] [US4] Unit test for MoodForm component in frontend/tests/unit/components/MoodForm.test.ts
- [ ] T088 [P] [US4] Integration test for update mood user journey in frontend/tests/integration/moods.test.ts

### Implementation for User Story 4

- [ ] T089 [US4] Add PUT /api/teammembers/{id}/mood endpoint to TeamMembersController in backend/src/DailyGoalTracker.Api/Controllers/TeamMembersController.cs
- [ ] T090 [US4] Add UpdateMood method to TeamMemberService in backend/src/DailyGoalTracker.Api/Services/TeamMemberService.cs
- [ ] T091 [US4] Add UpdateMood method to TeamMemberRepository in backend/src/DailyGoalTracker.Api/Data/Dapper/TeamMemberRepository.cs
- [ ] T092 [US4] Add mood validation (allowed emoji values) in backend/src/DailyGoalTracker.Api/Services/TeamMemberService.cs
- [ ] T093 [US4] Add PUT /api/teammembers/{id}/mood method to API service in frontend/src/services/api.ts
- [ ] T094 [US4] Update useMoods composable with updateMood function in frontend/src/composables/useMoods.ts
- [ ] T095 [US4] Create MoodForm component in frontend/src/components/MoodForm.vue
- [ ] T096 [US4] Add mood emoji selector (5 options) in frontend/src/components/MoodForm.vue
- [ ] T097 [US4] Integrate MoodForm with Dashboard view in frontend/src/views/Dashboard.vue
- [ ] T098 [US4] Update TeamMemberCard to display mood emoji in frontend/src/components/TeamMemberCard.vue
- [ ] T099 [US4] Update StatsPanel to calculate mood distribution in frontend/src/components/StatsPanel.vue
- [ ] T100 [US4] Add placeholder for no mood set in TeamMemberCard component in frontend/src/components/TeamMemberCard.vue

**Checkpoint**: At this point, all user stories should now be independently functional. Complete MVP is ready.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] T101 [P] Implement polling composable in frontend/src/composables/usePolling.ts
- [ ] T102 [P] Integrate polling into Dashboard view for real-time updates in frontend/src/views/Dashboard.vue
- [ ] T103 [P] Add keyboard navigation support to all interactive elements in frontend/src/components/
- [ ] T104 [P] Add ARIA labels and roles for screen reader support in frontend/src/components/
- [ ] T105 [P] Add loading states for async operations in frontend/src/components/
- [ ] T106 [P] Add error message display for API failures in frontend/src/components/
- [ ] T107 [P] Add error handling and logging in backend controllers in backend/src/DailyGoalTracker.Api/Controllers/
- [ ] T108 [P] Add input validation error responses in backend/src/DailyGoalTracker.Api/Controllers/
- [ ] T109 [P] Optimize database queries with proper indexing in backend/src/DailyGoalTracker.Api/Data/Dapper/
- [ ] T110 [P] Add performance monitoring/logging in backend/src/DailyGoalTracker.Api/Services/
- [ ] T111 [P] Add unit tests for remaining service methods in backend/src/DailyGoalTracker.Api.Tests/Unit/
- [ ] T112 [P] Add integration tests for error scenarios in backend/src/DailyGoalTracker.Api.Tests/Integration/
- [ ] T113 [P] Add component tests for edge cases in frontend/tests/unit/
- [ ] T114 [P] Run quickstart.md validation scenarios
- [ ] T115 [P] Performance testing and optimization (verify <2s page load, <200ms API)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3 → P4)
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - Depends on US1 for goal display
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Depends on US1 and US2 for goals
- **User Story 4 (P4)**: Can start after Foundational (Phase 2) - Depends on US1 for team member display

### Within Each User Story

- Tests MUST be written and FAIL before implementation (TDD approach)
- Models/Repositories before Services
- Services before Controllers
- Backend API before Frontend integration
- Core implementation before integration
- Story complete before moving to next priority

### Parallel Opportunities

- All Setup tasks marked [P] can run in parallel
- All Foundational tasks marked [P] can run in parallel (within Phase 2)
- Once Foundational phase completes, user stories can start in parallel (if team capacity allows)
- All tests for a user story marked [P] can run in parallel
- Models/Repositories within a story marked [P] can run in parallel
- Different user stories can be worked on in parallel by different team members (with coordination)

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together:
Task: "Unit test for TeamMember model in backend/src/DailyGoalTracker.Api.Tests/Unit/Models/TeamMemberTests.cs"
Task: "Unit test for Goal model in backend/src/DailyGoalTracker.Api.Tests/Unit/Models/GoalTests.cs"
Task: "Integration test for GET /api/teammembers endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/TeamMembersControllerTests.cs"
Task: "Integration test for GET /api/goals endpoint in backend/src/DailyGoalTracker.Api.Tests/Integration/GoalsControllerTests.cs"
Task: "Unit test for Dashboard view component in frontend/tests/unit/views/Dashboard.test.ts"

# Launch all repositories for User Story 1 together:
Task: "Implement TeamMember Dapper repository in backend/src/DailyGoalTracker.Api/Data/Dapper/TeamMemberRepository.cs"
Task: "Implement Goal Dapper repository in backend/src/DailyGoalTracker.Api/Data/Dapper/GoalRepository.cs"

# Launch all composables for User Story 1 together:
Task: "Create useGoals composable in frontend/src/composables/useGoals.ts"
Task: "Create useMoods composable in frontend/src/composables/useMoods.ts"
Task: "Create useStats composable in frontend/src/composables/useStats.ts"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Add User Story 4 → Test independently → Deploy/Demo
6. Add Polish phase → Final validation
7. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1 (backend + frontend)
   - Developer B: User Story 2 (backend + frontend)
   - Developer C: User Story 3 (backend + frontend)
3. Stories complete and integrate independently
4. Developer D: User Story 4 + Polish

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence
- All tasks include exact file paths for clarity
- Tests follow TDD approach (write tests first, ensure they fail, then implement)

