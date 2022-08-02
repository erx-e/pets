using System.Collections.Generic;
using mascotas.Models;
using mascotas.Services;
using Microsoft.AspNetCore.Mvc;

namespace mascotas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class categoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public categoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("species")]
        public IEnumerable<PetSpecieView> GetPetSpecies()
        {
            var species = _categoryService.GetPetSpecie();
            return species;
        }

        [HttpGet]
        [Route("breeds")]
        public IEnumerable<PetBreedView> GetPetBreeds()
        {
            var breeds = _categoryService.GetPetBreeds();
            return breeds;
        }

        [HttpGet]
        [Route("breedsBySpecie/{id}")]
        public IEnumerable<PetBreedView> GetPetBreedsBySpecie(int id)
        {
            var breeds = _categoryService.GetPetBreedsBySpecie(id);
            return breeds;
        }

        [HttpGet]
        [Route("provincias")]
        public IEnumerable<ProvinciaView> GetProvincias()
        {
            var provincias = _categoryService.GetProvincias();
            return provincias;
        }

        [HttpGet]
        [Route("cantones")]
        public IEnumerable<CantonView> GetCantones()
        {
            var cantons = _categoryService.GetCantons();
            return cantons;
        }

        [HttpGet]
        [Route("cantonesByProv/{id}")]
        public IEnumerable<CantonView> GetCantonesByProv(int id)
        {
            var cantons = _categoryService.GetCantonsByProv(id);
            return cantons;
        }

        [HttpGet]
        [Route("sectores")]
        public IEnumerable<SectorView> GetSectors()
        {
            var sectors = _categoryService.GetSectors();
            return sectors;
        }

        [HttpGet]
        [Route("sectoresByCanton/{id}")]
        public IEnumerable<SectorView> GetSectorsByCanton(int id)
        {
            var sectors = _categoryService.GetSectorsByCanton(id);
            return sectors;
        }
    }
}