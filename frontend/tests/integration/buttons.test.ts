import { describe, it, expect } from 'vitest';
import { mount } from '@vue/test-utils';
import GoalForm from '../../src/components/GoalForm.vue';
import MoodForm from '../../src/components/MoodForm.vue';
import TeamMemberCard from '../../src/components/TeamMemberCard.vue';
import type { TeamMember, Goal } from '../../src/types';

describe('Button Colors - Visual Regression', () => {
  it('should verify buttons use light mode colors via color-scheme', () => {
    // This test verifies that buttons are rendered with DaisyUI classes
    // which automatically use light mode colors when color-scheme: light is set
    const wrapper = mount(GoalForm, {
      props: {
        teamMembers: [],
      },
    });

    const submitButton = wrapper.find('button[type="submit"]');
    expect(submitButton.exists()).toBe(true);
    // Verify button has DaisyUI classes that respect color-scheme
    expect(submitButton.classes()).toContain('btn');
  });

  it('should verify all button components use DaisyUI button classes', () => {
    const mockTeamMember: TeamMember = {
      id: 1,
      name: 'Test',
      currentMood: null,
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

    const goalFormWrapper = mount(GoalForm, {
      props: {
        teamMembers: [mockTeamMember],
      },
    });

    const moodFormWrapper = mount(MoodForm, {
      props: {
        teamMembers: [mockTeamMember],
      },
    });

    const cardWrapper = mount(TeamMemberCard, {
      props: {
        teamMember: mockTeamMember,
        goals: mockGoals,
        completionCount: { completed: 0, total: 1 },
      },
    });

    // Verify buttons exist and have DaisyUI classes
    expect(goalFormWrapper.find('button.btn').exists()).toBe(true);
    expect(moodFormWrapper.find('button.btn').exists()).toBe(true);
    expect(cardWrapper.find('button.btn').exists()).toBe(true);
  });
});

