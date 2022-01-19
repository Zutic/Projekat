import { Agencija } from "./Agencija.js";
import { Grad } from "./Grad.js";
import { Kompanija } from "./Kompanija.js";

var listaGradova = [];

fetch("https://localhost:5001/Grad/Gradovi")
.then(p => {
    p.json().then(gradovi => {
        gradovi.forEach(grad => {
            var g = new Grad(grad.id, grad.naziv, grad.brojStanovnika, grad.brojNekretnina);
            listaGradova.push(g);
        });

        var listaKompanija = [];

        fetch("https://localhost:5001/Kompanija/UzmiKompanije")
        .then(p => {
            p.json().then(kompanije => {
                kompanije.forEach(kompanija => {
                    var k = new Kompanija(kompanija.id, kompanija.naziv);
                    listaKompanija.push(k);
                })

                var listaTipova = [];

                fetch("https://localhost:5001/Nekretnina/TipoviNekretnina")
                .then(p => {
                    p.json().then(tipovi => {
                        tipovi.forEach(tip => {
                            listaTipova.push(tip);
                        })

                        var agencija = new Agencija(listaGradova, listaKompanija, listaTipova);
                        agencija.crtaj(document.body);
                    })
                })
            })
        })
    })
})


