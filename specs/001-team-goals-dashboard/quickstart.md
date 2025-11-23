# Quickstart Guide: Team Goals Dashboard

**Feature**: Team Goals Dashboard  
**Date**: 2025-01-27

## Prerequisites

- .NET 8 SDK installed
- Node.js 18+ and npm installed
- SQLite (included with .NET)

## Setup Steps

### 1. Backend Setup

```bash
cd backend
dotnet restore
dotnet build
```

### 2. Database Initialization

```bash
# Run database schema creation script
dotnet run --project src/DailyGoalTracker.Api -- --init-db
```

Or manually create database using SQL scripts from `data-model.md`.

### 3. Team Member Configuration

Edit `backend/team-members.json`:
```json
[
  { "name": "Alice" },
  { "name": "Bob" },
  { "name": "Charlie" }
]
```

### 4. Frontend Setup

```bash
cd frontend
npm install
npm run dev
```

### 5. Start Backend API

```bash
cd backend
dotnet run --project src/DailyGoalTracker.Api
```

Backend runs on `http://localhost:5000` (or configured port).  
Frontend runs on `http://localhost:5173` (Vite default).

## Test Scenarios

### Scenario 1: View Empty Dashboard (P1 - MVP)

**Steps**:
1. Open browser to `http://localhost:5173`
2. View dashboard page

**Expected Result**:
- Dashboard displays all team members from config file
- Each member card shows:
  - Member name
  - No mood indicator (or placeholder)
  - Empty goal list
  - "0/0" completion count
- Stats panel shows:
  - 0% completion
  - Mood distribution with all zeros

**Validation**: ✅ User Story 1 acceptance scenarios 1, 5

### Scenario 2: Add Goals (P2)

**Steps**:
1. On dashboard, use "Add Goal" form
2. Select "Alice" from dropdown
3. Enter "Complete project documentation"
4. Submit form

**Expected Result**:
- Goal appears in Alice's card
- Goal shows as incomplete (checkbox unchecked)
- Alice's completion count updates to "0/1"
- Stats panel completion percentage updates
- Form clears after submission

**Validation**: ✅ User Story 2 acceptance scenarios 1, 2

### Scenario 3: Mark Goal Complete (P3)

**Steps**:
1. Find a goal in any team member's card
2. Click the checkbox next to the goal

**Expected Result**:
- Goal checkbox becomes checked
- Goal appears visually as complete (strikethrough or different style)
- Member's completion count updates (e.g., "0/1" → "1/1")
- Stats panel completion percentage updates
- Update happens immediately (no page refresh)

**Validation**: ✅ User Story 3 acceptance scenarios 1, 2, 3

### Scenario 4: Update Mood (P4)

**Steps**:
1. Use "Update Mood" form
2. Select "Alice" from dropdown
3. Select "😀" emoji
4. Submit form

**Expected Result**:
- Alice's card displays "😀" emoji
- Stats panel mood distribution updates (happy count increases)
- Update happens immediately

**Validation**: ✅ User Story 4 acceptance scenarios 1, 2

### Scenario 5: Delete Goal

**Steps**:
1. Find a goal in any team member's card
2. Click delete button/icon on the goal

**Expected Result**:
- Goal is removed from the card
- Member's completion count updates
- Stats panel completion percentage updates
- If last goal deleted, count shows "0/0"

**Validation**: ✅ User Story 2 acceptance scenarios 4, 5

### Scenario 6: Real-Time Updates via Polling

**Steps**:
1. Open dashboard in Browser Tab 1
2. Open dashboard in Browser Tab 2 (or use API directly)
3. In Tab 2, add a goal or update mood
4. Wait 5 seconds (polling interval)

**Expected Result**:
- Tab 1 automatically updates to show new goal/mood
- No manual refresh required
- Updates appear within polling interval

**Validation**: ✅ FR-012, SC-008

### Scenario 7: Form Validation

**Steps**:
1. Try to submit "Add Goal" form with empty description
2. Try to submit "Update Mood" form with invalid mood value (via API)

**Expected Result**:
- Empty goal description: Form shows error, prevents submission
- Invalid mood: API returns 400 Bad Request with error message

**Validation**: ✅ FR-013, User Story 2 acceptance scenario 3

### Scenario 8: Edge Cases

**Test Empty States**:
- No team members in config: Dashboard shows empty state message
- Team member with no goals: Shows "0 goals" or empty list
- Team member with no mood: Shows placeholder or "no mood set"

**Test Completion States**:
- All goals complete: Shows 100% completion
- No goals exist: Shows 0% completion

**Test Data Persistence**:
- Add goals and moods
- Refresh browser page
- Verify all data persists and displays correctly

**Validation**: ✅ Edge cases from spec

## Integration Testing

### API Integration Tests

Test each endpoint independently:

```bash
# Get all team members
curl http://localhost:5000/api/teammembers

# Create a goal
curl -X POST http://localhost:5000/api/goals \
  -H "Content-Type: application/json" \
  -d '{"teamMemberId": 1, "description": "Test goal"}'

# Update goal completion
curl -X PUT http://localhost:5000/api/goals/1 \
  -H "Content-Type: application/json" \
  -d '{"isComplete": true}'

# Update mood
curl -X PUT http://localhost:5000/api/teammembers/1/mood \
  -H "Content-Type: application/json" \
  -d '{"mood": "😀"}'

# Get stats
curl http://localhost:5000/api/stats

# Delete goal
curl -X DELETE http://localhost:5000/api/goals/1
```

### Frontend Integration Tests

Test Vue components with Vue Test Utils:

```bash
cd frontend
npm run test
```

## Performance Validation

### Page Load Performance (SC-001)

**Test**: Measure time from page load to fully rendered dashboard

**Target**: <2 seconds

**How to Test**:
1. Open browser DevTools → Network tab
2. Navigate to dashboard
3. Measure time until all API calls complete and UI renders
4. Verify <2 seconds

### API Response Time

**Test**: Measure API endpoint response times

**Target**: <200ms p95

**How to Test**:
1. Use API testing tool (Postman, curl with timing)
2. Make multiple requests to each endpoint
3. Calculate 95th percentile response time
4. Verify <200ms

## Accessibility Testing

### Keyboard Navigation

**Test**: Navigate entire dashboard using only keyboard

**Steps**:
1. Tab through all interactive elements
2. Use Enter/Space to activate buttons/checkboxes
3. Use arrow keys in dropdowns
4. Verify all functionality accessible

**Validation**: ✅ FR-019, FR-021

### Screen Reader

**Test**: Use screen reader (NVDA, JAWS, VoiceOver) to navigate

**Steps**:
1. Enable screen reader
2. Navigate dashboard
3. Verify all content is announced
4. Verify form labels are read correctly

**Validation**: ✅ FR-020

## Troubleshooting

### Database Issues

- **Issue**: Database file not found
- **Solution**: Run database initialization script

### CORS Errors

- **Issue**: Frontend can't call backend API
- **Solution**: Verify CORS configuration in `Program.cs` allows frontend origin

### Polling Not Working

- **Issue**: Dashboard doesn't update automatically
- **Solution**: Check browser console for errors, verify polling interval is set correctly

### Team Members Not Loading

- **Issue**: Dashboard shows no team members
- **Solution**: Verify `team-members.json` file exists and is valid JSON

## Next Steps

After quickstart validation:
1. Run full test suite
2. Verify all user stories independently
3. Check performance targets
4. Validate accessibility requirements
5. Proceed to implementation tasks

