<template>
    <div class="matches-component">
        <h1>Sport Events</h1>

        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See
            <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="groupedMatches && Object.keys(groupedMatches).length" class="content">
            <div v-for="(matches, sportName) in groupedMatches" :key="sportName">
                <h2>{{ sportName }}</h2>
                <table id="customers">
                    <thead>
                        <tr>
                            <th>Select</th>
                            <th>Date</th>
                            <th>Team Home</th>
                            <th>Team Away</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="match in matches" :key="match.id">
                            <td>
                                <input type="checkbox" v-model="selectedMatches" :value="match" />
                            </td>
                            <td>{{ match.date }}</td>
                            <td>{{ match.teamHome }}</td>
                            <td>{{ match.teamAway }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="selected-info">
                <h2>Selected Matches</h2>
                <ul>
                    <li v-for="match in selectedMatches" :key="match.id">
                        {{ match.teamHome }} vs {{ match.teamAway }} on {{ match.date }}
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type Match = {
        id: number;
        date: string;
        teamHome: string;
        teamAway: string;
        sport: {
            name: string;
        };
    };

    interface Data {
        loading: boolean;
        matches: Match[];
        selectedMatches: Match[];
    }

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                matches: [],
                selectedMatches: [],
            };
        },
        created() {
            this.fetchData();
        },
        methods: {
            async fetchData() {
                this.loading = true;
                try {
                    const response = await fetch('https://localhost:5173/match');
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    const data = await response.json();
                    this.matches = data;
                } catch (error) {
                    console.error('Error fetching matches:', error);
                } finally {
                    this.loading = false;
                }
            },
        },
        computed: {
            groupedMatches() {
                if (!this.matches || !this.matches.length) {
                    return {};
                }
                return this.matches.reduce((groups, match) => {
                    const sportName = match.sport.name;
                    if (!groups[sportName]) {
                        groups[sportName] = [];
                    }
                    groups[sportName].push(match);
                    return groups;
                }, {});
            },
        },
    });
</script>

<style scoped>
    #customers {
        font-family: Arial, Helvetica, sans-serif;
        border-collapse: collapse;
        width: 100%;
    }

    #customers td, #customers th {
        border: 1px solid #ddd;
        padding: 8px;
    }

    #customers tr:nth-child(even) {
        background-color: #f2f2f2;
    }

    #customers tr:hover {
        background-color: #ddd;
    }

    #customers th {
        padding-top: 12px;
        padding-bottom: 12px;
        text-align: left;
        background-color: #04AA6D;
        color: white;
    }

    th {
        font-weight: bold;
    }

    th, td {
        padding: 0.5rem;
    }

    .matches-component {
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
</style>
