# API Contracts: Activity Log

**Feature**: Activity Log  
**Date**: 2025-01-27

## Overview

This document defines the REST API contracts for the Activity Log feature. All endpoints follow the existing API patterns from the dashboard feature.

## Base URL

```
http://localhost:5001/api
```

## Endpoints

### GET /api/activities

Retrieves all activity records in chronological order (newest first).

**Request**:
- Method: `GET`
- Path: `/api/activities`
- Headers: `Content-Type: application/json`
- Query Parameters: None

**Response**:
- Status: `200 OK`
- Body: `ApiResponse<ActivityRecord[]>`

**Response Schema**:
```json
{
  "data": [
    {
      "id": 1,
      "activityType": "AddGoal",
      "teamMemberId": 1,
      "teamMemberName": "Alice",
      "description": "Alice's goal 'Complete project' was added",
      "goalId": 5,
      "goalDescription": "Complete project",
      "timestamp": "2025-01-27T10:30:00Z"
    },
    {
      "id": 2,
      "activityType": "UpdateMood",
      "teamMemberId": 1,
      "teamMemberName": "Alice",
      "description": "Alice's mood was updated to 😀",
      "goalId": null,
      "goalDescription": null,
      "timestamp": "2025-01-27T10:25:00Z"
    }
  ],
  "message": null,
  "error": null,
  "details": null
}
```

**Error Responses**:
- `500 Internal Server Error`: Server error retrieving activities
  ```json
  {
    "data": null,
    "message": null,
    "error": "An unexpected error occurred.",
    "details": "Database connection failed"
  }
  ```

**Validation Rules**:
- None (no request parameters)

**Business Rules**:
- Returns all activities ordered by timestamp DESC (newest first)
- Returns empty array if no activities exist
- Handles deleted team members (shows stored name or "Unknown")
- Handles deleted goals (shows stored description)

## Activity Recording (Internal)

Activity recording is performed internally by existing controllers. No new public endpoints are exposed for activity creation.

### Integration Points

#### GoalsController

**POST /api/goals** (existing endpoint)
- After successful goal creation, record activity:
  - Type: `AddGoal`
  - TeamMemberId: from request
  - TeamMemberName: from database lookup
  - Description: "{TeamMemberName}'s goal '{GoalDescription}' was added"
  - GoalId: newly created goal ID
  - GoalDescription: from request

**PUT /api/goals/{id}/complete** (existing endpoint)
- After successful completion update, record activity (only if `isComplete: true`):
  - Type: `CompleteGoal`
  - TeamMemberId: from goal lookup
  - TeamMemberName: from team member lookup
  - Description: "{TeamMemberName}'s goal '{GoalDescription}' was completed"
  - GoalId: goal ID
  - GoalDescription: from goal lookup

**DELETE /api/goals/{id}** (existing endpoint)
- Before goal deletion, record activity:
  - Type: `DeleteGoal`
  - TeamMemberId: from goal lookup
  - TeamMemberName: from team member lookup
  - Description: "{TeamMemberName}'s goal '{GoalDescription}' was deleted"
  - GoalId: goal ID
  - GoalDescription: from goal lookup (before deletion)

#### TeamMembersController

**PUT /api/teammembers/{id}/mood** (existing endpoint)
- After successful mood update, record activity:
  - Type: `UpdateMood`
  - TeamMemberId: from request
  - TeamMemberName: from database lookup
  - Description: "{TeamMemberName}'s mood was updated to {MoodEmoji}"
  - GoalId: null
  - GoalDescription: null

## Data Models

### ActivityRecord

```typescript
interface ActivityRecord {
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

### ApiResponse<T>

```typescript
interface ApiResponse<T> {
  data: T | null;
  message: string | null;
  error: string | null;
  details: string | null;
}
```

## Error Handling

All endpoints follow the existing error handling pattern:
- Use `ErrorHandlingMiddleware` for global error handling
- Return `ApiResponse<T>` with error information
- Log errors server-side
- Return user-friendly error messages

## Performance Requirements

- GET /api/activities: <200ms p95 response time for up to 1000 records
- Activity recording: Non-blocking, <10ms overhead per operation

## Testing Requirements

### Unit Tests
- ActivityService: Test activity description generation for each type
- ActivityRepository: Test database operations (create, read)

### Integration Tests
- ActivitiesController: Test GET /api/activities endpoint
- GoalsController: Test activity recording on goal operations
- TeamMembersController: Test activity recording on mood updates
- Error scenarios: Test error handling and best-effort recording

### Contract Tests
- Verify API response schema matches contract
- Verify activity recording occurs for all operation types
- Verify chronological ordering (newest first)

