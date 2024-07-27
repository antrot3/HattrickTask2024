<template>
    <div class="betting-component">
        <h1>User Profile</h1>

        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See
            <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-for="(sportMatches, sportName) in groupedMatches" :key="sportName">
            <h2>User Profile</h2>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

</script>

<style scoped>
    th {
        font-weight: bold;
    }

    th, td {
        padding: 0.5rem;
    }

    .betting-component {
        text-align: center;
    }

    table {
        margin-left: auto;
        margin-right: auto;
        border-collapse: collapse;
        width: 80%;
    }

    table, th, td {
        border: 1px solid black;
    }

    .loading {
        margin: 2rem;
    }

    .selected-info {
        margin-top: 2rem;
    }

    .multiplier-info {
        margin-top: 1rem;
        font-weight: bold;
    }
</style>
