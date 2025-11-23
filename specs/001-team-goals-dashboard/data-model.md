# Data Model: Team Goals Dashboard

**Feature**: Team Goals Dashboard  
**Date**: 2025-01-27

## Entities

### TeamMember

Represents a person on the team. Team members are loaded from a configuration file at application startup.

**Attributes**:
- `Id` (int, primary key, auto-increment)
- `Name` (string, required, max 100 chars)
- `CurrentMood` (string, nullable, enum: "😀", "😊", "😐", "😞", "😤")

**Relationships**:
- One-to-Many with `Goal` (a team member can have multiple goals)

**Validation Rules**:
- Name must not be empty
- Name must be unique (enforced at application level, not database)
- CurrentMood must be one of the five allowed emoji values or null

**State Transitions**:
- Created: When loaded from config file
- MoodUpdated: When mood is set or changed
- MoodCleared: When mood is set to null (not currently supported in UI)

**Database Schema**:
```sql
CREATE TABLE TeamMembers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    CurrentMood TEXT CHECK(CurrentMood IN ('😀', '😊', '😐', '😞', '😤') OR CurrentMood IS NULL)
);
```

### Goal

Represents a daily objective for a team member.

**Attributes**:
- `Id` (int, primary key, auto-increment)
- `TeamMemberId` (int, foreign key to TeamMember, required)
- `Description` (string, required, max 500 chars)
- `IsComplete` (bool, default false)
- `CreatedAt` (datetime, auto-set on creation)
- `UpdatedAt` (datetime, auto-updated on modification)

**Relationships**:
- Many-to-One with `TeamMember` (each goal belongs to one team member)

**Validation Rules**:
- Description must not be empty (FR-013)
- Description must not exceed 500 characters
- TeamMemberId must reference an existing TeamMember

**State Transitions**:
- Created: When goal is added via form
- Completed: When IsComplete changes from false to true
- Incomplete: When IsComplete changes from true to false
- Deleted: When goal is removed (soft delete not required, hard delete)

**Database Schema**:
```sql
CREATE TABLE Goals (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    TeamMemberId INTEGER NOT NULL,
    Description TEXT NOT NULL CHECK(LENGTH(Description) > 0 AND LENGTH(Description) <= 500),
    IsComplete INTEGER NOT NULL DEFAULT 0 CHECK(IsComplete IN (0, 1)),
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (TeamMemberId) REFERENCES TeamMembers(Id) ON DELETE CASCADE
);

CREATE INDEX idx_goals_teammemberid ON Goals(TeamMemberId);
CREATE INDEX idx_goals_iscomplete ON Goals(IsComplete);
```

### TeamStats (Computed/Aggregated)

Represents aggregated statistics for the team. This is not a database entity but computed from Goals and TeamMembers.

**Attributes** (computed):
- `TotalGoals` (int): Total number of goals across all team members
- `CompletedGoals` (int): Number of completed goals
- `CompletionPercentage` (decimal): (CompletedGoals / TotalGoals) * 100
- `MoodDistribution` (object): Count of each mood type
  - `Happy` (int): Count of 😀
  - `Good` (int): Count of 😊
  - `Neutral` (int): Count of 😐
  - `Sad` (int): Count of 😞
  - `Stressed` (int): Count of 😤

**Computation Logic**:
- Calculated on-demand from database queries
- No persistence required (always computed from current data)

## Data Flow

### Team Member Initialization

1. Application reads `team-members.json` file at startup
2. For each team member in config:
   - Check if TeamMember exists in database (by Name)
   - If not exists, insert new TeamMember record
   - If exists, update if name changed (rare case)

### Goal Lifecycle

1. **Create**: User submits "Add Goal" form → POST /api/goals → Insert into Goals table
2. **Read**: Dashboard loads → GET /api/goals → Query all goals with TeamMember info
3. **Update**: User toggles completion → PUT /api/goals/{id} → Update IsComplete and UpdatedAt
4. **Delete**: User clicks delete → DELETE /api/goals/{id} → Remove from Goals table

### Mood Lifecycle

1. **Update**: User submits "Update Mood" form → PUT /api/teammembers/{id}/mood → Update CurrentMood field
2. **Read**: Dashboard loads → GET /api/teammembers → Include CurrentMood in response

## Data Validation

### Application-Level Validation

- Goal description: Not empty, max 500 chars (FR-013)
- Team member name: Not empty, max 100 chars
- Mood value: Must be one of five allowed emoji values

### Database-Level Constraints

- Foreign key constraint on Goals.TeamMemberId
- Check constraint on Goals.Description length
- Check constraint on Goals.IsComplete (0 or 1)
- Check constraint on TeamMembers.CurrentMood (allowed values or NULL)

## Data Persistence

- All data persisted in SQLite database file
- Database file location: `backend/DailyGoalTracker.Api/Data/database.db`
- Team member config file: `backend/team-members.json`

## Data Migration Strategy

- Initial schema creation via SQL scripts
- Future migrations can use .NET migration tools if needed
- For MVP: Simple schema creation script is sufficient

## Indexes

- `idx_goals_teammemberid`: Fast lookup of goals by team member
- `idx_goals_iscomplete`: Fast filtering of completed/incomplete goals (if needed for stats)

## Relationships Summary

```
TeamMember (1) ──< (Many) Goal
```

- One TeamMember can have many Goals
- Each Goal belongs to exactly one TeamMember
- Cascade delete: If TeamMember is deleted, all their Goals are deleted (not applicable in MVP, but good practice)

