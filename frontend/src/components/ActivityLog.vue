<template>
  <div class="space-y-4" role="region" aria-labelledby="activity-log-title">
    <h2 id="activity-log-title" class="text-2xl font-bold mb-4">Activity Log</h2>

    <div v-if="loading" class="text-center py-8">
      <span class="loading loading-spinner loading-lg"></span>
      <p class="mt-4">Loading activities...</p>
    </div>

    <div v-else-if="error" class="alert alert-error" role="alert">
      <span>{{ error }}</span>
      <button @click="handleRetry" class="btn btn-sm btn-primary ml-4" :aria-label="'Retry loading activities'">
        Retry
      </button>
    </div>

    <div v-else-if="activities.length === 0" class="text-center py-8 text-gray-500">
      <p>No activities have been recorded yet.</p>
    </div>

    <div v-else class="space-y-2">
      <div
        v-for="activity in sortedActivities"
        :key="activity.id"
        class="card bg-base-100 shadow-md"
        role="listitem"
        :aria-label="`Activity: ${activity.description}`"
      >
        <div class="card-body p-4">
          <div class="flex items-start justify-between">
            <div class="flex-1">
              <p class="text-sm text-gray-600">{{ formatTimestamp(activity.timestamp) }}</p>
              <p class="mt-1">{{ activity.description }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { ActivityRecord } from '../types';

interface Props {
  activities: ActivityRecord[];
  loading: boolean;
  error: string | null;
}

const props = defineProps<Props>();

const emit = defineEmits<{
  retry: [];
}>();

const handleRetry = () => {
  emit('retry');
};

const formatTimestamp = (timestamp: string): string => {
  try {
    const date = new Date(timestamp);
    return date.toLocaleString();
  } catch {
    return timestamp;
  }
};

// Sort activities by timestamp (newest first) - computed property ensures reactivity
const sortedActivities = computed(() => {
  return [...props.activities].sort((a, b) => {
    const dateA = new Date(a.timestamp).getTime();
    const dateB = new Date(b.timestamp).getTime();
    return dateB - dateA; // Descending order (newest first)
  });
});
</script>

