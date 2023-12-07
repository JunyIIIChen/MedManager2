$(document).ready(function () {
    // 获取数据并初始化日历
    $.ajax({
        type: "GET",
        url: "/Clinics/GetClinicAppointments/@ClinicId",
        success: function (data) {
            var events = [];
            $.each(data, function (i, v) {
                events.push({
                    title: 'Booked',
                    start: moment(v.StartTime),
                    end: moment(v.EndTime),
                    color: '#ff0000' // 例如，将已预约的时间段显示为红色
                });
            });

            // 初始化日历
            $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                defaultView: 'agendaWeek', // 默认为周视图
                slotDuration: '00:15:00', // 时段为15分钟
                businessHours: { // 工作时间，可以根据需要调整
                    start: '09:00', // 开始时间
                    end: '18:00',   // 结束时间
                    daysOfWeek: [1, 2, 3, 4, 5] // 周一到周五
                },
                events: events
            });
        }
    });
});
