-- Team Goals Dashboard Database Schema
-- SQLite Database

-- TeamMembers table
CREATE TABLE IF NOT EXISTS TeamMembers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    CurrentMood TEXT CHECK(CurrentMood IN ('😀', '😊', '😐', '😞', '😤') OR CurrentMood IS NULL)
);

-- Goals table
CREATE TABLE IF NOT EXISTS Goals (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    TeamMemberId INTEGER NOT NULL,
    Description TEXT NOT NULL CHECK(LENGTH(Description) > 0 AND LENGTH(Description) <= 500),
    IsComplete INTEGER NOT NULL DEFAULT 0 CHECK(IsComplete IN (0, 1)),
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (TeamMemberId) REFERENCES TeamMembers(Id) ON DELETE CASCADE
);

-- Indexes for performance
CREATE INDEX IF NOT EXISTS idx_goals_teammemberid ON Goals(TeamMemberId);
CREATE INDEX IF NOT EXISTS idx_goals_iscomplete ON Goals(IsComplete);

-- ActivityRecords table
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
    FOREIGN KEY (GoalId) REFERENCES Goals(Id) ON DELETE SET NULL
);

-- Indexes for ActivityRecords
CREATE INDEX IF NOT EXISTS idx_activityrecords_timestamp ON ActivityRecords(Timestamp DESC);
CREATE INDEX IF NOT EXISTS idx_activityrecords_teammemberid ON ActivityRecords(TeamMemberId);
CREATE INDEX IF NOT EXISTS idx_activityrecords_activitytype ON ActivityRecords(ActivityType);

