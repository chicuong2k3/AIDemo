using TorchSharp;

namespace QLearning
{
    public partial class MazeSolver
    {

        Action[] actions = { Action.UP, Action.DOWN, Action.LEFT, Action.RIGHT };

        const int WALL_REWARD = -500;
        const int GOAL_REWARD = 500;
        const int FLOOR_REWARD = -10;

        private float epsilon;
        private float discountFactor;
        private float learningRate;
        private float episodes;
        private int[,] maze;
        private torch.Tensor qValues;

        private int startRow;
        private int startColumn;

        public MazeSolver(int[,] maze,
                          int startRow,
                          int startColumn,
                          float epsilon,
                          float discountFactor,
                          float learningRate,
                          float episodes)
        {
            this.epsilon = epsilon;
            this.discountFactor = discountFactor;
            this.learningRate = learningRate;
            this.episodes = episodes;
            this.maze = maze;

            var mazeRows = maze.GetLength(0);
            var mazeColumns = maze.GetLength(1);
            qValues = torch.zeros(mazeRows, mazeColumns, actions.Length);

            this.startRow = startRow;
            this.startColumn = startColumn;
        }

        int[,] InitRewards()
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

        bool IsTerminalState(int currentRow, int currentColumn)
        {
            return maze[currentRow, currentColumn] == (int)Tile.Wall
                || maze[currentRow, currentColumn] == (int)Tile.Goal;
        }

        long GetNextAction(int currentRow, int currentColumn)
        {
            var random = new Random();
            var randomValue = random.NextDouble();
            if (randomValue < epsilon)
            {
                return random.Next(actions.Length);
            }
            else
            {
                return torch.argmax(qValues[currentRow, currentColumn]).item<long>();
            }
        }

        (int, int) MoveOneStep(int currentRow, int currentColumn, long action)
        {
            var mazeRows = maze.GetLength(0);
            var mazeColumns = maze.GetLength(1);

            var newRow = currentRow;
            var newColumn = currentColumn;

            if (actions[action] == Action.UP && currentRow > 0 && maze[currentRow - 1, currentColumn] != (int)Tile.Wall)
            {
                newRow--;
            }
            else if (actions[action] == Action.DOWN && currentRow < mazeRows - 1 && maze[currentRow + 1, currentColumn] != (int)Tile.Wall)
            {
                newRow++;
            }
            else if (actions[action] == Action.LEFT && currentColumn > 0 && maze[currentRow, currentColumn - 1] != (int)Tile.Wall)
            {
                newColumn--;
            }
            else if (actions[action] == Action.RIGHT && currentColumn < mazeColumns - 1 && maze[currentRow, currentColumn + 1] != (int)Tile.Wall)
            {
                newColumn++;
            }

            return (newRow, newColumn);
        }

        public void Train()
        {
            var rewards = InitRewards();

            for (int episode = 0; episode < episodes; episode++)
            {
                var (currentRow, currentColumn) = (startRow, startColumn);

                while (!IsTerminalState(currentRow, currentColumn))
                {
                    var nextAction = GetNextAction(currentRow, currentColumn);
                    var previousRow = currentRow;
                    var previousColumn = currentColumn;

                    (currentRow, currentColumn) = MoveOneStep(currentRow, currentColumn, nextAction);

                    var reward = rewards[currentRow, currentColumn];
                    var qValue = qValues[previousRow, previousColumn, nextAction];
                    var temporalDifference = reward + (discountFactor * torch.max(qValues[currentRow, currentColumn])).item<float>() - qValue;
                    qValues[previousRow, previousColumn, nextAction] = qValue + learningRate * temporalDifference;
                }
            }
        }

        public void Solve()
        {
            Console.WriteLine("Solution Path:");

            int currentRow = startRow;
            int currentColumn = startColumn;

            var path = new List<(int, int)> { (currentRow, currentColumn) };

            while (!IsTerminalState(currentRow, currentColumn))
            {
                var bestAction = torch.argmax(qValues[currentRow, currentColumn]).item<long>();

                var (newRow, newColumn) = MoveOneStep(currentRow, currentColumn, bestAction);
                currentRow = newRow;
                currentColumn = newColumn;

                path.Add((currentRow, currentColumn));
            }

            foreach (var (row, column) in path)
            {
                Console.WriteLine($"({row}, {column})");
            }

            VisualizePath(path, maze);
        }

        void VisualizePath(List<(int, int)> path, int[,] maze)
        {
            Console.WriteLine("Maze with Solution Path:");
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == (int)Tile.Wall)
                    {
                        Console.Write("W ");
                    }
                    else if (maze[i, j] == (int)Tile.Goal)
                    {
                        Console.Write("G ");
                    }
                    else if (path.Contains((i, j)))
                    {
                        Console.Write("P ");
                    }
                    else
                    {
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}