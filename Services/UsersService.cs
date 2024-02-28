using tasks.Models;
using tasks.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;

namespace tasks.Services{

public class UsersService : IUserService
{
    private List<User> UsersList;
    private string fileName = "Users.json";

    public UsersService(/*IWebHostEnvinronment webHost*/)
    {
        this.fileName = Path.Combine(/*webHost.ContentRootPath,*/ "Data", fileName);
        using (var jsonFile = File.OpenText(fileName))
        {
            UsersList = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }

    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(UsersList));
    }

    public List<User> GetAll() => UsersList;

    public User GetById(int id) 
    {
        return UsersList.FirstOrDefault(u => u.Id == id)!;
    }

    public int Add(User newUser)
    {
        if (UsersList.Count == 0)

            {
                newUser.Id = 1;
            }
        else
            {
                 newUser.Id =  UsersList.Max(u => u.Id) + 1;

            }

        UsersList.Add(newUser);

        saveToFile();

        return newUser.Id;
    }
  
    public bool Update(int id, User newUser)
    {
        if (id != newUser.Id)
            return false;

        var existingUser = GetById(id);
        if (existingUser == null )
            return false;

        var index = UsersList.IndexOf(existingUser);
        if (index == -1 )
            return false;

        UsersList[index] = newUser;

        saveToFile();

        return true;
    }  

      
    public bool Delete(int id)
    {
        var existingUser = GetById(id);
        if (existingUser == null )
            return false;

        var index = UsersList.IndexOf(existingUser);
        if (index == -1 )
            return false;

        UsersList.RemoveAt(index);

        saveToFile();
        
        return true;
    }  

        // private static SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
        // private static string issuer = "https://localhost:7254";
        // public static SecurityToken GetToken(List<Claim> claims) =>
        //     new JwtSecurityToken(
        //         issuer,
        //         issuer,
        //         claims,
        //         expires: DateTime.Now.AddDays(1.0),
        //         signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        //     );

        // public static TokenValidationParameters GetTokenValidationParameters() =>
        //     new TokenValidationParameters
        //     {
        //         ValidIssuer = issuer,
        //         ValidAudience = issuer,
        //         IssuerSigningKey = key,
        //         ClockSkew = TimeSpan.Zero // remove delay of token when expire
        //     };

        // public static string WriteToken(SecurityToken token) =>
        //     new JwtSecurityTokenHandler().WriteToken(token);

        public User getUser(string name , string password){
            return UsersList.Find(u=>u.Name==name && u.Password==password)!;
        }

}

public static class UserUtils
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UsersService>();
    }
}
}
