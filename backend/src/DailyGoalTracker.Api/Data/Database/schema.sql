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

