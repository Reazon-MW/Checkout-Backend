import Vue from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import VeeValidate from 'vee-validate'
import validationMessages from 'vee-validate/dist/locale/en'
import ukValidationMessages from 'vee-validate/dist/locale/uk'
import Axios from 'axios'
import i18n from './i18n'
import Gravatar from 'vue-gravatar';

Vue.component('v-gravatar', Gravatar);

Vue.config.productionTip = false

Vue.use(VeeValidate, {
  i18nRootKey: 'validations', // customize the root path for validation messages.
  i18n,
  dictionary: {
    en: validationMessages,
    uk: ukValidationMessages
  }
});

Vue.prototype.$http = Axios;
const token = localStorage.getItem('token');

if (token) {
  Vue.prototype.$http.defaults.headers.common['Authorization'] = token
}

new Vue({
  router,
  store,
  i18n,
  render: function (h) { return h(App) }
}).$mount('#app')
