<template>
    <div class="betting-component">
        <div class="section">
            <div :class="[isActive ? 'active' : '', 'collapsible']" @click="toggle">
                Rules
                <span style="color:red" class="toggleIcon" id="toggleIcon">{{ toggleIcon }}</span>
            </div>
            <div v-show="isActive">
                <p>
                    Each pair has picks*, each of which has its own odds, which is >= 1.0
                    Some pairs have odds-free picks that can't be bet on.
                    There is a certain category of top offers in which the already existing ones are singled out.
                    Pairs with better odds
                    <br />
                    The wallet balance can't go below 0.
                    o The User can deposit funds into their wallet
                    <br>
                    The player selects a tip* on the matches he wants to bet on
                    o Calculating the total quota of tickets
                    o 5% handling costs are deducted from the player's bet amount for each ticket
                    Pairs of top offers cannot be combined with each other
                    If a pair is paid as a top bid, the same pair cannot be played as part of a regular bid.
                    offers and vice versa.
                    When paying for a ticket with a pair of top bids, the ticket must contain at least 5 more pairs
                    odds >= 1.1
                </p>
            </div>
        </div>
        <h1>Sport Events and Bets</h1>
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See
            <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>
        <!-- ======= Header tools ======= -->
        <div v-for="(sportMatches, sportName) in groupedMatches" :key="sportName">
            <div class="datatable-container">
                <h2>{{ sportName }}</h2>
                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>Team</th>
                            <th>Away</th>
                            <th>Date</th>
                            <th>1</th>
                            <th>2</th>
                            <th>X</th>
                            <th>1X</th>
                            <th>2X</th>
                            <th>12</th>
                            <th>Top offer description</th>
                            <th>Top offer odds</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="match in sportMatches" :key="match.id" class="table-dark">
                            <td>{{ match.teamHome }}</td>
                            <td>{{ match.teamAway }}</td>
                            <td>{{ new Date(match.date).toLocaleString() }}</td>
                            <td>
                                <input type="radio" :name="'oddValue1-' + match.id" v-model="selectedOdds[match.id]" :value="{id:match.id, hasTopOffer: match.oddValue !== 1, oddType: '1', odd: match.oddValue1, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue1 === 1" />
                                {{ match.oddValue1 }}
                            </td>
                            <td>
                                <input type="radio" :name="'oddValue2-' + match.id" v-model="selectedOdds[match.id]" :value="{id:match.id, hasTopOffer: match.oddValue !== 1, oddType: '2', odd: match.oddValue2, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue2 === 1" />
                                {{ match.oddValue2 }}
                            </td>
                            <td>
                                <input type="radio" :name="'oddValuex-' + match.id" v-model="selectedOdds[match.id]" :value="{id:match.id, hasTopOffer: match.oddValue !== 1, oddType: 'x', odd: match.oddValuex, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValuex === 1" />
                                {{ match.oddValuex }}
                            </td>
                            <td>
                                <input type="radio" :name="'oddValue' + match.id" v-model="selectedOdds[match.id]" :value="{ id:match.id, hasTopOffer: match.oddValue !== 1, oddType: '1x', odd: match.oddValuex, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValuex === 1" />
                                {{ match.oddValue1x }}
                            </td>
                            <td>
                                <input type="radio" :name="'oddValue2x-' + match.id" v-model="selectedOdds[match.id]" :value="{ id:match.id, hasTopOffer: match.oddValue !== 1, oddType: '2x', odd: match.oddValue2x, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue2x === 1" />
                                {{ match.oddValue2x }}
                            </td>
                            <td>
                                <input type="radio" :name="'oddValue12-' + match.id" v-model="selectedOdds[match.id]" :value="{ id:match.id, hasTopOffer: match.oddValue !== 1, oddType: '12', odd: match.oddValue12, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue12 === 1" />
                                {{ match.oddValue12 }}
                            </td>
                            <td>
                                {{ match.topDescription }}
                            </td>
                            <td>
                                <input type="radio" :name="'odd-' + match.id" v-model="selectedOdds[match.id]" :value="{ id:match.id,  hasTopOffer: match.oddValue !== 1, oddType: 'specialOffer', odd: match.oddValue, teamHome: match.teamHome, teamAway: match.teamAway }" :disabled="match.oddValue === 1" />
                                {{ match.oddValue }}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <button @click="clearOdds()">Clear All</button>


        <div class="selected-info" v-if="selectedMatches.length > 0">
            <h3>Selected Matches:</h3>
            <ul>
                <li v-for="match in selectedMatches" :key="match.id">
                    {{ match.teamHome }} vs {{ match.teamAway }} {{ match.odd }}
                </li>
            </ul>
        </div>

        <div class="multiplier-info" v-if="selectedOddsCount > 0">
            <p>Multiplier of selected odds: {{ totalMultiplier.toFixed(2)  }}</p>
            <input v-model="betingAmount" placeholder="How much do you want to bet" type="number" min="0" value="1" @change="onChange($event)" />
            <p>How much are you bettin after handling costs of 5%: {{ handlingCosts.toFixed(2)  }}</p>
            <p>Pottential Winning {{ potentialWinning.toFixed(2)  }}</p>
            <button @click="submitBets">Submit Ticket</button>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type Match = {
        id: number;
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
        topDescription: number;
        oddValue: number;
    };

    type SelectedOdd = {
        id: number;
        odd: number;
        oddType: string;
        teamHome: string;
        teamAway: string;
        hasTopOffer: boolean;
    };

    type MatchRequst = {
        betAmmount: number;
        selectedOdds: SelectedOdd[];
    };

    type Data = {
        loading: boolean;
        handlingCosts: number;
        potentialWinning: number;
        betingAmount: number;
        matches: Match[];
        isActive: false;
        toggleIcon: 'See Rules';
        selectedOdds: SelectedOdd[];
        matchRequst: MatchRequst
    };

    export default defineComponent({
        data(): Data {
            return {
                isActive: false,
                toggleIcon: 'See Rules',
                loading: false,
                matches: [],
                selectedOdds: {},
                matchRequst: {},
                handlingCosts: 0,
                potentialWinning: 0,
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
            onChange(event): void {
                this.handlingCosts = this.betingAmount * 0.95;
                this.potentialWinning = this.selectedMatches.reduce((acc, odd) => acc * odd.odd, 1) * this.handlingCosts;
            },
        },
        created() {
            this.fetchData();
        },
        methods: {
            toggle() {
                this.isActive = !this.isActive;
                if (this.isActive == true) {
                    this.toggleIcon = 'Colapse Rules';
                }
                else {
                    this.toggleIcon = 'See Rules';
                }
            },
            async fetchData() {
                this.loading = true;
                try {
                    const response = await fetch('match/get');
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
            clearOdds() {
                this.matches.forEach(match => {
                    if (this.selectedOdds[match.id]) {
                        delete this.selectedOdds[match.id];
                    }
                });
            },
            async submitBets() {
                if (this.betingAmount <= 0) {
                    alert("The balance to be added cannot be negative or 0.");
                    return;
                }
                try {
                    const selectedOddsList: SelectedOdd[] = Object.values(this.selectedOdds);
                    this.matchRequst.betAmmount = this.betingAmount;
                    this.matchRequst.selectedOdds = selectedOddsList;

                    const response = await fetch('https://localhost:7020/match/post', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(this.matchRequst),
                    });
                    if (response.ok) {
                        alert(response);
                    }
                    if (!response.ok) {
                         alert(response)
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    location.reload();
                } catch (error) {
                    console.error('Error adding sport:', error);
                }
            },
        },
    });
</script>


<style scoped>
    .collapsible {
        background-color: #d8f8ea;
        color: black;
        cursor: pointer;
        padding: 18px;
        width: 100%;
        border: none;
        text-align: left;
        outline: none;
    }

    .content {
        padding: 0 18px;
        overflow: hidden;
        background-color: #eefcf6;
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
