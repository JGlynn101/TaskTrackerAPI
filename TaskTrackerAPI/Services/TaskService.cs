using System.Text.Json;
using System.IO;
using System.Runtime.InteropServices;
using TaskTrackerAPI.Repositories;
using TaskTrackerAPI;
using TaskTrackerAPI.Contracts.Tasks;

namespace TaskTrackerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        private readonly List<TaskItem> _tasks;
        public int NextId { get; private set; }
        public TaskService(ITaskRepository repo)
        {
            _repo = repo;
            _tasks = _repo.Load();
            NextId = _tasks.Count == 0 ? 1 : _tasks.Max(t => t.Id) + 1;
        }
        public TaskItem? GetById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
        public TaskItem Add(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Task name cannot be empty");
            }
            var task = new TaskItem(NextId++, name, description);
            _tasks.Add(task);
            _repo.Save(_tasks);
            return task;
        }
        public bool Delete(int id)
        {
            bool deleted = false;
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                deleted = _tasks.Remove(task);
                _repo.Save(_tasks);
            }
            return deleted;
        }
        public bool Update(int id, UpdateTaskRequest request)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task == null) return false;
            if (!string.IsNullOrWhiteSpace(request.Name)) task.Name = request.Name;
            if (request.Description != null) task.Description = request.Description;
            if (!string.IsNullOrWhiteSpace(request.Status)) task.Status = request.Status;

            _repo.Save(_tasks);
            return true;
        }
        public IEnumerable<TaskItem> GetAll()
        {
            return _tasks;
        }
        public IEnumerable<TaskItem> GetInProgress()
        {
            return _tasks.Where(t => t.Status == "In Progress");
        }

        public void SaveTasks()
        {
            _repo.Save(_tasks);
        }

    }
}