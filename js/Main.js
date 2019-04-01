
$(document).ready(function () {
    $(".username").focus(function () {
        $(".user-icon").css("left", "-48px");
    });
    $(".username").blur(function () {
        $(".user-icon").css("left", "0px");
    });

    $(".password").focus(function () {
        $(".pass-icon").css("left", "-48px");
    });
    $(".password").blur(function () {
        $(".pass-icon").css("left", "0px");
    });

});

function successalert() {
    swal({
        title: 'Uspešno uneta faktura.',
        text: '',
        type: 'OK'
    });
};



function erroralertSearch() {
    swal({
        title: 'Greška prilikom pretraživanja fakture.',
        text: 'Kontaktirajte administratora.',
        type: 'OK'
    });
};