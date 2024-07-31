<template>
    <div class="sports-component">
        <h1 style="padding:2rem">User Profile</h1>
        <div class="header-row" id="header-row" style="padding: 0px; overflow: hidden; height: 28rem;">
            <div class="container-fluid" style="padding: 0px;">
                <div class="row">
                    <div class="col-xs-12">
                        <a class="navbar-brand logo">
                            <img src="../assets/UserProfile.jpg" style="width: 100%;">
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <div v-if="loading" class="loading">
            Loading... Please wait.
        </div>

        <div v-if="!loading">
            <div class="container">
                <h1 style="padding-top:3rem; text-align:center">
                    {{ user.name }} Current wallet ballance {{ user.walletBalance.toFixed(2)  }} Euro
                </h1>
            </div>
            <div class="container">
                <img style="height: 17rem; padding-left: 38%" src="../assets/Wallet.png">
            </div>
            <div class="container">
                <div class="d-flex justify-content-center" style="padding-bottom:4rem">
                    <input v-model="addToBallance" placeholder="0.00" type="number" min="0" step=".01" precision="2" />
                    <button @click="addBalance" style="margin-right: 2rem; margin-left: 2rem" class="btn btn-primary">Add to Wallet</button>
                </div>
                <div class="d-flex justify-content-center">
                    <input v-model="withdrwBalance" placeholder="0.00" type="number" min="0" step=".01" precision="2" />
                    <button @click="withdrwFounds" style="margin-right: 2rem; margin-left: 2rem" class="btn btn-primary">Deposit to Bank</button>
                </div>

            </div>
            <div class="datatable-container" style="padding-top:7rem; text-align:center">
                <h1> Transaction history</h1>
                <table class="table table-hover" style="height:4rem; overflow-y:auto">
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

            <div style="padding-top:3rem; text-align:center" class="datatable-container">
                <h1>Bet history</h1>
                <table>
                    <thead>
                        <tr>
                            <th>Total odds</th>
                            <th>Stake/ Money on bet</th>
                            <th>Potential winning</th>
                            <th>Did it win</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="ticket in ticketsPlaid" :key="ticket.totalOdd" :class="{ 'bg-success': ticket.didBetWin, 'bg-danger': !ticket.didBetWin}">
                            <td>{{ ticket.totalOdd.toFixed(2)  }}</td>
                            <td>{{ ticket.stake.toFixed(2)}} Euro</td>
                            <td>{{ ticket.potentialWinning.toFixed(2)}} Euro</td>
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
                withdrwBalance: 0,
                addToBallance: 0,
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
                this.addToBallance = Number(this.addToBallance).toFixed(2)
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
                    console.error('Error posting to user:', error);
                }
            },
            async withdrwFounds() {
                this.withdrwBalance = Number(this.withdrwBalance).toFixed(2)
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
                    console.error('Error posting to user:', error);
                }
            },
        },
    });</script>

<style scoped>
    input, button {
        vertical-align: middle;
    }

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

    input {
        -moz-appearance: textfield;
        font: inherit;
        padding: .6em .9em;
        border-radius: .6em;
        border: 1px solid darkgray;
        padding-inline: 2em .9em;
        width: 10em;
    }
</style>
