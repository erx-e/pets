using System.Threading.Tasks;
using mascotas.Models;
using mascotas.Models.Responses;

namespace mascotas.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthUserRequest request);
        Response getUserData(int id);
        Response updateUser(UpdateUserDTO userDTO);
        Response createUser(CreateUserDTO userDTO);
        Task<Response> deleteUser(int id);
    }
}