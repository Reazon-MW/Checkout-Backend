<template>
  <div class="about">
    <form class="uk-form-stacked uk-align-center uk-vertical-align-medium uk-position-center" @submit.prevent="login">
        <h1>{{ $t('signin.title') }}</h1>
        <div class="uk-margin">
            <div class="uk-inline">
                <span class="uk-form-icon" uk-icon="icon: mail"></span>
                <input v-validate="'required|email'" data-vv-as="mail" class="uk-input" name="mail" 
                type="email" :placeholder="$t('signin.email')" v-model="email" />
            </div>
            <br><span class="uk-text-danger">{{ errors.first('mail') }}</span>
        </div>

        <div class="uk-margin">
            <div class="uk-inline">
                <span class="uk-form-icon" uk-icon="icon: lock"></span>
                <input v-validate="'required|alpha_num|regex:(?=.*[0-9])|max:30|min:6'" data-vv-as="password" class="uk-input" name="password" 
                type="password" :placeholder="$t('signin.password')" v-model="password" />
            </div>
            <br><span class="uk-text-danger">{{ errors.first('password') }}</span>
        </div>

        <div class="uk-margin">
            <div class="uk-form-controls">
              <button class="uk-button uk-button-default" type="submit">{{ $t('signin.login') }}</button>  
            </div>
        </div>
    </form>
  </div>
</template>

<script>
export default {
    name: "Login",
    data() {
        return {
            email: "",
            password: ""
        };
    },
    methods: {
        login: function() {
            this.$validator.validate().then(valid => {
                if (valid) {
                    let email = this.email;
                    let password = this.password;
                    this.$store
                        .dispatch("login", { email, password })
                        .then(() => {
                            this.$router.push("/");
                            window.location.reload(true);
                        })
                        .catch(err => {
                                console.log(err);
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
        }
    }
};
</script>
