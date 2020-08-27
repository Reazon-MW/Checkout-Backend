<template>
  <div class="home">
    <div v-if="isLoggedIn" class="uk-grid">
        <div class="uk-align-center uk-width-1-4@s">
          <v-gravatar class="uk-margin-top" id="avatar" :email="$store.state.user.eMail" 
          :size="200" default-img="identicon" />
          <br><br>
          <div v-if="!edit">
            <span class="uk-text-bold uk-text-lead">
              {{ $store.state.user.name }} {{ $store.state.user.surname }}
            </span><br>
            <span class="uk-text-meta">
              <span uk-icon="icon: mail"></span> &nbsp;&nbsp;&nbsp; {{ $store.state.user.eMail }}
            </span>
          </div><br>
          <button @click="edit = true" v-if="!edit" class="uk-button uk-button-default uk-width-1-2">
            {{ $t('main.edit') }}
          </button>
          
          <form v-else class="uk-form-stacked" @submit.prevent="">
            <div class="uk-margin">
                <label class="uk-form-label uk-text-bold" for="email">{{ $t('signup.email') }}</label>
                <div class="uk-form-controls">
                    <input v-validate="'email'" class="uk-input uk-width-medium" id="email" v-model="email"
                     type="text" :placeholder="$store.state.user.eMail" name="mail">
                </div>
                <span class="uk-text-danger">{{ errors.first('mail') }}</span>
            </div>
            <div class="uk-margin">
                <label class="uk-form-label uk-text-bold" for="name">{{ $t('signup.name') }}</label>
                <div class="uk-form-controls">
                    <input v-validate="'alpha'" class="uk-input uk-width-medium" id="name" v-model="name"
                     type="text" :placeholder="$store.state.user.name" name="name">
                </div>
                <span class="uk-text-danger">{{ errors.first('name') }}</span>
            </div>
            <div class="uk-margin">
                <label class="uk-form-label uk-text-bold" for="surname">{{ $t('signup.surname') }}</label>
                <div class="uk-form-controls">
                    <input v-validate="'alpha'" class="uk-input uk-width-medium" id="surname" v-model="surname" 
                    type="text" :placeholder="$store.state.user.surname" name="surname">
                </div>
                <span class="uk-text-danger">{{ errors.first('surname') }}</span>
            </div>
            <div class="uk-button-group">
              <button class="uk-button uk-button-default" @click="edit=false; name=''; surname=''; email=''">{{ $t('main.cancel') }}</button>
              <button class="uk-button uk-button-default" @click="editUser()">{{ $t('main.confirm') }}</button>
            </div>
          </form>
          
          <div>
            <br>
            <span class="uk-text-bold">
              {{ $t('main.works') }}
            </span>
            <ul class="uk-list" :key="works.length" v-if="works.length != 0">
                <li v-for="(work, index) in works" :key="work.workID">
                  <router-link v-if="work.Work.currentPlace != null"  
                  :to="{ name: 'Schedule', params: {locationId: work.Work.currentPlaceID, workId: work.Work.workID } }">
                    <span
                    uk-icon="icon: bolt"></span> &nbsp;&nbsp;&nbsp; {{ work.Work.position }}
                    <br><span class="uk-text-meta">{{ work.FacilityName }}</span>
                  </router-link>
                  <span v-else>
                    {{ work.Work.position }}
                    <br><span class="uk-text-meta">{{ work.FacilityName }}</span>
                  </span>
                  <br>
                  <span @click="quit(work.Work.workID)"
                  v-if="edit" uk-icon="icon: close" style="cursor: pointer">{{ $t('main.fire') }}</span>
                </li>
            </ul>
            <div :key="works.length + 1" v-else>
              {{ $t('main.noworks') }}
            </div>
          </div>
        </div>

        <div uk-filter="target: .js-filter" class="uk-align-center uk-width-3-4@s">
          <div>
            <ul class="uk-subnav uk-subnav-pill">
              <li class="uk-active" uk-filter-control="sort: data-date; order: desc"><a href="#">{{ $t('main.desc') }}</a></li>
              <li uk-filter-control="sort: data-date"><a href="#">{{ $t('main.asc') }}</a></li>
            </ul>
          </div>
          <div class="uk-grid-small uk-grid-divider uk-child-width-auto" uk-grid>
              <div>
                  <ul class="uk-subnav uk-subnav-pill" uk-margin>
                      <li class="uk-active" uk-filter-control><a href="#">{{ $t('main.all') }}</a></li>
                  </ul>
              </div>
              <div>
                  <ul class="uk-subnav uk-subnav-pill" uk-margin>
                      <li v-for="f in facilities"
                      :uk-filter-control="'[data-color=' + f + ']'"><a href="#">{{ f }}</a></li>
                  </ul>
              </div>
              <div>
                  <ul class="uk-subnav uk-subnav-pill" uk-margin>
                      <li uk-filter-control="[data-size='false']"><a href="#">{{ $t('main.healthy') }}</a></li>
                      <li uk-filter-control="[data-size='true']"><a href="#">{{ $t('main.ill') }}</a></li>
                  </ul>
              </div>
          </div>
          <div class="js-filter uk-grid-match uk-child-width-1-3@s uk-text-center" uk-grid>
              <div v-for="c in checkouts" :data-color="facDict[c.place.facilityID]" :data-size="String(c.result)" :data-date="c.time">
                  <div :class="'uk-card uk-card-default uk-card-hover uk-card-body ' + String(c.result) + '-ill'">
                      <h3 class="uk-card-title">{{ $t('main.test') }} {{ c.disease.name }}</h3>
                      <span class="uk-text-meta">
                        {{ $d(new Date(c.time), 'long') }}
                        <br>
                        {{ facDict[c.place.facilityID] }}, {{ c.place.name }}
                      </span><br>
                      <span v-if="!c.result">{{ $t('main.resh') }}</span>
                      <span v-else>{{ $t('main.resd') }}</span>
                  </div>
              </div>
          </div>
        </div>
    </div>
  </div>
</template>

<script>
import Gravatar from 'vue-gravatar';
import baseUrl from '../store/index.js'

export default {
  name: 'Home',
  components: {
    Gravatar
  },
  data: function () {
    return {
      edit: false,
      workKey: 0,
      works: [],
      facilities: [],
      checkouts: [],
      facDict: {},
      email: '',
      name: '',
      surname: ''
    }
  },
  computed: {
    isLoggedIn: function() {
        return this.$store.getters.isLoggedIn;
    }
  },
  watch: {
    works: function(val) {
      this.$forceUpdate();
    }
  },
  mounted: function () {
    if (!this.$store.getters.isLoggedIn) {
      this.$router.push({ name: 'Login' })
    }

    var _this = this;
    this.$http.get(
      'https://localhost:5001/api/user/works'
    )
    .then(response => {
      _this.works = response.data;
      for (let i = 0; i < _this.works.length; ++i) {
        if (_this.facilities.indexOf(_this.works[i].FacilityName) == -1) {
          _this.facilities.push(_this.works[i].FacilityName);
          _this.facDict[_this.works[i].Work.facilityID] = _this.works[i].FacilityName;
        }
      }
    })
    .catch(err => {

    });
    this.$http.get(
      'https://localhost:5001/api/user/checkouts'
    )
    .then(response => {
      _this.checkouts = response.data.sort(function(a, b){
                        return Date.parse(b.time) - Date.parse(a.time);
                    });
    })
  },
  methods: {
    editUser: function () {
      this.$validator.validate().then(valid => {
          if (valid) {
              let userParams = '?';
              if (this.email != '') {
                userParams += `Email=${this.email}`;
              }
              
              if (this.name != '') {
                if (userParams != '?')
                  userParams += `&Name=${this.name}`;
                else
                  userParams += `Name=${this.name}`;
              }

              if (this.surname != '') {
                if (userParams != '?')
                  userParams += `&Surname=${this.surname}`;
                else
                  userParams += `Surname=${this.surname}`;
              }

              if (userParams == '?')
                userParams = '';

              var _this = this;

              this.$store
                  .dispatch("editUser", userParams)
                  .then(() => {
                      UIkit.notification({
                          message: 'You succesully edited profile',
                          status: 'success',
                          pos: 'top-right',
                          timeout: 5000
                      });
                      _this.edit = false;
                      _this.email = '';
                      _this.name = '';
                      _this.surname = '';
                  })
                  .catch(err => {
                          UIkit.notification({
                              message: err.response.data.message,
                              status: 'danger',
                              pos: 'top-right',
                              timeout: 5000
                          });
                      }
                  );
          }
      });
    },
    quit: function (id) {
      this.$http
      .delete(`https://localhost:5001/api/user/works/${id}`)
      .then(resp => {
        this.$http.get(
          'https://localhost:5001/api/user/works'
        )
        .then(response => {
          this.works = response.data;
          for (let i = 0; i < this.works.length; ++i) {
            if (this.facilities.indexOf(this.works[i].FacilityName) == -1) {
              this.facilities.push(this.works[i].FacilityName);
              this.facDict[this.works[i].Work.facilityID] = this.works[i].FacilityName;
            }
          }
        });
      });
    }
  }
}
</script>

<style>
  #avatar {
    border-radius: 50%;
  }

  .true-ill {
    border-top: 5px solid firebrick;
    border-radius: 6px;
  }

  .false-ill {
    border-top: 5px solid seagreen;
    border-radius: 6px;
  }
</style>
