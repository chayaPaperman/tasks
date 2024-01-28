using tasks.Models;
using System.Collections.Generic;

namespace tasks.Interfaces{

    public interface ITasksService
    {
        List<Task1> GetAll();
    
        Task1 GetById(int id);
        
        int Add(Task1 newTask);
    
        bool Update(int id, Task1 newTask);
        
        bool Delete(int id);
    }
}