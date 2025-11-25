import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import apiService from '../../../src/services/api';

// Mock global fetch
global.fetch = vi.fn();

describe('ApiService - deleteGoal', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  it('should handle HTTP 204 No Content response without JSON parsing error', async () => {
    // Mock fetch to return 204 No Content with empty body
    (global.fetch as any).mockResolvedValueOnce({
      ok: true,
      status: 204,
      headers: new Headers({
        'content-length': '0',
      }),
      json: vi.fn().mockRejectedValue(new Error('Unexpected end of JSON input')),
    });

    // Should not throw an error
    await expect(apiService.deleteGoal(1)).resolves.not.toThrow();
    
    // Verify fetch was called with correct endpoint and method
    expect(global.fetch).toHaveBeenCalledWith(
      expect.stringContaining('/api/goals/1'),
      expect.objectContaining({
        method: 'DELETE',
        headers: expect.objectContaining({
          'Content-Type': 'application/json',
        }),
      })
    );
  });

  it('should handle 204 response with no content-length header', async () => {
    // Mock fetch to return 204 No Content without content-length header
    (global.fetch as any).mockResolvedValueOnce({
      ok: true,
      status: 204,
      headers: new Headers(),
      json: vi.fn().mockRejectedValue(new Error('Unexpected end of JSON input')),
    });

    // Should not throw an error
    await expect(apiService.deleteGoal(1)).resolves.not.toThrow();
  });

  it('should throw error for non-204 error responses', async () => {
    // Mock fetch to return 404 error
    (global.fetch as any).mockResolvedValueOnce({
      ok: false,
      status: 404,
      json: vi.fn().mockResolvedValue({ error: 'Goal not found' }),
    });

    // Should throw an error
    await expect(apiService.deleteGoal(999)).rejects.toThrow('Goal not found');
  });
});

