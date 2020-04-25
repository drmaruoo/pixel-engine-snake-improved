/*
Improved version of an example at https://github.com/DevChrome/Pixel-Engine/blob/master/Examples/Snake.cs
*/

using System;
using System.Collections.Generic;
using PixelEngine;

namespace Examples
{
    public class Snake : Game
    {
        private List<SnakeSegment> snakeSegments;

        private Apple apple = new Apple();

        private int score;

        private Direction direction;

        private Pixel backgroundColor = Pixel.FromHsv(100, 0.90f, 0.15f);

        private bool dead; 
        private bool gameStarted;

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        static void Main(string[] args)
        {
            Snake SnakeGame = new Snake();
            SnakeGame.Construct(50, 50, 10, 10, 30);
            SnakeGame.Start();
        }

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

        private struct Apple
        {
            public int X { get; set; }
            public int Y { get; set; }

            public void SetCoordinates(int x, int y)
            {
                X = x;
                Y = y;
            }
        }


        public Snake() => AppName = "Pixel Engine Snake";

        public override void OnCreate()
        {
            Enable(Subsystem.HrText);

            ResetGame();
        }

        private void ResetGame()
        {
            snakeSegments = new List<SnakeSegment>();
            for (int i = 0; i < 4; i++)
                snakeSegments.Add(new SnakeSegment(20 + i, 15));
            apple.SetCoordinates(30,15);
            score = 0;
            direction = Direction.Left;
            dead = false;
        }

        public override void OnUpdate(float elapsed)
        {
            CheckStart();
            UpdateSnake();
            DrawGame();
        }

        private void DrawGame()
        {
            Clear(backgroundColor);

            if (gameStarted)
                DrawTextHr(new Point(15, 15), "Score: " + score, Pixel.Presets.White, 2);
            else
                DrawTextHr(new Point(15, 15), "Press Enter To Start", Pixel.Presets.White, 2);

            DrawRect(new Point(0, 0), ScreenWidth - 1, ScreenHeight - 1, Pixel.Presets.Black);

            for (int i = 1; i < snakeSegments.Count; i++)
                Draw(snakeSegments[i].X, snakeSegments[i].Y, Pixel.Presets.Green);

            Draw(snakeSegments[0].X, snakeSegments[0].Y, Pixel.Presets.Green);

            Draw(apple.X, apple.Y, Pixel.Presets.DarkGreen);
        }

        private void UpdateSnake()
        {
            if (dead)
                gameStarted = false;


            if (GetKey(Key.Up).Pressed && direction != Direction.Down)
            {
                direction = Direction.Up;
            }

            if (GetKey(Key.Right).Pressed && direction != Direction.Left)
            {
                direction = Direction.Right;
            }

            if (GetKey(Key.Down).Pressed && direction != Direction.Up)
            {
                direction = Direction.Down;
            }

            if (GetKey(Key.Left).Pressed && direction != Direction.Right)
            {
                direction = Direction.Left;
            }

            if (gameStarted)
            {
                switch (direction)
                {
                    case Direction.Up:
                        snakeSegments.Insert(0, new SnakeSegment(snakeSegments[0].X, snakeSegments[0].Y - 1));
                        break;
                    case Direction.Right:
                        snakeSegments.Insert(0, new SnakeSegment(snakeSegments[0].X + 1, snakeSegments[0].Y));
                        break;
                    case Direction.Down:
                        snakeSegments.Insert(0, new SnakeSegment(snakeSegments[0].X, snakeSegments[0].Y + 1));
                        break;
                    case Direction.Left:
                        snakeSegments.Insert(0, new SnakeSegment(snakeSegments[0].X - 1, snakeSegments[0].Y));
                        break;
                }

                snakeSegments.RemoveAt(snakeSegments.Count - 1);

                CheckCollision();
            }
        }

        private void CheckCollision()
        {
            if (snakeSegments[0].X == apple.X && snakeSegments[0].Y == apple.Y)
            {
                score++;
                RandomizeFood();

                snakeSegments.Add(new SnakeSegment(snakeSegments[snakeSegments.Count - 1].X, snakeSegments[snakeSegments.Count - 1].Y));
            }

            if (snakeSegments[0].X <= 0 || snakeSegments[0].X >= ScreenWidth || snakeSegments[0].Y <= 0 || snakeSegments[0].Y >= ScreenHeight - 1)
                dead = true;

            for (int i = 1; i < snakeSegments.Count; i++)
                if (snakeSegments[i].X == snakeSegments[0].X && snakeSegments[i].Y == snakeSegments[0].Y)
                    dead = true;
        }

        private void CheckStart()
        {
            if (!gameStarted)
            {
                if (GetKey(Key.Enter).Pressed)
                {
                    ResetGame();
                    RandomizeFood();
                    gameStarted = true;
                }
            }
        }

        private void RandomizeFood()
        {
            while (GetScreenPixel(apple.X, apple.Y) != backgroundColor)
            {
                apple.SetCoordinates(Random(ScreenWidth), Random(ScreenHeight));
            }
        }
    }
}