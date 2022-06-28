using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using mascotas.Models;
using mascotas.Models.Responses;
using mascotas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mascotas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class postpetController : ControllerBase
    {

        private readonly petDBContext _context;
        private readonly IMapper _mapper;
        private readonly IPostPetService _postPetService;

        public postpetController(petDBContext context, IMapper mapper, IPostPetService postPetService)
        {
            _context = context;
            _mapper = mapper;
            _postPetService = postPetService;
        }

        [HttpGet]
        [Route("get")]
        public IEnumerable<PostPetView> Get(int limit, int offset)
        {
            return _postPetService.getAllPosts();
        }

        [HttpGet]
        [Route("get/{id}")]
        public ActionResult<PostPetView> get([Required] int id)
        {
            var response = _postPetService.getPostById(id);
            if (response.Success == 0) return BadRequest(response.Message);
            return Ok(response.Data);
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<Response>> create([Required] CreatePostPetDTO postpetDTO)
        {
            var response = await _postPetService.createPostAsync(postpetDTO);
            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public ActionResult update([Required] UpdatePostPetDTO postpetDTO)
        {
            var response = _postPetService.updatePost(postpetDTO);
            if (response.Success == 0)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult delete([Required] int id)
        {
            var response = _postPetService.deletePost(id);
            return Ok(response);
        }
    }
}