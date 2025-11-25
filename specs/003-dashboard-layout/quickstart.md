# Quick Start: Dashboard Layout Changes

**Feature**: 003-dashboard-layout  
**Date**: 2025-01-27

## Overview

This feature implements dashboard layout improvements and fixes a delete goal bug. All changes are frontend-only (Vue 3 + Tailwind CSS + DaisyUI).

## Prerequisites

- Node.js and npm installed
- Backend API running on `http://localhost:5001`
- Frontend dependencies installed (`npm install` in `frontend/` directory)

## Implementation Steps

### 1. Fix Delete Goal JSON Parsing Error (P1)

**File**: `frontend/src/services/api.ts`

**Change**: Modify `request()` method to handle empty responses (HTTP 204)

```typescript
private async request<T>(
  endpoint: string,
  options: RequestInit = {}
): Promise<ApiResponse<T>> {
  const url = `${API_BASE_URL}${endpoint}`;
  
  const response = await fetch(url, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...options.headers,
    },
  });

  if (!response.ok) {
    const error = await response.json().catch(() => ({ error: 'Request failed' }));
    throw new Error(error.error || `HTTP error! status: ${response.status}`);
  }

  // Handle empty responses (e.g., HTTP 204 No Content)
  if (response.status === 204 || response.headers.get('content-length') === '0') {
    return {} as ApiResponse<T>;
  }

  // Only parse JSON if response has content
  const contentType = response.headers.get('content-type');
  if (contentType && contentType.includes('application/json')) {
    return await response.json();
  }

  return {} as ApiResponse<T>;
}
```

**Testing**: Delete a goal and verify no JSON parsing error occurs.

---

### 2. Force Light Mode Display (P2)

**File**: `frontend/src/style.css`

**Change**: Add CSS to force light mode regardless of browser preference

```css
@import "tailwindcss";
@plugin "daisyui";

/* Force light mode */
:root {
  color-scheme: light;
}

/* Override dark mode media queries */
@media (prefers-color-scheme: dark) {
  :root {
    color-scheme: light;
  }
}

/* ... existing styles ... */
```

**Testing**: Set browser to dark mode, load dashboard, verify it displays in light mode.

---

### 3. Reorganize Dashboard Card Layout (P2)

**File**: `frontend/src/views/Dashboard.vue`

**Change**: Update layout structure

```vue
<template>
  <div class="container mx-auto p-6">
    <h1 class="text-4xl font-bold mb-6">Team Goals Dashboard</h1>

    <!-- ... loading/error states ... -->

    <div v-else class="space-y-6">
      <!-- Team Statistics: Full width at top -->
      <div class="w-full">
        <StatsPanel
          :stats="stats"
          :loading="statsLoading"
          :error="statsError"
        />
      </div>

      <!-- Add Goal & Update Mood: Side-by-side on desktop, stacked on mobile -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <GoalForm
          :team-members="teamMembers"
          @goal-added="handleGoalAdded"
        />
        <MoodForm
          :team-members="teamMembers"
          @mood-updated="handleMoodUpdated"
        />
      </div>

      <!-- Team Member Cards: Existing grid layout -->
      <div v-if="teamMembers.length === 0" class="text-center py-8 text-gray-500">
        <p>No team members found.</p>
      </div>
      
      <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <TeamMemberCard
          v-for="member in teamMembers"
          :key="member.id"
          :team-member="member"
          :goals="getGoalsForTeamMember(member.id)"
          :completion-count="getCompletionCount(member.id)"
          @delete-goal="handleDeleteGoal"
          @toggle-complete="handleToggleComplete"
        />
      </div>
    </div>
  </div>
</template>
```

**File**: `frontend/src/components/StatsPanel.vue`

**Change**: Update content layout to horizontal (Goal Completion and Team Mood side-by-side)

```vue
<template>
  <div class="card bg-base-200 shadow-xl" role="region" aria-label="Team statistics">
    <div class="card-body">
      <h2 class="card-title">Team Statistics</h2>
      
      <!-- ... loading/error states ... -->
      
      <div v-else-if="stats" class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <!-- Goal Completion -->
        <div>
          <h3 class="font-semibold mb-2">Goal Completion</h3>
          <div class="text-3xl font-bold">{{ stats.completionPercentage }}%</div>
          <div class="text-sm text-gray-600">
            {{ stats.completedGoals }} of {{ stats.totalGoals }} goals completed
          </div>
        </div>

        <!-- Team Mood -->
        <div>
          <h3 class="font-semibold mb-2">Team Mood</h3>
          <div class="space-y-1">
            <div v-for="(count, mood) in stats.moodCounts" :key="mood" class="flex items-center gap-2">
              <span class="text-2xl">{{ mood }}</span>
              <span class="text-sm">{{ count }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
```

**Testing**: Verify layout matches spec: Team Statistics full-width at top, Add Goal and Update Mood side-by-side below, Team Member cards in grid below.

---

### 4. Update Error Display Location (P1)

**File**: `frontend/src/components/TeamMemberCard.vue`

**Change**: Display error messages within the card when goal deletion fails

```vue
<template>
  <div class="card bg-base-100 shadow-xl">
    <div class="card-body">
      <!-- ... existing content ... -->
      
      <!-- Error message display -->
      <div v-if="error" class="alert alert-error mt-4">
        <span>{{ error }}</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
// ... existing imports ...

const error = ref<string | null>(null);

// Update delete handler to set error state
const handleDelete = async (goalId: number) => {
  error.value = null;
  try {
    await deleteGoal(goalId);
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to delete goal';
  }
};
</script>
```

**Testing**: Simulate network error, verify error message appears within team member card.

---

### 5. Apply Lighter Button Colors (P3)

**Change**: No code changes needed. DaisyUI buttons automatically use light mode colors when `color-scheme: light` is set (from step 2).

**Verification**: Check all buttons use lighter colors consistent with light mode theme.

---

## Testing Checklist

- [ ] Delete goal works without JSON parsing error
- [ ] Dashboard displays in light mode when browser is set to dark mode
- [ ] Team Statistics card spans full width at top
- [ ] Team Statistics content arranged horizontally (Goal Completion and Team Mood side-by-side)
- [ ] Add Goal and Update Mood cards side-by-side on desktop
- [ ] Add Goal and Update Mood cards stack vertically on mobile (< 768px)
- [ ] Team Member cards maintain existing grid layout
- [ ] Error messages display within team member card on delete failure
- [ ] All buttons use lighter colors from design system
- [ ] All existing functionality works (add goal, update mood, toggle completion)
- [ ] Page load time not increased by more than 10%
- [ ] Accessibility contrast maintained (4.5:1 minimum)

## Running the Application

```bash
# Start backend (from backend/ directory)
cd backend/src/DailyGoalTracker.Api
dotnet run

# Start frontend (from frontend/ directory)
cd frontend
npm run dev
```

Visit `http://localhost:5173` (or port shown in terminal) to view the dashboard.

## Troubleshooting

- **Delete goal still shows error**: Check that `api.ts` handles 204 status correctly
- **Dark mode still appears**: Verify `color-scheme: light` is set in `style.css`
- **Layout not responsive**: Check Tailwind breakpoints (`md:grid-cols-2`)
- **Buttons not lighter**: Ensure DaisyUI theme respects `color-scheme: light`

