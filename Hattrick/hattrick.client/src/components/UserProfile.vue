<template>
    <div class="sports-component">
        <h1>User Profile</h1>

        <div v-if="loading" class="loading">
            Loading... Please wait.
        </div>

        <div v-if="!loading">
            <ul>
                {{ user.name }}

                {{ user.walletBalance.toFixed(2)  }} Euro
            </ul>

            <input v-model="addToBallance" placeholder="Add  to Wallet" type="number" min="0" />
            <button @click="addBalance">Add to Wallet</button>
            <br />
            <br />
            <br />
            <input v-model="withdrwBalance" placeholder="Deposit to bank" type="number" min="0" />
            <button @click="withdrwFounds">Deposit to Bank</button>
            <div class="datatable-container">
                <h1> Transaction history</h1>
                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>Amount</th>
                            <th>Transaction description</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="transactions in walletTransactionModels" :key="transactions.amount">
                            <td>{{ transactions.amount.toFixed(2)  }} Euro</td>
                            <td>{{ transactions.transactionType }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div style="padding-top:50px" class="datatable-container">
                <h1>Bet history</h1>
                <table>
                    <thead>
                        <tr>
                            <th>Total odds</th>
                            <th>Stake/ Money on bet</th>
                            <th>Potential winning</th>
                            <th>Is Bet played</th>
                            <th>Did it win</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="ticket in ticketsPlaid" :key="ticket.totalOdd" :class="{ 'bg-success': ticket.didBetWin, 'bg-danger': !ticket.didBetWin}">
                            <td>{{ ticket.totalOdd.toFixed(2)  }}</td>
                            <td>{{ ticket.stake.toFixed(2)}} Euro</td>
                            <td>{{ ticket.potentialWinning.toFixed(2)}} Euro</td>
                            <td>{{ ticket.isBetPlayed }}</td>
                            <td>{{ ticket.didBetWin }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type User = {
        walletBalance: number;
        name: string;

    };

    type Ticket = {
        totalOdd: number;
        stake: string;
        potentialWinning: string;
        isBetPlayed: bool;
        didBetWin: bool;
    };

    type Data = {
        loading: boolean;
        user: User;
        addToBallance: number;
    };

    type WalletTransactionModels = {
        amount: number;
        transactionType: string;
    };

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                user: null,
                walletTransactionModels: [],
                ticketsPlaid: [],
            };
        },
        created() {
            this.getUser();
        },
        methods: {
            async getUser() {
                this.loading = true;
                try {
                    const response = await fetch("https://localhost:7020/user/get", {
                    })

                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    const data = await response.json();
                    this.user = data.user
                    this.walletTransactionModels = data.walletTransactionModels
                    this.ticketsPlaid = data.ticketsPlaid
                } catch (error) {
                    console.error('Error fetching user data:', error);
                } finally {
                    this.loading = false;
                }
            },
            async addBalance() {
                if (this.addToBallance <= 0) {
                    alert("The ammount to add can not be negative or 0.");
                    return;
                }
                try {
                    const response = await fetch('https://localhost:7020/User/post', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(this.addToBallance),
                    });
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    location.reload();
                } catch (error) {
                    console.error('Error adding sport:', error);
                }
            },
            async withdrwFounds() {
                if (this.withdrwBalance <= 0) {
                    alert("The ammount to add can not be negative or 0.");
                    return;
                }
                if (this.user.walletBalance < this.withdrwBalance) {
                    alert("You can not withdraw more than you have");
                    return;
                }

                try {
                    const response = await fetch('https://localhost:7020/User/DepositToBank', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(this.withdrwBalance),
                    });
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    location.reload();
                } catch (error) {
                    console.error('Error adding sport:', error);
                }
            },
        },
    });</script>

<style scoped>
    table {
        width: 100%;
    }

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
