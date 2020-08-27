import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

var baseUrl = 'https://localhost:5001';

export default new Vuex.Store({
  state: {
    status: '',
    token: localStorage.getItem('token') || '',
    user: {},
    locale: 'en'
  },
  mutations: {
    auth_request(state) {
      state.status = 'loading'
    },
    auth_success(state, token) {
      state.status = 'success'
      state.token = token
    },
    auth_error(state) {
      state.status = 'error'
    },
    set_user(state, user) {
      state.user = user
    },
    logout(state) {
      state.status = ''
      state.token = ''
      state.user = {}
    },
    locale_request(state, locale) {
      state.locale = locale;
    },
    user_restored(state, user) {
      state.status = 'success'
      state.user = user
    }
  },
  actions: {
    login({ commit }, user) {
      console.log(user);
      console.log(baseUrl + `/login?Email=${user.email}&Password=${user.password}`);
      return new Promise((resolve, reject) => {
        commit('auth_request')
        axios({ url: baseUrl + `/login?Email=${user.email}&Password=${user.password}`, method: 'POST' })
          .then(resp => {
            const token = 'Bearer ' + resp.data.access_token
            localStorage.setItem('token', token)
            axios.defaults.headers.common['Authorization'] = token

            axios
            .get(baseUrl + '/api/user', { headers: { Authorization: token } })
            .then(response => {
              console.log('user ' + response.data);
              const user_info = response.data
              commit('set_user', user_info)
            })
            .catch(err => {
              console.log(err.response.data);
            })

            commit('auth_success', token)
            resolve(resp)
          })
          .catch(err => {
            commit('auth_error')
            localStorage.removeItem('token')
            reject(err)
          })
      })
    },
    register({ commit }, user) {
      return new Promise((resolve, reject) => {
          commit('auth_request')
          axios({ url: baseUrl + `/register?Email=${user.email}&Password=${user.password}&Name=${user.name}&Surname=${user.surname}`, method: 'POST' })
            .then(resp => {
              const token = 'Bearer ' + resp.data.access_token
              localStorage.setItem('token', token)
              axios.defaults.headers.common['Authorization'] = token

              axios
              .get(baseUrl + '/api/user', { headers: { Authorization: token } })
              .then(response => {
                console.log('user ' + response.data);
                const user_info = response.data
                commit('set_user', user_info)
              
                commit('auth_success', token)
                resolve(resp)
            })
            .catch(err => {
              reject(err)
            })
        });
      })
    },
    getUser({ commit }) {
      return new Promise((resolve, reject) => {
        axios({ url: baseUrl + '/api/user', method: 'GET' })
          .then(resp => {
            commit('user_restored', resp.data)
            resolve(resp)
          })
          .catch(err => {
            reject(err)
          })
      })
    },
    editUser({ commit }, userParams) {
      return new Promise((resolve, reject) => {
        axios({ url: baseUrl + '/api/user' + userParams, method: 'PUT' })
          .then(resp => {
            commit('user_restored', resp.data)
            resolve(resp)
          })
          .catch(err => {
            reject(err)
          })
      })
    },
    logout({ commit }) {
      return new Promise((resolve, reject) => {
        commit('logout')
        localStorage.removeItem('token')
        delete axios.defaults.headers.common['Authorization']
        resolve()
      })
    }
  },
  modules: {
  },
  getters: {
    isLoggedIn: state => !!state.token,
    userInfo: state => state.user
  }
})
