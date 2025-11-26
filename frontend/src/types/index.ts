export interface TeamMember {
  id: number;
  name: string;
  currentMood: string | null;
}

export interface Goal {
  id: number;
  teamMemberId: number;
  description: string;
  isComplete: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface TeamStats {
  totalGoals: number;
  completedGoals: number;
  completionPercentage: number;
  moodCounts: {
    '😀': number;
    '😊': number;
    '😐': number;
    '😞': number;
    '😤': number;
  };
}

export interface ApiResponse<T> {
  data?: T;
  message?: string;
  error?: string;
  details?: string;
}

export interface ActivityRecord {
  id: number;
  activityType: 'AddGoal' | 'UpdateMood' | 'CompleteGoal' | 'DeleteGoal';
  teamMemberId: number;
  teamMemberName: string;
  description: string;
  goalId: number | null;
  goalDescription: string | null;
  timestamp: string; // ISO 8601 format
}

