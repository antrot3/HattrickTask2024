<template>
    <div class="betting-component">
        <h1>Sport Events and Bets</h1>

        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See
            <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>
        <!-- ======= Header tools ======= -->
        <div v-for="(sportMatches, sportName) in groupedMatches" :key="sportName">
            <div class="datatable-container">
                <h2>{{ sportName }}</h2>
                <table class="table table-dark">
                    <thead>
                        <tr>
                            <th>Team Home</th>
                            <th>Team Away</th>
                            <th>Date</th>
                            <th>Odd Value 1</th>
                            <th>Odd Value 2</th>
                            <th>Odd Value X</th>
                            <th>Odd Value 1X</th>
                            <th>Odd Value 2X</th>
                            <th>Odd Value 12</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="match in sportMatches" :key="match.date" class="table-dark">
                            <td>{{ match.teamHome }}</td>
                            <td>{{ match.teamAway }}</td>
                            <td>{{ new Date(match.date).toLocaleString() }}</td>
                            <td>
                                <input type="radio" :name="'odd-' + match.date" v-model="selectedOdds[match.date]" :value="{ odd: match.oddValue1, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue1 === 1" />
                                {{ match.oddValue1 }}
                            </td>
                            <td>
                                <input type="radio" :name="'odd-' + match.date" v-model="selectedOdds[match.date]" :value="{ odd: match.oddValue2, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue2 === 1" />
                                {{ match.oddValue2 }}
                            </td>
                            <td>
                                <input type="radio" :name="'odd-' + match.date" v-model="selectedOdds[match.date]" :value="{ odd: match.oddValuex, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValuex === 1" />
                                {{ match.oddValuex }}
                            </td>
                            <td>
                                <input type="radio" :name="'odd-' + match.date" v-model="selectedOdds[match.date]" :value="{ odd: match.oddValue1x, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue1x === 1" />
                                {{ match.oddValue1x }}
                            </td>
                            <td>
                                <input type="radio" :name="'odd-' + match.date" v-model="selectedOdds[match.date]" :value="{ odd: match.oddValue2x, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue2x === 1" />
                                {{ match.oddValue2x }}
                            </td>
                            <td>
                                <input type="radio" :name="'odd-' + match.date" v-model="selectedOdds[match.date]" :value="{ odd: match.oddValue12, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue12 === 1" />
                                {{ match.oddValue12 }}
                            </td>
                        </tr>
                    </tbody>
                </table>
                <button @click="clearOdds(sportMatches)">Clear All</button>
            </div>
        </div>


        <div class="selected-info" v-if="selectedMatches.length > 0">
            <h3>Selected Matches:</h3>
            <ul>
                <li v-for="match in selectedMatches" :key="match.date">
                    {{ match.teamHome }} vs {{ match.teamAway }}
                </li>
            </ul>
        </div>

        <div class="multiplier-info" v-if="selectedOddsCount > 1">
            <p>Multiplier of selected odds: {{ totalMultiplier }}</p>
            <button @click="submitBets">Submit</button>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type Match = {
        sportName: string;
        teamHome: string;
        teamAway: string;
        date: string;
        oddValue1: number;
        oddValue2: number;
        oddValuex: number;
        oddValue1x: number;
        oddValue2x: number;
        oddValue12: number;
    };

    type SelectedOdd = {
        odd: number;
        teamHome: string;
        teamAway: string;
    };

    type Data = {
        loading: boolean;
        matches: Match[];
        selectedOdds: { [key: string]: SelectedOdd };
    };

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                matches: [],
                selectedOdds: {},
            };
        },
        computed: {
            groupedMatches(): { [key: string]: Match[] } {
                return this.matches.reduce((groups: { [key: string]: Match[] }, match: Match) => {
                    if (!groups[match.sportName]) {
                        groups[match.sportName] = [];
                    }
                    groups[match.sportName].push(match);
                    return groups;
                }, {});
            },
            selectedMatches(): SelectedOdd[] {
                return Object.values(this.selectedOdds);
            },
            selectedOddsCount(): number {
                return this.selectedMatches.length;
            },
            totalMultiplier(): number {
                return this.selectedMatches.reduce((acc, odd) => acc * odd.odd, 1);
            },
        },
        created() {
            this.fetchData();
        },
        methods: {
            async fetchData() {
                this.loading = true;
                try {
                    const response = await fetch('https://localhost:5173/match/get');
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    const data = await response.json();
                    console.log(data);
                    this.matches = data;
                } catch (error) {
                    console.error('Error fetching bets:', error);
                } finally {
                    this.loading = false;
                }
            },
            clearOdds(matches: Match[]) {
                matches.forEach(match => {
                    if (this.selectedOdds[match.date]) {
                        delete this.selectedOdds[match.date];
                    }
                });
            },
            submitBets() {
                // Placeholder for submitting the selected bets
                alert('Bets submitted successfully!');
            },
        },
    });
</script>

<style scoped>
    table {
        border-collapse: collapse;
        width: 100%;
    }

    th, td {
        padding: 8px;
        text-align: left;
        border-bottom: 1px solid #ddd;
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
