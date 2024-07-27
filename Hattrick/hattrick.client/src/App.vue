<script setup lang="ts">
    import { ref, computed } from 'vue'
    import HomePage from './components/HomePage.vue'
    import UserProfile from './components/UserProfile.vue'

    const routes = {
        '/': HomePage,
        '/userProfile': UserProfile
    }

    const currentPath = ref(window.location.hash)

    window.addEventListener('hashchange', () => {
        currentPath.value = window.location.hash
    })

    const currentView = computed(() => {
        return routes[currentPath.value.slice(1) || '/'] || NotFound
    })
</script>
<template>
    <a href="#/">Home</a> |
    <a href="#/userProfile">UserProfile</a> |
    <header>
        <div class="wrapper">
            <component :is="currentView" />
        </div>
    </header>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: block;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
