# Quickstart Guide: Activity Log

**Feature**: Activity Log  
**Date**: 2025-01-27

## Overview

This guide provides step-by-step instructions for setting up, testing, and using the Activity Log feature.

## Prerequisites

- Backend API running on `http://localhost:5001`
- Frontend application running on `http://localhost:5173`
- SQLite database initialized with TeamMembers and Goals tables
- At least one team member configured

## Setup

### 1. Database Migration

Run the database schema update to add the `ActivityRecords` table:

```sql
CREATE TABLE IF NOT EXISTS ActivityRecords (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ActivityType TEXT NOT NULL CHECK(ActivityType IN ('AddGoal', 'UpdateMood', 'CompleteGoal', 'DeleteGoal')),
    TeamMemberId INTEGER NOT NULL,
    TeamMemberName TEXT NOT NULL CHECK(LENGTH(TeamMemberName) > 0),
    Description TEXT NOT NULL CHECK(LENGTH(Description) > 0),
    GoalId INTEGER NULL,
    GoalDescription TEXT NULL,
    Timestamp TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (TeamMemberId) REFERENCES TeamMembers(Id),
    FOREIGN KEY (GoalId) REFERENCES Goals(Id)
);

CREATE INDEX IF NOT EXISTS idx_activityrecords_timestamp ON ActivityRecords(Timestamp DESC);
CREATE INDEX IF NOT EXISTS idx_activityrecords_teammemberid ON ActivityRecords(TeamMemberId);
CREATE INDEX IF NOT EXISTS idx_activityrecords_activitytype ON ActivityRecords(ActivityType);
```

### 2. Backend Setup

1. Add `ActivityRecord` model to `backend/src/DailyGoalTracker.Api/Models/`
2. Add `ActivityRepository` to `backend/src/DailyGoalTracker.Api/Data/Dapper/`
3. Add `ActivityService` to `backend/src/DailyGoalTracker.Api/Services/`
4. Add `ActivitiesController` to `backend/src/DailyGoalTracker.Api/Controllers/`
5. Update `GoalsController` and `TeamMembersController` to record activities
6. Register services in `Program.cs`

### 3. Frontend Setup

1. Add `ActivityRecord` type to `frontend/src/types/index.ts`
2. Add `getActivities()` method to `frontend/src/services/api.ts`
3. Create `useActivities` composable in `frontend/src/composables/`
4. Create `ActivityLog.vue` component in `frontend/src/components/`
5. Create `ActivityLogView.vue` in `frontend/src/views/`
6. Add route to `frontend/src/router/index.ts`
7. Add navigation link to dashboard

## Testing Scenarios

### Scenario 1: View Empty Activity Log

**Steps**:
1. Navigate to activity log page (no activities recorded yet)
2. Verify empty state message is displayed

**Expected Result**: 
- Empty state message: "No activities have been recorded yet"

### Scenario 2: Record and View Goal Addition

**Steps**:
1. Navigate to dashboard
2. Add a goal for a team member
3. Navigate to activity log page
4. Verify activity record appears

**Expected Result**:
- Activity record shows: "{TeamMemberName}'s goal '{GoalDescription}' was added"
- Record appears at top of list (newest first)
- Timestamp is displayed

### Scenario 3: Record and View Mood Update

**Steps**:
1. Navigate to dashboard
2. Update a team member's mood
3. Navigate to activity log page
4. Verify activity record appears

**Expected Result**:
- Activity record shows: "{TeamMemberName}'s mood was updated to {MoodEmoji}"
- Record appears at top of list
- Timestamp is displayed

### Scenario 4: Record and View Goal Completion

**Steps**:
1. Navigate to dashboard
2. Mark a goal as complete
3. Navigate to activity log page
4. Verify activity record appears

**Expected Result**:
- Activity record shows: "{TeamMemberName}'s goal '{GoalDescription}' was completed"
- Record appears at top of list

### Scenario 5: Record and View Goal Deletion

**Steps**:
1. Navigate to dashboard
2. Delete a goal
3. Navigate to activity log page
4. Verify activity record appears

**Expected Result**:
- Activity record shows: "{TeamMemberName}'s goal '{GoalDescription}' was deleted"
- Record appears at top of list
- Goal description is still visible (stored in activity record)

### Scenario 6: Chronological Ordering

**Steps**:
1. Perform multiple operations (add goal, update mood, complete goal)
2. Navigate to activity log page
3. Verify activities are ordered newest first

**Expected Result**:
- Most recent activity appears at top
- Activities ordered by timestamp descending

### Scenario 7: Error Handling

**Steps**:
1. Stop backend API
2. Navigate to activity log page
3. Verify error message with retry button

**Expected Result**:
- Error message displayed: "Failed to load activity log"
- Retry button is visible
- Clicking retry attempts to reload

### Scenario 8: Best-Effort Recording

**Steps**:
1. Simulate database error during activity recording
2. Perform operation (add goal, update mood, etc.)
3. Verify operation still succeeds
4. Check activity log (may be missing that activity)

**Expected Result**:
- Original operation succeeds
- Activity log may be incomplete (acceptable for best-effort)

## Performance Validation

### Load Test

1. Create 1000 activity records (via API or script)
2. Navigate to activity log page
3. Measure page load time

**Expected Result**:
- Page loads within 2 seconds (SC-002)

### API Response Time

1. Use API testing tool (Postman, curl, etc.)
2. Call `GET /api/activities` with 1000 records
3. Measure response time

**Expected Result**:
- Response time <200ms p95

## Accessibility Testing

1. Navigate to activity log using keyboard only
2. Verify all interactive elements are keyboard accessible
3. Test with screen reader

**Expected Result**:
- All elements keyboard accessible
- Screen reader announces activity records correctly
- ARIA labels present

## Troubleshooting

### No Activities Appearing

- Check database: Verify `ActivityRecords` table exists and has data
- Check API: Verify `GET /api/activities` returns data
- Check browser console: Look for API errors
- Check backend logs: Verify activity recording is working

### Activities Not Recording

- Check backend logs: Look for activity recording errors
- Verify services registered: Check `Program.cs` for service registration
- Verify controllers updated: Check `GoalsController` and `TeamMembersController`
- Test activity service: Run unit tests

### Performance Issues

- Check database indexes: Verify indexes are created
- Check query performance: Use EXPLAIN QUERY PLAN in SQLite
- Consider pagination: If records exceed 1000, implement pagination

## Next Steps

After completing MVP:
- Add filtering by activity type
- Add filtering by team member
- Add date range filtering
- Add pagination for large datasets
- Add activity export functionality

