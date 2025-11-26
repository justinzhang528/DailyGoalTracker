import { ref } from 'vue';
import type { ActivityRecord } from '../types';
import apiService from '../services/api';

export function useActivities() {
  const activities = ref<ActivityRecord[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const loadActivities = async () => {
    loading.value = true;
    error.value = null;
    try {
      const response = await apiService.getActivities();
      activities.value = response.data || [];
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to load activities';
      activities.value = [];
    } finally {
      loading.value = false;
    }
  };

  return {
    activities,
    loading,
    error,
    loadActivities,
  };
}

