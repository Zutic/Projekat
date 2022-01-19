using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Agencija.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NekretninaController : ControllerBase
    {
        public AgencijaContext Context { get; set; }
        public NekretninaController(AgencijaContext context)
        {
            Context = context;
        }

        [Route("IzlistajNekretnine/{gradovi}/{kompanije}")]
        [HttpGet]
        public async Task<ActionResult> IzlistajNekretnine(string gradovi, string kompanije)
        {
            if (string.IsNullOrEmpty(gradovi) || (string.IsNullOrEmpty(kompanije)))
            {
                return BadRequest("Nisu uneti podaci");
            }
            try
            {
                var gradoviIds = gradovi.Split('a')
                .Where(g => int.TryParse(g, out _))
                .Select(int.Parse)
                .ToList();

                var kompanijeIds = kompanije.Split('a')
                .Where(k => int.TryParse(k, out _))
                .Select(int.Parse)
                .ToList();

                var nekretnine = Context.Nekretnine
                    .Include(p => p.Lokacija)
                    .Include(p => p.Graditelj)
                    .Where(p => gradoviIds.Contains(p.Lokacija.ID)
                    && kompanijeIds.Contains(p.Graditelj.ID));

                var nekretnina = await nekretnine.ToListAsync();

                return Ok
                (
                    nekretnina.Select(p => 
                    new
                    {
                        Tip = p.Tip,
                        Lokacija = p.Lokacija.Naziv,
                        Izgradio = p.Graditelj.Naziv,
                        Slika = p.Slika,
                        PocetnaCena = p.PocetnaCena
                    })
                );
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("TipoviNekretnina")]
        [HttpGet]
        public ActionResult Tipovi()
        {
            List<string> lista = new List<string>();

            foreach (TIP v in Enum.GetValues(typeof(TIP)))
            {
                lista.Add(v.ToString());
            }

            return Ok(lista);
        }

        [Route("PrviVlasnik")]
        [HttpPost]
        public async Task<ActionResult> PrviVlasnik([FromBody] Nekretnina nek)
        {
            if (nek == null || string.IsNullOrEmpty(nek.Slika))
            {
                return BadRequest("Nekretnina nije uneta.");
            }
            try
            {
                var nekretnine = Context.Nekretnine 
                    .Include(p => p.NekretninaUgovorKompanije)
                    .ThenInclude(p => p.Kupac);

                var vlasnik = await nekretnine.Where(p => p.Slika == nek.Slika && p.NekretninaUgovorKompanije != null).ToListAsync();

                if (vlasnik == null)
                {
                    return BadRequest("Vlasnik nije pronadjen.");
                }

                var vl = vlasnik.Select
                (
                    p =>
                    new
                    {
                        Ime = p.NekretninaUgovorKompanije.Kupac.Ime,
                        Prezime = p.NekretninaUgovorKompanije.Kupac.Prezime,
                        Jmbg = p.NekretninaUgovorKompanije.Kupac.JMBG
                    }               
                );

                return Ok(vl);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("Vlasnici")]
        [HttpPost]
        public async Task<ActionResult> Vlasnici([FromBody] Nekretnina nek)
        {
            if (nek == null || string.IsNullOrEmpty(nek.Slika))
            {
                return BadRequest("Nema slike.");
            }
            try
            {
                var nekretnine = Context.Nekretnine
                .Include(p => p.NekretninaUgovori)
                .ThenInclude(p => p.Kupac);

                var vlasnici = await nekretnine.Where(p => p.Slika == nek.Slika).ToListAsync();

                var vl = vlasnici.Select
                (
                    p =>
                    new
                    {
                        Ugovori = p.NekretninaUgovori
                        .Select(q => new 
                        {
                            Ime = q.Kupac.Ime, 
                            Prezime = q.Kupac.Prezime, 
                            q.Kupac.JMBG
                        })
                    }               
                );

                return Ok(vl);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("TrenutnoProdali/{prodavac}")]
        [HttpPost]
        public async Task<ActionResult> TrenutnoProdali([FromBody] Nekretnina nek, string prodavac)
        {
            if (nek == null || string.IsNullOrEmpty(nek.Slika))
            {
                return BadRequest("Nekretnina nije uneta.");
            }
            if (string.IsNullOrEmpty(prodavac))
            {
                return BadRequest("Prodavac nije unet.");
            }
            try
            {
                var nekretnine = Context.Nekretnine
                                .Include(p => p.NekretninaUgovori)
                                .ThenInclude(p => p.Kupac)
                                .Include(p => p.NekretninaUgovorKompanije)
                                .ThenInclude(p => p.Kupac);

                var vlasnici = await nekretnine.Where(p => p.Slika == nek.Slika).ToListAsync();

                var prodali = vlasnici.Select
                (
                    p => p.NekretninaUgovori.Where(p => p.Prodavac.JMBG == prodavac)
                                            .GroupBy(a => a.Prodavac)
                                            .Select(a => 
                    new
                    {
                        Ukupno = a.Sum(b => b.Procenat)
                    })
                );

                return Ok(prodali);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("TrenutnoPoseduje/{kupac}")]
        [HttpPost]
        public async Task<ActionResult> TrenutnoPoseduje([FromBody] Nekretnina nek, string kupac)
        {
            if (nek == null || string.IsNullOrEmpty(nek.Slika))
            {
                return BadRequest("Nekretnina nije uneta.");
            }
            if (string.IsNullOrEmpty(kupac))
            {
                return BadRequest("Kupac nije unet.");
            }
            try
            {
                var nekretnine = Context.Nekretnine
                                .Include(p => p.NekretninaUgovori)
                                .ThenInclude(p => p.Kupac)
                                .Include(p => p.NekretninaUgovorKompanije)
                                .ThenInclude(p => p.Kupac);

                var vlasnici = await nekretnine.Where(p => p.Slika == nek.Slika).ToListAsync();

                var kupio = vlasnici.Select
                (
                    p => p.NekretninaUgovori.Where(p => p.Kupac.JMBG == kupac)
                                            .GroupBy(a => a.Prodavac)
                                            .Select(a => 
                    new
                    {
                        Ukupno = a.Sum(b => b.Procenat)
                    })
                );

                return Ok(kupio);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajNekretninu/{tip}/{idGrada}/{idKomp}")]
        [HttpPost]
        public async Task<ActionResult> DodajNekretninu([FromBody] Nekretnina nekretnina, string tip, int idGrada, int idKomp)
        {
            if (string.IsNullOrWhiteSpace(nekretnina.Slika))
            {
                return BadRequest("Nije uneta slika.");
            }
            try
            {
                var postoji = await Context.Nekretnine.Where(p => p.Slika == nekretnina.Slika).FirstOrDefaultAsync();
                if (postoji != null)
                {
                    return BadRequest("Vec postoji.");
                }

                int tipInt = (int)Enum.Parse(typeof(TIP), tip);

                var grad = await Context.Gradovi.Where(p => p.ID == idGrada).FirstOrDefaultAsync();
                var kompanija = await Context.Kompanije.Where(p => p.ID == idKomp).FirstOrDefaultAsync();

                nekretnina.Lokacija = grad;
                nekretnina.Graditelj = kompanija;
                nekretnina.Tip = (TIP)tipInt;

                grad.BrojNekretnina++;

                Context.Nekretnine.Add(nekretnina);
                await Context.SaveChangesAsync();
                return Ok("Nekretnina je uneta.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajUgovorKompanije/{jmbg}")]
        [HttpPost]
        public async Task<ActionResult> DodajUgovorKompanije(string jmbg, [FromBody] Nekretnina nek)
        {
            if (nek == null || string.IsNullOrEmpty(nek.Slika))
            {
                return BadRequest("Nekretnina nije uneta.");
            }
            try
            {
                var kupac = await Context.Stanovnici
                .Include(p => p.GradStanovanja)
                .Where(p => p.JMBG == jmbg).FirstOrDefaultAsync();
                
                var nekretnina = await Context.Nekretnine
                .Include(p => p.Lokacija)
                .Include(p => p.Graditelj)
                .Where(p => p.Slika == nek.Slika).FirstOrDefaultAsync();

                if (kupac == null || nekretnina == null)
                {
                    return BadRequest("Nisu uneti validni podaci!");
                }

                if (nekretnina.Tip == TIP.KUCA)
                {
                    if (kupac.GradStanovanja != nekretnina.Lokacija)
                    {
                       
                        var gradIseljenja = await Context.Gradovi.Where(p => p == kupac.GradStanovanja).FirstOrDefaultAsync();
                        var gradUseljenja = await Context.Gradovi.Where(p => p == nekretnina.Lokacija).FirstOrDefaultAsync();

                        gradIseljenja.BrojStanovnika--;
                        gradUseljenja.BrojStanovnika++;

                        kupac.GradStanovanja = nekretnina.Lokacija;
                    }
                }

                UgovorKompanije uk = new UgovorKompanije
                {
                    Prodavac = nekretnina.Graditelj,
                    Kupac = kupac,
                    Cena = nekretnina.PocetnaCena,
                    Objekat = nekretnina
                };

                Context.UgovoriKompanija.Add(uk);
                await Context.SaveChangesAsync();

                return Ok("Ugovor kompanije je unet.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajUgovor/{kupacJmbg}/{prodavacJmbg}/{procenat}/{cena}")]
        [HttpPost]
        public async Task<ActionResult> DodajUgovor(string kupacJmbg, string prodavacJmbg, int procenat, double cena,
                                                    [FromBody] Nekretnina nek)
        {
            if (nek == null || string.IsNullOrEmpty(nek.Slika))
            {
                return BadRequest("Nekretnina nije uneta.");
            }
            if (string.IsNullOrEmpty(kupacJmbg) || string.IsNullOrEmpty(prodavacJmbg) || procenat <= 0 || procenat > 100 ||
                cena <= 0)
            {
                return BadRequest("Podaci nisu validni.");
            }
            try
            {
                var kupac = await Context.Stanovnici.Where(p => p.JMBG == kupacJmbg).FirstOrDefaultAsync();
                var prodavac = await Context.Stanovnici.Where(p => p.JMBG == prodavacJmbg).FirstOrDefaultAsync();
                var nekretnina = await Context.Nekretnine.Where(p => p.Slika == nek.Slika).FirstOrDefaultAsync();

                Ugovor u = new Ugovor
                {
                    Kupac = kupac,
                    Prodavac = prodavac,
                    Procenat = procenat,
                    Cena = cena,
                    Objekat = nekretnina
                };

                Context.Ugovori.Add(u);
                await Context.SaveChangesAsync();

                return Ok("Ugovor je unet.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("UzmiCene")]
        [HttpPost]
        public async Task<ActionResult> UzmiCene([FromBody] Nekretnina nek)
        {
            if (nek == null || string.IsNullOrEmpty(nek.Slika))
            {
                return BadRequest("Nekretnina nije uneta.");
            }
            try
            {
                var vlasnici = Context.Nekretnine
                            .Include(p => p.NekretninaUgovori)
                            .ThenInclude(p => p.Kupac)
                            .Include(p => p.NekretninaUgovorKompanije)
                            .ThenInclude(p => p.Kupac);

                var ret = await vlasnici.Where(p => p.Slika == nek.Slika).FirstOrDefaultAsync();

                if (ret.NekretninaUgovorKompanije == null)
                {
                    return Ok("Nema cena za prikaz");
                }

                List<double> listaCena = new List<double>();
                listaCena.Add(ret.NekretninaUgovorKompanije.Cena);

                foreach(var v in ret.NekretninaUgovori)
                {
                    listaCena.Add(v.Cena * 100/v.Procenat);
                }
                

                return Ok(listaCena);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PromeniCenu/{cena}")]
        [HttpPut]
        public async Task<ActionResult> PromeniCenu(double cena, [FromBody] Nekretnina nek)
        {
            if (nek == null || string.IsNullOrEmpty(nek.Slika))
            {
                return BadRequest("Nekretnina nije uneta.");
            }
            if (cena <= 0)
            {
                return BadRequest("Cena nije validna.");
            }
            try
            {
                var nekretnina = Context.Nekretnine.Where(p => p.Slika == nek.Slika).FirstOrDefault();

                if (nekretnina != null)
                {
                    nekretnina.PocetnaCena = cena;

                    await Context.SaveChangesAsync();
                    return Ok("Uspesno promenjena cena.");
                }
                else
                {
                    return BadRequest("Nekretnina nije pronadjena.");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // [Route("PromeniSliku")]
        // [HttpPut]
        // public async Task<ActionResult> PromeniSliku([FromBody] Nekretnina nekretnina)
        // {
        //     if (nekretnina.ID <= 0)
        //     {
        //         return BadRequest("Pogresan ID.");
        //     }

        //     try
        //     {
        //         var nekretninaZaPromenu = await Context.Nekretnine.FindAsync(nekretnina.ID);
        //         nekretninaZaPromenu.Slika = nekretnina.Slika;
                
        //         await Context.SaveChangesAsync();
        //         return Ok("Slika nekretnine je promenjena.");
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

        [Route("IzbrisiNekretninu")]
        [HttpDelete]
        public async Task<ActionResult> Rusenje([FromBody] Nekretnina nek)
        {
            if (nek == null)
            {
                return BadRequest("Nekretnina nije uneta.");
            }

            try
            {
                var nekretnine = Context.Nekretnine.Include(p => p.Lokacija);
                var nekret = await nekretnine.Where(p => p.Slika == nek.Slika).FirstOrDefaultAsync();

                //var nekretnina = await Context.Nekretnine.Where(p => p.Slika == nek.Slika).FirstOrDefaultAsync();
                var ugovorNek = await Context.UgovoriKompanija.Where(p => p.Objekat == nekret).FirstOrDefaultAsync();
                var ugovori = await Context.Ugovori.Where(p => p.Objekat == nekret).ToListAsync();

                nekret.Lokacija.BrojNekretnina--;

                foreach(var v in ugovori)
                {
                    Context.Ugovori.Remove(v);
                }
                if (ugovorNek != null)
                {
                    Context.UgovoriKompanija.Remove(ugovorNek);
                }
                Context.Nekretnine.Remove(nekret);

                await Context.SaveChangesAsync();

                return Ok("Nekretnina je srusena.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}