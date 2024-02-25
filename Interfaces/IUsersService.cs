using tasks.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace tasks.Interfaces{

    public interface IUserService
    {
        List<User> GetAll();
    
        User GetById(int id);
        
        int Add(User newUser);
    
        bool Update(int id, User newUser);
        
        bool Delete(int id);

        SecurityToken GetToken(List<Claim> claims);

        TokenValidationParameters GetTokenValidationParameters();

        string WriteToken(SecurityToken token);
    }
}