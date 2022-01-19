using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Agencija.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KompanijaController : ControllerBase
    {
        public AgencijaContext Context { get; set; }
        public KompanijaController(AgencijaContext context)
        {
            Context = context;
        }

        [Route("UzmiKompanije")]
        [HttpGet]
        public async Task<ActionResult> Kompanije()
        {
            return Ok(await Context.Kompanije.Select(p => 
            new
            {
                ID = p.ID,
                Naziv = p.Naziv
            }).ToListAsync());
        }

        [Route("DodajKompaniju/{naziv}")]
        [HttpPost]
        public async Task<ActionResult> DodajKompaniju(string naziv)
        {
            if (string.IsNullOrWhiteSpace(naziv))
            {
                return BadRequest("Nije unet naziv.");
            }
            try
            {
                var postoji = await Context.Kompanije.Where(p => p.Naziv == naziv).FirstOrDefaultAsync();
                if (postoji != null)
                {
                    return BadRequest("Vec postoji.");
                }

                Kompanija kompanija = new Kompanija
                {
                    Naziv = naziv
                };

                Context.Kompanije.Add(kompanija);
                await Context.SaveChangesAsync();
                return Ok("Kompanija je dodata.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}