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
    public class UgovorController : ControllerBase
    {
        public AgencijaContext Context { get; set; }
        public UgovorController(AgencijaContext context)
        {
            Context = context;
        }

        
    }
}