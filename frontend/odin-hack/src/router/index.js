import { createRouter, createWebHistory } from "vue-router";
import StdView from "@/views/StdView.vue";
import AuthView from "@/views/AuthView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      component: StdView,
    },
    {
      path: "/auth",
      component: AuthView,
    },
  ],
});

export default router;
