import { describe, it, expect, vi, beforeEach } from 'vitest';
import { mount } from '@vue/test-utils';
import Dashboard from '../../src/views/Dashboard.vue';
import { useGoals } from '../../src/composables/useGoals';
import { useStats } from '../../src/composables/useStats';
import * as apiService from '../../src/services/api';

// Mock the composables
vi.mock('../../src/composables/useGoals');
vi.mock('../../src/composables/useStats');
vi.mock('../../src/composables/useMoods', () => ({
  useMoods: () => ({
    teamMembers: { value: [] },
    loading: { value: false },
    error: { value: null },
    loadTeamMembers: vi.fn(),
  }),
}));

describe('Stats Refresh After Goal Deletion - Integration', () => {
  const mockLoadStats = vi.fn();
  const mockDeleteGoal = vi.fn().mockResolvedValue(undefined);
  const mockLoadGoals = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
    
    (useGoals as any).mockReturnValue({
      loading: { value: false },
      error: { value: null },
      loadGoals: mockLoadGoals,
      getGoalsForTeamMember: vi.fn(() => []),
      getCompletionCount: vi.fn(() => ({ completed: 0, total: 0 })),
      deleteGoal: mockDeleteGoal,
      toggleGoalComplete: vi.fn(),
    });

    (useStats as any).mockReturnValue({
      stats: { value: null },
      loading: { value: false },
      error: { value: null },
      loadStats: mockLoadStats,
    });
  });

  it('should refresh stats after successful goal deletion', async () => {
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

    // Simulate goal deletion
    const handleDeleteGoal = (wrapper.vm as any).handleDeleteGoal;
    if (handleDeleteGoal) {
      await handleDeleteGoal(1);
    }

    // Verify loadStats was called to refresh statistics
    expect(mockLoadStats).toHaveBeenCalled();
  });

  it('should not refresh stats if deletion fails', async () => {
    const mockDeleteGoalError = vi.fn().mockRejectedValue(new Error('Delete failed'));
    
    (useGoals as any).mockReturnValue({
      loading: { value: false },
      error: { value: null },
      loadGoals: mockLoadGoals,
      getGoalsForTeamMember: vi.fn(() => [{ id: 1, teamMemberId: 1 }]),
      getCompletionCount: vi.fn(() => ({ completed: 0, total: 1 })),
      deleteGoal: mockDeleteGoalError,
      toggleGoalComplete: vi.fn(),
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

    // Simulate failed goal deletion
    const handleDeleteGoal = (wrapper.vm as any).handleDeleteGoal;
    if (handleDeleteGoal) {
      try {
        await handleDeleteGoal(1);
      } catch (err) {
        // Expected to throw
      }
    }

    // Verify loadStats was NOT called after failed deletion
    expect(mockLoadStats).not.toHaveBeenCalled();
  });
});

