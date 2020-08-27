<template>
    <div>
        <div class="uk-grid">
            <div class="uk-align-center uk-width-1-4@s">
                <h1 class="uk-heading-xlarge">
                    {{ checkouts.length }}
                </h1>
                <h3 class="uk-heading uk-margin-right uk-margin-left">
                    {{ $t('unreviewed') }}
                </h3>
                <h1 class="uk-heading-xlarge">
                    {{ checkouts.filter(c => new Date(c.time) > week).length }}
                </h1>
                <h3 class="uk-heading uk-margin-right uk-margin-left">
                    {{ $t('unreviewed_week') }}
                </h3>
            </div>
            <div class="uk-align-center uk-width-3-4@s">
                <div class="uk-grid-match uk-child-width-1-3@s uk-text-center" uk-grid>
                    <div v-for="c in checkouts">
                        <div :class="'uk-card uk-card-default uk-card-hover uk-card-body ' + String(c.result) + '-ill'">
                            <h3 class="uk-card-title">{{ $t('main.test') }} {{ c.disease.name }}</h3>
                            <span class="uk-text-meta">
                            {{ $d(new Date(c.time), 'long') }}
                            <br>
                            {{ c.place.name }}
                            </span><br>
                            <span v-if="!c.result">{{ $t('main.resh') }}</span>
                            <span v-else>{{ $t('main.resd') }}</span>
                            <br>
                            <button @click="review(c.chechoutID)" class="uk-margin uk-button uk-button-text">
                                {{ $t('get_reviewed') }}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>

export default {
    name: 'Checkout',
    data: function () {
        return {
            checkouts: [],
            week: new Date().getTime() - (7 * 24 * 60 * 60 * 1000)
        }
    },
    mounted: function () {
        this.$http
        .get(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/report`)
        .then(resp => {
            this.checkouts = resp.data.sort(function(a, b){
                        return Date.parse(b.time) - Date.parse(a.time);
                    });
        });
    },
    methods: {
        review: function (id) {
            this.$http
            .put(`https://localhost:5001/api/review_checkout/${id}`)
            .then(resp => {
                this.$http
                .get(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/report`)
                .then(resp => {
                    UIkit.notification({
                        message: this.$t('review'),
                        status: 'primary',
                        pos: 'top-right',
                        timeout: 5000
                    });
                    this.checkouts = resp.data.sort(function(a, b){
                                return Date.parse(b.time) - Date.parse(a.time);
                            });
                });
            })
        }
    }
}
</script>
