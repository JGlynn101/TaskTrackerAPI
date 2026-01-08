using System.Text.Json;

namespace TaskTracker.Repositories
{
    public class TaskRepository
    {
        private readonly string _path = "./tasks.json";
        private readonly JsonSerializerOptions _options =
            new() { WriteIndented = true };

        public List<TaskItem> Load()
        {
            if (!File.Exists(_path))
                return new List<TaskItem>();

            var json = File.ReadAllText(_path);
            return string.IsNullOrWhiteSpace(json)
                ? new List<TaskItem>()
                : JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        public void Save(List<TaskItem> tasks)
        {
            var json = JsonSerializer.Serialize(tasks, _options);
            File.WriteAllText(_path, json);
        }
    }
}
