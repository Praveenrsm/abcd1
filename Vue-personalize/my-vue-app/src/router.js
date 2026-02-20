import { createRouter, createWebHistory } from 'vue-router';
import HomeView from './views/HomeView.vue';
import ContactUs from './views/ContactUs.vue';
import SearchView from './views/SearchView.vue';
import SemanticSearch from './views/SemanticSearch.vue';
const routes = [
  { path: '/', component: HomeView },
  { path: '/contact-us', component: ContactUs },
  { path: '/search-view', component: SearchView },
  { path: '/semantic-search', component: SemanticSearch }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
