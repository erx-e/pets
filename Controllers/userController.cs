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
            var jwtResponse = (Response)HttpContext.Items["User"];
            if (jwtResponse.Success == 0)
            {
                return BadRequest(jwtResponse.Message);
            }
            return Ok(jwtResponse.Data);
        }

        [Authorize]
        [HttpPut]
        [Route("update")]
        public ActionResult update(UpdateUserDTO userDTO)
        {
            var response = _userService.updateUser(userDTO);
            if (response.Success == 0)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
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
        [Route("delete")]
        public ActionResult delete(int id)
        {
            var response = _userService.deleteUser(id);
            if (response.Success == 0)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
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