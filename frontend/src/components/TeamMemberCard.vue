<template>
  <div class="card bg-base-100 shadow-xl" role="article" :aria-label="`Team member card for ${teamMember.name}`">
    <div class="card-body">
      <div class="flex items-center justify-between mb-4">
        <h2 class="card-title">{{ teamMember.name }}</h2>
        <div class="text-3xl" v-if="teamMember.currentMood" :aria-label="`Mood: ${teamMember.currentMood}`">
          {{ teamMember.currentMood }}
        </div>
        <span v-else class="text-gray-400 text-sm" aria-label="No mood set">No mood</span>
      </div>

      <div class="mb-2">
        <span class="text-sm font-semibold">
          {{ completionCount.completed }}/{{ completionCount.total }} goals completed
        </span>
      </div>

      <div v-if="goals.length === 0" class="text-gray-500 text-sm italic">
        No goals yet
      </div>
      <ul v-else class="space-y-2">
        <li v-for="goal in goals" :key="goal.id" class="flex items-center gap-2">
          <input
            type="checkbox"
            :checked="goal.isComplete"
            @change="$emit('toggleComplete', goal.id, !goal.isComplete)"
            class="checkbox checkbox-sm"
            :aria-label="`Goal: ${goal.description}, ${goal.isComplete ? 'completed' : 'incomplete'}`"
          />
          <span :class="{ 'line-through text-gray-500': goal.isComplete }" class="flex-1">
            {{ goal.description }}
          </span>
          <button
            @click="$emit('deleteGoal', goal.id)"
            @keydown.enter="$emit('deleteGoal', goal.id)"
            @keydown.space.prevent="$emit('deleteGoal', goal.id)"
            class="btn btn-sm btn-ghost btn-circle"
            :aria-label="`Delete goal: ${goal.description}`"
            title="Delete goal"
            tabindex="0"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { TeamMember, Goal } from '../types';

interface Props {
  teamMember: TeamMember;
  goals: Goal[];
  completionCount: { completed: number; total: number };
}

defineProps<Props>();

defineEmits<{
  deleteGoal: [goalId: number];
  toggleComplete: [goalId: number, isComplete: boolean];
}>();
</script>

