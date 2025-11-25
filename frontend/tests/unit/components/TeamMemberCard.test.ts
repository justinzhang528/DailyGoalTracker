import { describe, it, expect } from 'vitest';
import { mount } from '@vue/test-utils';
import TeamMemberCard from '../../../src/components/TeamMemberCard.vue';
import type { TeamMember, Goal } from '../../../src/types';

describe('TeamMemberCard - Error Display', () => {
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

  it('should display error message when error prop is provided', () => {
    const wrapper = mount(TeamMemberCard, {
      props: {
        teamMember: mockTeamMember,
        goals: mockGoals,
        completionCount: mockCompletionCount,
        error: 'Failed to delete goal',
      },
    });

    const errorAlert = wrapper.find('.alert-error');
    expect(errorAlert.exists()).toBe(true);
    expect(errorAlert.text()).toContain('Failed to delete goal');
  });

  it('should not display error message when error prop is null', () => {
    const wrapper = mount(TeamMemberCard, {
      props: {
        teamMember: mockTeamMember,
        goals: mockGoals,
        completionCount: mockCompletionCount,
        error: null,
      },
    });

    const errorAlert = wrapper.find('.alert-error');
    expect(errorAlert.exists()).toBe(false);
  });

  it('should not display error message when error prop is undefined', () => {
    const wrapper = mount(TeamMemberCard, {
      props: {
        teamMember: mockTeamMember,
        goals: mockGoals,
        completionCount: mockCompletionCount,
      },
    });

    const errorAlert = wrapper.find('.alert-error');
    expect(errorAlert.exists()).toBe(false);
  });
});

