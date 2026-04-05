using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SnakeGameLog
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            string logPath = "gamelog.txt";
            File.WriteAllText(logPath, "--- YENI OYUN BASLADI ---\n");

            int Genislik = 40;
            int Yukseklik = 10;
            int Skor = 0;
            bool gameOver = false;

            List<int> snakeX = new List<int>();
            List<int> snakeY = new List<int>();
            
            snakeX.Add(Genislik / 2);
            snakeY.Add(Yukseklik / 2);
            
            string direction = "RIGHT";

            Random rnd = new Random();
            int foodX = rnd.Next(1, Genislik - 1);
            int foodY = rnd.Next(1, Yukseklik - 1);
            
            File.AppendAllText(logPath, $"UPDATE -> itemSpawned x={foodX} y={foodY}\n------------------------\n");

            Console.CursorVisible = false;

            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    
                    if (keyInfo.Key == ConsoleKey.UpArrow && direction != "DOWN") direction = "UP";
                    else if (keyInfo.Key == ConsoleKey.DownArrow && direction != "UP") direction = "DOWN";
                    else if (keyInfo.Key == ConsoleKey.LeftArrow && direction != "RIGHT") direction = "LEFT";
                    else if (keyInfo.Key == ConsoleKey.RightArrow && direction != "LEFT") direction = "RIGHT";

                    File.AppendAllText(logPath, $"INPUT -> key={keyInfo.Key} playerX={snakeX[0]} playerY={snakeY[0]}\n------------------------\n");
                }

                int nextX = snakeX[0];
                int nextY = snakeY[0];

                if (direction == "UP") nextY--;
                else if (direction == "DOWN") nextY++;
                else if (direction == "LEFT") nextX--;
                else if (direction == "RIGHT") nextX++;

                File.AppendAllText(logPath, $"UPDATE -> playerMoved newX={nextX} newY={nextY}\n------------------------\n");

                if (nextX <= 0 || nextX >= Genislik || nextY <= 0 || nextY >= Yukseklik)
                {
                    gameOver = true;
                    File.AppendAllText(logPath, "COLLISION -> hitWall\n------------------------\n");
                    break;
                }

                bool hitSelf = false;
                for (int i = 0; i < snakeX.Count; i++)
                {
                    if (snakeX[i] == nextX && snakeY[i] == nextY) hitSelf = true;
                }
                
                if (hitSelf)
                {
                    gameOver = true;
                    File.AppendAllText(logPath, "COLLISION -> hitSelf\n------------------------\n");
                    break;
                }

                snakeX.Insert(0, nextX);
                snakeY.Insert(0, nextY);

                File.AppendAllText(logPath, $"CHECK -> collisionCheck playerX={nextX} foodX={foodX}\n");
                if (nextX == foodX && nextY == foodY)
                {
                    Skor += 10;
                    File.AppendAllText(logPath, $"COLLISION -> Skor={Skor}\n------------------------\n");

                    foodX = rnd.Next(1, Genislik - 1);
                    foodY = rnd.Next(1, Yukseklik - 1);
                    File.AppendAllText(logPath, $"UPDATE -> itemSpawned x={foodX} y={foodY}\n------------------------\n");
                }
                else
                {
                    Console.SetCursorPosition(snakeX[snakeX.Count - 1], snakeY[snakeY.Count - 1]);
                    Console.Write(" ");
                    snakeX.RemoveAt(snakeX.Count - 1);
                    snakeY.RemoveAt(snakeY.Count - 1);
                }

                Console.SetCursorPosition(foodX, foodY);
                Console.Write("*");

                Console.SetCursorPosition(snakeX[0], snakeY[0]);
                Console.Write("#");

                Console.SetCursorPosition(0, Yukseklik + 1);
                Console.Write($"Skor: {Skor} | Cikmak icin duvara carp");

                Thread.Sleep(100);
            }

            File.AppendAllText(logPath, $"GAME OVER -> finalSkor={Skor}\n------------------------\n");
            Console.Clear();
            Console.WriteLine("Oyun Bitti!");
            Console.WriteLine("Skorunuz: " + Skor);
            Console.WriteLine("Tum oyun loglari 'gamelog.txt' dosyasina kaydedildi.");
            Console.ReadLine();
        }
    }
}