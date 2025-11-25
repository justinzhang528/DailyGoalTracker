import { describe, it, expect, beforeEach, vi } from 'vitest';
import { mount } from '@vue/test-utils';
import Dashboard from '../../src/views/Dashboard.vue';
import { useGoals } from '../../src/composables/useGoals';
import { useStats } from '../../src/composables/useStats';
import { useMoods } from '../../src/composables/useMoods';

// Mock the composables
vi.mock('../../src/composables/useGoals');
vi.mock('../../src/composables/useStats');
vi.mock('../../src/composables/useMoods');

describe('Responsive Layout - Mobile Breakpoint', () => {
  beforeEach(() => {
    (useGoals as any).mockReturnValue({
      loading: { value: false },
      error: { value: null },
      loadGoals: vi.fn(),
      getGoalsForTeamMember: vi.fn(() => []),
      getCompletionCount: vi.fn(() => ({ completed: 0, total: 0 })),
      deleteGoal: vi.fn(),
      toggleGoalComplete: vi.fn(),
    });

    (useStats as any).mockReturnValue({
      stats: { value: null },
      loading: { value: false },
      error: { value: null },
      loadStats: vi.fn(),
    });

    (useMoods as any).mockReturnValue({
      teamMembers: { value: [] },
      loading: { value: false },
      error: { value: null },
      loadTeamMembers: vi.fn(),
    });
  });

  it('should stack Add Goal and Update Mood cards vertically on mobile (< 768px)', () => {
    const wrapper = mount(Dashboard, {
      global: {
        stubs: {
          StatsPanel: true,
          GoalForm: true,
          MoodForm: true,
          TeamMemberCard: true,
        },
      },
    });

    const html = wrapper.html();
    // Verify grid-cols-1 for mobile (default, before md: breakpoint)
    expect(html).toMatch(/grid.*grid-cols-1/);
  });

  it('should display cards side-by-side on desktop (>= 768px)', () => {
    const wrapper = mount(Dashboard, {
      global: {
        stubs: {
          StatsPanel: true,
          GoalForm: true,
          MoodForm: true,
          TeamMemberCard: true,
        },
      },
    });

    const html = wrapper.html();
    // Verify md:grid-cols-2 for desktop breakpoint
    expect(html).toMatch(/md:grid-cols-2/);
  });
});

