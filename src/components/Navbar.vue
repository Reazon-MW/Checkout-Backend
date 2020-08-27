<template>
    <div id="topnav" uk-sticky="sel-target: .uk-navbar-container; show-on-up: true; cls-active: uk-navbar-sticky">
    
        <nav class="uk-navbar-container" uk-navbar>

            <div class="uk-navbar-left">
                <a class="uk-navbar-toggle" uk-navbar-toggle-icon uk-toggle="target: #offcanvas-nav" href=""></a>    
            </div>

            <div class="uk-navbar-center">
                <router-link v-if="logo" to="/" class="uk-navbar-item uk-logo">
                    <img class="uk-width-1-1 uk-height-1-1" src="../assets/logo.png" />
                </router-link>
            </div>

            <div class="uk-navbar-right">
                <router-link v-if="!isLoggedIn" to="/login">
                    <span class="uk-margin-small-right" uk-icon="icon: sign-in"></span>
                </router-link>
                &nbsp;&nbsp;&nbsp;
                <router-link v-if="!isLoggedIn" to="/register">
                    <span class="uk-margin-small-right" uk-icon="icon: user"></span>
                    &nbsp;&nbsp;&nbsp;
                </router-link>

                <a class="uk-navbar-toggle" v-if="isLoggedIn" @click="startSearch()" :uk-tooltip="$t('srch_work')"
                uk-search-icon uk-toggle="target: .nav-overlay; animation: uk-animation-fade" href="#"></a>
            </div>

            <div class="nav-overlay uk-navbar-left uk-flex-1" hidden>

                <div class="uk-navbar-item uk-width-expand">
                    <form class="uk-search uk-search-navbar uk-width-1-1">
                        <input v-model="workSearch" class="uk-search-input" type="search" :placeholder="$t('search') + '...'" autofocus>
                    </form>
                </div>

                <a class="uk-navbar-toggle" @click="logo=true" uk-close uk-toggle="target: .nav-overlay; animation: uk-animation-fade" href="#"></a>

            </div>

        </nav>

        <ul v-if="!logo" id="works" 
        class="uk-list uk-position-fixed uk-position-top-center" style="background: #B1DBFF!important; z-index: 2; width: 62vw; top: 9vh!important">
            <li v-for="work in result.slice(0, 5)">
                <span class="uk-text-bold">{{ work.name }}</span> <i @click="apply(work.facilityID)" uk-icon="icon: bookmark"></i> 
            </li>
        </ul>

    </div>
</template>

<script>
import Avatar from 'vue-avatar'
import baseUrl from '../store/index.js'

    export default {
        name: 'TopNav',
        data: function() {
            return {
                logo: true,
                workSearch: '',
                result: [],
                works: []
            }
        },
        watch: {
            workSearch: function (val) {
                if (val == '' || val == null || val == undefined) {
                    this.result = this.works;
                }
                else {
                    this.result = [];
                    for (let i = 0; i < this.works.length; ++i) {
                        if (this.works[i].name.toLowerCase().includes(val.toLowerCase())) {
                            this.result.push(this.works[i])
                        }
                    }
                }
            }
        },
        components: {
            Avatar
        },
        mounted: function () {
            if (this.$store.getters.isLoggedIn) {
                if (Object.keys(this.$store.getters.userInfo).length === 0 && 
                this.$store.getters.userInfo.constructor === Object) {
                    this.$store
                        .dispatch("getUser")
                }
            }
        },
        methods: {
            startSearch: function() {
                this.logo = false;
                var _this = this;
                this.$http
                .get(
                    'https://localhost:5001/api/facilities/all'
                )
                .then(response => {
                    _this.works = response.data;
                    _this.result = response.data;
                })
                .catch(err => {

                })
            },
            apply: function(id) {
                var _this = this;
                this.$http
                .post(`https://localhost:5001/api/user/works?FacilityID=${id}`)
                .then(respone => {
                    UIkit.notification({
                        message: this.$t('applied'),
                        status: 'primary',
                        pos: 'top-right',
                        timeout: 5000
                    });
                    _this.workSearch = '';
                })
            }
        },
        computed: {
            isLoggedIn: function() {
                return this.$store.getters.isLoggedIn;
            },
            userName: function() {
                if (this.$store.getters.isLoggedIn) {
                    return this.$store.getters.userName;
                }
            }
        }
    }
</script>

<style scoped>
    #topnav {
        overflow: hidden;
    }

    .uk-navbar-container {
        background: #B1DBFF!important;
    }

    a {
        color: black;
    }
</style>
