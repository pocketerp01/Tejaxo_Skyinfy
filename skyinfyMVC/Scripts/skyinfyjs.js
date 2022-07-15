/*---*/
var mtoggled = true;
function mtoggle() {
    if (mtoggled) {
        $("body")[0].classList.remove("nav-md");
        $("body")[0].classList.add("nav-sm");
        mtoggled = false;
    }
    else {
        $("body")[0].classList.remove("nav-sm");
        $("body")[0].classList.add("nav-md");
        mtoggled = true;
    }
}
/*---*/

/*--disable back button--*/
history.pushState(null, null, location.href);
window.onpopstate = function () {
    history.go(1);
};
/*--disable back button--*/

//set active menu
function setactive() {

    var url = window.location;
    // Will only work if string in href matches with location
    $('.treeview-menu li a[href="' + url + '"]').parent().addClass('active');
    // Will also work for relative and absolute hrefs
    $('.treeview-menu li a').filter(function () {
        return this.href == url;
    }).parent().parent().parent().addClass('active');

}

function maskformat(txt) {

    var characters = txt.split("");
    var ms = "";
    $.each(characters, function (i, singleChar) {
        if (singleChar === "/" || singleChar === " " || singleChar === "-" || singleChar === ".") { ms = ms + singleChar; }
        else { ms = ms + "9"; }
    });

    return ms;
}

/*--- */
window.onload = function () { hidewait(); }

var seti1 = 0;
var seti2 = 1;

//var timezone = getCookie("timezone");
//var dateformat = getCookie("dateformat");
//var timeformat = getCookie("timeformat");
//var datetimeformat = getCookie("datetimeformat");
//var Ac_from_dt = moment(getCookie("Ac_From_Date"), dformat(datetimeformat));
//var Ac_to_dt = moment(getCookie("Ac_To_Date"), dformat(datetimeformat));

//var today = moment();
//var fy_from_dt = moment(getCookie("year_from"), dformat(datetimeformat));
//var fy_to_dt = moment(getCookie("year_to"), dformat(datetimeformat));

function setMenuScroll() {

    var scrollTop = $(".menu_fixed").scrollTop();
    sessionStorage.setItem("menuTop", scrollTop);
}
/*--- */

/*###*/
$(document).ready(function () {

    $("form").submit(function (event) {
        showwait();
    });
    sapageleft = sessionStorage.getItem("sapageleft");
    sapagetop = sessionStorage.getItem("sapagetop");

    window.scrollTo(sapageleft, sapagetop);
    // date formatter

    //$.datetimepicker.setDateFormatter({
    //    parseDate: function (date, format) {
    //        var d = moment(date, format);
    //        return d.isValid() ? d.toDate() : false;
    //    },
    //    //formatDate: function (date, format) {
    //    //    return moment(date).format(format);
    //    //},
    //});

    var ssttop = sessionStorage.getItem("menuTop");

    $(".menu_fixed").scrollTop(ssttop);
    $(".sa-mandatory").attr("required", "required");
    $(".sa-pincode").attr("pattern", "([0-9]{6})");
    $(".sa-pincode").attr("title", "Fill 6 Digit Pin No eg.000000");
    $(".sa-tanno").attr("pattern", "([a-zA-z]{5})+([0-9]{4})+([a-zA-Z]{1})");
    $(".sa-tanno").attr("title", "Fille Proper TAN No eg.aaaaa0000a");
    $(".sa-mobile").attr("pattern", "[0-9\-+]{10}|[0-9\-+]{11}|[0-9\-+]{12}|[0-9\-+]{13}|[0-9\-+]{14}|[0-9\-+]{15}");
    $(".sa-mobile").attr("title", "Fill 15 Digit Mobile No eg.+000000000000 Or 10 Digit Mobile No eg.0000000000");
    $(".sa-email").attr("type", "email");
    $(".sa-email").attr("title", "Fill Proper Email ID eg.ab@ab.ab");
    $(".sa-panno").attr("pattern", "([a-zA-z]{5})+([0-9]{4})+([a-zA-z]{1})");
    $(".sa-panno").attr("title", "Fill Proper PAN No eg.aaaaa0000a");
    //$(".sa-gstno").attr("pattern", "^([0-9]{2})+([a-zA-z]{5})+([0-9]{4})+([a-zA-z]{1})+([1-9a-zA-Z]{1})+([zZ]{1})+([0-9a-zA-Z]{1})");
    $(".sa-gstno").attr("title", "Fill Proper GST No eg.00aaaaa0000a0za");

    //$(".sa-mobile").attr("onkeydown", "return false;")
    //$(".sa-mobile").keydown(function (event) {


    //    event = (event || window.event);

    //    if ($(this)[0].value.length == 10 || $(this)[0].value.length == 13) {
    //        $(this)[0].setCustomValidity("");
    //    }
    //    else {

    //        $(this)[0].value = $(this)[0].value.trim().substring(0, 13);
    //        $(this)[0].setCustomValidity("Fill 13 Digit Mobile No eg.+000000000000 Or 10 Digit Mobile No eg.0000000000");
    //    }
    //});

    //$(".modal").draggable({
    //    handle: ".modal-header"
    //});//09

    //seti1 = setInterval(function () { getDatag() }, 7000);
    if (detectmob()) {

        var lihide = $(".lihide");
        lihide.each(function () {
            $(this).hide();
        });

    }
    //$(".sa-date-dob,.sa-ac-time,.sa-fy-time,.sa-fy-datetime,.sa-fy-date,.sa-ac-datetime,.sa-ac-date,.sa-fy-datetime-today,.sa-fy-date-today,.sa-date,.sa-date-dob").attr("onkeydown", "return false;");
    $(".sa-date-dob").datetimepicker({
        ignoreReadonly: true,
        format: dformat(dateformat),
        // maxDate: today

    });

    $(".sa-date").inputmask({ "mask": maskformat(dformat(dateformat)) });

    $(".sa-fy-date-today").datetimepicker({
        ignoreReadonly: true,
        format: dformat(dateformat),
        maxDate: fy_to_dt,
        minDate: 0
    });

    $(".sa-fy-datetime-today").datetimepicker({
        ignoreReadonly: true,
        format: dformat(datetimeformat),
        maxDate: fy_to_dt,
        minDate: 0
    });

    $(".sa-date").datetimepicker({
        ignoreReadonly: true,
        format: dformat(dateformat),
        //minDate: today
    });

    $(".sa-date-pre").datetimepicker({
        ignoreReadonly: true,
        format: dformat(dateformat),
        minDate: today
    });

    $(".sa-date-post").datetimepicker({
        ignoreReadonly: true,
        format: dformat(dateformat),
        maxDate: today
    });

    $(".sa-ac-date").datetimepicker({
        ignoreReadonly: true,
        format: dformat(dateformat),
        maxDate: Ac_to_dt,
        minDate: Ac_from_dt,
    });

    $(".sa-ac-datetime").datetimepicker({
        ignoreReadonly: true,
        format: dformat(datetimeformat),

    });

    $(".sa-fy-date").datetimepicker({
        ignoreReadonly: true,
        format: dformat(dateformat),

    });
    $(".sa-fy-datetime").datetimepicker({
        ignoreReadonly: true,
        format: dformat(datetimeformat),
    });

    //$(".sa-ac-date").datetimepicker({
    //    ignoreReadonly: true,
    //    format: dformat(dateformat),
    //    maxDate: Ac_to_dt,
    //    minDate: Ac_from_dt
    //});

    //$(".sa-ac-datetime").datetimepicker({
    //    ignoreReadonly: true,
    //    format: dformat(datetimeformat),
    //    maxDate: Ac_to_dt,
    //    minDate: Ac_from_dt
    //});

    //$(".sa-fy-date").datetimepicker({
    //    ignoreReadonly: true,
    //    format: dformat(dateformat),
    //    maxDate: fy_to_dt,
    //    minDate: fy_from_dt
    //});
    //$(".sa-fy-datetime").datetimepicker({
    //    ignoreReadonly: true,
    //    format: dformat(datetimeformat),
    //    maxDate: fy_to_dt,
    //    minDate: fy_from_dt

    //});

    $(".sa-ac-time").datetimepicker({
        format: dformat(timeformat)

    });
    $(".sa-fy-time").datetimepicker({
        format: dformat(timeformat)

    });

    var hffocus = document.getElementById('hffocus');
    if (hffocus === "undefined") { }
    else {
        var elem = document.getElementById(hffocus.value);
        try {
            elem.focus();
        }
        catch (err) { }
    }

    $(".sa-from-date,.sa-to-date").on("dp.change", function (e) {

        var safrom = true;
        var dataname = "";
        var fromdt, todt;

        var ctrlfrom, ctrlto;
        try {
            var olddate = e.oldDate.format(dformat(dateformat));
            if ($(this)[0].classList.contains("sa-from-date")) safrom = true
            else if ($(this)[0].classList.contains("sa-to-date")) safrom = false;

            if ($(this).attr("data-name") === undefined) {

                if (safrom) {
                    ctrlfrom = $(this);
                    ctrlto = $(".sa-to-date");
                }
                else {
                    ctrlto = $(this);
                    ctrlfrom = $(".sa-from-date");
                }

                fromdt = moment($(ctrlfrom).val(), dformat(dateformat));
                todt = moment($(ctrlto).val(), dformat(dateformat));

                if (fromdt.isAfter(todt)) {

                    $(this).val(olddate);
                    alert('From Date can not greater Than ToDate');
                }
            }
            else {
                dataname = $(this).attr("data-name");
                if (safrom) {
                    ctrlfrom = $(this);
                    ctrlto = $("[data-name=" + dataname + "].sa-to-date");
                }
                else {
                    ctrlto = $(this);
                    ctrlfrom = $("[data-name=" + dataname + "].sa-from-date");
                }

                fromdt = moment($(ctrlfrom).val(), dformat(dateformat));
                todt = moment($(ctrlto).val(), dformat(dateformat));

                if (fromdt.isAfter(todt)) {

                    $(this).val(olddate);
                    alert('From Date can not greater Than ToDate');
                }
            }
        } catch (e) { }
    });

});
/*###*/

/* */
function updateActiveElement() {

    //
    //alert($(document.activeElement).attr('id'));
    //$('#celement').innerHTML($(document.activeElement).attr('id'));

    var hffocus = document.getElementById('hffocus');
    var val = $(document.activeElement).attr('id');

    if (val === "undefined") {

    }
    else { hffocus.value = $(document.activeElement).attr('id'); }
}
function showwait() {

    var options = { "backdrop": "static", keyboard: true };
    $('#DemoModal').modal(options);
    $('#DemoModal').modal('show');
}
function hidewait() {

    $('#DemoModal').modal('hide');
}
function showerror() {

    var options = { "backdrop": "static", keyboard: true };
    $("#MYID").modal(options);
    $("#MYID").modal('show');

}
//function BackToBack(title, tcase, seek) { var data = show_Foo(title, tcase, seek, '@viewName', '@controllerName'); }
/* */

/********/
function showmsgJS(msgtype, msg, alert_type) {
    try {

        //var lblmsg = $("[id=msgp]");
        document.getElementById("msgp").innerHTML = msg;
        //lblmsg.innerHTML = msg;
        if (msgtype == 1) {
            $("[id=btnyes]").hide();
            $("[id=btnno]").hide();
            $("[id=btnok]").show();


            switch (alert_type) {
                case 0:
                    $("[id=dtitle]")[0].style.backgroundColor = "#E74C3C";
                    $("[id=title_type]")[0].innerHTML = "Error";
                    break;
                case 1:
                    $("[id=dtitle]")[0].style.backgroundColor = "#1ABB9C";
                    $("[id=title_type]")[0].innerHTML = "Success";
                    break;
                case 2:
                    $("[id=dtitle]")[0].style.backgroundColor = "#f0ad4e";
                    $("[id=title_type]")[0].innerHTML = "Warning";
                    break;
            }
        }

        else if (msgtype == 2) {
            $("[id=btnyes]").show();
            $("[id=btnno]").show();
            $("[id=btnok]").hide();

            $("[id=title_type]")[0].innerHTML = "Confirmation";
            $("[id=dtitle]")[0].style.backgroundColor = "#f0ad4e";
        }

        else if (msgtype == 3) {
            $("[id=btnyes]").hide();
            $("[id=btnno]").hide();
            $("[id=btnok]").show();

            switch (alert_type) {
                case 0:
                    $("[id=dtitle]")[0].style.backgroundColor = "#E74C3C";
                    $("[id=title_type]")[0].innerHTML = "Error";
                    break;
                case 1:
                    $("[id=dtitle]")[0].style.backgroundColor = "#1ABB9C";
                    $("[id=title_type]")[0].innerHTML = "Success";
                    break;
                case 2:
                    $("[id=dtitle]")[0].style.backgroundColor = "#f0ad4e";
                    $("[id=title_type]")[0].innerHTML = "Warning";
                    break;
            }

        } else {
            $("[id=btnyes]").show();
            $("[id=btnno]").show();
            $("[id=btnok]").hide();
            $("[id=title_type]")[0].innerHTML = "Confirmation";
            $("[id=dtitle]")[0].style.backgroundColor = "#f0ad4e";
        }
        showerror();
    }
    catch (eerre) {
        alert(msg);

    }
}
function showsession() {
    $('#mybtn2').trigger('click');
}
function showfilter() {

    $.fancybox.open({
        modal: true,
        src: '../../../../../../erp/common/Frm_Filter1.aspx',
        type: 'iframe'
    });

    $(".fancybox-content")[0].style.maxHeight = '600px';
    $(".fancybox-content")[0].style.maxWidth = '60%';


}
function showRpt() {

    $.fancybox.open({
        modal: false,
        src: '../../../../../../erp/schoolReports_Rpts/Filter_Report_Viewer.aspx',
        type: 'iframe'
    });

    $(".fancybox-content")[0].style.maxHeight = '600px';
    $(".fancybox-content")[0].style.maxWidth = '100%';

}
/********/

//search menus
function searchmenus(e) {

    var input, filter, ul, li, a, i;
    input = document.getElementById("mySearch");
    filter = input.value.toUpperCase();
    ul = document.getElementById("myMenu");

    var anchs = $(ul).find("li").find("a");
    for (var i = 0; i < anchs.length; i++) {
        var a = anchs[i];
        var pare = $(a).parents();
        var cnt = 0;
        for (var o = 0; o < pare.length; o++) {
            if (pare[o].id == "myMenu") { break; }
            if (pare[o].innerHTML.toUpperCase().includes(filter)) {
                cnt++;
            }
        }
        if (cnt == 0) {
            $(a).parent()[0].style.display = "none";
        }
        else { $(a).parent()[0].style.display = ""; }
    }

    if (e.keyCode === 13) {
        return false;
    }
    else return true;
}

/***/
xhrPool = [];
var breakall = false;
function clickme(ct) {

    var val = ct;
    __doPostBack('href', val);
}

function givemyid(ct) {

    var val = ct.id;
    //document.cookie = "menuid=" + val + "";
    sessionStorage.setItem("menuid", val);
}


function cancelPostBack() {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm.get_isInAsyncPostBack()) {
        prm.abortPostBack();
    }
}
function abortall() {

    var request = PageMethods._staticInstance.getval(params, onSuccess, onFail);
    var request1 = PageMethods._staticInstance.Populate(params, onSuccess, onFail);

    var executor = request.get_executor();
    if (executor.get_started())
        executor.abort();
    var executor1 = request1.get_executor();
    if (executor1.get_started())
        executor1.abort();
}
function setactivemenu() {


    //var mmid = getCookie("menuid");
    try {
        var mmid = "";
        if (window.location.href.toUpperCase().includes("HOME/DASHBOARD?")) {
            mmid = "l1";
        }
        else {
            mmid = sessionStorage.getItem("menuid");
        }
        $("#" + mmid).addClass('current-page');
        $("#" + mmid).parent().addClass('current-page');
        $("#" + mmid).parent().show();
        $("#" + mmid).parent().parent().show();
        $("#" + mmid).parent().parent().parent().show();
        $("#" + mmid).parent().parent().addClass('active');
        $("#" + mmid).parent().parent().parent().addClass('active');
        $("#" + mmid).parent().parent().parent().addClass('active');
        $("#" + mmid).parent().parent().parent().parent().addClass('active');
    }
    catch (err) { }


}

function thirdfun(timeout2) {

    SessionExpireAlert(timeout2);
    setactivemenu();

}
/***/


var timeInSecondsAfterSessionOut = 10; // change this to change session time out (in seconds).
var secondTick = 0;
var tick;
var ispopup = false;

function SessionExpireAlert(timeInSecondsAfterSessionOut) {
    secondTick++;
    var timeLeft = (timeInSecondsAfterSessionOut - secondTick).toFixed(0); // in minutes
    timeLeft = timeInSecondsAfterSessionOut - secondTick; // override, we have 30 secs only

    //$("#spanTimeLeft").html(timeLeft);

    document.getElementById("seconds").innerHTML = timeLeft;
    document.getElementById("secondsIdle").innerHTML = timeLeft;

    if (timeLeft < 50 && ispopup == false) {

        ispopup = true;
        showsession();
    }
    if (timeLeft < 10 && ispopup == false) {

        ispopup = true;
        showsession();
    }
    if (timeLeft < 1) {

        window.location = "/HOME/login";
        return;
    }
    tick = setTimeout("SessionExpireAlert(" + timeInSecondsAfterSessionOut + ")", 1000);
}
//function ResetSession() {

//    var timeout1 =@timeout1;

//    clearTimeout(tick);
//    secondTick = 0;
//    ispopup = false;
//    SessionExpireAlert(timeout1);
//}
function Noselect() {
    ispopup = false;
}

function yesclick() {
    $('#hfbtnyes').click()
}

function noclick() {
    $('#hfbtnno').click();
}
function liclick(ctrl, e) {

    //$("#menudiv").find("ul").css("display", "");
    $("#menudiv").find("li").not($(ctrl)).removeClass('active');

    //ctrl = $("#menudiv").find("li")[c];
    if ($(ctrl).hasClass('active')) {

        $(ctrl).removeClass('active');
        //$(ctrl).children("ul").css("display", "none");
        var uls = $(ctrl).children("ul").children().length;

        for (var i = 0; i < uls; i++) {
            $(ctrl).children("ul").children().eq(i).hide(500, "linear");
        }
    }
    else {
        $(ctrl).addClass('active');
        uls = $(ctrl).children("ul").children().length;
        $(ctrl).children("ul").css("display", "block");
        if ($(ctrl).parent()[0].nodeName != "UL") {
            $("#menudiv").find("ul").not($(ctrl).children("ul")).css("display", "");
            $(ctrl).children("ul").children().hide();


        }
        for (var j = 0; j < uls; j++) {

            $(ctrl).children("ul").children().eq(j).show(500, "swing");

        }
        //$(ctrl).children("ul").css("display", "block");
    }


    //sessionStorage.setItem("menuid", ctrl.id);
    //setactivemenu();
    e.stopPropagation();



}
function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}
function okclick() {

    $('#hfbtnok').click();
}
function isPostBack() { //function to check if page is a postback-ed one
    return document.getElementById('_ispostback').value == 'True';
}

/*-------------*/

$(document).ready(function () {


    setactivemenu();
    $('a.movebottom').click(function () {
        $('html, body').animate({
            scrollTop: screen.height
        }, 700);
        return false;
    });
    $('a.movetop').click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 700);
        return false;
    });

    //if (window.IsDuplicate()) {
    //    //alert("Window Already Opened in Another Tab\nPlease Click Ok");
    //    window.location = '/home/login';
    //}

});
function wait() {
    $('#btnwait').trigger('click');
}

function startddlfor(id, fn_Name, contName) {

    var getSelectedValue = function () {
        var activeRow = $("tr.active");
        var firstColumn = $("td:nth-child(1)", activeRow).text();
        var secondColumn = $("td:nth-child(2)", activeRow).text();
        var entry = firstColumn + " (" + secondColumn + ")";
        return entry;
    }
    $("[id*=" + id + "]")
        .on("focusout", function (e) {
            window.setTimeout(function () {
                $(this).parent().find(".suggestions").addClass("hidden");
            }, 100);
        })
        .on("keyup keypress",
            function (e) {
                var active = $("tr.active", ".suggest-grid");
                if (e.which === 27) {
                    $(".suggestions").addClass("hidden");
                } else if (e.which === 40) {
                    if (active.length > 0) {
                        var next = $(active).next();
                        $(active).removeClass("active");
                        $(next).addClass("active");
                    } else {
                        $("tr:first", ".suggest-grid").addClass("active");
                    }
                } else if (e.which === 38) {
                    if (active.length > 0) {
                        var previous = $(active).prev();
                        $(active).removeClass("active");
                        $(previous).addClass("active");
                    } else {
                        $("tr:last", ".suggest-grid").addClass("active");
                    }
                } else if (e.which === 13) {
                    e.preventDefault();

                    //var selectedValue = getSelectedValue();
                    //$(this).val(selectedValue);

                    var activerow = $(currctrl).parent().find("tr.active");
                    var id = $("td:nth-child(1)", activerow).text();
                    var vall = $("td:nth-child(2)", activerow).text();
                    //$(".search-term").val(id);
                    if (vall.trim() != "") {
                        try {
                            $(currctrl).val(id);

                            //$(currctrl).parent().parent().parent().parent().parent().find("[id*=SearchTerm]").val(id);
                            var rowid = $(currctrl)[0].id.split('_')[1];
                            $(currctrl).parent().parent().parent().parent().prev().find("[id*=" + rowid + "]")[0].innerText = vall;
                        }
                        catch (err) { }
                    }
                    $(".suggestions").addClass("hidden");
                    $(currbody).empty();
                    return false;
                } else {
                    // We have a good value w/ no special keys.
                    var value = $(this).val();
                    if (value === "") {
                        $(".suggestions").addClass("hidden");
                    } else {

                        var uri = "/" + contName + "/" + fn_Name;
                        var datat = {
                            "searchTerm": value, "pageSize": 10, "pageNum": 1, "icode": 'aaaa'
                        };
                        //$(".suggestions").removeClass("hidden");
                        currctrl = $(this);

                        $(this).parent().find(".suggestions").removeClass("hidden");
                        $.getJSON(uri, datat)
                            .done(function (data) {
                                var grid = $(currctrl).parent().find(".suggest-grid");
                                currbody = $("tbody", grid);
                                $(currbody).empty();
                                //

                                for (var a = 0; a < data.Results.length; a++) {
                                    try {
                                        var value = data.Results[a];
                                        var row =
                                            "<td>" + value.text + "</td>" +
                                            "<td>" + value.id.split('!~!-!~!')[0] + "</td>";
                                        $("tbody", grid).append($("<tr></tr>").html(row));
                                        // On mouse click, set the value.

                                        //$("tr", grid)[a].setAttribute("onclick", "RowClick(event);");
                                        $("tr", grid).on("click",
                                            function (e) {
                                                e.preventDefault();
                                                // this = the row (tr)
                                                // Grabs the first column's value.

                                                //var selectedValue = getSelectedValue();
                                                var activerow = $(this).eq(0);
                                                var id = $("td:nth-child(1)", activerow).text();
                                                var vall = $("td:nth-child(2)", activerow).text();
                                                //$(".search-term").val(id);
                                                try {
                                                    $(this).parent().parent().parent().parent().parent().find("[id*=SearchTerm]").val(id);
                                                    var rowid = $(this).parent().parent().parent().parent().parent().parent()[0].id.split('_')[1];
                                                    $(this).parent().parent().parent().parent().parent().parent().parent().prev().find("[id*=" + rowid + "]")[0].innerText = vall;
                                                }
                                                catch (err) { }
                                                //$("[id*=SearchTerm]").eq(0).val(id);
                                                $(".suggestions").addClass("hidden");
                                                $(currbody).empty();
                                            });
                                    }
                                    catch (err) { }
                                }
                                ///
                            });
                    }
                }
            });


}

//function startddlfor(id, fn_Name, contName, ind) {

//    var getSelectedValue = function () {
//        var activeRow = $("tr.active");
//        var firstColumn = $("td:nth-child(1)", activeRow).text();
//        var secondColumn = $("td:nth-child(2)", activeRow).text();
//        var entry = firstColumn + " (" + secondColumn + ")";
//        return entry;
//    }
//    $("[id*=" + id + "]")
//        .on("focusout", function (e) {
//            window.setTimeout(function () {
//                $(this).parent().find(".suggestions").addClass("hidden");
//            }, 100);
//        })
//        .on("keyup keypress",
//            function (e) {
//                var active = $("tr.active", ".suggest-grid");
//                if (e.which === 27) {
//                    $(".suggestions").addClass("hidden");
//                } else if (e.which === 40) {
//                    if (active.length > 0) {
//                        var next = $(active).next();
//                        $(active).removeClass("active");
//                        $(next).addClass("active");
//                    } else {
//                        $("tr:first", ".suggest-grid").addClass("active");
//                    }
//                } else if (e.which === 38) {
//                    if (active.length > 0) {
//                        var previous = $(active).prev();
//                        $(active).removeClass("active");
//                        $(previous).addClass("active");
//                    } else {
//                        $("tr:last", ".suggest-grid").addClass("active");
//                    }
//                } else if (e.which === 13) {
//                    e.preventDefault();

//                    //var selectedValue = getSelectedValue();
//                    //$(this).val(selectedValue);

//                    var activerow = $(currctrl).parent().find("tr.active");
//                    var id = $("td:nth-child(1)", activerow).text();
//                    var vall = $("td:nth-child(2)", activerow).text();
//                    //$(".search-term").val(id);
//                    if (vall.trim() != "") {
//                        try {
//                            $(currctrl).val(id);

//                            //$(currctrl).parent().parent().parent().parent().parent().find("[id*=SearchTerm]").val(id);
//                            var rowid = $(currctrl)[0].id.split('_')[1];
//                            $(currctrl).parent().parent().parent().parent().prev().find("[id*=" + rowid + "]")[0].innerText = vall;
//                        }
//                        catch (err) { }
//                    }
//                    $(".suggestions").addClass("hidden");
//                    $(currbody).empty();
//                    return false;
//                } else {
//                    // We have a good value w/ no special keys.

//                    var icode = $(this).parent().parent().parent().parent().siblings()[1].childNodes[3].innerText;
//                    var value = $(this).val();
//                    if (value === "") {
//                        $(".suggestions").addClass("hidden");
//                    } else {

//                        var uri = "/" + contName + "/" + fn_Name;
//                        var datat = {
//                            "searchTerm": value, "pageSize": 10, "pageNum": 1, "icode": icode
//                        };
//                        //$(".suggestions").removeClass("hidden");
//                        currctrl = $(this);

//                        $(this).parent().find(".suggestions").removeClass("hidden");
//                        $.getJSON(uri, datat)
//                            .done(function (data) {
//                                var grid = $(currctrl).parent().find(".suggest-grid");
//                                currbody = $("tbody", grid);
//                                $(currbody).empty();
//                                //

//                                for (var a = 0; a < data.Results.length; a++) {
//                                    try {
//                                        var value = data.Results[a];
//                                        var row =
//                                            "<td>" + value.text + "</td>" +
//                                            "<td>" + value.id.split('!~!-!~!')[0] + "</td>";
//                                        $("tbody", grid).append($("<tr></tr>").html(row));
//                                        // On mouse click, set the value.

//                                        //$("tr", grid)[a].setAttribute("onclick", "RowClick(event);");
//                                        $("tr", grid).on("click",
//                                            function (e) {
//                                                e.preventDefault();
//                                                // this = the row (tr)
//                                                // Grabs the first column's value.

//                                                //var selectedValue = getSelectedValue();
//                                                var activerow = $(this).eq(0);
//                                                var id = $("td:nth-child(1)", activerow).text();
//                                                var vall = $("td:nth-child(2)", activerow).text();
//                                                //$(".search-term").val(id);
//                                                try {
//                                                    $(this).parent().parent().parent().parent().parent().find("[id*=SearchTerm]").val(id);
//                                                    var rowid = $(this).parent().parent().parent().parent().parent().parent()[0].id.split('_')[1];
//                                                    $(this).parent().parent().parent().parent().parent().parent().parent().prev().find("[id*=" + rowid + "]")[0].innerText = vall;
//                                                }
//                                                catch (err) { }
//                                                //$("[id*=SearchTerm]").eq(0).val(id);
//                                                $(".suggestions").addClass("hidden");
//                                                $(currbody).empty();
//                                            });
//                                    }
//                                    catch (err) { }
//                                }
//                                ///
//                            });
//                    }
//                }
//            });
//}

function startddlfor(id, fn_Name, contName, MyGuid) {
    debugger
    var getSelectedValue = function () {
        var activeRow = $("tr.active");
        var firstColumn = $("td:nth-child(1)", activeRow).text();
        var secondColumn = $("td:nth-child(2)", activeRow).text();
        var entry = firstColumn + " (" + secondColumn + ")";
        return entry;
    }
    $("[id*=" + id + "]")
        .on("focusout", function (e) {
            window.setTimeout(function () {
                $(this).parent().find(".suggestions").addClass("hidden");
            }, 100);
        })
        .on("keyup keypress",
            function (e) {
                var active = $("tr.active", ".suggest-grid");
                if (e.which === 27) {
                    $(".suggestions").addClass("hidden");
                } else if (e.which === 40) {
                    if (active.length > 0) {
                        var next = $(active).next();
                        $(active).removeClass("active");
                        $(next).addClass("active");
                    } else {
                        $("tr:first", ".suggest-grid").addClass("active");
                    }
                } else if (e.which === 38) {
                    if (active.length > 0) {
                        var previous = $(active).prev();
                        $(active).removeClass("active");
                        $(previous).addClass("active");
                    } else {
                        $("tr:last", ".suggest-grid").addClass("active");
                    }
                } else if (e.which === 13) {
                    e.preventDefault();

                    //var selectedValue = getSelectedValue();
                    //$(this).val(selectedValue);

                    var activerow = $(currctrl).parent().find("tr.active");
                    var id = $("td:nth-child(1)", activerow).text();
                    var vall = $("td:nth-child(2)", activerow).text();
                    //$(".search-term").val(id);
                    if (vall.trim() != "") {
                        try {
                            $(currctrl).val(id);

                            //$(currctrl).parent().parent().parent().parent().parent().find("[id*=SearchTerm]").val(id);
                            var rowid = $(currctrl)[0].id.split('_')[1];
                            $(currctrl).parent().parent().parent().parent().prev().find("[id*=" + rowid + "]")[0].innerText = vall;
                        }
                        catch (err) { }
                    }
                    $(".suggestions").addClass("hidden");
                    $(currbody).empty();
                    return false;
                } else {
                    // We have a good value w/ no special keys.

                    var icode = $(this).parent().parent().parent().parent().siblings()[1].childNodes[3].innerText;
                    var value = $(this).val();
                    if (value === "") {
                        $(".suggestions").addClass("hidden");
                    } else {

                        var uri = "/" + contName + "/" + fn_Name;
                        var datat = {
                            "searchTerm": value, "pageSize": 10, "pageNum": 1, "icode": icode, "Myguid": MyGuid
                        };
                        //$(".suggestions").removeClass("hidden");
                        currctrl = $(this);

                        $(this).parent().find(".suggestions").removeClass("hidden");
                        $.getJSON(uri, datat)
                            .done(function (data) {
                                var grid = $(currctrl).parent().find(".suggest-grid");
                                currbody = $("tbody", grid);
                                $(currbody).empty();
                                //

                                for (var a = 0; a < data.Results.length; a++) {
                                    try {
                                        var value = data.Results[a];
                                        var row =
                                            "<td>" + value.text + "</td>" +
                                            "<td>" + value.id.split('!~!-!~!')[0] + "</td>";
                                        $("tbody", grid).append($("<tr></tr>").html(row));
                                        // On mouse click, set the value.

                                        //$("tr", grid)[a].setAttribute("onclick", "RowClick(event);");
                                        $("tr", grid).on("click",
                                            function (e) {
                                                debugger
                                                e.preventDefault();
                                                // this = the row (tr)
                                                // Grabs the first column's value.

                                                //var selectedValue = getSelectedValue();
                                                var activerow = $(this).eq(0);
                                                var id = $("td:nth-child(1)", activerow).text();
                                                var vall = $("td:nth-child(2)", activerow).text();
                                                //$(".search-term").val(id);
                                                try {
                                                    $(this).parent().parent().parent().parent().parent().find("[id*=SearchTerm]").val(id);
                                                    var rowid = $(this).parent().parent().parent().parent().parent().parent()[0].id.split('_')[1];
                                                    $(this).parent().parent().parent().parent().parent().parent().parent().prev().find("[id*=" + rowid + "]")[0].innerText = vall;
                                                }
                                                catch (err) { }
                                                //$("[id*=SearchTerm]").eq(0).val(id);
                                                $(".suggestions").addClass("hidden");
                                                $(currbody).empty();
                                            });
                                    }
                                    catch (err) { }
                                }
                                ///
                            });
                    }
                }
            });
}

/***********************/



function closemodal() {

    try {
        $('#myModal').modal('hide');
        $("#footable_v2").remove();
        var obj = { "sname": 'parentname' };
        myMethod('Foo', 'footable_v2', 'delsession', obj, null);
    }
    catch (err) { }
}

/*?*?*?*?*/

function replaceAll(inputString, fromval, toval) {
    var res = inputString.replace("/" + fromval + "/gi", toval);
    return res;
}
$(document).ready(function () {
    $(".sa-select2-single").select2({


        maximumSelectionLength: 1,
        //placeholder: $(this).attr("placeholder"),
        //multiple: true,

        allowClear: true
    })

    //$(".sa-select2-multiple").select2({ placeholder: "Select a state"});
    $(".sa-select2-multiple").select2();
    $('.sa-selectfast').fastselect({
        placeholder: '',
    });
    $(window).scroll(function (event) {


        var doc = document.documentElement;
        var sapageleft = (window.pageXOffset || doc.scrollLeft) - (doc.clientLeft || 0);
        var sapagetop = (window.pageYOffset || doc.scrollTop) - (doc.clientTop || 0);
        sessionStorage.setItem("sapageleft", sapageleft);
        sessionStorage.setItem("sapagetop", sapagetop);
    });


    //for cell focus, tab key, arrow key work

    try {
        var editpors = $("[data-sakeys=gloval]");
        $(editpors).on('focus', function () {
            var cell = this;

            $(this)[0].style.backgroundColor = "#98FB98";
            if (parseFloat($(cell)[0].innerText) == 0) {
                $(cell)[0].innerText = "";
            }
            var range = document.createRange();
            range.selectNodeContents(cell);
            var sel = window.getSelection();
            sel.removeAllRanges();
            sel.addRange(range);

        });
        $(editpors).on('focusout', function () {
            var cell = this;
            $(this)[0].style.backgroundColor = "white";

        });
        for (var i = 0; i < editpors.length; i++) {

            $(editpors)[i].addEventListener("keyup", function () {
                var cDiv = $(this).parents(".grid-container")[0];
                var id = parseInt($(this)[0].id.split('_')[1].toString());
                var rowid = parseInt($(this)[0].id.split('_')[1].toString());
                var colid = parseInt($(this).parent()[0].id.split('_')[1].toString());
                var totcols = parseInt($(this).parent().siblings().length.toString());
                var totrows = parseInt($(this).siblings().length.toString());
                if (event.keyCode == 40) {
                    $(this).eq(0).next().focus();
                }
                else if (event.keyCode == 38) {
                    $(this).eq(0).prev().focus();
                }
                //else if (event.keyCode == 37) {
                //    $(this).parent().prev().children().eq(id).next().focus();
                //}
                //else if (event.keyCode == 39) {
                //    $(this).parent().next().children().eq(id).next().focus();
                //}
                else if (event.keyCode == 37) //left keycode
                {



                    var n = 1;
                    start_position: while (true) {
                        cDiv.scrollLeft = cDiv.scrollLeft - 100;
                        if (colid - n == 0) {
                            //colid = 1; n = 1;
                            rowid = rowid - 1;
                            cDiv.scrollLeft = 1000;
                        }


                        tcol = $(this).parent().siblings().eq(colid - n);
                        indx = n;
                        if (n == 0) indx = 1;
                        if ($(tcol).children().eq(rowid + 1)[0] != undefined && $(tcol).children().eq(rowid + 1)[0].isContentEditable) {

                            $(tcol).children().eq(rowid + 1).focus()

                            break;
                        }
                        else {
                            n++;
                            if (n > 20) break;
                            continue start_position;
                        }
                        if (colid - n == 0 && rowid == 0) break;

                    }
                }
                else if (event.keyCode == 39 || event.keyCode == 13)//right key code
                {
                    var n = 0;
                    start_position: while (true) {
                        if (colid == totcols) {
                            colid = 1; n = 0;
                            rowid = rowid + 1;
                        }
                        if (colid - n == 0 && rowid == 0) break;
                        tcol = $(this).parent().siblings().eq(colid + n);
                        indx = n;
                        if (n == 0) indx = 1;
                        if ($(tcol).children().eq(rowid + 1)[0] != undefined && $(tcol).children().eq(rowid + 1)[0].isContentEditable) {
                            $(tcol).children().eq(rowid + 1).focus();
                            var doc = document;
                            var element = $(tcol).children().eq(rowid + 1)[0];
                            console.log(this, element);
                            if (doc.body.createTextRange) {
                                var range = document.body.createTextRange();
                                range.moveToElementText(element);
                                range.select();
                            } else if (window.getSelection) {
                                var selection = window.getSelection();
                                var range = document.createRange();
                                range.selectNodeContents(element);
                                selection.removeAllRanges();
                                selection.addRange(range);
                            }
                            break;

                            //var cell = $(tcol).children().eq(rowid + 1);

                            //$(tcol).children().eq(rowid + 1)[0].style.backgroundColor = "#98FB98";
                            //if (parseFloat($(cell)[0].innerText) == 0) {
                            //    $(cell)[0].innerText = "";
                            //}
                            //var range = document.createRange();
                            //range.selectNodeContents(cell);
                            //var sel = window.getSelection();
                            //sel.removeAllRanges();
                            //sel.addRange(range);
                        }
                        else {
                            n++;
                            if (n > 20) break;
                            continue start_position;
                        }
                        if (colid + n == totcols && rowid + 1 == totrows) break;

                        break;
                    }
                }
                else if (event.shiftKey && (event.keyCode == 9)) {

                    //var n = 0;
                    //var tcol = $(this).parent().prevAll().eq(0);
                    //start_position: while (true) {

                    //    if ($(tcol).children().eq(id)[0] == undefined) {
                    //        tcol = $(this).parent().siblings().eq(0);
                    //    }
                    //    else tcol = $(this).parent().prevAll().eq(n);

                    //    if ($(tcol).children().eq(id)[0] != undefined && $(tcol).children().eq(id + 1 + n)[0].isContentEditable) {
                    //        //$(this).parent().next().children().eq(id).focus();
                    //        $(tcol).children().eq(id + 1 + n).focus()
                    //    }
                    //    else {
                    //        n++;
                    //        if (n > 20) break;
                    //        continue start_position;
                    //    }

                    //    break;
                    //}
                }
                else if (event.keyCode == 9) {


                    //var n = 0;
                    //var tcol = $(this).parent().nextAll().eq(0);
                    //start_position: while (true) {

                    //    if ($(tcol).children().eq(id)[0] == undefined) {
                    //        tcol = $(this).parent().siblings().eq(0);
                    //    }
                    //    else tcol = $(this).parent().nextAll().eq(n);

                    //    if ($(tcol).children().eq(id)[0] != undefined && $(tcol).children().eq(id)[0].isContentEditable) {
                    //        //$(this).parent().next().children().eq(id).focus();
                    //        $(tcol).children().eq(id).focus()
                    //    }
                    //    else {
                    //        n++;
                    //        if (n > 20) break;
                    //        continue start_position;
                    //    }

                    //    break;
                    //}
                }
            });
        }
    }
    catch (err) { }

    //for cell focus, tab key, arrow key work
});
function disableForm() {

    //$(".body").find("input[type=text],input[type=checkbox],select").prop("disabled", true);

    $(".body").find("input[type=text],input[type=checkbox],input[type=email],input[type=radio],input[type=password],select,textarea").not("#mySearch").not("#ddmenus").prop("disabled", true);
}
function enableForm() {

    $(".body").find("input[type=text],input[type=checkbox],input[type=email],input[type=radio],input[type=password],select,textarea").prop("disabled", false);
}
function fillBlanks() {
    $(".body").find("input[type=text],input[type=checkbox],select").val("").prop("checked", false);
    $(".sa-select2-multiple .sa-select2-single").select2('destroy');
    $(".sa-select2-multiple .sa-select2-single").empty();
    $(".sa-select2-single").select2({
        maximumSelectionLength: 1,
        //multiple: true,
        allowClear: true
    });
    $(".sa-select2-multiple").select2();
}

var options = { "backdrop": "static", keyboard: true };

function showret() {
    alert('something');
}


function fillDdl(ctrlid, mq) {
    $.getJSON({
        url: '/Home/GetDataJson',
        data: { mq: mq },
        datatype: "json",
        type: "post",
        contenttype: 'application/json; charset=utf-8',
        success: function (data) {
            //clear the current content of the select
            $("select[id*=" + ctrlid + "]").select2("destroy");
            $("select[id*=" + ctrlid + "]").empty();
            //iterate over the data and append a select option
            $.each(data, function (key, val) {
                $("select[id*=" + ctrlid + "]").append('<option id="' + key + '">' + val + '</option>');
            });
            $("select[id*=" + ctrlid + "]").select2();
        },
        error: function (xhr) {
            alert('error');
        }
    });
}

function onsucs(res) {
    // debugger
    switch (funckc) {

        case "checkadhaarself":
            if (res.trim() == "Y") {
                showmsgJS(1, "Adhaar No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;
        case "checkvid":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "VID No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;
        case "checkpan":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "PAN No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;
        case "checkpf":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "PF No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;
        case "checkpfuan":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "PF UAN No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;
        case "checkesi":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "ESI No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;
        case "checkbiometricid":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "Biometric Id Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;

        case "checkpp":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "Passport No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;

        case "checkextempcode":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "External Employee No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;
        case "checkoldempcode":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "Old Employee No Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            break;

        case "checkrefcode":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "refcode Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") {
                showmsgJS(1, "Something Went Wrong", 0);
            }
            else { }
            break;
        case "checkcoutitle":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "Name Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0); }
            else { }
            break;
        //std_reg
        case "checkadhaarfather":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "Adhaar Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            else { }
            break;
        case "checkadhaarmother":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "Adhaar Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") {
                showmsgJS(1, "Something Went Wrong", 0);
                ctrlkc.value = "";
            }
            else { }
            break;
        case "checkadhaargaurdian":
            if (res.trim().trim() == "Y") {
                showmsgJS(1, "Adhaar Already Exists", 2);
                ctrlkc.value = "";
            }
            else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            else { }
            break;

        case "Getlocations":
            if (res.trim() != "") {


                var locations = new Array();

                var xmlDoc = $.parseXML(res);
                var xml = $(xmlDoc);
                var customers = xml.find("MainData");
                var i = 0;
                customers.each(function () {
                    var customer = $(this);
                    i++;
                    locations.push([{ lat: parseFloat(customer[0].children[0].innerHTML) },
                    { long: parseFloat(customer[0].children[1].innerHTML) }]);

                });
                window.sessionStorage.setItem("locations", JSON.stringify(locations));
                addmarker(locations);

                //showmsgJS(1, "Adhaar Already Exists", 2);
                //ctrlkc.value = "";
            }
            //else if (res.trim() == "E") { showmsgJS(1, "Something Went Wrong", 0) }
            else { }
            break;

        case "sectionavail":
            if (res.trim() != "") {}
            else { }
            break;
        case "Backup_database":

            showmsgJS(1, res, 1);
            ctrlkc.value = "";

            break;

        case "setst":
            if (res.trim() == "N") { showmsgJS(1, "Something went wrong, status not updating.", 2); }
            break;

        case "callgrd":
            res
            if (res.trim() == "N") { showmsgJS(1, "Something went wrong, status not updating.", 2); }
            break;

        case "rdel":
            res
            if (res.trim() == "true") {
                
            }
            else { showmsgJS(1, "Something went wrong, data not deleting.", 2); }
            break;

        default:
            break;
    }
}
//var funckc;
//var ctrlkc;
//function myMethod(contname, vwname, func, obj, ctrl) {
//    //  debugger;
//    funckc = func;
//    ctrlkc = ctrl;
//    $.ajax({
//        type: "post",
//        url: "/" + contname + "/" + func + "",
//        contentType: "application/json; charset=utf-8",
//        data: JSON.stringify(obj),
//        datatype: "json",
//        success: onsucs,

//        error: function (err) {
//            //  debugger;
//            showmsgJS(1, "Error in " + func, 0);
//        }
//    });
//}

//function seekServer(contname, seekCode, seekVal, param1) {

//    var obj = {
//        "SeekCode": seekCode, "SeekVal": seekVal, "Param1": param1
//    };

//    var jqXHR = $.ajax({
//        type: "post",
//        url: "/" + contname + "/SeekServer",
//        contentType: "application/json; charset=utf-8",
//        data: JSON.stringify(obj),
//        datatype: "json",
//        async: false
//        //,
//        //success: function (res) {
//        //    return res;
//        //},
//        //error: function (err) {
//        //    return err;
//        //}
//    });
//    return jqXHR.responseText;
//}

//function myMethod_C(contname, vwname, func, obj, callbackfun, fail) {

//    funckc = func;
//    $.ajax({
//        type: "post",
//        async: true,
//        url: "/" + contname + "/" + func + "",
//        contentType: "application/json; charset=utf-8",
//        data: JSON.stringify(obj),
//        datatype: "json",
//        success: callbackfun,
//        error: fail
//    });
//}

function backupdata() {
    myMethod("Home", "-", "Backup_database", null, null);
}

/**/

//operator


//master ctrl
function getmst(ctrl, pgid, pgurl) {
    debugger;
    event.preventDefault();   
    var val = ctrl.value;
    if (val.trim() == "Save & New" || val.trim() == "Update & New") { val = val.replace('&', '#'); }
    var $form = $("#" + pgid);
    //if (val.trim() == "Save" || val.trim() == "Save # New" || val.trim() == "Update" || val.trim() == "Update # New") {
    //    $form[0].reportValidity();   
    //}
    if ($(ctrl)[0].formNoValidate || $($form)[0].reportValidity()) {
        ctrl.disabled = true;
        $.ajax({
            type: "POST",
            url: pgurl,
            data: $form.serialize() + "&command=" + val + "&layout=ok",
            dataType: "json",
            traditional: true,
            success: function (response) {
                //debugger
                $(".right_col").html(response.responseText);

                $(".sa-select2-single").select2({
                    maximumSelectionLength: 1,
                    allowClear: true
                })
                $(".sa-select2-multiple").select2();
            },
            error: function (response) {
                //debugger
                $(".right_col").html(response.responseText);

                $(".sa-select2-single").select2({
                    maximumSelectionLength: 1,
                    allowClear: true
                })
                $(".sa-select2-multiple").select2();
            }
        });
    }
}

//common
function getpgdt(ctrl, pgid, pgurl) {
    debugger;     
    var val = ctrl.value;
    if (val.trim() == "Save & New" || val.trim() == "Update & New") { val = val.replace('&', '#'); }
    //var $form = $("#itemservice");
    var $form = $("#" + pgid); 
    var data = new FormData();

    try {
        var upds = $("[id^=upd]")
        for (var k = 1; k <= upds.length; k++) {
            var fl = upds[k - 1].files;
            if (fl.length > 0) { data.append("upd" + upds[k - 1].name.replace("upd", "").trim(), fl[0]); }
        }
    }
    catch (err) { }

    data.append("strmd", JSON.stringify($form.serialize()));
    //data.append("strmd", JSON.stringify($($form).serializeArray()).toString());
    //data.append("myList", $($form).serializeArray());
    //data.append("ms", JSON.stringify($($form).serializeArray()));
    //data.append("strmd", JSON.stringify($($form).not("button").serialize()));
    data.append("command", val);
    data.append("layout", "ok");
    if ($(ctrl)[0].formNoValidate || $($form)[0].reportValidity()) {
        ctrl.disabled = true;
        $.ajax({
            //url: pgurl,
            //type: "POST",
            //processData: false,
            //data: data,
            //dataType: "json",

            url: pgurl, type: "POST", processData: false,
            data: data,
            dataType: 'json',
            contentType: false,
            success: function (response) {
                //   debugger
                $(".right_col").html(response.responseText);

                $(".sa-select2-single").select2({
                    maximumSelectionLength: 1,
                    allowClear: true
                })
                $(".sa-select2-multiple").select2();
            },
            error: function (response) {
                //  debugger
                $(".right_col").html(response.responseText);

                $(".sa-select2-single").select2({
                    maximumSelectionLength: 1,
                    allowClear: true
                })
                $(".sa-select2-multiple").select2();
            }
        });
    }
}