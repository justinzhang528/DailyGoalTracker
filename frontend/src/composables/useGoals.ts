import { ref, computed } from 'vue';
import type { Goal } from '../types';
import apiService from '../services/api';

export function useGoals() {
  const goals = ref<Goal[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const loadGoals = async (teamMemberId?: number) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await apiService.getGoals(teamMemberId);
      goals.value = response.data || [];
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to load goals';
      goals.value = [];
    } finally {
      loading.value = false;
    }
  };

  const goalsByTeamMember = computed(() => {
    const grouped: Record<number, Goal[]> = {};
    goals.value.forEach(goal => {
      if (!grouped[goal.teamMemberId]) {
        grouped[goal.teamMemberId] = [];
      }
      const array = grouped[goal.teamMemberId];
      if (array) {
        array.push(goal);
      }
    });
    return grouped;
  });

  const getGoalsForTeamMember = (teamMemberId: number) => {
    return goals.value.filter(g => g.teamMemberId === teamMemberId);
  };

  const getCompletionCount = (teamMemberId: number) => {
    const memberGoals = getGoalsForTeamMember(teamMemberId);
    const completed = memberGoals.filter(g => g.isComplete).length;
    return { completed, total: memberGoals.length };
  };

  const addGoal = async (teamMemberId: number, description: string) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await apiService.createGoal(teamMemberId, description);
      if (response.data) {
        goals.value.push(response.data);
      }
      return response.data;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to add goal';
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const deleteGoal = async (goalId: number) => {
    loading.value = true;
    error.value = null;
    try {
      await apiService.deleteGoal(goalId);
      goals.value = goals.value.filter(g => g.id !== goalId);
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to delete goal';
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const toggleGoalComplete = async (goalId: number, isComplete: boolean) => {
    try {
      const response = await apiService.updateGoalComplete(goalId, isComplete);
      if (response.data) {
        const index = goals.value.findIndex(g => g.id === goalId);
        if (index !== -1) {
          goals.value[index] = response.data;
        }
      }
      return response.data;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to update goal';
      throw err;
    }
  };

  return {
    goals,
    loading,
    error,
    loadGoals,
    goalsByTeamMember,
    getGoalsForTeamMember,
    getCompletionCount,
    addGoal,
    deleteGoal,
    toggleGoalComplete,
  };
}

