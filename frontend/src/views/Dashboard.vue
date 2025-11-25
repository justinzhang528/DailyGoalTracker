<template>
  <div class="container mx-auto p-6">
    <h1 class="text-4xl font-bold mb-6">Team Goals Dashboard</h1>

    <div v-if="loading" class="text-center py-8">
      <span class="loading loading-spinner loading-lg"></span>
      <p class="mt-4">Loading dashboard...</p>
    </div>

    <div v-else-if="error" class="alert alert-error mb-6">
      <span>{{ error }}</span>
    </div>

    <div v-else class="space-y-6">
      <!-- Team Statistics: Full width at top -->
      <div class="w-full">
        <StatsPanel
          :stats="stats"
          :loading="statsLoading"
          :error="statsError"
        />
      </div>

      <!-- Add Goal & Update Mood: Side-by-side on desktop, stacked on mobile -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <GoalForm
          :team-members="teamMembers"
          @goal-added="handleGoalAdded"
        />
        <MoodForm
          :team-members="teamMembers"
          @mood-updated="handleMoodUpdated"
        />
      </div>

      <!-- Team Member Cards: Existing grid layout -->
      <div v-if="teamMembers.length === 0" class="text-center py-8 text-gray-500">
        <p>No team members found. Please configure team members in team-members.json</p>
      </div>
      
      <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <TeamMemberCard
          v-for="member in teamMembers"
          :key="member.id"
          :team-member="member"
          :goals="getGoalsForTeamMember(member.id)"
          :completion-count="getCompletionCount(member.id)"
          :error="memberErrors[member.id] || null"
          @delete-goal="handleDeleteGoal"
          @toggle-complete="handleToggleComplete"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, computed, ref } from 'vue';
import { useGoals } from '../composables/useGoals';
import { useMoods } from '../composables/useMoods';
import { useStats } from '../composables/useStats';
import TeamMemberCard from '../components/TeamMemberCard.vue';
import StatsPanel from '../components/StatsPanel.vue';
import GoalForm from '../components/GoalForm.vue';
import MoodForm from '../components/MoodForm.vue';

const { loading: goalsLoading, error: goalsError, loadGoals, getGoalsForTeamMember, getCompletionCount, deleteGoal, toggleGoalComplete } = useGoals();
const { teamMembers, loading: moodsLoading, error: moodsError, loadTeamMembers } = useMoods();
const { stats, loading: statsLoading, error: statsError, loadStats } = useStats();

const loading = computed(() => goalsLoading.value || moodsLoading.value || statsLoading.value);
const error = computed(() => goalsError.value || moodsError.value || statsError.value);

// Track errors per team member card
const memberErrors = ref<Record<number, string | null>>({});

const loadDashboard = async () => {
  await Promise.all([
    loadTeamMembers(),
    loadGoals(),
    loadStats(),
  ]);
};

const handleGoalAdded = async () => {
  await Promise.all([
    loadGoals(),
    loadStats(),
  ]);
};

const handleDeleteGoal = async (goalId: number) => {
  // Find the team member for this goal to track error per card
  const goal = teamMembers.value
    .flatMap(m => getGoalsForTeamMember(m.id))
    .find(g => g.id === goalId);
  
  const teamMemberId = goal?.teamMemberId;
  
  // Clear any previous error for this member
  if (teamMemberId) {
    memberErrors.value[teamMemberId] = null;
  }
  
  try {
    await deleteGoal(goalId);
    await loadStats(); // Refresh stats after deletion
    // Clear error on success
    if (teamMemberId) {
      memberErrors.value[teamMemberId] = null;
    }
  } catch (err) {
    // Display error within the team member card
    const errorMessage = err instanceof Error ? err.message : 'Failed to delete goal';
    if (teamMemberId) {
      memberErrors.value[teamMemberId] = errorMessage;
    }
    console.error('Failed to delete goal:', err);
  }
};

const handleToggleComplete = async (goalId: number, isComplete: boolean) => {
  try {
    await toggleGoalComplete(goalId, isComplete);
    await loadStats(); // Refresh stats after toggle
  } catch (err) {
    // Error is handled by the composable
    console.error('Failed to toggle goal completion:', err);
  }
};

const handleMoodUpdated = async () => {
  await Promise.all([
    loadTeamMembers(), // Refresh team members to get updated mood
    loadStats(), // Refresh stats to update mood distribution
  ]);
};

onMounted(() => {
  loadDashboard();
});
</script>

