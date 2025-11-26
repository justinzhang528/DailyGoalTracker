# Research: Activity Log Feature

**Feature**: Activity Log  
**Date**: 2025-01-27  
**Phase**: 0 - Research & Technical Decisions

## Technical Decisions

### Decision 1: SQLite Database Storage

**Decision**: Store activity records in SQLite database (same database as goals and team members)

**Rationale**:
- Consistent with existing architecture (001-team-goals-dashboard uses SQLite)
- Simple file-based storage, no additional infrastructure needed
- Sufficient for expected scale (small team, up to 1000 records)
- Easy to query and maintain
- Supports append-only log pattern (activity records never deleted)

**Alternatives Considered**:
- Separate database: Rejected - adds complexity, no clear benefit for MVP
- File-based logging: Rejected - harder to query and integrate with existing data
- External logging service: Rejected - overdesign for MVP, adds dependencies

### Decision 2: Best-Effort Activity Recording

**Decision**: Activity recording is non-blocking; original operations succeed even if activity recording fails

**Rationale**:
- Prevents activity logging from impacting core functionality
- Aligns with MVP-first principle (core features take priority)
- Reduces complexity (no retry logic, no transaction rollback)
- Acceptable trade-off: activity log may be incomplete, but operations always succeed

**Alternatives Considered**:
- Blocking activity recording: Rejected - could cause operation failures due to logging issues
- Retry with backoff: Rejected - adds complexity beyond MVP needs
- Async queue: Rejected - overdesign for current scale

### Decision 3: Forward-Only Recording (No Retroactive)

**Decision**: Start recording activities only from feature deployment forward; no backfill of historical operations

**Rationale**:
- Simplifies implementation (no migration scripts needed)
- Aligns with MVP-first principle
- Historical data can be added later if needed
- Reduces risk of data inconsistencies

**Alternatives Considered**:
- Retroactive recording: Rejected - requires complex migration, adds risk
- Hybrid approach: Rejected - adds complexity without clear benefit

### Decision 4: TDD Implementation Approach

**Decision**: Use Test-Driven Development (TDD) - write tests before implementation

**Rationale**:
- Aligns with constitution principle II (Testing Standards)
- Ensures test coverage from the start
- Helps clarify requirements during implementation
- Reduces bugs and improves code quality
- Matches user's explicit requirement

**Alternatives Considered**:
- Test-after approach: Rejected - violates constitution and user requirement
- No tests: Rejected - violates constitution principle II

### Decision 5: Activity Record Structure

**Decision**: Store activity type, team member name, description/details, and timestamp

**Rationale**:
- Captures all required information for human-readable display
- Simple structure, easy to query and display
- Team member name stored (not just ID) to handle deleted members gracefully
- Description/details stored for goals (handles deleted goals)

**Alternatives Considered**:
- Store only IDs: Rejected - harder to display when referenced entities deleted
- Store full entity snapshots: Rejected - overdesign, adds unnecessary complexity
- Store previous values: Rejected - not needed for MVP (mood updates show only new value)

### Decision 6: Integration with Existing Controllers

**Decision**: Add activity recording to existing controllers (GoalsController, TeamMembersController) rather than creating separate activity recording service

**Rationale**:
- Minimal changes to existing code
- Keeps activity recording close to operations
- Simple, direct approach
- No need for complex event system

**Alternatives Considered**:
- Event-driven architecture: Rejected - overdesign for MVP
- Separate activity recording service: Rejected - adds unnecessary abstraction layer
- Middleware approach: Rejected - harder to capture operation-specific details

## Best Practices Research

### SQLite Activity Log Table Design

**Findings**:
- Use INTEGER PRIMARY KEY AUTOINCREMENT for ID
- Store timestamps as TEXT with ISO 8601 format or use datetime() function
- Index on timestamp for efficient chronological queries
- Index on activity type if filtering needed (future enhancement)
- Use CHECK constraints for activity type validation

**References**:
- SQLite documentation: https://www.sqlite.org/lang_createtable.html
- Existing schema patterns from 001-team-goals-dashboard

### TDD Best Practices for .NET and Vue

**Findings**:
- Write failing test first (Red)
- Implement minimal code to pass (Green)
- Refactor while keeping tests green (Refactor)
- Test one behavior at a time
- Use descriptive test names
- Arrange-Act-Assert pattern for test structure

**References**:
- xUnit documentation: https://xunit.net/
- Vitest documentation: https://vitest.dev/
- Vue Test Utils: https://test-utils.vuejs.org/

### Activity Log Display Patterns

**Findings**:
- Chronological order (newest first) is standard
- Human-readable descriptions improve UX
- Timestamp formatting should be readable (relative or absolute)
- Empty states and error states are essential
- Loading states improve perceived performance

**References**:
- Common activity feed patterns in web applications
- Existing dashboard patterns from 001-team-goals-dashboard

## Integration Points

### Backend Integration
- GoalsController: Record activities for create, update (complete), delete operations
- TeamMembersController: Record activities for mood update operations
- Database: Add ActivityRecords table to existing schema
- Program.cs: Register ActivityService and ActivityRepository

### Frontend Integration
- Router: Add /activity-log route
- Navigation: Add link/button to dashboard
- API service: Add getActivities() method
- Types: Add ActivityRecord interface

## Performance Considerations

### Database Queries
- Single query to retrieve all activities (up to 1000 records)
- Index on timestamp for efficient ORDER BY
- Consider pagination if records exceed 1000 (future enhancement)

### API Response
- Return all activities in single response (acceptable for 1000 records)
- No pagination needed for MVP
- Response size should be manageable (<100KB for 1000 records)

### Frontend Rendering
- Render all activities at once (acceptable for 1000 records)
- Consider virtual scrolling if performance issues (future enhancement)
- Loading state during API call

## Security Considerations

### Activity Log Access
- No authentication required (consistent with existing dashboard)
- All team members can view all activities (transparency)
- No sensitive data in activity records

### Data Validation
- Validate activity type (enum: AddGoal, UpdateMood, CompleteGoal, DeleteGoal)
- Validate team member exists (or handle gracefully if deleted)
- Sanitize goal descriptions for display

## Testing Strategy

### Backend Tests (TDD)
1. ActivityRepositoryTests: Test database operations
2. ActivityServiceTests: Test business logic and activity description generation
3. ActivitiesControllerTests: Test API endpoints
4. Integration tests: Test activity recording from existing controllers

### Frontend Tests (TDD)
1. ActivityLog component tests: Test rendering, empty state, error state
2. useActivities composable tests: Test state management
3. Integration tests: Test full user flow (navigate, load, display)

## Dependencies

### Existing Dependencies (No Changes)
- .NET 8 Web API
- Dapper ORM
- Microsoft.Data.Sqlite
- Vue 3 + TypeScript
- Vitest + Vue Test Utils
- xUnit

### No New Dependencies Required
- All functionality can be built with existing stack

