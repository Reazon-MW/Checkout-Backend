<template>
    <div>
        <br>
        <button uk-toggle="target: #create-new" class="uk-button uk-button-default">{{ $t('schedule.add') }}</button>
        
        <div id="create-new" uk-modal>
            <div class="uk-modal-dialog uk-modal-body">
                <button class="uk-modal-close-default" id="close" type="button" uk-close></button>
                <h2 class="uk-modal-title">{{ $t('schedule.create') }}</h2>
                <form class="uk-form-stacked" @submit.prevent="">

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-text">{{ $t('schedule.interval') }}</label>
                        <div class="uk-form-controls">
                            <input v-model="interval" class="uk-input" id="form-stacked-text" type="number" min="20" :placeholder="$t('schedule.int') + '...'">
                        </div>
                    </div>

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-select">{{ $t('schedule.select') }}</label>
                        <div class="uk-form-controls">
                            <select class="uk-select" v-model="diseaseId">
                                <option disabled value="">{{ $t('schedule.select_ds') }}</option>
                                <option v-for="dcs in diseases" :value="dcs.diseaseID">{{ dcs.name }}</option>
                            </select>
                        </div>
                    </div>

                    <div class="uk-margin">
                        <button @click="createSchedule()" class="uk-button uk-button-default">{{ $t('facility.submit') }}</button>
                    </div>

                </form>
            </div>
        </div>

        <br>
        <div class="uk-child-width-1-5@s uk-grid-match" uk-grid>
            <div v-for="(sch, index) in schedules">
                <div class="uk-card uk-card-default uk-card-hover uk-card-body">
                    <h3 class="uk-card-title">
                        {{ sch.disease.name }}
                        <br>
                        <span class="uk-text-meta">{{ sch.disease.description }}</span>
                    </h3>
                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-text">{{ $t('schedule.check_int') }}</label>
                        <div class="uk-form-controls">
                            <input min="20" class="uk-input" id="form-stacked-text" type="number" v-model="sch.intervalMin">
                        </div>
                    </div>
                    <button @click="changeInterval(index)" class="uk-button uk-button-default">{{ $t('schedule.apply') }}</button>
                    <br><br>
                    <button @click="deleteSchedule(sch.scheduleID, index)" class="uk-button uk-button-text">{{ $t('schedule.remove') }}</button>
                </div>
            </div>
        </div>
    </div>
</template>
<script>

export default {
    name: 'ScheduleChange',
    data: function () {
        return {
            schedules: [],
            diseases: [],
            interval: 20,
            diseaseId: -1
        }
    },
    mounted: function () {
        this.$http
        .get(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places/${this.$route.params.placeId}/schedules`)
        .then(resp => {
            this.schedules = resp.data;
        });

        this.$http
        .get(`https://localhost:5001/api/diseases`)
        .then(resp => {
            this.diseases = resp.data;
        })
    },
    methods: {
        createSchedule: function () {
            this.$http
            .post(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places/${this.$route.params.placeId}\
            /schedules?DiseaseID=${this.diseaseId}&IntervalMin=${this.interval}`)
            .then(resp => {
                this.$http
                .get(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places/${this.$route.params.placeId}/schedules`)
                .then(resp => {
                    this.schedules = resp.data;
                });
                this.interval = 20;
                this.diseaseID = -1;
                document.getElementById('close').click();
            });
        },
        changeInterval: function (index) {
            this.$http
            .put(`https://localhost:5001/api/facilities/my/\
            ${this.$route.params.facilityId}/places/${this.$route.params.placeId}\
            /schedules/${this.schedules[index].scheduleID}?IntervalMin=${this.schedules[index].intervalMin}`)
            .then(resp => {
                UIkit.notification({
                    message: this.$t('schedule.int_ch'),
                    status: 'primary',
                    pos: 'top-right',
                    timeout: 5000
                });
            });
        },
        deleteSchedule: function (id, index) {
            this.$http
            .delete(`https://localhost:5001/api/facilities/my/\
            ${this.$route.params.facilityId}/places/${this.$route.params.placeId}\
            /schedules/${id}`)
            .then(resp => {
                UIkit.notification({
                    message: this.$t('schedule.removed'),
                    status: 'primary',
                    pos: 'top-right',
                    timeout: 5000
                });
                this.schedules.splice(index, 1);
            });
        }
    }
}
</script>