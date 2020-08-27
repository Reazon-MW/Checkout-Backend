<template>
    <div>
        <br><br><br>
        <form class="uk-form-stacked uk-align-center uk-vertical-align-medium uk-position-center" @submit.prevent="register">
            <br>
            <h1>{{ $t('signup.info') }}</h1>
            <div class="uk-margin-small uk-width-medium uk-align-center">
                <label for="mail">{{ $t('signup.email') }}</label>
                <input v-validate="'required|email'" data-vv-as="mail" class="uk-input" name="mail" 
                type="email" :placeholder="$t('signin.email')" v-model="email" />
                <br><span class="uk-text-danger">{{ errors.first('mail') }}</span>
            </div>

            <div class="uk-margin-small uk-width-medium uk-align-center">
                <label for="name">{{ $t('signup.name') }}</label>
                <input v-validate="'required|min:1|alpha'" data-vv-as="name" class="uk-input" name="name" 
                type="text" :placeholder="$t('signup.np')" v-model="name" />
                <span class="uk-text-danger">{{ errors.first('name') }}</span>
            </div>

            <div class="uk-margin-small uk-width-medium uk-align-center">
                <label for="surname">{{ $t('signup.surname') }}</label>
                <input v-validate="'required|min:1|alpha'" data-vv-as="surname" class="uk-input" name="surname" 
                type="text" :placeholder="$t('signup.sp')" v-model="surname" />
                <br><span class="uk-text-danger">{{ errors.first('surname') }}</span>
            </div>
            
            <div class="uk-margin-small uk-width-medium uk-align-center">
                <label for="password">{{ $t('signup.password') }}</label>
                <input v-validate="'required|alpha_num|regex:(?=.*[0-9])|max:30|min:6'" data-vv-as="password" class="uk-input" name="password" 
                type="password" ref="passw" :placeholder="$t('signin.password')" v-model="password" />
                <br><span class="uk-text-danger">{{ errors.first('password') }}</span>
            </div>

            <div class="uk-margin-small uk-width-medium uk-align-center">
                <label for="repeat">{{ $t('signup.repeat') }}</label>
                <input v-validate="'required|confirmed:passw'" data-vv-as="repeat" class="uk-input" name="repeat" ref="repeat"
                type="password" :placeholder="$t('signup.rp')" v-model="repeat" />
                <br><span class="uk-text-danger">{{ errors.first('repeat') }}</span>
            </div>

            <div class="uk-margin-small">
                <div class="uk-form-controls">
                    <button class="uk-button uk-button-default" type="submit">{{ $t('signup.register') }}</button>
                </div>
            </div>
        </form>
    </div>
</template>
<script>
    export default {
        name: "Register",
        data() {
            return {
                email: "",
                name: "",
                surname: "",
                password: "",
                repeat: ""
            };
        },
        methods: {
            choose_type: function(type) {
                this.type = type;
                this.step = 2;
            },
            comeback: function() {
                this.type = -1;
                this.step = 1;
            },
            register: function() {
                var _this = this;
                this.$validator.validate().then(valid => {
                    if (valid) {
                        var data = {
                            name: _this.name,
                            surname: _this.surname,
                            email: _this.email,
                            password: _this.password
                        };
                        _this.$store
                        .dispatch("register", data)
                        .then(resp => {
                            this.$router.push("/");
                            window.location.reload(true);
                        })
                        .catch(err => UIkit.notification({
                                message: err.response.data.message,
                                status: 'danger',
                                pos: 'top-right',
                                timeout: 5000
                            }));
                    }
                });
        }
        }
    };
</script>

<style>
    .big-form {
        overflow-y: auto;
    }
</style>
