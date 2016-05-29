
/*Begin of declarating global variables*/
/*Check the last click button in Account menu : use it to disable click and reload every time*/

var typeOfUser = "";
var appointmentIDtime = "";
/*End of declarating global variables*/

//Begin datepicker

$(document).on("click touch", ".hasDatepicker", function () {
    
    $(".hasDatepicker").css("z-index", "999999");
    setTimeout(function () { $(".hasDatepicker").modal("hide"); }, 120000);
    setTimeout(function () { $(".hasDatepicker").css("z-index", "100"); }, 5000);
})

$(function () {

    

    $("#appointmentDate").datepicker({
        minDate: 0,
        dateFormat: 'yy-mm-dd'
    });


});


//END datepicker

//Begin of Login Event
//On click and ontouch events declarated if the user
//click on button #LoginButton he we recieve error message if the passed data is wrong
$(document).on("click touch", "#LoginButton", function (ev) {
    var user = $("#accId").attr("data-userid");

    var userName = $('[name=logUser]').val();
    var pass = $('[name=logPass]').val();

    var isEmpty=$("#profile_result").text();
    if(isEmpty!="")
    {
        $("#modal-login-success").modal("show");
        $("#profile_result").show();
       
        return true;
    }
    

    $.post('/Hospitology/Login/getUser', {
        Username: userName,
        Password: pass
    }, function (result) {
        if (result.length === 0) {

            $("#modal-data-error").css("z-index", "999999");
            $("#modal-data-error").modal("show");

            $("#error-result").html("<p class='error'>Въведете правилен потребител и парола .</p>");
            $("#error-result").css("font-weight", "bold");
            $("#error-result").css("color", "red");
            setTimeout(function () { $("#modal-date-error").modal("hide"); $("#modal-data-error").css("z-index", "100"); }, 20000);
            return false;
        }
        if (userName == result.Username && pass == result.Password) {
            $('#modal-account').modal("hide");
            $('.username').text(userName);
            $('.glyphicon-user').css("color", "green");
            //Start
            //patient profile initialization
            if (result.EGN !== undefined) {
                typeOfUser="Patient";
                $('#accId').attr("data-userid", result.EGN);
                $('#accId').attr("data-doctorid", result.DoctorID);

                $("#login-My-Acc").text("Профил");
                $("#login-My-Medical-Card").text("Медицински картон");
                $("#login-x").text("Моите описания");
                $("#login-y").text("Прегледи при GP");

        
                $("#profile_result").html("<div class='table-responsive'>" +
                "<table class='table '>" +
                "<thead><tr class='text-center'><th><h3>За мен</h3></th></tr></thead><tbody>" +
                "<tr><td><strong>ЕГН: </strong>" + result.EGN + "</td></tr>" +
                "<tr><td><strong>Email: </strong>"+result.Email+"</td></tr>" +
                "<tr><td><strong>Име: </strong>"+result.Firstname+"</td></tr>" +
                        "<tr><td><strong>Фамилия: </strong>"+result.Lastname+"</td></tr></tbody></table></div>" +
                "<div class='table-responsive'>" +
                "<table class='table'>"+
                "<thead class='text-center'><tr><th><h3>Личен лекар</h3></th></tr></thead><tbody>" +
                "<tr><td><strong>Име: </strong>" + result.docLastname + "</td></tr>" +
                "<tr><td><strong>Работно време: </strong>"+ result.StartTime + "-" + result.EndTime +  "</td></tr>" +
                "<tr><td><strong>Мобилен тел.: </strong><a href='tel:" + result.docMobilePhone + "'>" + result.docMobilePhone + "</a></td></tr>"+
                "<tr><td><strong>Стационарен тел.: </strong><a href='tel:" + result.docPhone + "'>" + result.docPhone + "</a></td></tr>"+
                "<tr><td><strong>Адрес : </strong>" + result.City + "</td></tr><tr><td>" + result.docAddress + "</td></tr>" +
                "</tbody></table></div>");
                /*
                $("#profile_result").html("<div class='text-center'><div><h3>Лична информация</h3></div><p>Псевдоним :  " + result.Username + "</p><br/><p>ЕГН : " + result.EGN + "</p><br/>" +
               "<p>Email :  " + result.Email + "</p><br/><p>Име :  " + result.Firstname + "</p><br/>" +
               "<p>Фамилия :  " + result.Lastname + "</p><div><h3>Информация за личен лекар/GP/</h3></div><br/><p>Личен лекар: " + result.docLastname + "</p><br/><p>Работно време: " + result.StartTime + "-" + result.EndTime + "</p><br/>" +
               "<p>Личен лекар мобилен тел. : </p><a href='tel:" + result.docMobilePhone + "'>" + result.docMobilePhone + "</a><br/>"+
               "<p>Личен лекар стационарен тел.: </p><a href='tel:" + result.docPhone + "'>" + result.docPhone + "</a><br/>"+
               "<p>Адрес на личен лекар: гр." + result.City + " ," + result.docAddress + "</p><br/></div>");
               */
            }
            //End

            //Start
            //admin profile initialization
            if (result.AdminName !== undefined) {
                $('#accId').attr('data-adminid', result.AdminName);
                typeOfUser = "Admin";
           }

            //End




            //Start
            //docotr profile initialization
            if (result.IDDoctor !== undefined) {
            typeOfUser = "Doctor";
            $('#accId').attr('data-doctorid', result.IDDoctor);

            $('#accId').attr("data-doctorid", result.IDDoctor);

            $("#login-My-Acc").text("Профил");
            $("#login-My-Medical-Card").text("Лични амбулаторни картони");
            $("#login-x").text("Пациенти");
            $("#login-y").text("Регистритай пациент");
            //The Picture should be resizable !
            $("#profile_result").html("<img src='" + result.Picture + "' alt='Доктор' style='width:120px;height:170px'></img><div>Д-р Валери Пенев !</div>");

        }

            //End
        }



        $("#accId").removeAttr('data-target').attr({ 'data-target': '#modal-login-success' });

       

        //End of initialization


        $("#modal-login-success").modal("show");
        lastClick = "#login-My-Acc";

    })
});
    
//End of Login Event



//Login-history start
$(document).on("click touch", "#login-x", function () {

    //create a global variable to detect what type of user is Logged Patient/Doctor/User
    $("#profile_result").hide();
    $("#profile_Medical_Cards").hide();
    $("#profile_y").hide();
    
    var checkIfEmpty = $("#profile_x").text();
        if(typeOfUser=="Patient")
        {
            
            $("#profile_x").html("<div class='table-responsive'>" +
               "<table  class='table'>" +
               "<thead><tr class='text-center'><th><h3>Изберете период</h3></th></tr></thead><tbody>" +
               "<tr><td>От:<span class='glyphicon glyphicon-calendar'></span><input type='text'  id='PatientAppFrom' " +
               "class='datepicker dpHistoryApp input-sm'></td></tr>" +
               "<tr><td>До: <span class='glyphicon glyphicon-calendar'></span><input type='text'  id='PatientAppTo' class='dpHistoryApp datepicker input-sm'></td></tr>" +
               "<tr><td><button  id='myDescriptionHistory' class='btn btn-primary btn-sm' value='Търси'>Търси</button></td></tr>" +
               "</tbody></table></div><div class='table-responsive'></div>"+
               "<table id='patient_description_table' class='table'>" +
               "<thead><tr><th><h3>Описания</h3></th></tr></thead><tbody class='p_d_b'>" +
               "<tr><td><strong>Личен лекар</strong></td><td><strong>Дата на преглед</strong></td></tr>" +
               "</tbody></table></div>");
               
               

            $(".dpHistoryApp").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        }
        
        if (typeOfUser == "Doctor") {

        }

        if (typeOfUser == "Admin") {

        }
        $("#profile_x").show();
});
//login-history end


//login-my-account
$(document).on("click touch", "#login-My-Acc", function () {
    $("#profile_Medical_Cards").hide();
    $("#profile_x").hide();
    $("#profile_y").hide();
    $("#profile_result").show();
    
});






//Start Check if user is Logged
$(document).on("touch click", "#appId, #accId", function () {
    var patient = $("#accId").attr("data-userid");
    var doctor = $("#accId").attr("data-doctorid");
    var admin = $("#accId").attr("data-adminid");
    if (patient == "" && doctor == "" && admin == "") {
        $("#modal-account").modal("show");
    }
});

//End Check if user is Logged

//Start First Login load
$(document).ready(function () {
    $("#modal-account").modal("show");
});
//End First Login load

//Start Show Appointment Modal for PATIENTS
$(document).on("touch click", "#appId", function () {
    $("#modal-appointment-picker").modal("show");

});
//END Show Appointment Modal for PATIENTS

$(document).on("touch click", "#appointmentSearch", function () {
    var patient = $("#accId").attr("data-userid");
    var doctor = $("#accId").attr("data-doctorid");
    var admin = $("#accId").attr("data-adminid");

    var date = $("#appointmentDate").val();
    var tomorrow = new Date(date);
    tomorrow.setDate(tomorrow.getDate() + 1);
    //after new SHIT 
   // $("#appointment-result").empty();
    var dayAfterToday = $.datepicker.formatDate("yy-mm-dd", tomorrow);
    //this would keep idiots safe
     $("#x").find("tr:gt(0)").remove();

    if (patient != "") {

        //POST Request to get the engaged hours of doctor appointment
        $.post('/Hospitology/DoctorBreaks/getDoctorBreaks', {
            Patient: patient,
            DoctorID: doctor,
            AppDate: date,
            AppDatePlusOne: dayAfterToday
        }, function (result) {

            //IF THERE IS ALLREADY APPOINTMENT 
            if(result.length==0)
            {
                $("#modal-appointment-exist").css("z-index", "999999");
                $("#modal-appointment-exist").modal("show");
                    $("#x").children('tr:not(:first)').remove();
                setTimeout(function () { $("#modal-appointment-exist").modal("hide");}, 20000)
                setTimeout(function () { $("#modal-data-error").css("z-index", "100"); },100);
                return false;
            }
            //TODO HOW TO REMOVE THE ENGAGED DATE
            
            //CHECK IF THERE ARE Brakes
            if (result.length != 0) {
                
                $.each(result, function (row, field) {

                  

                    var r = "<tr><td>"+field+"</td><td>"+
                    "<button  type='button' name='"+field+"' class='appoint-btn btn-primary btn-md'>Запази</button></td></tr>";
                    $("#x tr:last").after(r);
                });

            } 
        })
    }

    //Doctor will search for his appointments And Doctor is Logged
    if(doctor!="" && patient=="" && admin=="")
    {

    }

    //Admin will search for specific data And Admin is Logged
    if (admin != "")
    {

    }
});
       

//On click tocuh .app-btn
$(document).on("click touch", ".appoint-btn", function (ev)
{
    appointmentIDtime = $(this).attr("name");
    $("#modal-description").modal("show");
});



//INSERT DATE INTO APPOINTMENT HISTORY START
$(document).on("click touch", "#succesAppointment", function () {
    var description = $("textarea#PatientDescription").val();

    var appointmentDatePicker = $("#appointmentDate").datepicker({ dateFormat: 'yy-mm-dd' }).val();

    appointmentDatePicker += " " + appointmentIDtime;
    var patientPain = "Пациента изпитва болки в ";
    if ($('#Head').is(":checked")) {
        patientPain += "главата, ";
    }

    if ($('#Rear').is(":checked")) {
        patientPain += "тила, ";
    }

    if ($('#Waist').is(":checked")) {
        patientPain += "кръста, ";
    }

    if ($('#Eyes').is(":checked")) {
        patientPain += "очите, ";
    }

    if ($('#Ears').is(":checked")) {
        patientPain += "ушите, ";
    }
    if ($('#Shoulder').is(":checked")) {
        patientPain += "раменте, ";
    }
    if ($('#Hands').is(":checked")) {
        patientPain += "ръцете, ";
    }
    if ($('#Chest').is(":checked")) {
        patientPain += "гръдният кош, ";
    }
    if ($('#Heart').is(":checked")) {
        patientPain += "сърцето, ";
    }
    if ($('#Lung').is(":checked")) {
        patientPain += "белият дроб, ";
    }
    if ($('#Liver').is(":checked")) {
        patientPain += "черниият дроб, ";
    }
    if ($('#Kidneys').is(":checked")) {

        patientPain += "бъбреците, ";
    }


    var lastComma = patientPain.lastIndexOf(',');
    patientPain = patientPain.slice(0, lastComma);
    patientPain += ".";

    if ($('#MedCheck').is(":checked")) {

        patientPain += "профилактичен преглед.";
    }


    var patient = $("#accId").attr("data-userid");
    var doctor = $("#accId").attr("data-doctorid");

    $.post('/Hospitology/InsertAppointment/InsertAppointmentIntoDB', {
        IDPatient: patient,
        IDDoctor: doctor,
        appointment_date: appointmentDatePicker,
        patient_paint: patientPain,
        description: description
    }, function (result) {
        if(result=="True")
        {
            //if there is no crash in the server
            $("#appointment-result").empty();
            $("#modal-appointment-question").modal("hide");
            $("#modal-description").modal("hide");
            $("#modal-appointment-success").modal("show");
            setTimeout(function () { $("#modal-appointment-success").modal("hide"); }, 3350)
        } else {
            //if there is a crash in the server
            $("#appointment-result").empty();
            $("#modal-appointment-question").modal("hide");
            $("#modal-description").modal("hide");
            $("#modal-appointment-unsuccess").modal("show");
            setTimeout(function () { $("#modal-appointment-unsuccess").modal("hide"); }, 3350)
        }
        

    });
});

//INSERT DATE INTO APPOINTMENT HISTORY END



//Chose is this your appointment START
$(document).on("click touch", "#AppTrue", function () {
   
    var description = $("textarea#PatientDescription").val();

    var appointmentDatePicker = $("#appointmentDate").datepicker({ dateFormat: 'yy-mm-dd' }).val();
    var x = true;
    appointmentDatePicker += " " + appointmentIDtime;
    var patientPain = "Пациента изпитва болки в ";
    if ($('#Head').is(":checked")) {
        patientPain += "главата, ";
        x = false;
    }

    if ($('#Rear').is(":checked")) {
        patientPain += "тила, ";
        x = false;
    }

    if ($('#Waist').is(":checked")) {
        patientPain += "кръста, ";
        x = false;
    }

    if ($('#Eyes').is(":checked")) {
        patientPain += "очите, ";
        x = false;
    }

    if ($('#Ears').is(":checked")) {
        patientPain += "ушите, ";
        x = false;
    }
    if ($('#Shoulder').is(":checked")) {
        patientPain += "раменте, ";
        x = false;
    }
    if ($('#Hands').is(":checked")) {
        patientPain += "ръцете, ";
        x = false;
    }
    if ($('#Chest').is(":checked")) {
        patientPain += "гръдният кош, ";
        x = false;
    }
    if ($('#Heart').is(":checked")) {
        patientPain += "сърцето, ";
        x = false;
    }
    if ($('#Lung').is(":checked")) {
        patientPain += "белият дроб, ";
        x = false;
    }
    if ($('#Liver').is(":checked")) {
        patientPain += "черниият дроб, ";

    }
    if ($('#Kidneys').is(":checked")) {
        patientPain += "бъбреците, ";
        x = false;
    }

    if(x==true)
    {
        patientPain = "";
    }
    var lastComma = patientPain.lastIndexOf(',');
    patientPain=    patientPain.slice(0, lastComma);
    patientPain += "."

    if ($('#MedCheck').is(":checked")) {

        patientPain += "Профилактичен преглед.";
    }

   
    $("#modal-description").modal("hide");
    $(".app_question").append("<div><strong>Дата на прегледа: </strong>" + appointmentDatePicker + "</div><br/>" +
        "<div>" + patientPain + "<div><br/>" +
        "<div>" + description + "</div></div>");

    $("#modal-appointment-question").modal("show");

});
//Chose is this your appointment END




//Get All the descriptions
$(document).on("click touch", "#myDescriptionHistory", function () {
    var appDateFrom = $("#PatientAppFrom").val();
    var appDateTo = $("#PatientAppTo").val();
    var patient = $("#accId").attr("data-userid");
    var doctor = $("#accId").attr("data-doctorid");
    if ((appDateFrom > appDateTo) || appDateFrom=="" || appDateTo=="") {
        $("#modal-data-error").css("z-index", "999999");
        $("#modal-data-error").modal("show");

        $("#error-result").html("<p class='error'>Въведете валидни дати .</p>");
        $("#error-result").css("font-weight", "bold");
        $("#error-result").css("color", "red");
        setTimeout(function () { $("#modal-date-error").modal("hide");  }, 3550);
        setTimeout(function () { $("#modal-data-error").css("z-index", "100"); }, 100);
        return false;
    }
    //remove previous results
    $(".p_d_b").find("tr:gt(0)").remove();

    $.post('/Hospitology/GetPatientAppHistory/getPatientAppHistory', {
        DateFrom: appDateFrom,
        DateTo: appDateTo,
        PatientID: patient,
        DoctorID: doctor
    }, function (result) {
        $.each(result, function (row, field) {
            var r = "<tr rowspan='2'><td name='doctorName"+field.appointment_date+"'>" + field.IDDoctor + "</td><td name='PAD"+field.appointment_date+"'>" + field.appointment_date + 
            "</td><td name='patientPain" + field.appointment_date + "' class='hidder'>" + field.patient_paint + "</td><td name='patientDescription" + field.appointment_date + "' class='hidder'>" + field.description + "</td><td></tr>" +
            "<tr><td><button  type='button' name='"+field.appointment_date+"'class='btn-primary btn-md descriptionTriger'>Описание</button></td></tr>";
            $("#patient_description_table tr:last").after(r);

        });



    });
    //ENd Get all the description
 

});
//Patient Profile Account}

$(document).on("click touch", ".descriptionTriger", function (ev) {
    ev.stopPropagation();
    
    var x = $(this).attr("name");
    var docNameBuilder = "doctorName" + x;
    var appDateBuilder = "PAD" + x;
    var patientPainBuilder = "patientPain" + x;
    var patientDescriptionBuilder = "patientDescription" + x;
    
    var docName = $("[name='" + docNameBuilder + "']").text();
    var appDate = $("[name='" + appDateBuilder + "']").text();
    var patPain = $("[name='" + patientPainBuilder + "']").text();
    var patDescription = $("[name='" + patientDescriptionBuilder + "']").text();
   
    $("[name='bodyDescription']").empty();
    $("[name='bodyDescription']").html(
        "<div><strong>Име на лекар:</strong> " + docName + "</div>" +
        "<div><strong>Дата на преглед:</strong> " + appDate + "</div>" +
        "<div><strong>Болки на пациента:</strong> " + patPain + "</div>" +
        "<div><strong>Описание на пациента:</strong> " + patDescription + "</div>"
        );
    $("#modal-history-description").modal("show");
    
    //add the values to Modal  
    
});
$(document).on("click touch", "#login-y", function (ev) {
    var patient = $("#accId").attr("data-userid");
    var doctor = $("#accId").attr("data-doctorid");

    $("#profile_result").hide();
    $("#profile_Medical_Cards").hide();
    $("#profile_x").hide();

    $("#profile_y").empty();
    var checkIfEmpty = $("#profile_y").text();

    if (checkIfEmpty == "") {

        if (typeOfUser == "Patient") {
            $("#profile_y").html("<div class='table-responsive'>" +
               "<table  class='table'>" +
               "<thead><tr class='text-center'><th><h3>Текущи прегледи</h3></th></tr></thead><tbody>" +
               
               "</tbody></table></div><div class='table-responsive'></div>" +
               "<table id='patient_future_app_table' class='table'>" +
               "<thead><tr><th><h3>Прегледи</h3></th></tr></thead><tbody>" +
               //this is new
                "<tr><td>От:<span class='glyphicon glyphicon-calendar'></span><input type='text'  id='PatientAppFrom' " +
               "class='datepicker dpHistoryApp input-sm'></td></tr>" +
               "<tr><td>До: <span class='glyphicon glyphicon-calendar'></span><input type='text'  id='PatientAppTo' class='dpHistoryApp datepicker input-sm'></td></tr>" +
               "<tr><td><button  id='myDescriptionHistory' class='btn btn-primary btn-sm' value='Търси'>Търси</button></td></tr>" +

               "<tr><td><strong>Личен лекар</strong></td><td><strong>Дата на преглед</strong></td></tr></tbody>" +
               "</tbody></table></div>"
               );
            $(".dpHistoryApp").datepicker({
                dateFormat: 'yy-mm-dd'
            });


        }

        if (typeOfUser == "Doctor") {

        }

        if (typeOfUser == "Admin") {

        }


        $("#profile_y").show();


    }

    else {
        $("#profile_y").show();

    }


    $.post('/Hospitology/GetFutureAppointments/getPatientFutureApp', {
        PatientID: patient,
        DoctorID: doctor
    }, function (result) {
        $.each(result, function (row, field) {
            if (result !== 0) {
                var r = "<tr rowspan='2'><td name='doctorNameCurrApp" + field.appointment_date + "'>" + field.IDDoctor + "</td><td name='PADCurrApp" + field.appointment_date + "'>" + field.appointment_date +
                "</td></tr>" +
                "<tr><td><button  type='button' name='" + field.appointment_date + "'class='btn-primary btn-md removeA'>Премахни преглед</button></td></tr>";
                $("#patient_future_app_table tr:last").after(r);
            }
            

        });
    });    
});

$(document).on("click touch", ".removeA", function (ev) {


    ev.stopPropagation();
    //x->this is the date of the appointment
    var x = $(this).attr("name");

    //patient id 
    var patient = $("#accId").attr("data-userid");

    $.post('/Hospitology/GetFutureAppointments/getPatientFutureApp', {
        PatientID: patient,
        DoctorID: doctor
    }, function (result) {
        $.each(result, function (row, field) {
            if (result !== 0) {
                var r = "<tr rowspan='2'><td name='doctorNameCurrApp" + field.appointment_date + "'>" + field.IDDoctor + "</td><td name='PADCurrApp" + field.appointment_date + "'>" + field.appointment_date +
                "</td></tr>" +
                "<tr><td><button  type='button' name='" + field.appointment_date + "'class='btn-primary btn-md removeA'>Премахни преглед</button></td></tr>";
                $("#patient_future_app_table tr:last").after(r);
            }


        });

        //create post request to remove this appointment from the database oK

    });

});
    //hide the modal-description 
    $(document).on("click touch", "#AppFalse", function (ev) {
        $("#modal-description").modal("hide");
        $("#modal-appointment-question").modal("hide");
    });

    $(document).on("click touch", "#falseAppointment", function (ev) {
        $("#modal-appointment-question").modal("hide");
    });



