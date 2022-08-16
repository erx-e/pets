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
        public IEnumerable<PostPetView> Get([FromQuery] int? limit = null, int? offset = null)
        {
            if (limit != null && offset != null)
            {
                return _postPetService.getAllPosts(limit, offset);
            }
            return _postPetService.getAllPosts();
        }

        [HttpGet]
        [Route("getByState/{id}")]
        public ActionResult<IEnumerable<PostPetView>> GetByState(string id, [FromQuery] int? limit = null, int? offset = null)
        {
            if (id == null)
            {
                return BadRequest("Must send id state");
            }
            if (limit != null && offset != null)
            {
                return _postPetService.getByState(id, limit, offset);
            }
            return _postPetService.getByState(id);
        }

        [HttpGet]
        [Route("getByFilter")]
        public ActionResult<IEnumerable<PostPetView>> GetByFilter([FromQuery] string? stateId, int? petSpecieId, int? petBreedId, int? provinciaId, int? cantonId, int? sectorId, DateTime? date, int? order, int? limit = null, int? offset = null)
        {
            if (stateId == null)
            {
                return BadRequest("Must send id state");
            }

            if (limit != null && offset != null)
            {
                var response = _postPetService.getByFilter(stateId, petSpecieId, petBreedId, provinciaId, cantonId, sectorId, date, order, limit, offset);
                if (response.Data != null)
                {
                    return Ok((IEnumerable<PostPetView>)response.Data);
                }
                else
                {
                    return null;
                }
            }
            return Ok((IEnumerable<PostPetView>)_postPetService.getByFilter(stateId, petSpecieId, petBreedId, provinciaId, cantonId, sectorId, date, order).Data);
        }

        [HttpGet]
        [Route("get/{id}")]
        public ActionResult<PostPetView> get([Required] int id)
        {
            var response = _postPetService.getViewPostById(id);
            if (response.Success == 0) return BadRequest();
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("getUpdate/{id}")]
        public ActionResult<UpdatePostPetDTO> getUpdate([Required] int id)
        {
            var response = _postPetService.getPostById(id);
            if (response.Success == 0) return BadRequest();
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