using System.Collections.Generic;
using AutoMapper;
using mascotas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using mascotas.Services;
using Microsoft.AspNetCore.Authorization;
using mascotas.Models.Responses;
using System.Threading.Tasks;

namespace mascotas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class userController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly petDBContext _context;
        private readonly IMapper _mapper;

        public userController(petDBContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }



        [Authorize]
        [HttpGet]
        [Route("get/{id?}")]
        public ActionResult<UserView> Get(int? id = null)
        {
            if (id != null)
            {
                var response = _userService.getUserData((int)id);
                if (response.Success == 0)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response.Data);
            }
            var authR = (Response)HttpContext.Items["User"];
            if (authR.Success == 0)
            {
                return BadRequest(authR.Message);
            }
            return Ok(authR.Data);
        }

        [Authorize]
        [HttpPut]
        [Route("update")]
        public ActionResult update(UpdateUserDTO userDTO)
        {
            var authR = (Response)HttpContext.Items["User"];
            var authUser = authR.Data as UserView;
            if (authUser.idUser == userDTO.idUser)
            {
                var response = _userService.updateUser(userDTO);
                if (response.Success == 0)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response.Data);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("create")]
        public ActionResult create(CreateUserDTO userDTO)
        {
            var response = _userService.createUser(userDTO);
            if (response.Success == 0)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> deleteAsync(int id)
        {
            var authR = (Response)HttpContext.Items["User"];
            var authUser = authR.Data as UserView;
            if (authUser.idUser == id)
            {

                var response = await _userService.deleteUser(id);
                if (response.Success == 0)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult authenticate(AuthUserRequest authRequest)
        {
            UserResponse response = _userService.Auth(authRequest);
            if (response == null)
            {
                return BadRequest("Correo o contraseña incorrecta");
            }
            return Ok(response);
        }
    }
}