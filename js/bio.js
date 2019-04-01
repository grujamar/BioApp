function successalert() {
    swal({
        title: 'Uspešno uneta faktura.',
        text: '',
        type: 'OK'
    });
};

function erroralert() {
    swal({
        title: 'Greška prilikom unosa fakture.',
        text: 'Ispravite podatke i pokušajte ponovo.',
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