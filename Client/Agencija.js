import { Nekretnina } from "./Nekretnina.js";
import { Grad } from "./Grad.js";

export class Agencija{

    constructor(listaGradova, listaKompanija, listaTipova){
        this.listaGradova = listaGradova;
        this.listaKompanija = listaKompanija;
        this.listaTipova = listaTipova;
        this.kontejner = null;
        this.nekretnineForma = null;
        this.filterGrad = document.createElement("div");
        this.filterKomp = document.createElement("div");
    }

    crtaj(host){
        this.kontejner = document.createElement("div");
        this.kontejner.className = "GlavniKontejner";
        host.appendChild(this.kontejner);

        this.upravKontejner = document.createElement("div");
        this.upravKontejner.className = "Upravljanje";
        this.kontejner.appendChild(this.upravKontejner);

        let dodajForma = document.createElement("div");
        dodajForma.className = "Forma";
        this.upravKontejner.appendChild(dodajForma);

        let prikazForma = document.createElement("div");
        prikazForma.className = "Forma";
        this.upravKontejner.appendChild(prikazForma);

        prikazForma.appendChild(this.filterGrad);
        prikazForma.appendChild(this.filterKomp);

        let azuriranjeForma = document.createElement("div");
        azuriranjeForma.className = "Forma";
        this.upravKontejner.appendChild(azuriranjeForma);

        let gradoviForma = document.createElement("div");
        gradoviForma.className = "Forma";
        this.upravKontejner.appendChild(gradoviForma);

        this.nekretnineForma = document.createElement("div");
        this.nekretnineForma.className = "OdabraneNekretnine";
        this.kontejner.appendChild(this.nekretnineForma);

        this.crtajFormuDodaj(dodajForma);
        this.crtajFormuPrikaz(prikazForma, this.filterGrad, this.filterKomp);
        this.crtajFormuAzuriranje(azuriranjeForma);
        this.crtajFormuGradovi(gradoviForma);
        this.crtajFormuNekretnine(this.nekretnineForma);
    }

    crtajRed(host){
        let red = document.createElement("div");
        red.className = "Red";
        host.appendChild(red);
        return red;
    }

    crtajFormuDodaj(host){
        let red = this.crtajRed(host);

        let dodajL = document.createElement("label");
        dodajL.innerHTML = "Dodavanje nekretnine";
        dodajL.className = "Naslov";
        red.appendChild(dodajL);

        red = this.crtajRed(host);

        let divLabele = document.createElement("div");
        divLabele.className = "LabeleDodavanja";
        let divIzbor = document.createElement("div");
        divIzbor.className = "IzborDodavanja";
        red.appendChild(divLabele);
        red.appendChild(divIzbor);

        let graditeljL = document.createElement("label");
        graditeljL.innerHTML = "Graditelj";
        divLabele.appendChild(graditeljL);

        let graditeljSe = document.createElement("select");
        divIzbor.appendChild(graditeljSe);

        let graditeljOp;
        this.listaKompanija.forEach(p => {
            graditeljOp = document.createElement("option");
            graditeljOp.innerHTML = p.naziv;
            graditeljOp.value = p.id;
            graditeljSe.appendChild(graditeljOp);
        })

        let tipL = document.createElement("label");
        tipL.innerHTML = "Tip";
        divLabele.appendChild(tipL);

        let tipSe = document.createElement("select");
        divIzbor.appendChild(tipSe);

        let tipOp;
        this.listaTipova.forEach(t => {
            tipOp = document.createElement("option");
            tipOp.innerHTML = t;
            tipOp.value = t;
            tipSe.appendChild(tipOp);
        })

        let cenaL = document.createElement("label");
        cenaL.innerHTML = "Pocetna cena";
        divLabele.appendChild(cenaL);

        let cenaSe = document.createElement("input");
        cenaSe.setAttribute("type", "number");
        divIzbor.appendChild(cenaSe);

        let slikaL = document.createElement("label");
        slikaL.innerHTML = "Slika";
        divLabele.appendChild(slikaL);

        let slikaSe = document.createElement("input");
        divIzbor.appendChild(slikaSe);

        let gradL = document.createElement("label");
        gradL.innerHTML = "Lokacija";
        divLabele.appendChild(gradL);

        let gradSe = document.createElement("select");
        divIzbor.appendChild(gradSe);

        let gradOp;
        this.listaGradova.forEach(p => {
            gradOp = document.createElement("option");
            gradOp.innerHTML = p.naziv;
            gradOp.value = p.id;
            gradSe.appendChild(gradOp);
        })

        red = this.crtajRed(host);
        red.className = "DugmePotvrde";

        let dodajbtn = document.createElement("button");
        dodajbtn.onclick = (ev) => this.dodajNekretninu(graditeljSe.value, tipSe.value, cenaSe.value, 
                                                                slikaSe.value, gradSe.value);
        dodajbtn.innerHTML = "Dodaj nekretninu";
        red.appendChild(dodajbtn);
    }

    crtajFormuPrikaz(host, host1, host2){
        let red = this.crtajRed(host);
        let red1 = this.crtajRed(host1);
        let red2 = this.crtajRed(host2);

        let prikazL = document.createElement("label");
        prikazL.innerHTML = "Prikaz nekretnine";
        red.className = "Naslov";
        red.appendChild(prikazL);

        let filtrirajGL = document.createElement("label");
        filtrirajGL.innerHTML = "Filtriraj po gradu:";
        red1.appendChild(filtrirajGL);

        let cbGrad;
        this.listaGradova.forEach(g => {
            red1 = this.crtajRed(host1);
            cbGrad = document.createElement("input");
            cbGrad.type = "checkbox";
            cbGrad.value = g.id;
            red1.appendChild(cbGrad);

            let imeL = document.createElement("label");
            imeL.innerHTML = g.naziv;
            red1.appendChild(imeL);
        })

        let filtrirajKL = document.createElement("label");
        filtrirajKL.innerHTML = "Filtriraj po kompaniji:";
        host2.appendChild(filtrirajKL);

        let cbKomp;
        this.listaKompanija.forEach(k => {
            red2 = this.crtajRed(host2);
            cbKomp = document.createElement("input");
            cbKomp.type = "checkbox";
            cbKomp.value = k.id;
            red2.appendChild(cbKomp);

            let imeL = document.createElement("label");
            imeL.innerHTML = k.naziv;
            red2.appendChild(imeL);
        })

        red = this.crtajRed(host);
        red.className = "DugmePotvrde";

        let prikazibtn = document.createElement("button");
        prikazibtn.className = "PrikaziNekretnineDugme";
        prikazibtn.onclick = (ev) => this.nadjiNekretnine();
        prikazibtn.innerHTML = "Prikazi nekretnine";
        red.appendChild(prikazibtn);
    }

    crtajFormuGradovi(host){
        let red = this.crtajRed(host);

        let infoGradoviL = document.createElement("label");
        infoGradoviL.innerHTML = "Informacije o gradovima:";
        infoGradoviL.className = "Naslov";
        red.appendChild(infoGradoviL);

        var listaGradova = [];

        fetch("https://localhost:5001/Grad/Gradovi")
        .then(p => {
            p.json().then(gradovi => {
                gradovi.forEach(grad => {
                    var g = new Grad(grad.id, grad.naziv, grad.brojStanovnika, grad.brojNekretnina);
                    listaGradova.push(g);
                });
        
                this.listaGradova.forEach(p => {
                    red = this.crtajRed(host);
                    let gradL = document.createElement("label");
                    gradL.innerHTML = p.naziv + ":";
                    red.appendChild(gradL);

                    red = this.crtajRed(host);
                    let infoL = document.createElement("label");
                    infoL.innerHTML = "Broj stanovnika - " + p.brojStanovnika + "; Broj nekretnina - " + p.brojNekretnina;
                    red.appendChild(infoL);
                })
            })
        })

        // let gradovibtn = document.createElement("button");
        // gradovibtn.className = "PrikaziGradoveDugme";
        // gradovibtn.onclick = (ev) => this.crtajFormuGradovi(host);
        // red.appendChild(gradovibtn);
        // gradovibtn.style.display = "none";
    }

    crtajFormuAzuriranje(host){
        let red = this.crtajRed(host);

        let infoGradoviL = document.createElement("label");
        infoGradoviL.innerHTML = "Azuriranje:";
        infoGradoviL.className = "Naslov";
        red.appendChild(infoGradoviL);

        var dugmad = document.createElement("div");
        dugmad.className = "Dugmad";
        host.appendChild(dugmad);

        var dodajGradDeo = document.createElement("div");
        dodajGradDeo.style.display = "none";
        host.appendChild(dodajGradDeo);
        this.crtajDodavanjeGrada(dodajGradDeo);

        var dodajKompanijuDeo = document.createElement("div");
        dodajKompanijuDeo.style.display = "none";
        host.appendChild(dodajKompanijuDeo);
        this.crtajDodavanjeKompanije(dodajKompanijuDeo);

        var dodajStanovnikaDeo = document.createElement("div");
        dodajStanovnikaDeo.style.display = "none";
        host.appendChild(dodajStanovnikaDeo);
        this.crtajDodavanjeStanovnika(dodajStanovnikaDeo);

        var dodajGradbtn = document.createElement("button");
        dodajGradbtn.innerHTML = "Prosiri poslovanje";
        dodajGradbtn.onclick = (ev) =>
        {
            if (dodajGradDeo.style.display === "none")
            dodajGradDeo.style.display = "block";
            else
            dodajGradDeo.style.display = "none";
        };
        dugmad.appendChild(dodajGradbtn);

        var dodajKompanijubtn = document.createElement("button");
        dodajKompanijubtn.innerHTML = "Dodaj kompaniju";
        dodajKompanijubtn.onclick = (ev) =>
        {
            if (dodajKompanijuDeo.style.display === "none")
            dodajKompanijuDeo.style.display = "block";
            else
            dodajKompanijuDeo.style.display = "none";
        };
        dugmad.appendChild(dodajKompanijubtn);
        
        var dodajStanovnikabtn = document.createElement("button");
        dodajStanovnikabtn.innerHTML = "Unesi stanovnika";
        dodajStanovnikabtn.onclick = (ev) =>
        {
            if (dodajStanovnikaDeo.style.display === "none")
            dodajStanovnikaDeo.style.display = "block";
            else
            dodajStanovnikaDeo.style.display = "none";
        };
        dugmad.appendChild(dodajStanovnikabtn);
    }

    crtajDodavanjeGrada(host){
        let red = this.crtajRed(host);

        let naslov = document.createElement("label");
        naslov.innerHTML = "Dodaj novi grad";
        naslov.className = "Naslov";
        red.appendChild(naslov);

        red = this.crtajRed(host);

        let gradL = document.createElement("label");
        gradL.innerHTML = "Novi grad:";
        red.appendChild(gradL);

        let gradIn = document.createElement("input");
        gradIn.setAttribute("type", "text");
        red.appendChild(gradIn);

        red = this.crtajRed(host);
        red.className = "DugmePotvrde";

        let unesiGradbtn = document.createElement("button");
        unesiGradbtn.onclick = (ev) => this.dodajGrad(gradIn.value);
        unesiGradbtn.innerHTML = "Potvrdi";
        red.appendChild(unesiGradbtn);
    }

    dodajGrad(naziv){
        if (naziv == "")
        {
            alert("Nije upisan grad!");
            return;
        }
        fetch("https://localhost:5001/Grad/DodajGrad/" + naziv,
        {
            method:"POST"
        })
        .then(response => {
            if(response.ok)
            {
                response.text().then(p =>
                    alert(p)
                )
                location.reload();
            }
            response.text().then(p =>
                alert(p)
            )
        })
    }

    crtajDodavanjeKompanije(host){
        let red = this.crtajRed(host);

        let naslov = document.createElement("label");
        naslov.innerHTML = "Dodaj novu kompaniju";
        naslov.className = "Naslov";
        red.appendChild(naslov);

        red = this.crtajRed(host);

        let kompanijaL = document.createElement("label");
        kompanijaL.innerHTML = "Nova kompanija:";
        red.appendChild(kompanijaL);

        let kompanijaIn = document.createElement("input");
        kompanijaIn.setAttribute("type", "text");
        red.appendChild(kompanijaIn);

        red = this.crtajRed(host);
        red.className = "DugmePotvrde";

        let unesiKompanijubtn = document.createElement("button");
        unesiKompanijubtn.onclick = (ev) => this.dodajKompaniju(kompanijaIn.value);
        unesiKompanijubtn.innerHTML = "Potvrdi";
        red.appendChild(unesiKompanijubtn);
    }

    dodajKompaniju(naziv){
        if (naziv == "")
        {
            alert("Nije upisana kompanija!");
            return;
        }
        fetch("https://localhost:5001/Kompanija/DodajKompaniju/" + naziv,
        {
            method:"POST"
        })
        .then(response => {
            if(response.ok)
            {
                response.text().then(p =>
                    alert(p)
                )
                location.reload();
            }
            response.text().then(p =>
                alert(p)
            )
        })
    }

    crtajDodavanjeStanovnika(host){
        let red = this.crtajRed(host);

        let naslov = document.createElement("label");
        naslov.innerHTML = "Dodaj novog stanovnika";
        naslov.className = "Naslov";
        red.appendChild(naslov);

        red = this.crtajRed(host);

        let divLabele = document.createElement("div");
        divLabele.className = "LabeleDodavanja";
        let divIzbor = document.createElement("div");
        divIzbor.className = "IzborDodavanja";
        red.appendChild(divLabele);
        red.appendChild(divIzbor);

        let imeL = document.createElement("label");
        imeL.innerHTML = "Ime:";
        divLabele.appendChild(imeL);

        let imeIn = document.createElement("input");
        imeIn.setAttribute("type", "text");
        divIzbor.appendChild(imeIn);

        let prezimeL = document.createElement("label");
        prezimeL.innerHTML = "Prezime:";
        divLabele.appendChild(prezimeL);

        let prezimeIn = document.createElement("input");
        prezimeIn.setAttribute("type", "text");
        divIzbor.appendChild(prezimeIn);

        let jmbgL = document.createElement("label");
        jmbgL.innerHTML = "JMBG:";
        divLabele.appendChild(jmbgL);

        let jmbgIn = document.createElement("input");
        jmbgIn.setAttribute("type", "text");
        divIzbor.appendChild(jmbgIn);

        let novacL = document.createElement("label");
        novacL.innerHTML = "Sredstva:";
        divLabele.appendChild(novacL);

        let novacIn = document.createElement("input");
        novacIn.setAttribute("type", "number");
        divIzbor.appendChild(novacIn);

        let gradL = document.createElement("label");
        gradL.innerHTML = "Zivi u:";
        divLabele.appendChild(gradL);

        let gradSe = document.createElement("select");
        divIzbor.appendChild(gradSe);

        let gradOp;
        this.listaGradova.forEach(p => {
            gradOp = document.createElement("option");
            gradOp.innerHTML = p.naziv;
            gradOp.value = p.naziv;
            gradSe.appendChild(gradOp);
        })

        red = this.crtajRed(host);
        red.className = "DugmePotvrde";

        let unesiStanovnikabtn = document.createElement("button");
        unesiStanovnikabtn.onclick = (ev) => this.dodajStanovnika(imeIn.value, prezimeIn.value, jmbgIn.value,
                                                                 novacIn.value, gradSe.value);
        unesiStanovnikabtn.innerHTML = "Potvrdi";
        red.appendChild(unesiStanovnikabtn);
    }

    dodajStanovnika(ime, prezime, jmbg, novac, grad){
        if (ime == "" || prezime == "" || jmbg == "" || grad == "")
        {
            alert("Nisu uneti svi podaci!");
            return;
        }
        if (novac <= 0)
        {
            alert("Novcano stanje nije validno!");
            return;
        }
        fetch("https://localhost:5001/Stanovnik/DodajStanovnika/" + ime + "/" + prezime + "/" + jmbg + "/" + novac + "/" + grad,
        {
            method:"POST"
        })
        .then(response => {
            if(response.ok)
            {
                response.text().then(p =>
                    alert(p)
                )
                location.reload();
            }
            response.text().then(p =>
                alert(p)
            )
        })
    }

    crtajFormuNekretnine(host){
        var lista = document.createElement("div");
        lista.className = "FiltriraneNekretnine";
        host.appendChild(lista);
    }

    dodajNekretninu(graditelj, tip, cena, slikaNova, grad){
        if (cena == 0 || slikaNova == ""){
            alert("Nisu uneti svi podaci!");
            return;
        }

        fetch("https://localhost:5001/Nekretnina/DodajNekretninu/" + tip + "/" + grad + "/" + graditelj,
        {
            method:"POST",
            headers:{
                "Content-Type": "application/json"
            },
            body:JSON.stringify({pocetnaCena: cena, slika: slikaNova})
        })
        .then(response => {
            if(response.ok)
            {
                alert("Nekretnina je uspesno dodata.");
                location.reload();
            }
            response.text();
        })
    }

    nadjiNekretnine(){
        let filterGrad = this.filterGrad.querySelectorAll("input[type='checkbox']:checked");
        if (filterGrad.length == 0){
            alert("Izberite filter grada");
            return;
        }

        let nizFilteraGrad = "";
        for (let i = 0; i < filterGrad.length; i++){
            nizFilteraGrad = nizFilteraGrad.concat(filterGrad[i].value, "a")
        }

        let filterKomp = this.filterKomp.querySelectorAll("input[type='checkbox']:checked");

        let nizFilteraKomp = "";
        for (let i = 0; i < filterKomp.length; i++){
            nizFilteraKomp = nizFilteraKomp.concat(filterKomp[i].value, "a")
        }

        if(nizFilteraKomp == "")
        {
            for (let i = 0; i < this.listaKompanija.length; i++){
                nizFilteraKomp = nizFilteraKomp.concat(this.listaKompanija[i].id, "a")
            }
        }

        this.ucitajNekretnine(nizFilteraGrad, nizFilteraKomp);
    }

    ucitajNekretnine(nizFilteraGrad, nizFilteraKomp){
        fetch("https://localhost:5001/Nekretnina/IzlistajNekretnine/" + nizFilteraGrad +"/" + nizFilteraKomp,
        {
            method:"GET"
        }).then(s => {
            if (s.ok){
                var listaNek = this.obrisiPrethodniSadrzaj();

                s.json().then(data => {
                    data.forEach(nek => {
                        let nekr = new Nekretnina(nek.tip, nek.lokacija, nek.izgradio, nek.slika, nek.pocetnaCena);
                        nekr.crtaj(listaNek);
                    })
                    
                })
            }
        })
    }

    obrisiPrethodniSadrzaj(){
        var listaNek = document.querySelector(".FiltriraneNekretnine");
        var roditelj = listaNek.parentNode;
        roditelj.removeChild(listaNek);

        listaNek = document.createElement("div");
        listaNek.className = "FiltriraneNekretnine";
        roditelj.appendChild(listaNek);
        return listaNek;
    }
}