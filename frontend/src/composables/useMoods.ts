import { ref } from 'vue';
import type { TeamMember } from '../types';
import apiService from '../services/api';

export function useMoods() {
  const teamMembers = ref<TeamMember[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const loadTeamMembers = async () => {
    loading.value = true;
    error.value = null;
    try {
      const response = await apiService.getTeamMembers();
      teamMembers.value = response.data || [];
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to load team members';
      teamMembers.value = [];
    } finally {
      loading.value = false;
    }
  };

  const updateMood = async (teamMemberId: number, mood: string) => {
    try {
      const response = await apiService.updateMood(teamMemberId, mood);
      if (response.data) {
        const index = teamMembers.value.findIndex(m => m.id === teamMemberId);
        if (index !== -1) {
          teamMembers.value[index] = response.data;
        }
      }
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to update mood';
      throw err;
    }
  };

  return {
    teamMembers,
    loading,
    error,
    loadTeamMembers,
    updateMood,
  };
}

