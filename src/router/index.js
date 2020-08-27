import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

  const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/login',
    name: 'Login',
    component: function () {
      return import('../views/Login.vue')
    }
  },
  {
    path: '/register',
    name: 'Register',
    component: function () {
      return import('../views/Register.vue')
    }
  },
  {
    path: '/schedule/:locatiodId/',
    name: 'Schedule',
    component: function () {
      return import('../views/Schedule.vue')
    }
  },
  {
    path: '/facilities/',
    name: 'Facility',
    component: function () {
      return import('../views/Facility.vue')
    }
  },
  {
    path: '/facilities/locations',
    name: 'Location',
    component: function () {
      return import('../views/Location.vue')
    }
  },
  {
    path: '/facilities/locations/schedules',
    name: 'ScheduleChange',
    component: function () {
      return import('../views/ScheduleChange.vue')
    }
  },
  {
    path: '/facilities/checkouts',
    name: 'Checkout',
    component: function () {
      return import('../views/Checkouts.vue')
    }
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
