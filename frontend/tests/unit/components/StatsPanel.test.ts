import { describe, it, expect } from 'vitest';
import { mount } from '@vue/test-utils';
import StatsPanel from '../../../src/components/StatsPanel.vue';
import type { TeamStats } from '../../../src/types';

describe('StatsPanel - Horizontal Content Layout', () => {
  const mockStats: TeamStats = {
    totalGoals: 10,
    completedGoals: 5,
    completionPercentage: 50,
    moodCounts: {
      '😀': 2,
      '😊': 1,
      '😐': 1,
      '😞': 0,
      '😤': 0,
    },
  };

  it('should display Goal Completion and Team Mood side-by-side', () => {
    const wrapper = mount(StatsPanel, {
      props: {
        stats: mockStats,
        loading: false,
        error: null,
      },
    });

    const html = wrapper.html();
    // Verify grid layout with md:grid-cols-2 for horizontal arrangement
    expect(html).toMatch(/grid.*grid-cols-1.*md:grid-cols-2/);
  });

  it('should display Goal Completion section', () => {
    const wrapper = mount(StatsPanel, {
      props: {
        stats: mockStats,
        loading: false,
        error: null,
      },
    });

    expect(wrapper.text()).toContain('Goal Completion');
    expect(wrapper.text()).toContain('50%');
    expect(wrapper.text()).toContain('5 of 10 goals completed');
  });

  it('should display Team Mood section', () => {
    const wrapper = mount(StatsPanel, {
      props: {
        stats: mockStats,
        loading: false,
        error: null,
      },
    });

    expect(wrapper.text()).toContain('Team Mood');
    expect(wrapper.text()).toContain('😀');
    expect(wrapper.text()).toContain('😊');
  });
});

