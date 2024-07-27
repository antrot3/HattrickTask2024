<template>
    <div class="sports-component">
        <h1>Sports List</h1>

        <div v-if="loading" class="loading">
            Loading... Please wait.
        </div>

        <div v-if="!loading">
            <ul>
                <li v-for="sport in sports" :key="sport.id">
                    {{ sport.name }}
                </li>
            </ul>

            <input v-model="newSportName" placeholder="Add new sport" />
            <button @click="addSport">Add Sport</button>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type Sport = {
        id: number;
        name: string;
    };

    type Data = {
        loading: boolean;
        sports: Sport[];
        newSportName: string;
    };

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                sports: [],
                newSportName: '',
            };
        },
        created() {
            this.fetchSports();
        },
        methods: {
            async fetchSports() {
                this.loading = true;
                try {
                    const response = await fetch('http://localhost:5173/sports');
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    const data = await response.json();
                    this.sports = data.map((name: string, index: number) => ({ id: index, name }));
                } catch (error) {
                    console.error('Error fetching sports:', error);
                } finally {
                    this.loading = false;
                }
            },
            async addSport() {
                if (this.newSportName.trim() === '') return;

                try {
                    const response = await fetch('http://localhost:5076/Sports', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(this.newSportName),
                    });
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    this.sports.push({ id: this.sports.length, name: this.newSportName });
                    this.newSportName = '';
                } catch (error) {
                    console.error('Error adding sport:', error);
                }
            },
            async deleteSport(id: number) {
                try {
                    const response = await fetch(`http://localhost:5076/Sports/${id}`, {
                        method: 'DELETE',
                    });
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    this.sports = this.sports.filter(sport => sport.id !== id);
                } catch (error) {
                    console.error('Error deleting sport:', error);
                }
            },
        },
    });
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
