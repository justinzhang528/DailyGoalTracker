<template>
  <div class="card bg-base-200 shadow-xl" role="region" aria-label="Team statistics">
    <div class="card-body">
      <h2 class="card-title">Team Statistics</h2>
      
      <div v-if="loading" class="text-center py-4">
        <span class="loading loading-spinner loading-md"></span>
      </div>
      
      <div v-else-if="error" class="alert alert-error">
        <span>{{ error }}</span>
      </div>
      
      <div v-else-if="stats" class="space-y-4">
        <div>
          <h3 class="font-semibold mb-2">Goal Completion</h3>
          <div class="text-3xl font-bold">{{ stats.completionPercentage }}%</div>
          <div class="text-sm text-gray-600">
            {{ stats.completedGoals }} of {{ stats.totalGoals }} goals completed
          </div>
        </div>

        <div>
          <h3 class="font-semibold mb-2">Team Mood</h3>
          <div class="space-y-1">
            <div v-for="(count, mood) in stats.moodCounts" :key="mood" class="flex items-center gap-2">
              <span class="text-2xl">{{ mood }}</span>
              <span class="text-sm">{{ count }}</span>
            </div>
          </div>
        </div>
      </div>
      
      <div v-else class="text-gray-500 text-sm">
        No statistics available
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { TeamStats } from '../types';

interface Props {
  stats: TeamStats | null;
  loading: boolean;
  error: string | null;
}

defineProps<Props>();
</script>

