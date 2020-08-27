<template>
    <div>
        <h2>
           {{ $route.params.facilityName }} 
        </h2>
        <button uk-toggle="target: #create-new" class="uk-button uk-button-default">{{ $t('location.add') }}</button>
        
        <div id="create-new" uk-modal>
            <div class="uk-modal-dialog uk-modal-body">
                <button class="uk-modal-close-default" id="close" type="button" uk-close></button>
                <h2 class="uk-modal-title">{{ $t('location.create') }}</h2>
                <form class="uk-form-stacked" @submit.prevent="">

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-text">{{ $t('location.name') }}</label>
                        <div class="uk-form-controls">
                            <input v-model="name" class="uk-input" id="form-stacked-text" type="text" :placeholder="$t('location.name') + '...'">
                        </div>
                    </div>

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-text">{{ $t('location.address') }}</label>
                        <div class="uk-form-controls">
                            <input v-model="address" class="uk-input" id="form-stacked-text" type="text" :placeholder="$t('location.address') + '...'">
                        </div>
                    </div>

                    <div class="uk-margin">
                        <button @click="create()" class="uk-button uk-button-default">{{ $t('facility.submit') }}</button>
                    </div>

                </form>
            </div>
        </div>

        <br>
        <div class="uk-child-width-1-5@s uk-grid-match" uk-grid>
            <div v-for="(plc, index) in places">
                <div class="uk-card uk-card-default uk-card-hover uk-card-body">
                    <h3 class="uk-card-title">
                        {{ plc.name }}
                        <div class="uk-text-meta">
                            {{ plc.address }}
                        </div>
                    </h3>
                    <router-link :to="{ name: 'ScheduleChange', 
                    params: { placeId: plc.placeID, placeName: plc.name, facilityId: $route.params.facilityId, facilityName: $route.params.facilityName } }">
                        {{ plc.schedules.length }} {{ $t('location.sch') }}
                    </router-link>
                    <br>
                    <button uk-toggle="target: #edit" @click="prepareEdit(index)" class="uk-button uk-button-text">{{ $t('location.edit') }}</button>
                    <button @click="deletePlace(plc.placeID)" class="uk-button uk-button-text ">{{ $t('location.delete') }}</button>
                </div>
            </div>
        </div>

        <div id="edit" uk-modal>
            <div v-if="ind != -1" class="uk-modal-dialog uk-modal-body">
                <button class="uk-modal-close-default" id="eclose" type="button" uk-close></button>
                <h2 class="uk-modal-title">{{ $t('location.edit_loc') }}</h2>
                <form class="uk-form-stacked" @submit.prevent="">

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-text">{{ $t('location.name') }}</label>
                        <div class="uk-form-controls">
                            <input v-model="eName" class="uk-input" id="form-stacked-text" type="text" :placeholder="places[ind].name">
                        </div>
                    </div>

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-text">{{ $t('location.address') }}</label>
                        <div class="uk-form-controls">
                            <input v-model="eAddress" class="uk-input" id="form-stacked-text" type="text" :placeholder="places[ind].address">
                        </div>
                    </div>

                    <div class="uk-margin">
                        <button @click="editLocation()" class="uk-button uk-button-default">{{ $t('facility.submit') }}</button>
                    </div>

                </form>
            </div>
        </div>
    </div>
</template>
<script>

export default {
    name: 'Location',
    data: function () {
        return {
            ind: -1,
            places: [],
            facilityName: '',
            address: '',
            name: '',
            eAddress: '',
            eName: ''
        }
    },
    mounted: function () {
        var _this = this;
        
        _this.$http
        .get(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places`)
        .then(response => {
            this.places = response.data;
            this.facilityName = this.$route.params.facilityName;
        })
    },
    methods: {
        create: function () {
            this.$http
            .post(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places?Name=${this.name}&Address=${this.address}`)
            .then(resp => {
                this.$http
                .get(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places`)
                .then(resp => {
                    this.places = resp.data;
                    this.address = '';
                    this.name = '';
                    document.getElementById('close').click();
                })
            });
        },
        deletePlace: function (id) {
            this.$http
            .delete(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places/${id}`)
            .then(resp => {
                this.$http
                .get(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places`)
                .then(resp => {
                    this.places = resp.data;
                })
            });
        },
        prepareEdit: function (index) {
            this.ind = index;
            this.eName = this.places[index].name;
            this.eAddress = this.places[index].address;
        },
        editLocation: function () {
            var params = '?';
            if (this.eName != '') {
                params += `Name=${this.eName}`;
            }
            if (this.eAddress != '' && params == '?') {
                params += `eAddress=${this.eAddress}`;
            }
            if (this.eAddress != '' && params != '?') {
                params += `&eAddress=${this.eAddress}`;
            }

            this.$http
            .put(`https://localhost:5001/api/facilities/my/${this.$route.params.facilityId}/places/${this.places[this.ind].placeID}` + params)
            .then(resp => {
                this.places[this.ind].name = this.eName;
                this.places[this.ind].address = this.eAddress;
                this.eAddress = '';
                this.eName = '';
                document.getElementById('eclose').click();
            });
        }
    }
}
</script>