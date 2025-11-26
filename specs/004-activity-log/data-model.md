# Data Model: Activity Log

**Feature**: Activity Log  
**Date**: 2025-01-27

## Entities

### ActivityRecord

Represents a single activity/operation that occurred in the system.

**Attributes**:
- `Id` (INTEGER, PRIMARY KEY, AUTOINCREMENT): Unique identifier for the activity record
- `ActivityType` (TEXT, NOT NULL): Type of activity (AddGoal, UpdateMood, CompleteGoal, DeleteGoal)
- `TeamMemberId` (INTEGER, NOT NULL): ID of the team member affected by the activity
- `TeamMemberName` (TEXT, NOT NULL): Name of the team member (stored for display when member deleted)
- `Description` (TEXT, NOT NULL): Human-readable description of the activity (e.g., "Alice's mood was updated to happy", "Goal 'Complete project' was added")
- `GoalId` (INTEGER, NULL): ID of the goal (if activity is goal-related, NULL for mood updates)
- `GoalDescription` (TEXT, NULL): Description of the goal (stored for display when goal deleted)
- `Timestamp` (TEXT, NOT NULL): When the activity occurred (ISO 8601 format, UTC)

**Constraints**:
- `ActivityType` must be one of: 'AddGoal', 'UpdateMood', 'CompleteGoal', 'DeleteGoal'
- `TeamMemberName` cannot be empty
- `Description` cannot be empty
- `Timestamp` must be valid ISO 8601 datetime string
- For goal-related activities (AddGoal, CompleteGoal, DeleteGoal): `GoalId` and `GoalDescription` must be provided
- For mood updates (UpdateMood): `GoalId` and `GoalDescription` are NULL

**Relationships**:
- References `TeamMembers` table via `TeamMemberId` (foreign key, but may reference deleted members)
- References `Goals` table via `GoalId` (foreign key, but may reference deleted goals)

**State Transitions**: None (append-only log, records never modified or deleted)

## Database Schema

### ActivityRecords Table

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

**Indexes**:
- `idx_activityrecords_timestamp`: For efficient chronological queries (newest first)
- `idx_activityrecords_teammemberid`: For potential future filtering by team member
- `idx_activityrecords_activitytype`: For potential future filtering by activity type

## Activity Type Enumeration

### AddGoal
- **Trigger**: When a goal is created via `POST /api/goals`
- **Required Fields**: TeamMemberId, TeamMemberName, Description, GoalId, GoalDescription, Timestamp
- **Description Format**: "{TeamMemberName}'s goal '{GoalDescription}' was added"

### UpdateMood
- **Trigger**: When a mood is updated via `PUT /api/teammembers/{id}/mood`
- **Required Fields**: TeamMemberId, TeamMemberName, Description, Timestamp
- **Optional Fields**: GoalId (NULL), GoalDescription (NULL)
- **Description Format**: "{TeamMemberName}'s mood was updated to {MoodEmoji}"

### CompleteGoal
- **Trigger**: When a goal is marked complete via `PUT /api/goals/{id}/complete` with `isComplete: true`
- **Required Fields**: TeamMemberId, TeamMemberName, Description, GoalId, GoalDescription, Timestamp
- **Description Format**: "{TeamMemberName}'s goal '{GoalDescription}' was completed"

### DeleteGoal
- **Trigger**: When a goal is deleted via `DELETE /api/goals/{id}`
- **Required Fields**: TeamMemberId, TeamMemberName, Description, GoalId, GoalDescription, Timestamp
- **Description Format**: "{TeamMemberName}'s goal '{GoalDescription}' was deleted"

## Data Validation Rules

### On Activity Creation
1. `ActivityType` must be valid enum value
2. `TeamMemberId` must reference existing team member (or handle gracefully if deleted)
3. `TeamMemberName` must match team member's current name (or stored name if member deleted)
4. `Description` must be non-empty and human-readable
5. For goal-related activities: `GoalId` and `GoalDescription` must be provided
6. `Timestamp` must be valid ISO 8601 datetime (defaults to current UTC time)

### On Activity Retrieval
1. Handle deleted team members: Display stored `TeamMemberName` or "Unknown" if not available
2. Handle deleted goals: Display stored `GoalDescription` or indicate goal was deleted
3. Sort by `Timestamp` DESC (newest first)
4. Limit to 1000 records for performance (future: pagination)

## Frontend TypeScript Interface

```typescript
export interface ActivityRecord {
  id: number;
  activityType: 'AddGoal' | 'UpdateMood' | 'CompleteGoal' | 'DeleteGoal';
  teamMemberId: number;
  teamMemberName: string;
  description: string;
  goalId: number | null;
  goalDescription: string | null;
  timestamp: string; // ISO 8601 format
}
```

## Backend C# Model

```csharp
namespace DailyGoalTracker.Api.Models;

public class ActivityRecord
{
    public int Id { get; set; }
    public string ActivityType { get; set; } = string.Empty;
    public int TeamMemberId { get; set; }
    public string TeamMemberName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? GoalId { get; set; }
    public string? GoalDescription { get; set; }
    public DateTime Timestamp { get; set; }
}

public enum ActivityType
{
    AddGoal,
    UpdateMood,
    CompleteGoal,
    DeleteGoal
}
```

## Data Flow

### Activity Recording Flow
1. User performs operation (add goal, update mood, etc.)
2. Controller handles operation (existing logic)
3. Operation succeeds (goal added, mood updated, etc.)
4. Controller calls `ActivityService.RecordActivityAsync()` (best-effort, non-blocking)
5. `ActivityService` generates human-readable description
6. `ActivityRepository` inserts record into database
7. If recording fails, error is logged but operation still succeeds

### Activity Retrieval Flow
1. User navigates to activity log page
2. Frontend calls `GET /api/activities`
3. `ActivitiesController` calls `ActivityService.GetAllAsync()`
4. `ActivityService` calls `ActivityRepository.GetAllAsync()`
5. Repository queries database, orders by timestamp DESC
6. Results returned to frontend
7. Frontend displays activities in chronological order

