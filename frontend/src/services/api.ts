import type { ApiResponse, TeamMember, Goal, TeamStats, ActivityRecord } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5001/api';

class ApiService {
  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    const url = `${API_BASE_URL}${endpoint}`;
    
    const response = await fetch(url, {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({ error: 'Request failed' }));
      throw new Error(error.error || `HTTP error! status: ${response.status}`);
    }

    // Handle empty responses (e.g., HTTP 204 No Content)
    if (response.status === 204 || response.headers.get('content-length') === '0') {
      return {} as ApiResponse<T>;
    }

    // Only parse JSON if response has content
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      return await response.json();
    }

    return {} as ApiResponse<T>;
  }

  // Team Members
  async getTeamMembers(): Promise<ApiResponse<TeamMember[]>> {
    return this.request<TeamMember[]>('/teammembers');
  }

  async updateMood(teamMemberId: number, mood: string): Promise<ApiResponse<TeamMember>> {
    return this.request<TeamMember>(`/teammembers/${teamMemberId}/mood`, {
      method: 'PUT',
      body: JSON.stringify({ mood }),
    });
  }

  // Goals
  async getGoals(teamMemberId?: number): Promise<ApiResponse<Goal[]>> {
    const endpoint = teamMemberId 
      ? `/goals?teamMemberId=${teamMemberId}`
      : '/goals';
    return this.request<Goal[]>(endpoint);
  }

  async createGoal(teamMemberId: number, description: string): Promise<ApiResponse<Goal>> {
    return this.request<Goal>('/goals', {
      method: 'POST',
      body: JSON.stringify({ teamMemberId, description }),
    });
  }

  async updateGoalComplete(goalId: number, isComplete: boolean): Promise<ApiResponse<Goal>> {
    return this.request<Goal>(`/goals/${goalId}/complete`, {
      method: 'PUT',
      body: JSON.stringify({ isComplete }),
    });
  }

  async deleteGoal(goalId: number): Promise<void> {
    await this.request(`/goals/${goalId}`, {
      method: 'DELETE',
    });
  }

  // Stats
  async getStats(): Promise<ApiResponse<TeamStats>> {
    return this.request<TeamStats>('/stats');
  }

  // Activities
  async getActivities(): Promise<ApiResponse<ActivityRecord[]>> {
    return this.request<ActivityRecord[]>('/activities');
  }
}

export const apiService = new ApiService();
export default apiService;

