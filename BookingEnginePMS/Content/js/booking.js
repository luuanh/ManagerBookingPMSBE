

$(document).ready(function () {
    var getCheckOut = false;
    
    var checkIn = new Date();
    var checkOut = new Date();
    checkOut.setDate(checkIn.getDate() + 1);
    var termDate = new Date();

    var arrayDate = new Array();
    arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkIn));
    arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkOut));

    $("#Arrive").val($.datepicker.formatDate('D, M d yy', checkIn));
    $("#Depart").val($.datepicker.formatDate('D, M d yy', checkOut));
    $("#Arrive").datepicker({
        showAnim: 'show',
        setDate: '1/1/2018',
        numberOfMonths: 1,
        duration: 600,
        minDate: new Date(),
        onSelect: function (dataText, inst) {
            getCheckOut = false;
            checkIn = $(this).datepicker("getDate");
            termDate = $(this).datepicker("getDate");
            termDate.setDate(checkIn.getDate() + 1);
            checkOut = termDate;
            $('#Depart').datepicker('option', { minDate: checkIn });
            arrayDate = new Array();
            arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkIn));
            arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkOut));

            //Hiển thị những ngày được chọn trên datepicker (Display inline) 
            ShowDate();

        },
        beforeShowDay: function (date) {

            var getDay = parseInt(date.getDate()) < 10 ? "0" + date.getDate() : date.getDate();
            var getMonth = parseInt(date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var theday = getMonth + '/' + getDay + '/' + date.getFullYear();
            return [true, $.datepicker.formatDate('mm/dd/yy', checkIn) == theday ? "first-specialDate" : $.datepicker.formatDate('mm/dd/yy', checkOut) == theday ? "last-specialDate" : $.inArray(theday, arrayDate) >= 0 ? "specialDate" : ""];
        },
        beforeShow: function () {
            $("#Arrive").val("Select a date");
        },
        onClose: function () {
            $("#Arrive").val($.datepicker.formatDate('D, M d yy', checkIn));
            $("#Depart").val($.datepicker.formatDate('D, M d yy', checkOut));
        }
    });

    $("#Depart").datepicker({
        showAnim: 'show',
        numberOfMonths: 1,
        duration: 600,
        minDate: checkOut,
        onSelect: function (dataText, inst) {
            getCheckOut = true;
            checkIn = new Date($("#Arrive").val());
            checkOut = $(this).datepicker("getDate");

            if (checkOut <= checkIn) {
                checkOut.setDate(checkIn.getDate() + 1);
                $("#Depart").val($.datepicker.formatDate('mm/dd/yy', checkOut));
            }

            termDate = new Date($("#Arrive").val());
            arrayDate = new Array();
            while (termDate <= checkOut) {
                arrayDate.push($.datepicker.formatDate('mm/dd/yy', termDate));
                termDate.setDate(termDate.getDate() + 1);

                //Hiển thị những ngày được chọn trên datepicker (Display inline) 
                ShowDate();

            }
        },
        beforeShowDay: function (date) {

            var getDay = parseInt(date.getDate()) < 10 ? "0" + date.getDate() : date.getDate();
            var getMonth = parseInt(date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var theday = getMonth + '/' + getDay + '/' + date.getFullYear();
            return [true, $.datepicker.formatDate('mm/dd/yy', checkIn) == theday ? "first-specialDate" : $.datepicker.formatDate('mm/dd/yy', checkOut) == theday ? "last-specialDate" : $.inArray(theday, arrayDate) >= 0 ? "specialDate" : ""];
        },
        beforeShow: function () {
            $("#Depart").datepicker('setDate', new Date($("#Arrive").val()));
            $("#Depart").val("Select a date");
        },
        onClose: function () {
            $("#Depart").val($.datepicker.formatDate('D, M d yy', checkOut));
        }
    });

    $("#date-CalendarAvailability").datepicker(
        {
            numberOfMonths: 2,
            minDate: new Date(),
            showOtherMonths: true,
            onSelect: function (selectDate) {
                arrayDate = new Array();
                if (getCheckOut) {
                    //trường hợp  đã lựa chọn checkout
                    getCheckOut = false;
                    checkIn = $(this).datepicker("getDate");
                    termDate = $(this).datepicker("getDate");
                    termDate.setDate(termDate.getDate() + 1);
                    checkOut = termDate;

                    arrayDate.push($.datepicker.formatDate('dd/MM/yy', checkIn));
                    arrayDate.push($.datepicker.formatDate('mm/dd/yy', checkOut));

                    //Hiển thị những ngày được chọn trên datepicker (Display inline) 
                    ShowDate();

                } else {
                    //trường hợp chưa lựa chọn checkout
                    getCheckOut = true;
                    termDate = new Date($("#Arrive").val());
                    var getDate = $(this).datepicker("getDate");

                    if (getDate <= checkIn) {
                        checkIn = $(this).datepicker("getDate");
                        termDate = $(this).datepicker("getDate");
                        checkOut.setDate(termDate.getDate() + 1);
                    }
                    else if ($.datepicker.formatDate('mm/dd/yy', $(this).datepicker("getDate")) == $.datepicker.formatDate('mm/dd/yy', checkOut)) {
                        checkIn = $(this).datepicker("getDate");
                        termDate = $(this).datepicker("getDate");
                        checkOut.setDate(termDate.getDate() + 1);
                    } else {
                        termDate = new Date($("#Arrive").val());
                        checkOut = $(this).datepicker("getDate");
                    }

                    arrayDate = [];
                    while (termDate <= checkOut) {
                        arrayDate.push($.datepicker.formatDate('mm/dd/yy', termDate));
                        termDate.setDate(termDate.getDate() + 1);
                    }


                    //Hiển thị những ngày được chọn trên datepicker (Display inline) 
                    ShowDate();
                }
                //Gán ngày cho textbox
                $('#Depart').datepicker('option', { minDate: checkIn });
                $("#Arrive").val($.datepicker.formatDate('D, M d yy', checkIn));
                $("#Depart").val($.datepicker.formatDate("D, M d yy", checkOut));
            },
            beforeShowDay: function (date) {
                var getDay = parseInt(date.getDate()) < 10 ? "0" + date.getDate() : date.getDate();
                var getMonth = parseInt(date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                var theday = getMonth + '/' + getDay + '/' + date.getFullYear();
                return [true, $.datepicker.formatDate('mm/dd/yy', checkIn) == theday ? "first-specialDate" : $.datepicker.formatDate('mm/dd/yy', checkOut) == theday ? "last-specialDate" : $.inArray(theday, arrayDate) >= 0 ? "specialDate" : ""];
            },
        });
    
});

function ShowDate() {
    //Hiển thị những ngày được chọn trên datepicker (Display inline)
    $("#date-CalendarAvailability").datepicker("refresh");
    $("#date-CalendarAvailability").datepicker({
        beforeShowDay: function (date) {

            var getDay = parseInt(date.getDate()) < 10 ? "0" + date.getDate() : date.getDate();
            var getMonth = parseInt(date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var theday = getMonth + '/' + getDay + '/' + date.getFullYear();
            return [true, $.datepicker.formatDate('mm/dd/yy', checkIn) == theday ? "first-specialDate" : $.datepicker.formatDate('mm/dd/yy', checkOut) == theday ? "last-specialDate" : $.inArray(theday, arrayDate) >= 0 ? "specialDate" : ""];
        }
    });
}