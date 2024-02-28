using tasks.Models;
using System.Collections.Generic;

namespace tasks.Interfaces{

    public interface ITasksService
    {
        List<Task1> GetAll(int userId);
    
        Task1 GetById(int userId,int id);
        
        int Add(Task1 newTask);
    
        bool Update(int id, Task1 newTask);
        
        bool Delete(int userId,int id);
    }
}