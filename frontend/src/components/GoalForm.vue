<template>
  <div class="card bg-base-100 shadow-xl" role="region" aria-label="Add goal form">
    <div class="card-body">
      <h2 class="card-title">Add Goal</h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div class="form-control">
          <label class="label" for="teamMember">
            <span class="label-text">Team Member</span>
          </label>
          <select
            id="teamMember"
            v-model="selectedTeamMemberId"
            class="select select-bordered w-full"
            required
            :disabled="loading || teamMembers.length === 0"
            aria-label="Select team member"
          >
            <option value="" disabled>Select a team member</option>
            <option v-for="member in teamMembers" :key="member.id" :value="member.id">
              {{ member.name }}
            </option>
          </select>
        </div>

        <div class="form-control">
          <label class="label" for="description">
            <span class="label-text">Goal Description</span>
          </label>
          <input
            id="description"
            v-model="description"
            type="text"
            class="input input-bordered w-full"
            placeholder="Enter goal description..."
            required
            :disabled="loading"
            maxlength="500"
            aria-label="Goal description"
          />
          <label class="label">
            <span class="label-text-alt">{{ description.length }}/500 characters</span>
          </label>
        </div>

        <div v-if="error" class="alert alert-error">
          <span>{{ error }}</span>
        </div>

        <div class="form-control mt-4">
          <button
            type="submit"
            class="btn btn-primary"
            :disabled="loading || !selectedTeamMemberId || !description.trim()"
            aria-label="Add goal"
          >
            <span v-if="loading" class="loading loading-spinner loading-sm"></span>
            <span v-else>Add Goal</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import type { TeamMember } from '../types';
import apiService from '../services/api';

interface Props {
  teamMembers: TeamMember[];
}

defineProps<Props>();

const emit = defineEmits<{
  goalAdded: [];
}>();

const selectedTeamMemberId = ref<number | ''>('');
const description = ref('');
const loading = ref(false);
const error = ref<string | null>(null);

const handleSubmit = async () => {
  if (!selectedTeamMemberId.value || !description.value.trim()) {
    error.value = 'Please select a team member and enter a goal description';
    return;
  }

  loading.value = true;
  error.value = null;

  try {
    await apiService.createGoal(selectedTeamMemberId.value as number, description.value.trim());
    description.value = '';
    error.value = null;
    emit('goalAdded');
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to add goal';
  } finally {
    loading.value = false;
  }
};
</script>

