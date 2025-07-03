<template>
  <div>
    <nav>
      <router-link to="/">Home page</router-link> |
      <router-link to="/contact-us">Contact Us</router-link>
    </nav>
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'App'
};
</script>

<style scoped>
nav {
  padding: 10px;
  background: #f0f0f0;
}
nav a {
  margin-right: 10px;
  text-decoration: none;
}
</style>
