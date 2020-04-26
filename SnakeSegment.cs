namespace Snake
{
    public partial class Snake
    {
        private struct SnakeSegment
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            public SnakeSegment(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}