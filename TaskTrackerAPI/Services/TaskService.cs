using System.Text.Json;
using System.IO;
using System.Runtime.InteropServices;
using TaskTracker.Repositories;
using TaskTracker;

namespace TaskTrackerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskRepository _repo;
        public List<TaskItem> Tasks { get; }
        public int NextId { get; private set; }
        public TaskService(TaskRepository repo)
        {
            _repo = repo;
            Tasks = _repo.Load();
            NextId = Tasks.Count == 0 ? 1 : Tasks.Max(t => t.Id) + 1;
        }
        public TaskItem? GetById(int id)
        {
            return Tasks.FirstOrDefault(t => t.Id == id);
        }
        public TaskItem Add(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Task name cannot be empty");
            }
            var task = new TaskItem(NextId++, name, description);
            Tasks.Add(task);
            _repo.Save(Tasks);
            return task;
        }
        public void Delete(int id)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                Tasks.Remove(task);
                _repo.Save(Tasks);
            }
        }
        public bool Update(int id, int fieldInteger, string userTaskChange)
        {
            bool returnBool = false;
            if (id >= 1)
            {
                var task = Tasks.FirstOrDefault(t => t.Id == id);
                if (userTaskChange != "")
                {
                    if (fieldInteger == 1)
                    {
                        task.Name = userTaskChange;
                        returnBool = true;
                    }
                    if (fieldInteger == 2)
                    {
                        task.Status = userTaskChange;
                        returnBool = true;
                    }
                    if (fieldInteger == 3)
                    {
                        task.Description = userTaskChange;
                        returnBool = true;
                    }
                }
            }
            return returnBool;
        }
        public IEnumerable<TaskItem> GetAll()
        {
            return Tasks.Where(t => !t.Deleted);
        }
        public TaskItem? GetTask(int id)
        {
            return Tasks.SingleOrDefault(t => t.Id == id);
        }
        public IEnumerable<TaskItem> GetInProgress()
        {
            return Tasks.Where(t => t.Status == "In Progress");
        }
        public IEnumerable<TaskItem> GetDeleted()
        {
            return Tasks.Where(t => t.Deleted == true);
        }
        public IEnumerable<TaskItem> GetUnfinished()
        {
            return Tasks.Where(t => !t.Finished);
        }

        public void SaveTasks()
        {
            _repo.Save(Tasks);
        }

    }
}