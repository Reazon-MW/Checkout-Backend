<template>
    <div>
        <FullCalendar :options="calendarOptions" />
    </div>
</template>
<script>
import FullCalendar from '@fullcalendar/vue'
import timeGridPlugin from '@fullcalendar/timegrid'
import rrulePlugin from '@fullcalendar/rrule'

export default {
    name: 'Schedule',
    components: {
        FullCalendar
    },
    props: ['locationId'],
    mounted: function () {
        var _this = this;
        
        _this.$http
        .get('https://localhost:5001/api/user/checkouts')
        .then(response => {
            var result = [];

            for (let i = 0; i < response.data.length; ++i) {
                if (response.data[i].place.placeID == _this.$route.params.locationId) {
                    if (response.data[i].result)
                        result.push({
                            groupId: 'redEvents',
                            title: response.data[i].disease.name + _this.$t('checkout'),
                            backgroundColor: 'firebrick',
                            start: response.data[i].time
                        });
                    else
                        result.push({
                            groupId: 'redEvents',
                            title: response.data[i].disease.name + _this.$t('checkout'),
                            backgroundColor: 'seagreen',
                            start: response.data[i].time
                        });
                }
            }

            _this.$http
            .get(`https://localhost:5001/api/user/works/${_this.$route.params.workId}/places/${_this.$route.params.locationId}/schedules`)
            .then(resp => {
                var schedule = resp.data;
                var checks = response.data;

                for (let i = 0; i < schedule.length; ++i){
                    var id = resp.data[i].disease.diseaseID;
                    var diseaseLast = checks.filter(check => check.disease.diseaseID == id).sort(function(a, b){
                        return Date.parse(b.time) - Date.parse(a.time);
                    })[0];

                    console.log(checks.filter(check => check.disease.diseaseID == id).sort(function(a, b){
                        return Date.parse(b.time) - Date.parse(a.time);
                    }));

                    console.log(diseaseLast);

                    if (diseaseLast == -Infinity || 
                    ((new Date() - Date.parse(diseaseLast.time)) / 1000) / 60 > resp.data[i].intervalMin) {
                        var start = new Date();
                        //console.log(start);
                        start.setHours(start.getHours() + (-(new Date().getTimezoneOffset()) / 60));
                        
                        result.push({
                            groupId: 'next',
                            title: resp.data[i].disease.name + _this.$t('checkout'),
                            rrule: {
                                freq: 'minutely',
                                interval: resp.data[i].intervalMin,
                                dtstart: start,
                                until: '2020-12-31'
                            }
                        })
                    }
                    else {
                        var start = new Date(Date.parse(diseaseLast.time) + resp.data[i].intervalMin*60000);
                        start.setHours(start.getHours() + (-(new Date().getTimezoneOffset()) / 60));
                        result.push({
                            groupId: 'next',
                            title: resp.data[i].disease.name + _this.$t('checkout'),
                            rrule: {
                                freq: 'minutely',
                                interval: resp.data[i].intervalMin,
                                dtstart: start,
                                until: '2020-12-31'
                            }
                        })
                    }
                }
            })

            _this.calendarOptions.events = result;
        })
    },
    data() {
        return {
            calendarOptions: {
                plugins: [ timeGridPlugin, rrulePlugin ],
                timeZone: 'local',
                initialView: 'timeGridWeek',
                locale: this.$i18n.locale,
                height: 'auto',
                cellHeight: 10,
                scrollTime: '08:00:00',
                allDaySlot: false,
                nowIndicator: true,
                headerToolbar: {
                    start: 'title',
                    center: '',
                    end: 'prev,next'
                },
                events: [],
                editable: false
            }
        }
    }
}
</script>
