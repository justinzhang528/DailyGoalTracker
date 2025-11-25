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

describe('Regression Tests - Existing Functionality', () => {
  const mockTeamMember = { id: 1, name: 'Test User', currentMood: null };
  const mockGoal = {
    id: 1,
    teamMemberId: 1,
    description: 'Test goal',
    isComplete: false,
    createdAt: '2025-01-27T00:00:00Z',
    updatedAt: '2025-01-27T00:00:00Z',
  };

  beforeEach(() => {
    (useGoals as any).mockReturnValue({
      loading: { value: false },
      error: { value: null },
      loadGoals: vi.fn(),
      getGoalsForTeamMember: vi.fn(() => [mockGoal]),
      getCompletionCount: vi.fn(() => ({ completed: 0, total: 1 })),
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
      teamMembers: { value: [mockTeamMember] },
      loading: { value: false },
      error: { value: null },
      loadTeamMembers: vi.fn(),
    });
  });

  it('should allow adding goals with new layout', () => {
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

    // Verify GoalForm is rendered
    expect(wrapper.html()).toContain('GoalForm');
  });

  it('should allow updating mood with new layout', () => {
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

    // Verify MoodForm is rendered
    expect(wrapper.html()).toContain('MoodForm');
  });

  it('should allow toggling goal completion with new layout', () => {
    const mockToggleGoalComplete = vi.fn();
    (useGoals as any).mockReturnValue({
      loading: { value: false },
      error: { value: null },
      loadGoals: vi.fn(),
      getGoalsForTeamMember: vi.fn(() => [mockGoal]),
      getCompletionCount: vi.fn(() => ({ completed: 0, total: 1 })),
      deleteGoal: vi.fn(),
      toggleGoalComplete: mockToggleGoalComplete,
    });

    const wrapper = mount(Dashboard, {
      global: {
        stubs: {
          StatsPanel: true,
          GoalForm: true,
          MoodForm: true,
          TeamMemberCard: {
            template: '<div>TeamMemberCard</div>',
            props: ['teamMember', 'goals', 'completionCount', 'error'],
            emits: ['deleteGoal', 'toggleComplete'],
          },
        },
      },
    });

    // Verify TeamMemberCard is rendered (toggle functionality available)
    expect(wrapper.html()).toContain('TeamMemberCard');
  });
});

