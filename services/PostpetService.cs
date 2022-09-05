using System.Collections.Generic;
using AutoMapper;
using mascotas.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using mascotas.Models.Responses;
using Amazon.S3;
using System.Threading.Tasks;
using Amazon.S3.Model;

namespace mascotas.Services
{
    public class PostpetService : IPostPetService
    {
        private readonly petDBContext _context;
        private readonly IMapper _mapper;
        private readonly IAmazonS3 s3Client;
        string bucketName = "petslighthouse";
        public PostpetService(petDBContext context, IMapper mapper, IAmazonS3 s3Client)
        {
            _context = context;
            _mapper = mapper;
            this.s3Client = s3Client;
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
                                            petStateId = post.IdState,
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
                                            petStateId = post.IdState,
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

        public Response getByFilter(string? stateId, int? petSpecieId, int? petBreedId, int? provinciaId, int? cantonId, int? sectorId, int? userId, DateTime? date, int? order, int? limit = null, int? offset = null)
        {

            List<PostPet> listPostFilter = _context.PostPets
                                .Where(post => (
                                    (stateId == null || (stateId != null && post.IdState == stateId)) &&
                                    (petSpecieId == null || (petSpecieId != null && post.IdPetSpecie == petSpecieId)) &&
                                    (petBreedId == null || (petBreedId != null && post.IdPetBreed == petBreedId)) &&
                                    (provinciaId == null || (provinciaId != null && post.IdProvincia == provinciaId)) &&
                                    (cantonId == null || (cantonId != null && post.IdCanton == cantonId)) &&
                                    (sectorId == null || (sectorId != null && post.IdSector == sectorId)) &&
                                    (userId == null || (userId != null && post.IdUser == userId)) &&
                                    (date == null || (date != null && post.LastTimeSeen >= date))
                                    )).ToList();

            List<PostPetView> result;
            if (order != null && order >= 1)
            {
                result = (from post in listPostFilter
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
                              petStateId = post.IdState,
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
                          }).OrderBy(p => p.lastTimeSeen).ToList();
            }
            else
            {
                result = (from post in listPostFilter
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
                              petStateId = post.IdState,
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
                               contact = post.Contact,
                               petAge = post.PetAge,
                               petState = state.StateName,
                               petStateId = post.IdState,
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
                               contact = post.Contact,
                               petAge = post.PetAge,
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
                        Url = urlImg.url
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

        public async Task<Response> updatePostAsync(UpdatePostPetDTO postpetDTO)
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
                            var imgkey = oldImg.Url.Split("/").Last();

                            if (ImgNew.url == null)
                            {
                                var respons = await s3Client.DeleteObjectAsync(bucketName, imgkey);
                        

                                _context.PostImages.Remove(_context.PostImages.Find((int)ImgNew.idImage));
                            }
                            if (ImgNew.url != null && ImgNew.url != oldImg.Url)
                            {
                                await s3Client.DeleteObjectAsync(bucketName, imgkey);
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

        public async Task<Response> deletePost(int id)
        {
            var response = new Response();
            var postpet = _context.PostPets.Find(id);
            if (postpet == null)
            {
                response.Message = "Incorrect post id";
                return response;
            }

            var postImgs = _context.PostImages.Where(img => img.IdPostPet == postpet.IdPostPet).ToList();
            if (postImgs != null)
            {
                var imgskey = postImgs.Select(img => new KeyVersion { Key = img.Url.Split("/").Last() }).ToList();
                var client = new AmazonS3Client();
                var deleteRequest = new DeleteObjectsRequest()
                {
                    Objects = imgskey,
                    BucketName = bucketName
                };
                await client.DeleteObjectsAsync(deleteRequest);
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