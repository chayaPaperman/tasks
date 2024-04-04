using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using tasks.Interfaces;
using tasks.Models;

namespace tasks.Services
{
    public class UsersService : IUserService
    {
        private List<User> UsersList;
        private string fileName = "Users.json";

        public UsersService()
        {
            this.fileName = Path.Combine("Data", fileName);
            using (var jsonFile = File.OpenText(fileName))
            {
                UsersList = JsonSerializer.Deserialize<List<User>>(
                    jsonFile.ReadToEnd(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                )!;
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
                newUser.Id = UsersList.Max(u => u.Id) + 1;
            }

            UsersList.Add(newUser);

            saveToFile();

            return newUser.Id;
        }

        public bool Update(User newUser)
        {
            var existingUser = GetById(newUser.Id);
            if (existingUser == null)
                return false;

            var index = UsersList.IndexOf(existingUser);
            if (index == -1)
                return false;

            UsersList[index] = newUser;

            saveToFile();

            return true;
        }

        public bool Delete(int id)
        {
            var existingUser = GetById(id);
            if (existingUser == null)
                return false;

            var index = UsersList.IndexOf(existingUser);
            if (index == -1)
                return false;

            UsersList.RemoveAt(index);

            saveToFile();

            return true;
        }

        public User getUser(string name, string password)
        {
            return UsersList.Find(u => u.Name == name && u.Password == password)!;
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
