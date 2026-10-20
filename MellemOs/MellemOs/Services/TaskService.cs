using MellemOs.Models;

namespace MellemOs.Services
{
    public class TaskService
    {
        private List<GameTask> Tasks = new List<GameTask>()
        {
            new GameTask
            {
                Id = 0,
                Name = "ShapeGame"
            },
            new GameTask
            {
                Id = 1,
                Name = "ElectricalGame"
            },
            new GameTask
            {
                Id = 2,
                Name = "RedLightGame"
            },
              new GameTask
            {
                Id = 3,
                Name = "NumberMinigame"
            },
                   new GameTask
            {
                Id = 4,
                Name = "ColorMinigame"
            }
        };
        public List<GameTask> GenerateRndTasks()
        {
            System.Random rnd = new System.Random();
            List<GameTask> tasksToDo = new List<GameTask>();
            for (int i = 0; i < 3; i++)
            {
                tasksToDo.Add(Tasks[rnd.Next(0, Tasks.Count - 1)]);
            }
            return tasksToDo;
        }
    }
}
