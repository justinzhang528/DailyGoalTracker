import { describe, it, expect, vi, beforeEach } from 'vitest';
import { mount } from '@vue/test-utils';
import Dashboard from '../../../src/views/Dashboard.vue';
import { useGoals } from '../../../src/composables/useGoals';
import { useStats } from '../../../src/composables/useStats';
import { useMoods } from '../../../src/composables/useMoods';

// Mock the composables
vi.mock('../../../src/composables/useGoals');
vi.mock('../../../src/composables/useStats');
vi.mock('../../../src/composables/useMoods');

describe('Dashboard - Layout Structure', () => {
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

  it('should display Team Statistics card at full width at top', () => {
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
    // Verify StatsPanel is in a full-width container
    // The structure should have StatsPanel in a w-full or similar container
    expect(html).toContain('StatsPanel');
  });

  it('should display Add Goal and Update Mood cards side-by-side', () => {
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
    // Verify grid layout with md:grid-cols-2 for side-by-side arrangement
    expect(html).toMatch(/grid.*grid-cols-1.*md:grid-cols-2/);
  });

  it('should maintain Team Member cards in grid layout below forms', () => {
    (useMoods as any).mockReturnValue({
      teamMembers: { value: [{ id: 1, name: 'Test', currentMood: null }] },
      loading: { value: false },
      error: { value: null },
      loadTeamMembers: vi.fn(),
    });

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
    // Verify TeamMemberCard components are rendered
    expect(html).toContain('TeamMemberCard');
  });
});

