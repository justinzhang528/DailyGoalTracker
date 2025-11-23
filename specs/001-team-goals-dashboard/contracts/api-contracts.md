# API Contracts: Team Goals Dashboard

**Feature**: Team Goals Dashboard  
**Date**: 2025-01-27  
**Base URL**: `/api`

## Overview

RESTful API for managing team goals and moods. All endpoints return JSON and use standard HTTP status codes.

## Common Response Formats

### Success Response
```json
{
  "data": { ... },
  "message": "Success message (optional)"
}
```

### Error Response
```json
{
  "error": "Error message",
  "details": "Additional error details (optional)"
}
```

## Endpoints

### Team Members

#### GET /api/teammembers

Get all team members with their current mood.

**Response**: `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "name": "Alice",
      "currentMood": "😀"
    },
    {
      "id": 2,
      "name": "Bob",
      "currentMood": null
    }
  ]
}
```

#### PUT /api/teammembers/{id}/mood

Update a team member's mood.

**Request Body**:
```json
{
  "mood": "😊"
}
```

**Validation**:
- `mood` must be one of: "😀", "😊", "😐", "😞", "😤"
- `id` must exist

**Response**: `200 OK`
```json
{
  "data": {
    "id": 1,
    "name": "Alice",
    "currentMood": "😊"
  },
  "message": "Mood updated successfully"
}
```

**Error Responses**:
- `400 Bad Request`: Invalid mood value
- `404 Not Found`: Team member not found

### Goals

#### GET /api/goals

Get all goals for all team members.

**Query Parameters** (optional):
- `teamMemberId` (int): Filter by team member ID

**Response**: `200 OK`
```json
{
  "data": [
    {
      "id": 1,
      "teamMemberId": 1,
      "teamMemberName": "Alice",
      "description": "Complete project documentation",
      "isComplete": false,
      "createdAt": "2025-01-27T10:00:00Z",
      "updatedAt": "2025-01-27T10:00:00Z"
    },
    {
      "id": 2,
      "teamMemberId": 1,
      "teamMemberName": "Alice",
      "description": "Review pull requests",
      "isComplete": true,
      "createdAt": "2025-01-27T09:00:00Z",
      "updatedAt": "2025-01-27T11:00:00Z"
    }
  ]
}
```

#### POST /api/goals

Create a new goal.

**Request Body**:
```json
{
  "teamMemberId": 1,
  "description": "Complete project documentation"
}
```

**Validation**:
- `teamMemberId` must exist
- `description` must not be empty
- `description` must not exceed 500 characters

**Response**: `201 Created`
```json
{
  "data": {
    "id": 3,
    "teamMemberId": 1,
    "teamMemberName": "Alice",
    "description": "Complete project documentation",
    "isComplete": false,
    "createdAt": "2025-01-27T12:00:00Z",
    "updatedAt": "2025-01-27T12:00:00Z"
  },
  "message": "Goal created successfully"
}
```

**Error Responses**:
- `400 Bad Request`: Invalid input (empty description, invalid teamMemberId)
- `404 Not Found`: Team member not found

#### PUT /api/goals/{id}

Update a goal (primarily for toggling completion status).

**Request Body**:
```json
{
  "isComplete": true
}
```

**Alternative**: Can also update description if needed
```json
{
  "description": "Updated goal description",
  "isComplete": false
}
```

**Response**: `200 OK`
```json
{
  "data": {
    "id": 1,
    "teamMemberId": 1,
    "teamMemberName": "Alice",
    "description": "Complete project documentation",
    "isComplete": true,
    "createdAt": "2025-01-27T10:00:00Z",
    "updatedAt": "2025-01-27T12:30:00Z"
  },
  "message": "Goal updated successfully"
}
```

**Error Responses**:
- `400 Bad Request`: Invalid input
- `404 Not Found`: Goal not found

#### DELETE /api/goals/{id}

Delete a goal.

**Response**: `200 OK`
```json
{
  "message": "Goal deleted successfully"
}
```

**Error Responses**:
- `404 Not Found`: Goal not found

### Statistics

#### GET /api/stats

Get team statistics (completion percentage and mood distribution).

**Response**: `200 OK`
```json
{
  "data": {
    "completionPercentage": 66.67,
    "totalGoals": 6,
    "completedGoals": 4,
    "moodDistribution": {
      "happy": 2,
      "good": 1,
      "neutral": 1,
      "sad": 0,
      "stressed": 0
    }
  }
}
```

**Calculation Logic**:
- `completionPercentage`: (completedGoals / totalGoals) * 100
- If totalGoals is 0, completionPercentage is 0
- Mood counts: Count of each mood emoji across all team members (null moods not counted)

## HTTP Status Codes

- `200 OK`: Successful GET, PUT, DELETE
- `201 Created`: Successful POST
- `400 Bad Request`: Invalid request data
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

## Error Handling

All errors return consistent format:
```json
{
  "error": "Error message",
  "details": "Additional context (optional)"
}
```

## CORS

API must allow CORS from frontend origin (configured in .NET middleware).

## Rate Limiting

Not required for MVP (single user, local deployment).

## Authentication

Not required for MVP (as per spec - out of scope).

