<template>
  <div class="container mx-auto p-6">
    <div class="mb-6 flex items-center justify-between">
      <h1 class="text-4xl font-bold">Activity Log</h1>
      <router-link to="/" class="btn btn-primary" aria-label="Return to dashboard">
        Back to Dashboard
      </router-link>
    </div>

    <ActivityLog
      :activities="activities"
      :loading="loading"
      :error="error"
      @retry="handleRetry"
    />
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useActivities } from '../composables/useActivities';
import ActivityLog from '../components/ActivityLog.vue';

const { activities, loading, error, loadActivities } = useActivities();

const handleRetry = () => {
  loadActivities();
};

onMounted(() => {
  loadActivities();
});
</script>

