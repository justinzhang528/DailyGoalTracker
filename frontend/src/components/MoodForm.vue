<template>
  <div class="card bg-base-100 shadow-xl" role="region" aria-label="Update mood form">
    <div class="card-body">
      <h2 class="card-title">Update Mood</h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div class="form-control">
          <label class="label" for="teamMemberMood">
            <span class="label-text">Team Member</span>
          </label>
          <select
            id="teamMemberMood"
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
          <label class="label" for="mood">
            <span class="label-text">Mood</span>
          </label>
          <div class="flex gap-2 flex-wrap" role="group" aria-label="Mood selector">
            <button
              v-for="moodOption in moodOptions"
              :key="moodOption.value"
              type="button"
              @click="selectedMood = moodOption.value"
              @keydown.enter="selectedMood = moodOption.value"
              @keydown.space.prevent="selectedMood = moodOption.value"
              :class="[
                'btn btn-lg',
                selectedMood === moodOption.value ? 'btn-primary' : 'btn-outline'
              ]"
              :aria-label="`Select mood: ${moodOption.label}`"
              :aria-pressed="selectedMood === moodOption.value"
              tabindex="0"
            >
              <span class="text-2xl">{{ moodOption.value }}</span>
              <span class="ml-2 text-sm">{{ moodOption.label }}</span>
            </button>
          </div>
        </div>

        <div v-if="error" class="alert alert-error">
          <span>{{ error }}</span>
        </div>

        <div class="form-control mt-4">
          <button
            type="submit"
            class="btn btn-primary"
            :disabled="loading || !selectedTeamMemberId || !selectedMood"
            aria-label="Update mood"
          >
            <span v-if="loading" class="loading loading-spinner loading-sm"></span>
            <span v-else>Update Mood</span>
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
  moodUpdated: [];
}>();

const moodOptions = [
  { value: '😀', label: 'Happy' },
  { value: '😊', label: 'Good' },
  { value: '😐', label: 'Neutral' },
  { value: '😞', label: 'Sad' },
  { value: '😤', label: 'Stressed' },
];

const selectedTeamMemberId = ref<number | ''>('');
const selectedMood = ref<string>('');
const loading = ref(false);
const error = ref<string | null>(null);

const handleSubmit = async () => {
  if (!selectedTeamMemberId.value || !selectedMood.value) {
    error.value = 'Please select a team member and mood';
    return;
  }

  loading.value = true;
  error.value = null;

  try {
    await apiService.updateMood(selectedTeamMemberId.value as number, selectedMood.value);
    selectedMood.value = '';
    error.value = null;
    emit('moodUpdated');
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to update mood';
  } finally {
    loading.value = false;
  }
};
</script>

