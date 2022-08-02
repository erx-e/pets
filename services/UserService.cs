using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using mascotas.Models;
using mascotas.Models.Responses;
using mascotas.Options;
using mascotas.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace mascotas.Services
{
    class UserService : IUserService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly petDBContext _context;
        public UserService(petDBContext context, IOptions<JwtOptions> jwtOptions)
        {
            _context = context;
            _jwtOptions = jwtOptions.Value;
        }
        public UserResponse Auth(AuthUserRequest request)
        {
            UserResponse userresponse = new UserResponse();
            var epassword = Encrypt.GetSHA256(request.password);
            var user = _context.Users.Where(u => u.Email == request.email &&
                                 u.Password == epassword).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            UserView authUser = new UserView{
                name = user.Name,
                email = user.Email,
                cellNumber = user.CellNumber,
                facebookProfile = user.FacebookProfile
            };
            userresponse.token = GetToken(user);
            userresponse.user = authUser;
            return userresponse;
        }

        public Response createUser(CreateUserDTO userDTO)
        {
            var response = new Response();
            var user = _context.Users.Where(u => u.Email == userDTO.email).FirstOrDefault();
            if (user != null)
            {
                response.Message = "Email already registered";
                return response;
            }

            string password = Encrypt.GetSHA256(userDTO.password);

            var userNew = new User
            {
                Name = userDTO.name,
                Email = userDTO.email,
                CellNumber = userDTO.cellNumber,
                FacebookProfile = userDTO.facebookProfile,
                Password = password
            };
            response.Success = 1;
            _context.Add(userNew);
            userNew.Password = "";
            response.Data = userNew;
            _context.SaveChanges();

            response.token = GetToken(userNew);

            return response;
        }

        public Response getUserData(int id)
        {
            var response = new Response();
            if (_context.Users.Find(id) == null)
            {
                response.Message = "Incorrect user id";
                return response;
            }
            UserView user = (from usr in _context.Users
                             where usr.IdUser == id
                             select new UserView
                             {
                                 idUser = usr.IdUser,
                                 name = usr.Name,
                                 email = usr.Email,
                                 cellNumber = usr.CellNumber,
                                 facebookProfile = usr.FacebookProfile
                             }).Single();
            response.Success = 1;
            response.Data = user;
            return response;
        }

        public Response updateUser(UpdateUserDTO userDTO)
        {
            var response = new Response();
            var userOld = _context.Users.AsNoTracking().Where(u => u.IdUser == userDTO.idUser).FirstOrDefault();
            if (userOld == null)
            {
                response.Message = "Incorrect user id";
                return response;
            }
            var userNew = new User
            {
                IdUser = userDTO.idUser,
                Name = userDTO.name != null ? userDTO.name : userOld.Name,
                Email = userDTO.email != null ? userDTO.email : userOld.Email,
                CellNumber = userDTO.cellNumber != null ? userDTO.cellNumber : userOld.CellNumber,
                FacebookProfile = userDTO.facebookProfile != null ? userDTO.facebookProfile : userOld.FacebookProfile,
                CreatedAt = userOld.CreatedAt,
                UpdatedAt = DateTime.Today,
                Password = userDTO.password != null ? Encrypt.GetSHA256(userDTO.password) : userOld.Password
            };
            _context.Entry(userNew).State = EntityState.Modified;
            _context.SaveChanges();
            response.Success = 1;
            response.Data = userNew;
            response.Message = "User updated correctly";
            return response;
        }

        private string GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]{
                        new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                        new Claim(ClaimTypes.Email, user.Email)
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public IEnumerable<UserView> Get()
        {
            List<UserView> listUsers = (from user in _context.Users
                                        select new UserView
                                        {
                                            name = user.Name,
                                            email = user.Email,
                                            cellNumber = user.CellNumber,
                                            facebookProfile = user.FacebookProfile
                                        }).ToList();
            return listUsers;
        }

        public Response deleteUser(int id)
        {
            var response = new Response();
            var user = _context.Users.Find(id);
            if (user == null)
            {
                response.Message = "Incorrect user id";
                return response;
            }
            _context.Remove(user);
            _context.SaveChanges();
            response.Success = 1;
            response.Message = "User deleted correctly";
            return response;
        }
    }
}