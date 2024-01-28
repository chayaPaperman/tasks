using tasks.Models;
using tasks.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.Json;

namespace tasks.Services{

public class TasksService : ITasksService
{
    private List<Task1> tasksList;
    private string fileName = "Tasks.json";

    public TasksService(/*IWebHostEnvinronment webHost*/)
    {
        this.fileName = Path.Combine(/*webHost.ContentRootPath,*/ "Data", "Tasks.json");
        using (var jsonFile = File.OpenText(fileName))
        {
            tasksList = JsonSerializer.Deserialize<List<Task1>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(tasksList));
    }

    public List<Task1> GetAll() => tasksList;

    public Task1 GetById(int id) 
    {
        return tasksList.FirstOrDefault(t => t.Id == id);
    }

    public int Add(Task1 newTask)
    {
        if (tasksList.Count == 0)

            {
                newTask.Id = 1;
            }
        else
            {
                 newTask.Id =  tasksList.Max(t => t.Id) + 1;

            }

        tasksList.Add(newTask);

        saveToFile();

        return newTask.Id;
    }
  
    public bool Update(int id, Task1 newTask)
    {
        if (id != newTask.Id)
            return false;

        var existingTask = GetById(id);
        if (existingTask == null )
            return false;

        var index = tasksList.IndexOf(existingTask);
        if (index == -1 )
            return false;

        tasksList[index] = newTask;

        saveToFile();

        return true;
    }  

      
    public bool Delete(int id)
    {
         var existingTask = GetById(id);
        if (existingTask == null )
            return false;

        var index = tasksList.IndexOf(existingTask);
        if (index == -1 )
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