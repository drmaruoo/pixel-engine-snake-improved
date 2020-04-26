using PixelEngine;

namespace Snake
{
    public class Apple : Game
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void SetCoordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void RandomizeFood(int width, int height)
        {
            X = Random(width - 2)+1;
            Y = Random(height - 2)+1;
        }
    }
}
