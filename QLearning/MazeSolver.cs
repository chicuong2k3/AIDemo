using TorchSharp;

namespace QLearning
{
    public class MazeSolver
    {
        enum Action
        {
            UP, DOWN, LEFT, RIGHT
        }

        Action[] actions = { Action.UP, Action.DOWN, Action.LEFT, Action.RIGHT };

        const int WALL_REWARD = -500;
        const int GOAL_REWARD = 500;
        const int FLOOR_REWARD = -10;

        public MazeSolver()
        {

        }

        int[,] InitRewards(int[,] maze)
        {
            var mazeRows = maze.GetLength(0);
            var mazeColumns = maze.GetLength(1);
            var rewards = new int[mazeRows, mazeColumns];

            for (int i = 0; i < mazeRows; i++)
            {
                for (int j = 0; j < mazeColumns; j++)
                {
                    switch (maze[i, j])
                    {
                        case 0:
                            rewards[i, j] = WALL_REWARD;
                            break;
                        case 1:
                            rewards[i, j] = FLOOR_REWARD;
                            break;
                        case 2:
                            rewards[i, j] = GOAL_REWARD;
                            break;
                    }
                }
            }

            return rewards;
        }


        torch.Tensor InitQValues(int[,] maze)
        {
            var mazeRows = maze.GetLength(0);
            var mazeColumns = maze.GetLength(1);
            var qValues = torch.zeros(mazeRows, mazeColumns, actions.Length);
            return qValues;
        }

        bool IsTerminalState(int[,] maze, int currentRow, int currentColumn, int[,] rewards)
        {
            return rewards[currentRow, currentColumn] != FLOOR_REWARD;
        }

        long GetNextAction(torch.Tensor qValues, int currentRow, int currentColumn, float epsilon)
        {
            var random = new Random();
            var randomValue = random.NextDouble();
            if (randomValue < epsilon)
            {
                return torch.argmax(qValues[currentRow, currentColumn]).item<long>();
            }
            else
            {
                return random.Next(actions.Length);
            }
        }

        (int, int) MoveOneStep(int[,] maze, int currentRow, int currentColumn, long action)
        {
            var mazeRows = maze.GetLength(0);
            var mazeColumns = maze.GetLength(1);

            var newRow = currentRow;
            var newColumn = currentColumn;

            if (actions[action] == Action.UP && currentRow > 0)
            {
                newRow--;
            }
            else if (actions[action] == Action.DOWN && currentRow < mazeRows - 1)
            {
                newRow++;
            }
            else if (actions[action] == Action.LEFT && currentColumn > 0)
            {
                newColumn--;
            }
            else if (actions[action] == Action.RIGHT && currentColumn < mazeColumns - 1)
            {
                newColumn++;
            }

            return (newRow, newColumn);
        }

        public void Train(int[,] maze, float epsilon, float discountFactor, float learningRate, float episodes)
        {
            var rewards = InitRewards(maze);
            var qValues = InitQValues(maze);

            for (int episode = 0; episode < episodes; episode++)
            {
                var currentRow = 0;
                var currentColumn = 0;
                while (!IsTerminalState(maze, currentRow, currentColumn, rewards))
                {
                    var action = GetNextAction(qValues, currentRow, currentColumn, epsilon);
                    var previousRow = currentRow;
                    var previousColumn = currentColumn;
                    var (newRow, newColumn) = MoveOneStep(maze, currentRow, currentColumn, action);
                    currentRow = newRow;
                    currentColumn = newColumn;
                    var reward = rewards[currentRow, currentColumn];
                    var qValue = qValues[previousRow, previousColumn, action];
                    var temporalDifference = reward + (discountFactor * torch.max(qValues[currentRow, currentColumn])).item<float>() - qValue;
                    qValues[previousRow, previousColumn, action] = qValue + learningRate * temporalDifference;
                }
            }
        }
    }
}