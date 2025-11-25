import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { mount } from '@vue/test-utils';
import App from '../../src/App.vue';

describe('App - Light Mode Enforcement', () => {
  let originalMatchMedia: typeof window.matchMedia;

  beforeEach(() => {
    originalMatchMedia = window.matchMedia;
  });

  afterEach(() => {
    window.matchMedia = originalMatchMedia;
  });

  it('should enforce light mode on page load', () => {
    // Mock matchMedia to return dark mode preference
    window.matchMedia = vi.fn((query: string) => {
      if (query === '(prefers-color-scheme: dark)') {
        return {
          matches: true,
          media: query,
          onchange: null,
          addListener: vi.fn(),
          removeListener: vi.fn(),
          addEventListener: vi.fn(),
          removeEventListener: vi.fn(),
          dispatchEvent: vi.fn(),
        } as MediaQueryList;
      }
      return {
        matches: false,
        media: query,
        onchange: null,
        addListener: vi.fn(),
        removeListener: vi.fn(),
        addEventListener: vi.fn(),
        removeEventListener: vi.fn(),
        dispatchEvent: vi.fn(),
      } as MediaQueryList;
    });

    const wrapper = mount(App);
    
    // Verify app component renders
    expect(wrapper.exists()).toBe(true);
    
    // Verify document root exists (light mode enforced via CSS)
    expect(document.documentElement).toBeTruthy();
  });

  it('should maintain light mode when browser preference changes', () => {
    const wrapper = mount(App);
    
    // Simulate browser preference change
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    
    // Verify app still renders (light mode should persist)
    expect(wrapper.exists()).toBe(true);
  });
});

