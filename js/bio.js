function successalert() {
    swal({
        title: 'Uspešno uneta faktura.',
        text: '',
        type: 'OK'
    });
};

function erroralert() {
    swal({
        title: 'Greška prilikom unosa.',
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

function erroralertTermin() {
    swal({
        title: 'Greška prilikom unosa termina.',
        text: 'Morate završiti predavanje koje je u toku.',
        type: 'OK'
    });
};

function erroralertFileName() {
    swal({
        title: 'Greška.',
        text: 'Greška prilikom unosa imena izveštaja u bazu podataka.',
        type: 'OK'
    });
};