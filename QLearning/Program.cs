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

            int[,] maze =
            {
                { 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 },
                { 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 },
                { 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 },
                { 0 , 1 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 1 , 0 },
                { 0 , 1 , 1 , 1 , 0 , 0 , 0 , 0 , 1 , 0 , 1 , 0 },
                { 0 , 1 , 0 , 1 , 0 , 1 , 1 , 0 , 1 , 0 , 1 , 0 },
                { 0 , 1 , 0 , 1 , 0 , 1 , 0 , 0 , 1 , 0 , 1 , 0 },
                { 0 , 1 , 0 , 1 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 0 },
                { 0 , 1 , 1 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 1 , 0 },
                { 0 , 1 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 },
                { 0 , 1 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 2 , 0 },
                { 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 }
            };

            int startRow = 6;
            int startColumn = 1;


            var mazeSolver = new MazeSolver(maze,
                                            startRow,
                                            startColumn,
                                            epsilon,
                                            discountFactor,
                                            learningRate,
                                            episodes);
            mazeSolver.Train();

            mazeSolver.Solve();
        }
    }
}
