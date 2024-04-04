using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using tasks.Interfaces;
using tasks.Models;

namespace tasks.Services
{
    public class TasksService : ITasksService
    {
        private List<Task1> tasksList;
        private string fileName = "Tasks.json";

        public TasksService()
        {
            this.fileName = Path.Combine("Data", "Tasks.json");
            using (var jsonFile = File.OpenText(fileName))
            {
                tasksList = JsonSerializer.Deserialize<List<Task1>>(
                    jsonFile.ReadToEnd(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                )!;
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(tasksList));
        }

        public List<Task1> GetAll(int userId) => tasksList.FindAll(t => t.UserId == userId);

        public Task1 GetById(int userId, int id)
        {
            return tasksList.FirstOrDefault(t => t.UserId == userId && t.Id == id)!;
        }

        public int Add(Task1 newTask)
        {
            if (tasksList.Count == 0)
            {
                newTask.Id = 1;
            }
            else
            {
                newTask.Id = tasksList.Max(t => t.Id) + 1;
            }

            tasksList.Add(newTask);

            saveToFile();

            return newTask.Id;
        }

        public bool Update(int id, Task1 newTask)
        {
            if (id != newTask.Id)
                return false;

            var existingTask = GetById(newTask.UserId, id);
            if (existingTask == null)
                return false;

            var index = tasksList.IndexOf(existingTask);
            if (index == -1)
                return false;

            tasksList[index] = newTask;

            saveToFile();

            return true;
        }

        public bool Delete(int userId, int id)
        {
            var existingTask = GetById(userId, id);
            if (existingTask == null)
                return false;

            var index = tasksList.IndexOf(existingTask);
            if (index == -1)
                return false;

            tasksList.RemoveAt(index);

            saveToFile();

            return true;
        }
    }

    public static class TaskUtils
    {
        public static void AddTask(this IServiceCollection services)
        {
            services.AddSingleton<ITasksService, TasksService>();
        }
    }
}
