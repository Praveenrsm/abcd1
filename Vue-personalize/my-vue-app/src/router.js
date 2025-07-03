import { createRouter, createWebHistory } from 'vue-router';
import HomeView from './views/HomeView.vue';
import ContactUs from './views/ContactUs.vue';

const routes = [
  { path: '/', component: HomeView },
  { path: '/contact-us', component: ContactUs }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
