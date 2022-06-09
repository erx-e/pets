using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mascotas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace mascotas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly petDBContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, petDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CantonView>> Get()
        {
            using (_context)
            {
                List<CantonView> listCantons = (from c in _context.Cantons
                                                join p in _context.Provincia
                                                on c.IdProvincia equals p.IdProvincia
                                                select new CantonView{
                                                    id = c.IdCanton,
                                                    name = c.Name,
                                                    provincia = new ProvinciaView{
                                                        id = p.IdProvincia,
                                                        name = p.Name
                                                    }
                                                }).ToList();
                return listCantons;
            }
        }
    }
}
