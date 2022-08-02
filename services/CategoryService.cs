using System.Collections.Generic;
using System.Linq;
using mascotas.Models;

namespace mascotas.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly petDBContext _context;

        public CategoryService(petDBContext context)
        {
            _context = context;
        }


        public List<PetSpecieView> GetPetSpecie()
        {
            var especies = _context.PetSpecies.Select(s => new PetSpecieView
            {
                name = s.SpecieName,
                id = s.IdPetSpecie
            }).ToList();
            return especies;
        }

        public List<PetBreedView> GetPetBreeds()
        {
            var breeds = _context.PetBreeds.Select(b => new PetBreedView
            {
                name = b.BreedName,
                id = b.IdPetBreed,
                petSpecieId = b.IdSpecie
            }).ToList();
            return breeds;
        }

        public List<PetBreedView> GetPetBreedsBySpecie(int id)
        {
            var breeds = _context.PetBreeds.Where(b => b.IdSpecie == id).Select(b => new PetBreedView
            {
                name = b.BreedName,
                id = b.IdPetBreed,
                petSpecieId = b.IdSpecie
            }).ToList();
            return breeds;
        }

        public List<ProvinciaView> GetProvincias()
        {
            var provincias = _context.Provincia.Select(p => new ProvinciaView
            {
                name = p.Name,
                id = p.IdProvincia
            }).ToList();
            return provincias;
        }
        public List<CantonView> GetCantons()
        {
            var cantones = _context.Cantons.Select(c => new CantonView
            {
                name = c.Name,
                id = c.IdCanton,
                provinciaId = c.IdProvincia
            }).ToList();
            return cantones;
        }

        public List<CantonView> GetCantonsByProv(int id)
        {
            var cantones = _context.Cantons.Where(c => c.IdProvincia == id).Select(c => new CantonView
            {
                name = c.Name,
                id = c.IdCanton,
                provinciaId = c.IdProvincia
            }).ToList();
            return cantones;
        }

        public List<SectorView> GetSectors()
        {
            var sectors = _context.Sectors.Select(s => new SectorView
            {
                name = s.Name,
                id = s.IdSector,
                cantonId = s.IdCanton
            }).ToList();
            return sectors;
        }

        public List<SectorView> GetSectorsByCanton(int id)
        {
            var sectors = _context.Sectors.Where(s => s.IdCanton == id).Select(s => new SectorView
            {
                name = s.Name,
                id = s.IdSector,
                cantonId = s.IdCanton
            }).ToList();
            return sectors;
        }
    }
}