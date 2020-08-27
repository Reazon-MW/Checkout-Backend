<template>
    <div id="offcanvas-nav" uk-offcanvas="mode: push; overlay: true">
        <div class="uk-offcanvas-bar uk-flex uk-flex-column">
            <button class="uk-offcanvas-close" type="button" uk-close></button>

            <ul class="uk-nav uk-nav-primary uk-nav-center uk-margin-auto-vertical">
                <li>
                    <router-link to="/">
                        <span class="uk-margin-small-right" uk-icon="icon: home"></span> {{ $t('menu.home') }}
                    </router-link>
                </li>
                <li v-if="!isLoggedIn">
                    <router-link to="/login">
                        <span class="uk-margin-small-right" uk-icon="icon: sign-in"></span> {{ $t('menu.login') }}
                    </router-link>
                </li>
                <li v-if="!isLoggedIn">
                    <router-link to="/register">
                        <span class="uk-margin-small-right" uk-icon="icon: user"></span> {{ $t('menu.register') }}
                    </router-link>
                </li>
                <li v-if="isLoggedIn">
                    <router-link to="/facilities">
                        <span class="uk-margin-small-right" uk-icon="icon: file-text"></span> {{ $t('menu.facilities') }}
                    </router-link>
                </li>
                <li v-if="isLoggedIn" class="uk-nav-divider"></li>
                <li v-if="isLoggedIn">
                    <a @click="logout">
                        <span class="uk-margin-small-right" uk-icon="icon: sign-out"></span>
                        {{ $t('menu.logout') }}
                    </a>
                </li>
                <li>
                    <select v-model="$i18n.locale" class="uk-select">
                        <option v-for="(lang, i) in langs" 
                        :key="`Lang${i}`" :value="lang">{{ fullLangs[i] }}</option>
                    </select>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'OffCanvas',
        data: function () {
            return {
                langs: ['en', 'uk'],
                fullLangs: ['English', 'Українська']
            }
        },
        computed: {
            locale: function () {
                return this.$i18n.locale;
            },
            isLoggedIn: function() {
                return this.$store.getters.isLoggedIn;
            },
            isUtilizer: function() {
                return this.$store.getters.isUtilizer;
            }
        },
        watch: {
            locale: function (val) {
                if (val == 'en') {
                    localStorage.setItem('locale', 'en_UK');
                    this.$http.defaults.headers.common['Accept-Language'] = 'en_UK';
                    this.$store
                    .dispatch('setLocale', 'en')
                }
                else if (val == 'uk') {
                    localStorage.setItem('locale', 'uk_UA');
                    this.$http.defaults.headers.common['Accept-Language'] = 'uk_UA';
                    this.$store
                    .dispatch('setLocale', 'uk')
                }
            }
        },
        methods: {
            logout: function() {
                this.$store.dispatch("logout");
                this.$router.push("/login")
            }
        }
    }
</script>

<style scoped>
 .uk-offcanvas-bar {
     background: #B1DBFF!important;
     color: black!important;
 }
 a, option, select, button{
     color: black!important;
 }
 select{
     border-color: black;
 }
</style>
