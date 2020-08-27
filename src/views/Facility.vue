<template>
    <div>
        <br>
        <button uk-toggle="target: #create-new" class="uk-button uk-button-default">{{ $t('facility.add') }}</button>
        <div id="create-new" uk-modal>
            <div class="uk-modal-dialog uk-modal-body">
                <button class="uk-modal-close-default" id="close" type="button" uk-close></button>
                <h2 class="uk-modal-title">{{ $t('facility.create') }}</h2>
                <form class="uk-form-stacked" @submit.prevent="">

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-text">{{ $t('facility.name') }}</label>
                        <div class="uk-form-controls">
                            <input v-model="name" class="uk-input" id="form-stacked-text" type="text" :placeholder="$t('facility.name') + '...'">
                        </div>
                    </div>

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-select">{{ $t('facility.select') }}</label>
                        <div class="uk-form-controls">
                            <select class="uk-select" v-model="type">
                                <option disabled value="">{{ $t('facility.pls_select') }}</option>
                                <option value="Private">{{ $t('facility.prv') }}</option>
                                <option value="Government">{{ $t('facility.grv') }}</option>
                            </select>
                        </div>
                    </div>

                    <div class="uk-margin">
                        <button @click="create()" class="uk-button uk-button-default">{{ $t('facility.submit') }}</button>
                    </div>

                </form>
            </div>
        </div>
        
        <ul class="uk-align-center uk-width-1-2@s" uk-accordion="multiple: true">
            <li v-for="(fcl, index) in facilities">
                <a class="uk-accordion-title" href="#">{{ fcl.name }} <sup class="uk-badge">{{ fcl.type }}</sup></a>
                <div class="uk-accordion-content">
                    <div>
                        <div class="uk-button-group">
                            <button @click="ind = index; eName = fcl.name; eType = fcl.type" 
                            uk-toggle="target: #edit-facility" class="uk-button uk-button-default">{{ $t('facility.edit_fac') }}</button>
                            <button @click="deleteId=fcl.facilityID" uk-toggle="target: #delete-facility" class="uk-button uk-button-default">{{ $t('facility.close_fac') }}</button>
                        </div>
                    </div>
                    
                    <br>
                    <router-link :to="{ name: 'Location', params: { facilityId: fcl.facilityID, facilityName: fcl.name } }"
                    class="uk-button uk-button-text uk-margin-right">{{ $t('facility.locs') }}</router-link>
                    <router-link :to="{ name: 'Checkout', params: { facilityId: fcl.facilityID, facilityName: fcl.name } }"
                    class="uk-button uk-button-text uk-margin-left">{{ $t('facility.checks') }}</router-link>
                    <br><br>

                    <span class="uk-text-bold">{{ $t('facility.wrk') }}</span>
                    <div class="uk-child-width-1-2@s uk-grid-match" uk-grid>
                        <div v-for="(wrk, wInd) in fcl.workers">
                            <div class="uk-card uk-card-hover uk-card-body">
                                <h3 class="uk-card-title">
                                    {{ wrk.user.name }}&nbsp;{{ wrk.user.surname }}
                                    <br>
                                    <span class="uk-text-meta">
                                        <span :id="String(wrk.userID) + String(wrk.workID)" contenteditable="true" v-model="wrk.position" class="uk-text-bold">
                                            {{ wrk.position }}
                                        </span><br>
                                        {{ wrk.user.eMail }}
                                    </span>
                                </h3>
                                <button @click="reassignWorker(wrk.userID, wrk.workID, index, wInd)" class="uk-button uk-button-text">{{ $t('facility.reassign') }}</button>
                                <button @click="fireWorker(wrk.workID, fcl.facilityID)" class="uk-button uk-button-text uk-margin-left">{{ $t('facility.fire') }}</button>
                            </div>
                        </div>
                    </div>
                </div>
            </li>
        </ul>

        <div id="edit-facility" uk-modal>
            <div v-if="ind != -1" class="uk-modal-dialog uk-modal-body">
                <button class="uk-modal-close-default" id="editClose" type="button" uk-close></button>
                <h2 class="uk-modal-title">{{ $t('facility.edit_info') }}</h2>
                <form class="uk-form-stacked" @submit.prevent="">

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-text">{{ $t('facility.fcl_name') }}</label>
                        <div class="uk-form-controls">
                            <input v-model="eName" class="uk-input" id="form-stacked-text" type="text" :placeholder="facilities[ind].fclName">
                        </div>
                    </div>

                    <div class="uk-margin">
                        <label class="uk-form-label" for="form-stacked-select">{{ $t('facility.fcl_type') }}</label>
                        <div class="uk-form-controls">
                            <select class="uk-select" :selected="eType" v-model="eType">
                                <option disabled value="">{{ $t('facility.pls_select') }}</option>
                                <option value="Private">{{ $t('facility.prv') }}</option>
                                <option value="Government">{{ $t('facility.grv') }}</option>
                            </select>
                        </div>
                    </div>

                    <div class="uk-margin">
                        <button @click="edit()" class="uk-button uk-button-default">{{ $t('facility.submit') }}</button>
                    </div>

                </form>
            </div>
        </div>        

        <div id="delete-facility" uk-modal>
            <div class="uk-modal-dialog uk-modal-body">
                <button id="delClose" class="uk-modal-close-default" type="button" uk-close></button>
                <h2 class="uk-modal-title">{{ $t('facility.del_confirm') }}</h2>
                <p>
                    <div class="uk-button-group">
                        <button @click="deleteFacility()" class="uk-button uk-button-default">{{ $t('main.confirm') }}</button>
                        <button @click="UIkit.modal('#delete-facility').hide()" class="uk-button uk-button-default">{{ $t('main.cancel') }}</button>
                    </div>
                </p>
            </div>
        </div>
    </div>
</template>
<script>
import Gravatar from 'vue-gravatar';

export default {
    name: 'Facility',
    components: {
        Gravatar
    },
    data: function() {
        return {
            ind: -1,
            name: '',
            type: '',
            eName: '',
            eType: '',
            deleteId: -1,
            facilities: []
        }
    },
    mounted: function () {
        this.$http
        .get('https://localhost:5001/api/facilities/my')
        .then(resp => {
            this.facilities = resp.data;
        })
    },
    methods: {
        create: function() {
            this.$http
            .post(`https://localhost:5001/api/facilities/my?Name=${this.name}&Type=${this.type}`)
            .then(resp => {
                this.$http
                .get('https://localhost:5001/api/facilities/my')
                .then(resp => {
                    this.facilities = resp.data;
                    this.type = '';
                    this.name = '';
                    document.getElementById('close').click();
                })
            });
        },
        deleteFacility: function() {
            this.$http
            .delete(`https://localhost:5001/api/facilities/my/${this.deleteId}`)
            .then(resp => {
                this.$http
                .get('https://localhost:5001/api/facilities/my')
                .then(resp => {
                    this.facilities = resp.data;
                    document.getElementById('delClose').click();
                })
            });
        },
        edit: function() {
            this.$http
            .put(`https://localhost:5001/api/facilities/my/${this.facilities[this.ind].facilityID}?Name=${this.eName}&Type=${this.eType}`)
            .then(resp => {
                this.$http
                .get('https://localhost:5001/api/facilities/my')
                .then(resp => {
                    this.facilities = resp.data;
                    this.type = '';
                    this.name = '';
                    this.ind = -1;
                    document.getElementById('editClose').click();
                })
            });
        },
        reassignWorker: function(userId, workerId, fclNum, wrkNum) {
            var position = document.getElementById(String(userId) + String(workerId));
            if (position.innerText == this.facilities[fclNum].workers[wrkNum].position) {
                position.focus();
                UIkit.notification({
                    message: this.$t('facility.pos_change'),
                    status: 'primary',
                    pos: 'top-right',
                    timeout: 5000
                });
            }
            else {
                this.$http
                .put(`https://localhost:5001/api/facilities/my/${this.facilities[fclNum].facilityID}\
                /workers/${workerId}?Position=${position.innerText}`)
                .then(resp => {
                    this.facilities[fclNum].workers[wrkNum].position = position.innerText;
                    UIkit.notification({
                        message: this.$t('facility.done'),
                        status: 'primary',
                        pos: 'top-right',
                        timeout: 5000
                    });
                });
            }
        },
        fireWorker: function(workerId, facilityId) {
            this.$http
            .delete(`https://localhost:5001/api/facilities/my/${facilityId}\
            /workers/${workerId}`)
            .then(resp => {
                UIkit.notification({
                    message: this.$t('facility.fired'),
                    status: 'primary',
                    pos: 'top-right',
                    timeout: 5000
                });
            });
        }
    }
}
</script>