using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mascotas.Models;
using mascotas.Models.Responses;

namespace mascotas.Services
{
    public interface IPostPetService
    {
        List<PostPetView> getAllPosts(int? limit = null, int? offset = null);
        Response getViewPostById(int id);
        Response getPostById(int id);
        List<PostPetView> getByState(string id, int? limit = null, int? offset = null);
        Response getByFilter(string stateId, int? petSpecieId, int? petBreedId, int? provinciaId, int? cantonId, int? sectorId, int? userId, DateTime? date, int? order, int? limit = null, int? offset = null);
        Response createPost(CreatePostPetDTO postpetDTO);
        Task<Response> createPostAsync(CreatePostPetDTO postpetDTO);
        Response updatePost(UpdatePostPetDTO postpetDTO);
        Task<Response> deletePost(int id);
    }
}