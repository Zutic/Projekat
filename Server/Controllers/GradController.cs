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
    public class GradController : ControllerBase
    {
        public AgencijaContext Context { get; set; }
        public GradController(AgencijaContext context)
        {
            Context = context;
        }

        [Route("Gradovi")]
        [HttpGet]
        public async Task<ActionResult> Gradovi()
        {
            return Ok(await Context.Gradovi.Select(p => 
            new
            {
                ID = p.ID,
                Naziv = p.Naziv,
                BrojStanovnika = p.BrojStanovnika,
                BrojNekretnina = p.BrojNekretnina
            }).ToListAsync());
        }

        [Route("DodajGrad/{naziv}")]
        [HttpPost]
        public async Task<ActionResult> DodajGrad(string naziv)
        {
            if (string.IsNullOrWhiteSpace(naziv))
            {
                return BadRequest("Nije unet naziv.");
            }
            try
            {
                var postoji = await Context.Gradovi.Where(p => p.Naziv == naziv).FirstOrDefaultAsync();
                if (postoji != null)
                {
                    return BadRequest("Vec postoji.");
                }

                Grad grad = new Grad
                {
                    Naziv = naziv
                };

                Context.Gradovi.Add(grad);
                await Context.SaveChangesAsync();
                return Ok("Grad je dodat.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}