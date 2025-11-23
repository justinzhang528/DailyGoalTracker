import { ref } from 'vue';
import type { TeamStats } from '../types';
import apiService from '../services/api';

export function useStats() {
  const stats = ref<TeamStats | null>(null);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const loadStats = async () => {
    loading.value = true;
    error.value = null;
    try {
      const response = await apiService.getStats();
      stats.value = response.data || null;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to load stats';
      stats.value = null;
    } finally {
      loading.value = false;
    }
  };

  return {
    stats,
    loading,
    error,
    loadStats,
  };
}

