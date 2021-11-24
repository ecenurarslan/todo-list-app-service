using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using todo_app_api.Dto;
using todo_app_api.Entity;
using todo_app_api.Helper;

namespace todo_app_api.Service
{
    public interface IUserService
    {
        GeneralDto.Response Get();
        GeneralDto.Response Login(UserDto.Login userModel);
        GeneralDto.Response Register(UserDto.Register userModel);

    }
    public class UserService : IUserService
    {
        todoContext _context;
        IConfiguration _configuration;
        public UserService(todoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public GeneralDto.Response Get()
        {
            return new GeneralDto.Response { Data = "UserService", Message = "Basarili" };
        }
        public GeneralDto.Response Login(UserDto.Login userModel)
        {
            try
            {
                User result = _context.User.Where(user => user.Username == userModel.Username && user.Password == userModel.Password).FirstOrDefault();
                if (result != null)
                {
                    var claimList = new List<Claim> {
                        new Claim("Id", result.Id.ToString()),
                        new Claim(ClaimTypes.Name, userModel.Username),
                        new Claim(ClaimTypes.Role, Enum.GetName(typeof(Enums.UserRole), result.RoleId))
                    };
                    SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(5),
                        claims: claimList,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return new GeneralDto.Response { Message = "Login gerceklesti", Data = new JwtSecurityTokenHandler().WriteToken(token) };
                }

                return new GeneralDto.Response { Error = true, Message = "Login gerceklesemedi" };
            }
            catch (Exception)
            {
                return new GeneralDto.Response { Error = true, Message = "Basarisiz" };
            }
        }
        public GeneralDto.Response Register(UserDto.Register userModel)
        {
            try
            {
                User result = _context.User.Where(user => user.Username == userModel.Username || user.Email == userModel.Email).FirstOrDefault();
                if (result == null)
                {
                    User user = new User
                    {
                        Username = userModel.Username,
                        Password = userModel.Password,
                        Email = userModel.Email,
                        RoleId = userModel.RoleId
                    };
                    _context.User.Add(user);
                    _context.SaveChanges();
                    return new GeneralDto.Response { Message = "Basarili" };
                }

                return new GeneralDto.Response { Error = true, Message = "Username or email is already in use" };
            }
            catch (Exception)
            {
                return new GeneralDto.Response { Error = true, Message = "Basarisiz" };
            }
        }
    }
}
