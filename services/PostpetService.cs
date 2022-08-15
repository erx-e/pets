using System.Collections.Generic;
using AutoMapper;
using mascotas.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using mascotas.Models.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading.Tasks;

namespace mascotas.Services
{
    public class PostpetService : IPostPetService
    {
        private readonly petDBContext _context;
        private readonly IMapper _mapper;
        public PostpetService(petDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<PostPetView> getAllPosts(int? limit = null, int? offset = null)
        {

            var listAllPosts = _context.PostPets.ToList();

            List<PostPetView> result = (from post in listAllPosts
                                        join user in _context.Users
                                        on post.IdUser equals user.IdUser
                                        join state in _context.PetStates
                                        on post.IdState equals state.IdState
                                        join specie in _context.PetSpecies
                                        on post.IdPetSpecie equals specie.IdPetSpecie
                                        join breed in _context.PetBreeds
                                        on post.IdPetBreed equals breed.IdPetBreed
                                        join canton in _context.Cantons
                                        on post.IdCanton equals canton.IdCanton
                                        join sector in _context.Sectors
                                        on post.IdSector != null ? post.IdSector : 1 equals sector.IdSector
                                        join provincia in _context.Provincia
                                        on post.IdProvincia equals provincia.IdProvincia
                                        select new PostPetView
                                        {
                                            id = post.IdPostPet,
                                            userName = user.Name,
                                            petName = post.PetName,
                                            petState = state.StateName,
                                            petSpecie = specie.SpecieName,
                                            petBreed = breed.BreedName,
                                            provinciaName = provincia.Name,
                                            cantonName = canton.Name,
                                            sectorName = post.IdSector != null ? sector.Name : null,
                                            description = post.Description,
                                            reward = post.Reward,
                                            lastTimeSeen = post.LastTimeSeen,
                                            linkMapSeen = post.LinkMapSeen != null ? post.LinkMapSeen : null,
                                            urlImgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                img => new imgModel
                                                {
                                                    url = img.Url,
                                                }).ToList()
                                        }).ToList();

            if (limit != null && offset != null)
            {
                if (result.Count >= offset)
                {
                    if (result.Count >= limit + offset)
                    {
                        return result.GetRange((int)offset, (int)limit);
                    }
                    return result.GetRange((int)offset, (result.Count - (int)offset));
                }
                else
                {
                    return null;
                }
            }
            return result;
        }

        public List<PostPetView> getByState(string stateId, int? limit = null, int? offset = null)
        {
            var listAllPosts = _context.PostPets.ToList();

            List<PostPetView> result = (from post in listAllPosts
                                        where post.IdState == stateId
                                        join user in _context.Users
                                        on post.IdUser equals user.IdUser
                                        join state in _context.PetStates
                                        on post.IdState equals state.IdState
                                        join specie in _context.PetSpecies
                                        on post.IdPetSpecie equals specie.IdPetSpecie
                                        join breed in _context.PetBreeds
                                        on post.IdPetBreed equals breed.IdPetBreed
                                        join canton in _context.Cantons
                                        on post.IdCanton equals canton.IdCanton
                                        join sector in _context.Sectors
                                        on post.IdSector != null ? post.IdSector : 1 equals sector.IdSector
                                        join provincia in _context.Provincia
                                        on post.IdProvincia equals provincia.IdProvincia
                                        select new PostPetView
                                        {
                                            id = post.IdPostPet,
                                            userName = user.Name,
                                            petName = post.PetName,
                                            petState = state.StateName,
                                            petSpecie = specie.SpecieName,
                                            petBreed = breed.BreedName,
                                            provinciaName = provincia.Name,
                                            cantonName = canton.Name,
                                            sectorName = post.IdSector != null ? sector.Name : null,
                                            description = post.Description,
                                            reward = post.Reward,
                                            lastTimeSeen = post.LastTimeSeen,
                                            linkMapSeen = post.LinkMapSeen != null ? post.LinkMapSeen : null,
                                            urlImgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                img => new imgModel
                                                {
                                                    url = img.Url,
                                                }).ToList()
                                        }).OrderByDescending(p => p.lastTimeSeen).ToList();

            if (limit != null && offset != null)
            {
                if (result.Count >= offset)
                {
                    if (result.Count >= limit + offset)
                    {
                        return result.GetRange((int)offset, (int)limit);
                    }
                    return result.GetRange((int)offset, (result.Count - (int)offset));
                }
                else
                {
                    return null;
                }
            }

            return result;
        }

        public Response getByFilter(string stateId, int? petSpecieId, int? petBreedId, int? provinciaId, int? cantonId, int? sectorId, DateTime? date, int? order, int? limit = null, int? offset = null)
        {

            List<PostPet> listPostFilter = _context.PostPets
                                .Where(post => (
                                    post.IdState == stateId &&
                                    (petSpecieId == null || (petSpecieId != null && post.IdPetSpecie == petSpecieId)) &&
                                    (petBreedId == null || (petBreedId != null && post.IdPetBreed == petBreedId)) &&
                                    (provinciaId == null || (provinciaId != null && post.IdProvincia == provinciaId)) &&
                                    (cantonId == null || (cantonId != null && post.IdCanton == cantonId)) &&
                                    (sectorId == null || (sectorId != null && post.IdSector == sectorId)) &&
                                    (date == null || (date != null && post.LastTimeSeen >= date))
                                    )).ToList();



            List<PostPetView> result = (from post in listPostFilter
                                        join user in _context.Users
                                        on post.IdUser equals user.IdUser
                                        join state in _context.PetStates
                                        on post.IdState equals state.IdState
                                        join specie in _context.PetSpecies
                                        on post.IdPetSpecie equals specie.IdPetSpecie
                                        join breed in _context.PetBreeds
                                        on post.IdPetBreed equals breed.IdPetBreed
                                        join canton in _context.Cantons
                                        on post.IdCanton equals canton.IdCanton
                                        join sector in _context.Sectors
                                        on post.IdSector != null ? post.IdSector : 1 equals sector.IdSector
                                        join provincia in _context.Provincia
                                        on post.IdProvincia equals provincia.IdProvincia
                                        select new PostPetView
                                        {
                                            id = post.IdPostPet,
                                            userName = user.Name,
                                            petName = post.PetName,
                                            petState = state.StateName,
                                            petSpecie = specie.SpecieName,
                                            petBreed = breed.BreedName,
                                            provinciaName = provincia.Name,
                                            cantonName = canton.Name,
                                            sectorName = post.IdSector != null ? sector.Name : null,
                                            description = post.Description,
                                            reward = post.Reward,
                                            lastTimeSeen = post.LastTimeSeen,
                                            linkMapSeen = post.LinkMapSeen,
                                            urlImgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                img => new imgModel
                                                {
                                                    url = img.Url,
                                                }).ToList()
                                        }).OrderByDescending(p => p.lastTimeSeen).ToList();

            if(order != null && order >= 1){
                result.OrderBy(p => p.lastTimeSeen);
            }

            var response = new Response();
            if (limit != null && offset != null)
            {
                if (result.Count >= offset)
                {
                    if (result.Count >= limit + offset)
                    {
                        response.Data = result.GetRange((int)offset, (int)limit);
                        return response;
                    }
                    response.Data = result.GetRange((int)offset, (result.Count - (int)offset));
                    return response;
                }
                else
                {
                    response.Data = null;
                    return response;
                }
            }

            response.Data = result;
            return response;

        }

        public Response getViewPostById(int id)
        {
            var response = new Response();
            if (_context.PostPets.Find(id) == null)
            {
                response.Message = "Incorrect user id";
                return response;
            }

            var postpet = (from post in _context.PostPets
                           where post.IdPostPet == id
                           join user in _context.Users
                           on post.IdUser equals user.IdUser
                           join state in _context.PetStates
                           on post.IdState equals state.IdState
                           join specie in _context.PetSpecies
                           on post.IdPetSpecie equals specie.IdPetSpecie
                           join breed in _context.PetBreeds
                           on post.IdPetBreed equals breed.IdPetBreed
                           join canton in _context.Cantons
                           on post.IdCanton equals canton.IdCanton
                           join sector in _context.Sectors
                           on post.IdSector != null ? post.IdSector : 1 equals sector.IdSector
                           join provincia in _context.Provincia
                           on post.IdProvincia equals provincia.IdProvincia
                           select new PostPetView
                           {
                               id = post.IdPostPet,
                               userName = user.Name,
                               idUser = post.IdUser,
                               petName = post.PetName,
                               petState = state.StateName,
                               petSpecie = specie.SpecieName,
                               petBreed = breed.BreedName,
                               provinciaName = provincia.Name,
                               cantonName = canton.Name,
                               sectorName = post.IdSector != null ? sector.Name : null,
                               description = post.Description,
                               reward = post.Reward,
                               lastTimeSeen = post.LastTimeSeen,
                               linkMapSeen = post.LinkMapSeen,
                               urlImgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                    img => new imgModel
                                                    {
                                                        url = img.Url,
                                                    }).ToList()

                           }).SingleOrDefault();
            response.Success = 1;
            response.Data = postpet;

            return response;
        }

        public Response getPostById(int id)
        {
            var response = new Response();
            if (_context.PostPets.Find(id) == null)
            {
                response.Message = "Incorrect user id";
                return response;
            }


            var postpet = (from post in _context.PostPets
                           where post.IdPostPet == id
                           select new UpdatePostPetDTO
                           {
                               idPostPet = post.IdPostPet,
                               idUser = post.IdUser,
                               petName = post.PetName,
                               idState = post.IdState,
                               idPetSpecie = post.IdPetSpecie,
                               idPetBreed = post.IdPetBreed,
                               idProvincia = post.IdProvincia,
                               idCanton = post.IdCanton,
                               idSector = post.IdSector,
                               description = post.Description,
                               reward = post.Reward,
                               lastTimeSeen = post.LastTimeSeen,
                               linkMapSeen = post.LinkMapSeen,
                               urlImgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                    img => new updatePostImgDTO
                                                    {
                                                        idImage = img.IdImage,
                                                        url = img.Url
                                                    }).ToList()

                           }).SingleOrDefault();
            response.Success = 1;
            response.Data = postpet;

            return response;
        }

        public Response createPost(CreatePostPetDTO postpetDTO)
        {
            var response = new Response();

            var postpetNew = new PostPet
            {
                IdUser = postpetDTO.idUser,
                PetName = postpetDTO.petName != null ? postpetDTO.petName : null,
                PetAge = postpetDTO.petAge != null ? postpetDTO.petAge : null,
                PetSpecialCondition = postpetDTO.petSpecialCondition != null ? postpetDTO.petSpecialCondition : null,
                Contact = postpetDTO.contact != null ? postpetDTO.contact : null,
                IdState = postpetDTO.idState,
                IdPetSpecie = postpetDTO.idPetSpecie,
                IdPetBreed = postpetDTO.idPetBreed,
                IdProvincia = postpetDTO.idProvincia,
                IdCanton = postpetDTO.idCanton,
                IdSector = postpetDTO.idSector != null ? postpetDTO.idSector : null,
                Description = postpetDTO.description,
                Reward = postpetDTO.reward != null ? postpetDTO.reward : null,
                LastTimeSeen = postpetDTO.lastTimeSeen,
                LinkMapSeen = postpetDTO.linkMapSeen != null ? postpetDTO.linkMapSeen : null
            };
            _context.Add(postpetNew);
            _context.SaveChanges();

            var lastPostpet = _context.PostPets.OrderBy(p => p.IdPostPet).LastOrDefault();
            if (postpetDTO.urlImgs != null)
            {
                foreach (var urlImg in postpetDTO.urlImgs)
                {
                    var imgObj = new PostImage
                    {
                        IdPostPet = lastPostpet.IdPostPet,
                        Url = urlImg
                    };
                    _context.Add(imgObj);
                }
            }
            _context.SaveChanges();

            response.Data = postpetNew;
            response.Success = 1;
            response.Message = "Post created correctly";
            return response;
        }

        public async Task<Response> createPostAsync(CreatePostPetDTO postpetDTO)
        {
            var task = new Task<Response>(() => { return createPost(postpetDTO); });
            task.Start();
            var result = await task;
            return result;
        }

        public Response updatePost(UpdatePostPetDTO postpetDTO)
        {
            var response = new Response();
            var postpetOld = _context.PostPets.AsNoTracking().Where(p => p.IdPostPet == postpetDTO.idPostPet).FirstOrDefault();
            if (postpetOld == null)
            {
                response.Message = "Incorrect post id";
                return response;
            }

            var postpetNew = new PostPet();
            postpetNew.CreatedAt = postpetOld.CreatedAt;
            postpetNew.UpdatedAt = DateTime.Now;
            postpetNew.IdState = postpetOld.IdState;
            postpetNew.IdUser = postpetOld.IdUser;
            postpetNew.IdPostPet = postpetOld.IdPostPet;

            postpetNew.PetAge = postpetDTO.petAge != null ? postpetDTO.petAge : postpetOld.PetAge;
            postpetNew.PetSpecialCondition = postpetDTO.petSpecialCondition != null ? postpetDTO.petSpecialCondition : postpetOld.PetSpecialCondition;
            postpetNew.Contact = postpetDTO.contact != null ? postpetDTO.contact : postpetOld.Contact;
            postpetNew.IdPetSpecie = postpetDTO.idPetSpecie != null ? (int)postpetDTO.idPetSpecie : postpetOld.IdPetSpecie;
            postpetNew.IdPetBreed = postpetDTO.idPetBreed != null ? (int)postpetDTO.idPetBreed : postpetOld.IdPetBreed;
            postpetNew.IdProvincia = postpetDTO.idProvincia != null ? (int)postpetDTO.idProvincia : postpetOld.IdProvincia;
            postpetNew.IdCanton = postpetDTO.idCanton != null ? (int)postpetDTO.idCanton : postpetOld.IdCanton;
            postpetNew.IdSector = postpetDTO.idSector != null ? (int)postpetDTO.idSector : postpetOld.IdSector;
            postpetNew.Description = postpetDTO.description != null ? postpetDTO.description : postpetOld.Description;
            postpetNew.Reward = postpetDTO.reward != null ? (decimal)postpetDTO.reward : postpetOld.Reward;
            postpetNew.LastTimeSeen = postpetDTO.lastTimeSeen.HasValue ? (DateTime)postpetDTO.lastTimeSeen : postpetOld.LastTimeSeen;


            if (postpetDTO.urlImgs != null)
            {
                foreach (var ImgNew in postpetDTO.urlImgs)
                {
                    if (ImgNew.idImage == null)
                    {
                        var img = new PostImage
                        {
                            IdPostPet = postpetDTO.idPostPet,
                            Url = ImgNew.url
                        };
                        _context.Add(img);
                    }
                    else
                    {
                        var oldImg = _context.PostImages.Where(img => img.IdImage == ImgNew.idImage).FirstOrDefault();
                        if (oldImg != null)
                        {
                            if (ImgNew.url == null)
                            {
                                _context.PostImages.Remove(_context.PostImages.Find((int)ImgNew.idImage));
                            }
                            if (ImgNew.url != null && ImgNew.url != oldImg.Url)
                            {
                                oldImg.Url = ImgNew.url;
                                _context.Entry(oldImg).State = EntityState.Modified;
                            }
                        }
                    }
                }
            }
            _context.Entry(postpetNew).State = EntityState.Modified;
            _context.SaveChanges();
            response.Data = postpetNew;
            response.Success = 1;
            response.Message = "Post updated correctly";
            return response;
        }

        public Response deletePost(int id)
        {
            var response = new Response();
            var postpet = _context.PostPets.Find(id);
            if (postpet == null)
            {
                response.Message = "Incorrect post id";
                return response;
            }

            var postImgs = _context.PostImages.Where(img => img.IdPostPet == postpet.IdPostPet);
            if (postImgs != null)
            {
                _context.PostImages.RemoveRange(postImgs);
            }
            _context.PostPets.Remove(postpet);
            _context.SaveChanges();
            response.Data = postpet;
            response.Success = 1;
            response.Message = "Post deleted correctly";
            return response;
        }

    }
}