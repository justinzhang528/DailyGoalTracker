import { createRouter, createWebHistory } from 'vue-router';
import Dashboard from '../views/Dashboard.vue';
import ActivityLogView from '../views/ActivityLogView.vue';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'Dashboard',
      component: Dashboard,
    },
    {
      path: '/activity-log',
      name: 'ActivityLog',
      component: ActivityLogView,
    },
  ],
});

export default router;

