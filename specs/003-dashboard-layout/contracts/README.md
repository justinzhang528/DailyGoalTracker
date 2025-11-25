# API Contracts: Dashboard Layout Changes

**Feature**: 003-dashboard-layout  
**Date**: 2025-01-27

## Overview

This feature involves **no API contract changes**. The bug fix for delete goal only affects how the frontend handles the existing API response (HTTP 204 No Content), but does not change the API contract itself.

## Existing API Contracts (Unchanged)

All API endpoints remain unchanged:

- `GET /api/teammembers` - Get team members
- `PUT /api/teammembers/{id}/mood` - Update mood
- `GET /api/goals` - Get goals
- `POST /api/goals` - Create goal
- `DELETE /api/goals/{id}` - Delete goal (response handling fixed, contract unchanged)
- `PUT /api/goals/{id}/complete` - Update goal completion
- `GET /api/stats` - Get team statistics

## Frontend Response Handling Change

### DELETE /api/goals/{id}

**Contract**: Unchanged
- **Request**: `DELETE /api/goals/{id}`
- **Response**: HTTP 204 No Content (empty body)

**Frontend Change**: 
- **Before**: Attempted to parse JSON from empty response → Error
- **After**: Check for 204 status or empty content, skip JSON parsing → Success

**Impact**: No API contract change. Only frontend response handling improved.

## Summary

**No API contract changes required.** The bug fix only improves frontend handling of the existing HTTP 204 response. All API endpoints, request/response formats, and contracts remain unchanged.

