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
    public class StanovnikController : ControllerBase
    {
        public AgencijaContext Context { get; set; }
        public StanovnikController(AgencijaContext context)
        {
            Context = context;
        }

        [Route("NovcanoStanje/{jmbg}")]
        [HttpGet]
        public async Task<ActionResult> NovcanoStanje(string jmbg)
        {
            var stanovnik = await Context.Stanovnici.Where(p => p.JMBG == jmbg).ToListAsync();

            var novac = stanovnik.Select(p => p.Novac);

            return Ok(novac);
        }

        [Route("DodajStanovnika/{ime}/{prezime}/{jmbg}/{novac}/{grad}")]
        [HttpPost]
        public async Task<ActionResult> DodajStanovnika(string ime, string prezime, string jmbg, double novac, string grad)
        {
            if (string.IsNullOrWhiteSpace(ime) || string.IsNullOrWhiteSpace(prezime) || jmbg.Length != 13 ||
                string.IsNullOrWhiteSpace(jmbg) || string.IsNullOrWhiteSpace(grad) || novac <= 0)
            {
                return BadRequest("Nisu uneti validni podaci.");
            }
            try
            {
                var postoji = await Context.Stanovnici.Where(p => p.JMBG == jmbg).FirstOrDefaultAsync();
                if (postoji != null)
                {
                    return BadRequest("Vec postoji.");
                }

                var gradStanovanja = await Context.Gradovi.Where(p => p.Naziv == grad).FirstOrDefaultAsync();
                if (gradStanovanja == null)
                {
                    return BadRequest("Grad ne postoji.");
                }

                gradStanovanja.BrojStanovnika++;

                Stanovnik stanovnik = new Stanovnik
                {
                    Ime = ime,
                    Prezime = prezime,
                    JMBG = jmbg,
                    Novac = novac,
                    GradStanovanja = gradStanovanja
                };

                Context.Stanovnici.Add(stanovnik);
                await Context.SaveChangesAsync();
                return Ok("Stanovnik je dodat.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        

        [Route("NovoStanje/{jmbg}/{novac}")]
        [HttpPut]
        public async Task<ActionResult> NovoStanje(string jmbg, long novac)
        {
            try
            {
                var stanovnik = Context.Stanovnici.Where(p => p.JMBG == jmbg).FirstOrDefault();

                if (stanovnik != null)
                {
                    stanovnik.Novac += novac;

                    await Context.SaveChangesAsync();
                    return Ok($"Novo stanje: {stanovnik.Novac}.");
                }
                else
                {
                    return BadRequest("Stanovnik nije pronadjen.");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        } 

    }
}