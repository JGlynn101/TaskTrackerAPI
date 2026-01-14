using System.Text.Json;
using System.IO;
using System.Runtime.InteropServices;
using TaskTracker.Repositories;
using TaskTracker;
using TaskTrackerAPI.Contracts.Tasks;

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
        public bool Update(int id, UpdateTaskRequest request)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == id);

            if (task == null) return false;
            if (!string.IsNullOrEmpty(request.Name)) task.Name = request.Name;
            if (request.Description != null) task.Description = request.Description;
            if (!string.IsNullOrEmpty(request.Status)) task.Status = request.Status;

            _repo.Save(Tasks);
            return true;
        }
        public IEnumerable<TaskItem> GetAll()
        {
            return Tasks;
        }
        public TaskItem? GetTask(int id)
        {
            return Tasks.SingleOrDefault(t => t.Id == id);
        }
        public IEnumerable<TaskItem> GetInProgress()
        {
            return Tasks.Where(t => t.Status == "In Progress");
        }

        public void SaveTasks()
        {
            _repo.Save(Tasks);
        }

    }
}