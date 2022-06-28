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

            List<PostPetView> baseListPosts = (from post in listAllPosts
                                               where post.IdSector != null && post.IdPetBreed != null
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
                                               on post.IdSector equals sector.IdSector
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
                                                   sectorName = sector.Name,
                                                   description = post.Description,
                                                   reward = post.Reward != null ? post.Reward : null,
                                                   lastTimeSeen = post.LastTimeSeen,
                                                   linkMapSeen = post.LinkMapSeen != null ? post.LinkMapSeen : null,
                                                   imgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                       img => new imgModel
                                                       {
                                                           fileName = img.FileName,
                                                           url = img.Url,
                                                       }).ToList()
                                               }).ToList();

            List<PostPetView> listPostsNotSector = (from post in listAllPosts
                                                    where post.IdSector == null
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
                                                        sectorName = null,
                                                        description = post.Description,
                                                        reward = post.Reward != null ? post.Reward : null,
                                                        lastTimeSeen = post.LastTimeSeen,
                                                        linkMapSeen = post.LinkMapSeen != null ? post.LinkMapSeen : null,
                                                        imgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                            img => new imgModel
                                                            {
                                                                fileName = img.FileName,
                                                                url = img.Url,
                                                            }).ToList()
                                                    }).ToList();

            List<PostPetView> listPostNotBreed = (from post in listAllPosts
                                                  where post.IdPetBreed == null
                                                  join user in _context.Users
                                                  on post.IdUser equals user.IdUser
                                                  join state in _context.PetStates
                                                  on post.IdState equals state.IdState
                                                  join specie in _context.PetSpecies
                                                  on post.IdPetSpecie equals specie.IdPetSpecie
                                                  join canton in _context.Cantons
                                                  on post.IdCanton equals canton.IdCanton
                                                  join sector in _context.Sectors
                                                  on post.IdSector equals sector.IdSector
                                                  join provincia in _context.Provincia
                                                  on post.IdProvincia equals provincia.IdProvincia
                                                  select new PostPetView
                                                  {
                                                      id = post.IdPostPet,
                                                      userName = user.Name,
                                                      petName = post.PetName,
                                                      petState = state.StateName,
                                                      petSpecie = specie.SpecieName,
                                                      petBreed = null,
                                                      provinciaName = provincia.Name,
                                                      sectorName = sector.Name,
                                                      cantonName = canton.Name,
                                                      description = post.Description,
                                                      reward = post.Reward != null ? post.Reward : null,
                                                      lastTimeSeen = post.LastTimeSeen,
                                                      linkMapSeen = post.LinkMapSeen != null ? post.LinkMapSeen : null,
                                                      imgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                          img => new imgModel
                                                          {
                                                              fileName = img.FileName,
                                                              url = img.Url,
                                                          }).ToList()
                                                  }).ToList();

            baseListPosts.AddRange(listPostsNotSector);
            baseListPosts.AddRange(listPostNotBreed);
            var result = baseListPosts.Distinct().ToList();
            if (limit != null && offset != null)
            {
                return result.GetRange((int)offset,(int)limit);
            }
            return result;
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
                           on post.IdSector equals sector.IdSector
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
                               sectorName = sector.Name,
                               description = post.Description,
                               reward = post.Reward,
                               lastTimeSeen = post.LastTimeSeen,
                               linkMapSeen = post.LinkMapSeen,
                               imgs = _context.PostImages.Where(img => img.IdPostPet == post.IdPostPet).Select(
                                                    img => new imgModel
                                                    {
                                                        fileName = img.FileName,
                                                        url = img.Url,
                                                    }).ToList()

                           }).Single();
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
                PetName = postpetDTO.petName,
                IdState = postpetDTO.idState,
                IdPetSpecie = postpetDTO.idPetSpecie,
                IdPetBreed = postpetDTO.idPetBreed != null ? postpetDTO.idPetBreed : null,
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
                foreach (var img in postpetDTO.urlImgs)
                {
                    var imgObj = new PostImage
                    {
                        IdPostPet = lastPostpet.IdPostPet,
                        FileName = img.fileName,
                        Url = img.url
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
            var postpetOld = _context.PostPets.AsNoTracking().Where(p => p.IdPostPet == postpetDTO.IdPostPet).FirstOrDefault();
            if (postpetOld == null)
            {
                response.Message = "Incorrect post id";
                return response;
            }

            var postpetNew = _mapper.Map<PostPet>(postpetDTO);
            postpetNew.CreatedAt = postpetOld.CreatedAt;
            postpetNew.UpdatedAt = DateTime.Now;

            if (postpetDTO.Imgs != null)
            {
                foreach (var ImgNew in postpetDTO.Imgs)
                {
                    if (ImgNew.idImage == null)
                    {
                        var img = new PostImage
                        {
                            IdPostPet = postpetDTO.IdPostPet,
                            FileName = ImgNew.fileName,
                            Url = ImgNew.url
                        };
                        _context.Add(img);
                    }
                    var oldImg = _context.PostImages.Where(img => img.IdImage == ImgNew.idImage).FirstOrDefault();
                    if (oldImg != null)
                    {
                        oldImg.FileName = ImgNew.fileName != null ? ImgNew.fileName : oldImg.FileName;
                        oldImg.Url = ImgNew.url != null ? ImgNew.url : oldImg.Url;
                        _context.Entry(oldImg).State = EntityState.Modified;
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