using tasks.Models;
using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;

namespace tasks.Interfaces{

    public interface IUserService
    {
        List<User> GetAll();
    
        User GetById(int id);
        
        int Add(User newUser);
    
        bool Update(User newUser);
        
        bool Delete(int id);

        // // SecurityToken GetToken(List<Claim> claims);

        // // TokenValidationParameters GetTokenValidationParameters();

        // // string WriteToken(SecurityToken token);

        // static SecurityToken GetToken(List<Claim> claims){
        //     return new SecurityToken();
        // }

        // static staticTokenValidationParameters GetTokenValidationParameters(){
        //     return new staticTokenValidationParameters();
        // }

        // static string WriteToken(SecurityToken token){
        //     return "";
        // }

        User getUser(string name , string password);
    }
}