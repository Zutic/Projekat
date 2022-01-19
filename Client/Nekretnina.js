import { Stanovnik } from "./Stanovnik.js";

export class Nekretnina{

    constructor(tip, lokacija, kompanija, slika, cena){
        this.tip = tip;
        this.lokacija = lokacija;
        this.kompanija = kompanija;
        this.slika = slika;
        this.cena = cena;
    }

    crtajRed(host){
        let red = document.createElement("div");
        red.className = "Red";
        host.appendChild(red);
        return red;
    }

    crtaj(host){
        let prozor = document.createElement("div");
        prozor.className = "NekretninaProzor";
        host.appendChild(prozor);

        var gornjiDeo = document.createElement("div");
        gornjiDeo.className = "GornjiDeo";
        var dugmad = document.createElement("div");
        dugmad.className = "Dugmad";
        var ugovorKompanijeDeo = document.createElement("div");
        ugovorKompanijeDeo.className = "UgovorDeo";
        ugovorKompanijeDeo.style.display = "none";
        var ugovorDeo = document.createElement("div");
        ugovorDeo.className = "UgovorDeo";
        ugovorDeo.style.display = "none";

        prozor.appendChild(gornjiDeo);
        prozor.appendChild(dugmad);
        prozor.appendChild(ugovorKompanijeDeo);
        prozor.appendChild(ugovorDeo);

        this.crtajUgovorKompanije(ugovorKompanijeDeo);
        this.crtajUgovor(ugovorDeo);

        var info = document.createElement("div");
        info.className = "InfoDeo";
        var foto = document.createElement("div");
        foto.className = "FotoDeo";
        var grafik = document.createElement("div");
        grafik.className = "GrafDeo";
        gornjiDeo.appendChild(info);
        gornjiDeo.appendChild(grafik);
        gornjiDeo.appendChild(foto);

        var img = new Image();
        img.src = this.slika;
        foto.appendChild(img);

        let red = this.crtajRed(info);
        var naslov = document.createElement("label");
        naslov.innerHTML = "Osnovne informacije: ";
        naslov.className = "Naslov";
        red.appendChild(naslov);

        red = this.crtajRed(info);
        var lok = document.createElement("label");
        lok.innerHTML = "Lokacija: " + this.lokacija;
        red.appendChild(lok);

        red = this.crtajRed(info);
        var tip = document.createElement("label");
        let tipNaziv;
        switch(this.tip){
            case 0: tipNaziv = "Kuca"; break;
            case 1: tipNaziv = "Stan"; break;
            case 2: tipNaziv = "Lokal"; break;
            case 3: tipNaziv = "Zgrada"; break;
            default: tipNaziv = "Nepoznato"; break;
        }
        tip.innerHTML = "Tip nekretnine: " + tipNaziv;
        red.appendChild(tip);

        red = this.crtajRed(info);
        var komp = document.createElement("label");
        komp.innerHTML = "Graditelj: " + this.kompanija;
        red.appendChild(komp);

        red = this.crtajRed(info);
        var cenaL = document.createElement("label");
        cenaL.innerHTML = "Cena: " + Math.round(this.cena * 100) / 100 + "$";
        red.appendChild(cenaL);

        var ugovorKompbtn = document.createElement("button");
        ugovorKompbtn.className = "UgovorKompanijeDugme"
        ugovorKompbtn.innerHTML = "Ugovor kompanije";
        ugovorKompbtn.onclick = (ev) =>
        {
            if (ugovorKompanijeDeo.style.display === "none")
                ugovorKompanijeDeo.style.display = "block";
            else
                ugovorKompanijeDeo.style.display = "none";
        };
        dugmad.appendChild(ugovorKompbtn);

        var ugovorbtn = document.createElement("button");
        ugovorbtn.innerHTML = "Ugovor o kupoprodaji";
        ugovorbtn.onclick = (ev) =>
        {
            if (ugovorDeo.style.display === "none")
                ugovorDeo.style.display = "block";
            else
                ugovorDeo.style.display = "none";
        };
        dugmad.appendChild(ugovorbtn);

        var rusi = document.createElement("button");
        rusi.innerHTML = "Sruši";
        rusi.className = "DugmeSrusi"
        rusi.onclick = (ev) => this.obrisiNekretninu();
        dugmad.appendChild(rusi);

        fetch("https://localhost:5001/Nekretnina/PrviVlasnik",
        {
            method:"POST",
            headers:{
                "Content-Type": "application/json"
            },
            body:JSON.stringify({slika: this.slika})
        })
        .then(prviVlasnik => {
            if(prviVlasnik.ok){
                prviVlasnik.json().then(tipovi => {
                    if (tipovi.length != 0)
                    {
                        ugovorKompbtn.disabled = true;
                        ugovorbtn.disabled = false;
                    }
                    else
                    {
                        ugovorbtn.disabled = true;
                    }
                })
            }
        })

        fetch("https://localhost:5001/Nekretnina/UzmiCene",
        {
            method:"POST",
            headers:{
                "Content-Type": "application/json"
            },
            body:JSON.stringify({slika: this.slika})
        })
        .then(response => {
            if(response.ok){
                const contentType = response.headers.get("content-type");
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    var xValues = [];
                    var yValues = [];
                    var i = 0;
                    response.json().then(cene => {
                        cene.forEach(cena => {
                            xValues.push(i++);
                            yValues.push(cena);
                        })
                        let graf = document.createElement("canvas");
                        graf.className = "myChart";
                        graf.id = this.slika;
                        grafik.appendChild(graf);
    
                        new Chart(this.slika, {
                            type: "line",
                            data: {
                              labels: xValues,
                              datasets: [{ 
                                label: "Cena tokom vremena",
                                data: yValues,
                                borderColor: "red",
                                fill: false
                              }]
                            },
                            options: {
                                plugins:{   
                                   legend: {
                                        display: false
                                    }
                                }
                            }
                        })                   
                    })
                }
                else{
                    response.text().then(p =>
                        console.log(p)
                    )
                }
            }
        })
    }

    obrisiNekretninu(){
        fetch("https://localhost:5001/Nekretnina/IzbrisiNekretninu",
        {
            method:"DELETE",
            headers:{
                "Content-Type": "application/json"
            },
            body:JSON.stringify({slika: this.slika})
        })
        .then(response => {
            return response.text();
        })
        .then(data => {
            alert(data);
            this.osvezi();
        });
    }

    crtajUgovor(host){
        let red = this.crtajRed(host);

        let dodajL = document.createElement("label");
        dodajL.innerHTML = "Ugovor";
        dodajL.className = "Naslov";
        red.appendChild(dodajL);

        red = this.crtajRed(host);

        let divLabele = document.createElement("div");
        divLabele.className = "LabeleDodavanja";
        let divIzbor = document.createElement("div");
        divIzbor.className = "IzborDodavanja";
        red.appendChild(divLabele);
        red.appendChild(divIzbor);

        let prodavacL = document.createElement("label");
        prodavacL.innerHTML = "Prodavac:";
        divLabele.appendChild(prodavacL);

        let prodavacSe = document.createElement("select");
        divIzbor.appendChild(prodavacSe);

        var listaStanovnika = [];

        fetch("https://localhost:5001/Nekretnina/PrviVlasnik",
        {
            method:"POST",
            headers:{
                "Content-Type": "application/json"
            },
            body:JSON.stringify({slika: this.slika})
        })
        .then(prviVlasnik => {
            if(prviVlasnik.ok){
                prviVlasnik.json().then(tipovi => {
                    tipovi.forEach(tip => {
                        var s = new Stanovnik(tip.ime, tip.prezime, tip.jmbg);
                        s.poseduje = 100;
                        listaStanovnika.push(s);
                    })
    
                    fetch("https://localhost:5001/Nekretnina/Vlasnici",
                    {
                        method:"POST",
                        headers:{
                            "Content-Type": "application/json"
                        },
                        body:JSON.stringify({slika: this.slika})
                    })
                    .then(ostaliVlasnici => {
                        if(ostaliVlasnici.ok){
                            ostaliVlasnici.json().then(tipovi => {
                                tipovi.forEach(tip => {
                                    tip.ugovori.forEach(vl => {
                                        var s = new Stanovnik(vl.ime, vl.prezime, vl.jmbg);
                                        listaStanovnika.push(s);
                                    });
                                })
                
                                let prodavacOp;
                                const listaStanovnikaJedinstveni = Array.from(new Set(listaStanovnika.map(a => a.jmbg)))
                                .map(jmbg => {
                                    return listaStanovnika.find(a => a.jmbg === jmbg)
                                })
                                listaStanovnikaJedinstveni.forEach(stanovnik => {
                                    var procenatPosedovanja;

                                    fetch("https://localhost:5001/Nekretnina/TrenutnoPoseduje/"+ stanovnik.jmbg, 
                                    {
                                        method:"POST",
                                        headers:{
                                            "Content-Type": "application/json"
                                        },
                                        body:JSON.stringify({slika: this.slika})
                                    })
                                    .then(p => {
                                        p.json().then(vlasnici => {
                                            vlasnici.forEach(vlasnik => {
                                                vlasnik.forEach(vl => {
                                                    procenatPosedovanja = parseInt(vl.ukupno);
                                                    stanovnik.poseduje += procenatPosedovanja;
                                                })
                                            })

                                            var procenatProdatog;

                                            fetch("https://localhost:5001/Nekretnina/TrenutnoProdali/"+ stanovnik.jmbg, 
                                            {
                                                method:"POST",
                                                headers:{
                                                    "Content-Type": "application/json"
                                                },
                                                body:JSON.stringify({slika: this.slika})
                                            })
                                            .then(p => {
                                                p.json().then(vlasnici => {
                                                    vlasnici.forEach(vlasnik => {
                                                        vlasnik.forEach(vl => {
                                                            procenatProdatog = parseInt(vl.ukupno);
                                                            stanovnik.prodao += procenatProdatog;
                                                        })
                                                    })

                                                    if (stanovnik.poseduje > stanovnik.prodao)
                                                    {
                                                        prodavacOp = document.createElement("option");
                                                        prodavacOp.innerHTML = stanovnik.ime + " " + stanovnik.prezime +
                                                        "(" + stanovnik.jmbg + ")" + " - " +
                                                        (stanovnik.poseduje - stanovnik.prodao) + "%";
                                                        
                                                        prodavacOp.value = stanovnik.jmbg + " " + stanovnik.prodao +
                                                         " " + stanovnik.poseduje;
                                                        prodavacSe.appendChild(prodavacOp);
                                                    }
                                                })          
                                            })       
                                        })          
                                    }) 
                                })
                            })
                        }
                    })
                })
            }
            else
            {
                return;
            }
        })
        
        let kupacL = document.createElement("label");
        kupacL.innerHTML = "Kupac:";
        divLabele.appendChild(kupacL);

        let kupacSe = document.createElement("input");
        kupacSe.setAttribute("type", "number");
        divIzbor.appendChild(kupacSe);

        let udeoProdajeL = document.createElement("label");
        udeoProdajeL.innerHTML = "Udeo prodaje: ";
        divLabele.appendChild(udeoProdajeL);

        let udeoProdajeSe = document.createElement("select");
        divIzbor.appendChild(udeoProdajeSe);

        let udeoProdajeOp;
        for (var i = 10; i <= 100; i += 10){
            udeoProdajeOp = document.createElement("option");
            udeoProdajeOp.innerHTML = i + "%";
            udeoProdajeOp.value = i;
            udeoProdajeSe.appendChild(udeoProdajeOp);
        }

        let cenaL = document.createElement("label");
        cenaL.innerHTML = "Cena:";
        divLabele.appendChild(cenaL);

        let cenaSe = document.createElement("input");
        cenaSe.setAttribute("type", "number");
        divIzbor.appendChild(cenaSe);

        red = this.crtajRed(host);
        red.className = "DugmePotvrde";

        let sklopiUgovorbtn = document.createElement("button");
        sklopiUgovorbtn.onclick = (ev) => this.sklopiUgovor(kupacSe.value.toString(), prodavacSe.value,
                                                            udeoProdajeSe.value, cenaSe.value);
        sklopiUgovorbtn.innerHTML = "Sklopi ugovor";
        red.appendChild(sklopiUgovorbtn);
    }

    crtajUgovorKompanije(host){
        let red = this.crtajRed(host);

        let ugovorNaslov = document.createElement("label");
        ugovorNaslov.innerHTML = "Ugovor kompanije";
        ugovorNaslov.className = "Naslov";
        red.appendChild(ugovorNaslov);

        red = this.crtajRed(host);

        let kupacL = document.createElement("label");
        kupacL.innerHTML = "Kupac:";
        red.appendChild(kupacL);

        let kupacSe = document.createElement("input");
        kupacSe.setAttribute("type", "number");
        red.appendChild(kupacSe);

        red = this.crtajRed(host);
        red.className = "DugmePotvrde";

        let sklopiUgovorKompbtn = document.createElement("button");
        sklopiUgovorKompbtn.onclick = (ev) => this.sklopiUgovorKompanije(kupacSe.value);
        sklopiUgovorKompbtn.innerHTML = "Sklopi ugovor";
        red.appendChild(sklopiUgovorKompbtn);
    }

    sklopiUgovor(kupac, prodavac, procenat, cena){
        var nizVrednosti = prodavac.split(" ");
        var prodavacJmbg = nizVrednosti[0];
        var prodavacProdao = parseInt(nizVrednosti[1]);
        var prodavacKupio = parseInt(nizVrednosti[2]);

        if (prodavacProdao + parseInt(procenat) > prodavacKupio)
        {
            alert("Vlasnik ne poseduje toliki udeo nekretnine");
            return; 
        }
        else
        {
            fetch("https://localhost:5001/Stanovnik/NovcanoStanje/" + kupac)
            .then(p => {
                p.json().then(poseduje =>{
                    if(poseduje[0] < parseInt(cena))
                    {
                        alert("Kupac nema tolika novcana sredstva!");
                        return;
                    }
                    else
                    {
                        fetch("https://localhost:5001/Nekretnina/DodajUgovor/" + kupac +"/" + prodavacJmbg + 
                        "/" + procenat + "/" + cena,
                        {
                            method:"POST",
                            headers:{
                                "Content-Type": "application/json"
                            },
                            body:JSON.stringify({slika: this.slika})
                        })
                        .then(response =>{
                            if(response.ok){
                                fetch("https://localhost:5001/Stanovnik/NovoStanje/"+ kupac + "/" + (-parseInt(cena)),
                                {
                                    method:"PUT"
                                })
                                .then(response => {
                                    if(response.ok){
                                        fetch("https://localhost:5001/Stanovnik/NovoStanje/"+ prodavacJmbg + "/" + (parseInt(cena)),
                                        {
                                            method:"PUT"
                                        })
                                        .then(response => {
                                            if(response.ok){
                                                fetch("https://localhost:5001/Nekretnina/PromeniCenu/" + (parseInt(cena)*100/parseInt(procenat)),
                                                {
                                                    method:"PUT",
                                                    headers:{
                                                        "Content-Type": "application/json"
                                                    },
                                                    body:JSON.stringify({slika: this.slika})
                                                })
                                                .then(
                                                    alert("Ugovor je uspesno formiran."),
                                                    this.osvezi()
                                                );
                                            }
                                        })
                                    }
                                })
                            }
                        })
                    }
                })
            })
        }
    }

    sklopiUgovorKompanije(osoba){
        let jmbg = osoba.toString();

        fetch("https://localhost:5001/Stanovnik/NovcanoStanje/" + jmbg)
        .then(p => {
            p.json().then(poseduje =>{
                if(poseduje[0] < this.cena)
                {
                    alert("Kupac nema tolika novcana sredstva!");
                    return;
                }
                else
                {
                    fetch("https://localhost:5001/Nekretnina/DodajUgovorKompanije/" + jmbg,
                    {
                        method:"POST",
                        headers:{
                            "Content-Type": "application/json"
                        },
                        body:JSON.stringify({slika: this.slika})
                    })
                    .then(response =>{
                        if(response.ok){
                            fetch("https://localhost:5001/Stanovnik/NovoStanje/"+ jmbg + "/" + (-parseInt(this.cena)),
                            {
                                method:"PUT"
                            })
                            .then(
                                alert("Ugovor je uspesno formiran."),
                                this.osvezi(),
                               
                            )
                            .then( p => {if(this.tip == 0) location.reload()})
                        }
                    })
                }
            })
        })
    }

    osvezi(){
        var prikaz = document.querySelector(".PrikaziNekretnineDugme");
        prikaz.click();
        // var gradovi = document.querySelector(".PrikaziGradoveDugme");
        // gradovi.click();
    }
}