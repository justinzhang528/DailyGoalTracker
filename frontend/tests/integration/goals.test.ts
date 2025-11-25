import { describe, it, expect, vi, beforeEach } from 'vitest';
import { mount } from '@vue/test-utils';
import TeamMemberCard from '../../src/components/TeamMemberCard.vue';
import type { TeamMember, Goal } from '../../src/types';

describe('Delete Goal Error Handling - Integration', () => {
  const mockTeamMember: TeamMember = {
    id: 1,
    name: 'John Doe',
    currentMood: '😀',
  };

  const mockGoals: Goal[] = [
    {
      id: 1,
      teamMemberId: 1,
      description: 'Test goal',
      isComplete: false,
      createdAt: '2025-01-27T00:00:00Z',
      updatedAt: '2025-01-27T00:00:00Z',
    },
  ];

  const mockCompletionCount = { completed: 0, total: 1 };

  it('should display error message within team member card when deletion fails', async () => {
    const wrapper = mount(TeamMemberCard, {
      props: {
        teamMember: mockTeamMember,
        goals: mockGoals,
        completionCount: mockCompletionCount,
        error: 'Failed to delete goal: Network error',
      },
    });

    // Verify error message is displayed
    expect(wrapper.text()).toContain('Failed to delete goal: Network error');
    expect(wrapper.find('.alert-error').exists()).toBe(true);
  });

  it('should not display error message when error is null', () => {
    const wrapper = mount(TeamMemberCard, {
      props: {
        teamMember: mockTeamMember,
        goals: mockGoals,
        completionCount: mockCompletionCount,
        error: null,
      },
    });

    // Verify error message is not displayed
    expect(wrapper.find('.alert-error').exists()).toBe(false);
  });

  it('should emit deleteGoal event when delete button is clicked', async () => {
    const wrapper = mount(TeamMemberCard, {
      props: {
        teamMember: mockTeamMember,
        goals: mockGoals,
        completionCount: mockCompletionCount,
      },
    });

    const deleteButton = wrapper.find('button[aria-label="Delete goal: Test goal"]');
    await deleteButton.trigger('click');

    // Verify deleteGoal event was emitted with correct goal ID
    expect(wrapper.emitted('deleteGoal')).toBeTruthy();
    expect(wrapper.emitted('deleteGoal')?.[0]).toEqual([1]);
  });
});

