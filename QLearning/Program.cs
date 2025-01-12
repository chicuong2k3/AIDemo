namespace QLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var epsilon = 0.1f;
            var discountFactor = 0.9f;
            var learningRate = 0.1f;
            var episodes = 1000;
            int[,] maze = {
                            { 0 , 0 , 0 , 0 , 0 , 2 , 0 , 0 , 0 , 0 , 0 , 0 },
                            { 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 },
                            { 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 1 , 0 },
                            { 0 , 1 , 1 , 0 , 1 , 1 , 1 , 1 , 0 , 1 , 1 , 0 },
                            { 0 , 0 , 0 , 0 , 1 , 1 , 0 , 1 , 0 , 1 , 1 , 0 },
                            { 0 , 1 , 1 , 1 , 1 , 1 , 0 , 1 , 1 , 1 , 1 , 0 },
                            { 0 , 1 , 1 , 1 , 1 , 1 , 0 , 1 , 1 , 1 , 1 , 0 },
                            { 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 1 , 0 },
                            { 0 , 1 , 0 , 1 , 1 , 1 , 1 , 1 , 0 , 1 , 1 , 0 },
                            { 0 , 1 , 0 , 1 , 0 , 0 , 0 , 1 , 0 , 1 , 1 , 0 },
                            { 0 , 1 , 1 , 1 , 0 , 1 , 1 , 1 , 0 , 1 , 1 , 0 },
                            { 0 , 0 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 0 , 0, 0 }
                        };

            var mazeSolver = new MazeSolver();
            mazeSolver.Train(maze, epsilon, discountFactor, learningRate, episodes);
        }
    }
}
