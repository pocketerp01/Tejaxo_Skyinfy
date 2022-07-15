function showerror() {

    $('#mybtn').trigger('click');
}

function yesclick() {
    debugger;


    $('#hfbtnyes').click()

}
$(document).ready(function () {


    try {
        var hfg = document.getElementById('hfg');

        FillBilling(hfg.value);
    }
    catch{ }

    var hfstepc = document.getElementById('hfstep');

    var navListItems = $('div.setup-panel div a'),
        allWells = $('.setup-content'),
        alllastBtn = $('.lastBtn'),
        allNextBtn = $('.nextBtn');

    allWells.hide();

    navListItems.click(function (e) {
        e.preventDefault();
        var $target = $($(this).attr('href')),
            $item = $(this);

        if (!$item.hasClass('disabled')) {
            navListItems.removeClass('btn-primary').addClass('btn-default');
            $item.addClass('btn-primary');
            allWells.hide();
            $target.show();
            $target.find('input:eq(0)').focus();
        }
    });

    alllastBtn.click(function () {
        var curStep = $(this).closest(".setup-content");
        var curStepBtn = curStep.attr("id");
        var idd = curStepBtn.split('-')[1] - 1;
        hfstepc.value = idd - 1;
        var nextStepWizard = $('div.setup-panel div a[href="#step-' + idd + '"]').trigger('click');
        isValid = true;
    });

    allNextBtn.click(function () {

        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id");
        var idd = curStepBtn.split('-')[1];
        hfstepc.value = idd;
        nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            //curInputs = curStep.find("input[type='text'],input[type='url'],input[type='select-one']"),
            curInputs = curStep.find("input[type='text'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {

            var curitem = curInputs[i].id;
            if (curitem == 'acn') {

                if (document.getElementById(curitem).value.length != 14) {
                    document.getElementById(curitem).value = "";
                    alert('Please Enter Applicant Adhaar Card Number');
                    return;
                    isValid = false;
                }
            }
            else if (curitem == 'facn') {
                if (document.getElementById(curitem).value.length != 14) {
                    document.getElementById(curitem).value = "";
                    alert('Please Enter Father Adhaar Card Number');
                    return;
                    isValid = false;
                }
            }
            else if (curitem == 'macn') {
                if (document.getElementById(curitem).value.length != 14) {
                    alert('Please Enter Father Adhaar Card Number');
                    return;
                    isValid = false;
                }
            }
            else if (curitem == 'fcontact_number') {
                if (document.getElementById(curitem).value.length != 10) {
                    document.getElementById(curitem).value = "";
                    alert('Please Enter Father Contact Number');
                    return;
                    isValid = false;
                }
            }
            else if (curitem == 'mcontact_number') {
                if (document.getElementById(curitem).value.length != 10) {
                    alert('Please Enter Mother Contact Number');
                    return;
                    isValid = false;
                }
            }
            else if (curitem == 'gcontact_number') {
                if (document.getElementById(curitem).value.length != 10) {
                    alert('Please Enter Mother Contact Number');
                    return;
                    isValid = false;
                }
            }
            else if (curitem == 'siblings') {
                debugger;
                if (document.getElementById(curitem).value == "1") {
                    {

                    }
                }

                else if (document.getElementById(curitem).value == "2") {
                    // if (document.getElementById('sibling_roll_no').value == "" || document.getElementById('sibling_class').value == "" || document.getElementById('sibling_name').value == "") {
                    if ($('.sib_reg_sch').is(":checked")) {
                        if (document.getElementById('sibling_roll_no').value == "" || document.getElementById('sibling_class').value == "" || document.getElementById('sibling_name').value == "") {
                            alert('Please Fill Sibling 1 All details');
                            return;
                            isValid = false;
                        }
                    }

                    else {
                        if (document.getElementById('s1_school_name').value == "" || document.getElementById('sibling_class').value == "" || document.getElementById('sibling_name').value == "") {
                            alert('Please Fill Sibling 1 All details');
                            return;
                            isValid = false;

                        }
                    }
                }
                // else if (document.getElementById('sibling2_roll_no').value == "" || document.getElementById('sibling2_name').value == "" || document.getElementById('sibling2_class').value == "") {
                else if (document.getElementById(curitem).value == "3") {

                    if (document.getElementById('s1_school_name').value == "" || document.getElementById('sibling_class').value == "" || document.getElementById('sibling_name').value == "") {
                        alert('Please Fill Sibling 1 All details');
                        return;
                        isValid = false;

                    }

                    if (document.getElementById('s2_school_name').value == "" || document.getElementById('sibling2_name').value == "" || document.getElementById('sibling2_class').value == "") {
                        alert('Please Fill Sibling 2 All details');
                        return;
                        isValid = false;
                    }
                }

                //else{
                //}
            }
            else if ($('.coupon_question').is(":checked")) {
                if (document.getElementById('l_school_name').value == "") {

                    alert('Please Enter Last School Name');
                    return;

                    isValid = false;

                }
            }
            else if (curitem == 'ddl_caste') {
                if (document.getElementById(curitem).selectedIndex == "0") {
                    alert('Please Select Caste Category');
                    return;

                }
            }
            else if (curitem == 'nationality') {
                if (document.getElementById(curitem).selectedIndex == "0") {
                    alert('Please Select Nationality');
                    return;
                    //document.getElementById('Span6').innerText = 'Please Select Nationality';
                    //document.getElementById('Span6').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span6').hide();
                //}
            }
            else if (curitem == 'class_applied') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Select Class');
                    return;
                    //document.getElementById('Span7').innerText = 'Please Select Class ';
                    //document.getElementById('Span7').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span6').hide();
                //}
            }
            else if (curitem == 'academic_year') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Select Academic Year');
                    return;
                    //document.getElementById('Span8').innerText = 'Please Select Academic Year';
                    //document.getElementById('Span8').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span6').hide();
                //}
            }
            else if (curitem == 'address_country') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Select Country');
                    return;
                    //document.getElementById('Span10').innerText = 'Please Select Country';
                    //document.getElementById('Span10').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span6').hide();
                //}
            }
            else if (curitem == 'address_state') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Select State');
                    return;
                    //document.getElementById('Span11').innerText = 'Please Select State';
                    //document.getElementById('Span11').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span11s').hide();
                //}
            }
            else if (curitem == 'fqualification') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Select Qualification');
                    return;
                    //document.getElementById('Span18').innerText = 'Please Select Qualification';
                    //document.getElementById('Span18').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span18').hide();
                //}
            }
            else if (curitem == 'fannual_income') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Select Annual Income');
                    return;
                    //document.getElementById('Span21').innerText = 'Please Select Annual Income';
                    //document.getElementById('Span21').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span21').hide();
                //}
            }
            else if (curitem == 'mqualification') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Select Qualification');
                    return;
                    //document.getElementById('Span29').innerText = 'Please Select Qualification';
                    //document.getElementById('Span29').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span29').hide();
                //}
            }
            else if (curitem == 'mannual_income') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Select Annual Income');
                    return;
                    //document.getElementById('Span32').innerText = 'Please Select Annual Income';
                    //document.getElementById('Span32').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span32').hide();
                //}
            }
            else if (curitem == 'gqualification') {
                if (document.getElementById(curitem).value == "0") {
                    alert('Please Fill Qualification');
                    return;
                    //document.getElementById('Span40').innerText = 'Please Fill Qualification';
                    //document.getElementById('Span40').show();
                    isValid = false;
                }
                //else {
                //    document.getElementById('Span40').hide();
                //}
            }
         
            else if (curitem == 'email2') {
                if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(document.getElementById(curitem).value)) {

                }
                else {
                    alert("Please Check Father Email Format");
                    return;
                    isValid = false;
                }
            }
            else if (curitem == 'email3') {
                if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(document.getElementById(curitem).value)) {

                }
                else {
                    alert("Please Check Mother Email Format");
                    return;
                    isValid = false;
                }
            }
            else if (curitem == 'email4') {
                if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(document.getElementById(curitem).value)) {

                }
                else {
                    alert("Please Check Gurdian Email Format");
                    return;
                    isValid = false;
                }
            }
        
            else if (curitem == 'adhar') {

            }
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid)
            nextStepWizard.removeAttr('disabled').trigger('click');
    });

    $('div.setup-panel div a.btn-primary').trigger('click');

    //my script 

    var stepval = parseInt(hfstepc.value) + 1;
    if (hfstepc.value >= 1) {
        var nextStepa = $('div.setup-panel div a[href="#step-' + stepval + '"]');
        //var nextStepWizard = $('div.setup-panel div a[href="#step-' + hfstepc.value + '"]').trigger('click');
        isValid = true;
        nextStepa.removeAttr('disabled').trigger('click');
    }

    DateSelectionChanged();
    ////other scripts

    $("#FileUpload1").change(function () {

        // Get uploaded file extension
        var extension = $(this).val().split('.').pop().toLowerCase();
        // Create array with the files extensions that we wish to upload
        var validFileExtensions = ['pdf', 'jpg', 'jpeg'];
        //Check file extension in the array.if -1 that means the file extension is not in the list. 
        if ($.inArray(extension, validFileExtensions) == -1) {
            $('#spnDocMsg').text("Sorry!! Upload only pdf,jpg,jpeg file").show();
            // Clear fileuload control selected file
            $(this).replaceWith($(this).val('').clone(true));
            //Disable Submit Button
            //$('#Save_btn').prop('disabled', true);
        }
        else {
            // Check and restrict the file size to 32 KB.
            if ($(this).get(0).files[0].size > (10000000)) {
                $('#spnDocMsg').text("Sorry!! Max allowed file size is 10 MB").show();
                // Clear fileuload control selected file
                $(this).replaceWith($(this).val('').clone(true));
                //Disable Submit Button
                //$('#Save_btn').prop('disabled', true);
            }
            else {
                //Clear and Hide message span
                $('#spnDocMsg').text('').hide();
                //Enable Submit Button
                //$('#Save_btn').prop('disabled', false);
            }
        }
    });

    $("#FileUpload2").change(function () {

        // Get uploaded file extension
        var extension = $(this).val().split('.').pop().toLowerCase();
        // Create array with the files extensions that we wish to upload
        var validFileExtensions = ['jpg', 'jpeg', 'png'];
        //Check file extension in the array.if -1 that means the file extension is not in the list. 
        if ($.inArray(extension, validFileExtensions) == -1) {
            $('#spnDocMsg1').text("Sorry!! Upload only jpg,jpeg,png file").show();
            // Clear fileuload control selected file
            $(this).replaceWith($(this).val('').clone(true));
            //Disable Submit Button
            //$('#Save_btn').prop('disabled', true);
        }
        else {
            // Check and restrict the file size to 32 KB.
            if ($(this).get(0).files[0].size > (5000000)) {
                $('#spnDocMsg1').text("Sorry!! Max allowed file size is 5 MB").show();
                // Clear fileuload control selected file
                $(this).replaceWith($(this).val('').clone(true));
                //Disable Submit Button
                //$('#Save_btn').prop('disabled', true);
            }
            else {
                //Clear and Hide message span
                $('#spnDocMsg1').text('').hide();
                //Enable Submit Button
                //$('#Save_btn').prop('disabled', false);
            }
        }
    });

    $("#FileUpload3").change(function () {

        // Get uploaded file extension
        var extension = $(this).val().split('.').pop().toLowerCase();
        // Create array with the files extensions that we wish to upload
        var validFileExtensions = ['jpg', 'pdf', 'jpeg'];
        //Check file extension in the array.if -1 that means the file extension is not in the list. 
        if ($.inArray(extension, validFileExtensions) == -1) {
            $('#spnDocMsg2').text("Sorry!! Upload only pdf,jpg,jpeg file").show();
            // Clear fileuload control selected file
            $(this).replaceWith($(this).val('').clone(true));
            //Disable Submit Button
            //$('#Save_btn').prop('disabled', true);
        }
        else {
            // Check and restrict the file size to 32 KB.
            if ($(this).get(0).files[0].size > (10000000)) {
                $('#spnDocMsg2').text("Sorry!! Max allowed file size is 10 MB").show();
                // Clear fileuload control selected file
                $(this).replaceWith($(this).val('').clone(true));
                //Disable Submit Button
                //$('#Save_btn').prop('disabled', true);
            }
            else {
                //Clear and Hide message span
                $('#spnDocMsg2').text('').hide();
                //Enable Submit Button
                //$('#Save_btn').prop('disabled', false);
            }
        }
    });

    //check sibling on load

    var sibling1 = document.getElementById('siblings').value;
    $("#8").hide();

    if (sibling1 == '2')
    //.....................^.......
    {
        $("#11").show();
        $("#2").show();
        $("#3").show();
        $("#7").hide();
        $("#5").hide();
        $("#6").hide();

    }
    else if (sibling1 == '3') {
        $("#11").show();
        $("#2").hide();
        $("#3").hide();
        $("#7").show();
        $("#5").show();
        $("#6").show();

    }
    else if (sibling1 == '0') {
        $("#11").hide();
        $("#1").hide();
        $("#2").hide();
        $("#8").hide();
        $("#3").hide();
        $("#7").hide();
        $("#5").hide();
        $("#6").hide();


    }
    else if (sibling1 == '1') {
        $("#11").hide();
        $("#1").hide();
        $("#2").hide();
        $("#8").hide();
        $("#3").hide();
        $("#7").hide();
        $("#5").hide();
        $("#6").hide();

    }
    $('#siblings').on('change', function () {

        if (this.value == '2')
        //.....................^.......
        {
            $("#11").show();
            $("#2").show();
            $("#3").show();
            $("#7").hide();
            $("#5").hide();
            $("#6").hide();
            $("#8").show();
        }
        else if (this.value == '3') {
            $("#11").show();
            $("#8").show();
            $("#2").show();
            $("#3").show();
            $("#7").show();
            $("#5").show();
            $("#6").show();
        }
        else if (this.value == '0') {
            $("#11").hide();
            $("#1").hide();
            $("#2").hide();
            $("#8").hide();
            $("#3").hide();
            $("#7").hide();
            $("#5").hide();
            $("#6").hide();
            //$("select[id$='Checkbox1']").prop("checked", false);
            // document.getElementById("Checkbox1").checked = false;
            var chk = document.getElementById('CheckBox1')
            var chk = document.getElementById('CheckBox1')
            chk.checked = false;


        }
        else if (this.value == '1') {
            $("#11").hide();
            $("#1").hide();
            $("#2").hide();
            $("#8").hide();
            $("#3").hide();
            $("#7").hide();
            $("#5").hide();
            $("#6").hide();
            //$("select[id$='Checkbox1']").prop("checked", false);
            // document.getElementById("Checkbox1").checked = false;
            var chk = document.getElementById('CheckBox1')
            chk.checked = false;
        }
    });

    valueChanged();
    reg_school_name();

    $('body').on('keydown', 'input, select, textarea', function (e) {
        var self = $(this)
            , form = self.parents('form:eq(0)')
            , focusable
            , next
            ;
        if (e.keyCode == 13) {
            focusable = form.find('input,a,select,button,textarea').filter(':visible');
            next = focusable.eq(focusable.index(this) + 1);
            if (next.length) {
                next.focus();
            } else {
                //form.submit();
            }
            return false;
        }
    });

    $("#acn").keyup(function () {
        if ($(this).val().length == 4) {
            $(this).val($(this).val() + "-");
        }
        else if ($(this).val().length == 9) {
            $(this).val($(this).val() + "-");
        }
        //else if ($(this).val().length == 14) {
        //    $(this).val($(this).val() + "-");
        //}
    });
    $("#facn").keyup(function () {
        if ($(this).val().length == 4) {
            $(this).val($(this).val() + "-");
        }
        else if ($(this).val().length == 9) {
            $(this).val($(this).val() + "-");
        }
        //else if ($(this).val().length == 14) {
        //    $(this).val($(this).val() + "-");
        //}
    });
    $("#macn").keyup(function () {
        if ($(this).val().length == 4) {
            $(this).val($(this).val() + "-");
        }
        else if ($(this).val().length == 9) {
            $(this).val($(this).val() + "-");
        }
        //else if ($(this).val().length == 14) {
        //    $(this).val($(this).val() + "-");
        //}
    });
    $("#gacn").keyup(function () {
        if ($(this).val().length == 4) {
            $(this).val($(this).val() + "-");
        }
        else if ($(this).val().length == 9) {
            $(this).val($(this).val() + "-");
        }
        //else if ($(this).val().length == 14) {
        //    $(this).val($(this).val() + "-");
        //}
    });


});

try {
    var t1 = document.getElementById('transfer');
    if (t1.checked == true) {
        $("#lsn").hide();
        $("#tc").hide();
    }
    else {
        $("#lsn").hide();
        $("#tc").hide();
    }
}
catch
{ }
var school = document.getElementById('CheckBox1')
if ($('.sib_reg_sch').is(":checked")) {
    $("#1").show();
    $("#8").hide();
}
else {
    $("#8").show();
    $("#1").hide();
}

function valueChanged() {
    if ($('.coupon_question').is(":checked")) {
        $("#lsn").show();
        $("#tc").show();
    }
    else {
        $("#lsn").hide();
        $("#tc").hide();
    }
}


function reg_school_name() {
    if ($('.sib_reg_sch').is(":checked")) {
        $("#1").show();
        $("#8").hide();
    }
    else {
        $("#8").hide();
        $("#1").hide();
    }
}

function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
        return (true)
    }
    alert("You have entered an invalid email address!")
    return (false)
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function FillBilling(f) {

    var selected;

    try {
        var hfg = document.getElementById('hfg');
        var inputs = f.getElementsByTagName("input");

        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                selected = inputs[i].value;
                hfg.value = selected;
                break;
            }
        }
    } catch{


        selected = f;
    }

    g_first_name.value = "";
    g_middle_name.value = "";
    g_last_name.value = "";
    gdob.value = "";
    gqualification.value = "";
    goccupation.value = "";
    gdesignation.value = "";
    gorg_name.value = "";
    gacn.value = "";
    gcontact_number.value = "";

    g_first_name.disabled = true;
    g_middle_name.disabled = true;
    g_last_name.disabled = true;
    gdob.disabled = true;
    gqualification.disabled = true;
    goccupation.disabled = true;
    gdesignation.disabled = true;
    gorg_name.disabled = true;
    gacn.disabled = true;
    gcontact_number.disabled = true;

    if (selected == 1) {

        g_first_name.value = f_first_name.value;
        g_middle_name.value = f_middle_name.value;
        g_last_name.value = f_last_name.value;
        gdob.value = fdob.value;
        gqualification.value = fqualification.value;
        goccupation.value = foccupation.value;
        gdesignation.value = fdesignation.value;
        gorg_name.value = forg_name.value;
        gacn.value = facn.value;
        email4.value = email2.value;
        gcontact_number.value = fcontact_number.value;
        gacn.disabled = true;
        DropDownList3.value = DropDownList1.value;
    }

    else if (selected == 2) {
        g_first_name.value = m_first_name.value;
        g_middle_name.value = m_middle_name.value;
        g_last_name.value = m_last_name.value;
        gdob.value = mdob.value;
        gqualification.value = mqualification.value;
        goccupation.value = moccupation.value;
        gdesignation.value = mdesignation.value;
        gorg_name.value = morg_name.value;
        gacn.value = macn.value;
        email4.value = email3.value;
        gcontact_number.value = mcontact_number.value;
        gacn.disabled = true;

        DropDownList3.value = DropDownList2.value;
    }

    else {

        g_first_name.disabled = false;
        g_middle_name.disabled = false;
        g_last_name.disabled = false;
        gdob.disabled = false;
        gqualification.disabled = false;
        goccupation.disabled = false;
        gdesignation.disabled = false;
        gorg_name.disabled = false;
        gacn.disabled = false;
        gcontact_number.disabled = false;
        gacn.disabled = false;

    }

}

function FillBilling1(f) {

    if (f.billingtoo1.checked == true) {

        $("#f").hide();
        billingtoo.checked == false;
    }
    else if (f.billingtoo1.checked == false) {
        f.g_first_name.value = "";
        f.g_middle_name.value = "";
        f.g_last_name.value = "";
        f.gdob.value = "";
        f.gqualification.value = "";
        f.goccupation.value = "";
        f.gdesignation.value = "";
        f.gorg_name.value = "";
        f.gacn.value = "";
        f.email4.value = "";
        f.gcontact_number.value = "";
        $("#f").show();
    }
}

function forcalendar(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 0 && charCode < 126) {
        return false;
    }
    return true;
}

function DateSelectionChanged() {


    var today_date = new Date();
    var txtdob = document.getElementById('dob');
    if (txtdob.value == "") {
        document.getElementById("txtCONSULTANT_AGE").innerText = "";
    }
    else {
        //var from = $("#datepicker").val().split("-");
        var from = txtdob.value.split("-");
        var user_date = new Date(from[0], from[1] - 1, from[2]);
        var diff_date = today_date - user_date - 1;

        if (diff_date < 0) {

            alert('Date of Birth Can Not Be Greater Than Today');
            return;
        }
        var num_years = diff_date / 31536000000;
        var num_months = (diff_date % 31536000000) / 2628000000;
        var num_days = ((diff_date % 31536000000) % 2628000000) / 86400000;

        var yearString = "";
        if (num_years > 1) yearString = " years";
        else yearString = Math.floor(num_years) + "Years ,";
        if (num_months > 1) monthString = " months";
        else yearString = yearString + Math.floor(num_months) + "Months,";
        if (num_days > 1) yearString = yearString + Math.floor(num_days) + "Days";
        else yearString = "0 day";
        document.getElementById("txtCONSULTANT_AGE").innerText = Math.floor(num_years) + "Years ," + Math.floor(num_months) + "Months," + Math.floor(num_days) + "Days";
    }

}


$("#upd4").change(function () {
    debugger;


    var ext = this.value.match(/\.(.+)$/)[1];
    switch (ext) {
        case 'jpg':
        case 'png':
        case 'pdf':
        case 'jpeg':
            break
        default:
            $("#spanupd4").text("Sorry!! Upload only pdf,jpg,jpeg,png file");
            return;
            break;
    }

    var filesize = this.files[0].size / 1024 / 1024;
    if (filesize < 2) {
        var filenme = $(this).val().split('\\');
        var lenn = filenme.length;
        var namee = filenme[lenn - 1];
        $("#spanupd4").text(namee);
    }
    else {
        $("#spanupd4").text('Sorry!! This file size is:' + filesize + 'MB');

    }


});
$("#spanupd4").click(function () {
    $("#upd4").click();
});
$("#imgupd4").click(function () {
    debugger;
    $("#upd4").click();
});


$("#upd5").change(function () {
    debugger;

    var ext = this.value.match(/\.(.+)$/)[1];
    switch (ext) {

        case 'pdf':

            break
        default:
            $("#spanupd5").text("Sorry!! Upload only pdf file");
            return;
            break;
    }

    var filesize = this.files[0].size / 1024 / 1024;
    if (filesize < 2) {

        var filenme = $(this).val().split('\\');
        var lenn = filenme.length;
        var namee = filenme[lenn - 1];



        $("#spanupd5").text(namee);

    }
    else alert('this file size is:' + filesize + 'MB');
});
$("#spanupd5").click(function () {
    $("#upd5").click();
});
$("#imgupd5").click(function () {
    debugger;
    $("#upd5").click();
});


$("#upd6").change(function () {
    debugger;

    var ext = this.value.match(/\.(.+)$/)[1];
    switch (ext) {

        case 'pdf':

            break
        default:
            $("#spanupd6").text("Sorry!! Upload only pdf file");
            return;
            break;
    }
    var filesize = this.files[0].size / 1024 / 1024;
    if (filesize < 2) {

        var filenme = $(this).val().split('\\');
        var lenn = filenme.length;
        var namee = filenme[lenn - 1];



        $("#spanupd6").text(namee);

    }
    else alert('this file size is:' + filesize + 'MB');
});
$("#spanupd6").click(function () {
    $("#upd6").click();
});
$("#imgupd6").click(function () {
    debugger;
    $("#upd6").click();
});