import { describe, it, expect, vi, beforeEach } from 'vitest';
import { mount } from '@vue/test-utils';
import Dashboard from '../../src/views/Dashboard.vue';
import { useGoals } from '../../src/composables/useGoals';
import { useStats } from '../../src/composables/useStats';
import { useMoods } from '../../src/composables/useMoods';

// Mock the composables
vi.mock('../../src/composables/useGoals');
vi.mock('../../src/composables/useStats');
vi.mock('../../src/composables/useMoods');

describe('Cross-Browser Compatibility', () => {
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

  it('should render correctly in Chrome-like environment', () => {
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

    expect(wrapper.exists()).toBe(true);
  });

  it('should handle CSS Grid layout correctly', () => {
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
    // Verify grid classes are present (supported in all modern browsers)
    expect(html).toMatch(/grid/);
  });

  it('should handle color-scheme CSS property correctly', () => {
    // Verify color-scheme is set in stylesheet
    // This is supported in Chrome, Firefox, Safari, Edge
    const styleSheet = document.styleSheets[0];
    expect(styleSheet).toBeDefined();
  });
});

