<script setup lang="ts">
    import { ref, computed } from 'vue'
    import '../Content\\bootstrap.css'
    import HomePage from './components/HomePage.vue'
    import UserProfile from './components/UserProfile.vue'

    const isShowModal = ref(false)

    function closeModal() {
        isShowModal.value = false
    }
    function showModal() {
        isShowModal.value = true
    }
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


    <header class="p-3 mb-2 bg-dark text-white">
        <div class="wrapper">
            <div class="p-3 mb-2 bg-dark text-white">
                <component :is="currentView" />
            </div>
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
            display: contents;
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
