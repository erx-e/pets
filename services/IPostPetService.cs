using System.Collections.Generic;
using System.Threading.Tasks;
using mascotas.Models;
using mascotas.Models.Responses;

namespace mascotas.Services
{
    public interface IPostPetService
    {
        List<PostPetView> getAllPosts(int? limit = null, int? offset = null);
        Response getPostById(int id);
        Response createPost(CreatePostPetDTO postpetDTO);
        Task<Response> createPostAsync(CreatePostPetDTO postpetDTO);
        Response updatePost(UpdatePostPetDTO postpetDTO);
        Response deletePost(int id);
    }
}