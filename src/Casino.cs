using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game21
{
    /// <summary>
    /// Казино. Диспетчер игровых столов.
    /// </summary>
    class Casino
    {
        private static int TableAmount = 10;

        //======================= Поля =======================

        Game game = new Game();
        MemoryStream[] gameStates = new MemoryStream[TableAmount];
        BinaryFormatter BF = new BinaryFormatter();
        private event OnKey GameKey;
        private int activeTable = 0;

        //======================= Методы =======================

        static Casino()
        {
            Console.SetWindowSize(100, 50);
            Console.SetBufferSize(100, 50);
        }

        public Casino()
        {

            //Обработчик клавиатуры для игры

            GameKey += game.MenuAction;

            //Создание потоков для хранения состояний игры

            for (int i = 0; i < TableAmount; i++)
            {
                gameStates[i] = new MemoryStream();
            }

            //Сохраним начальное состояние объекта(игра не начата) для всех столов

            for (int i = 0; i < TableAmount; i++)
            {
                BF.Serialize(gameStates[i], game);
            }
        }

        public void BeginGame()
        {
            //Начало игры

            game.BeginGame();
            game.DisplayGame();
            game.DisplayMenu();

            //Игра (обработка событий)

            while (true)
            {
                PrintTableNumber();

                Thread.Sleep(10);

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userKey = Console.ReadKey(true);
                    if (userKey.Modifiers == ConsoleModifiers.Alt)
                    {
                        switch (userKey.Key)
                        {
                            case ConsoleKey.F1:
                                ChangeTable(0);
                                break;
                            case ConsoleKey.F2:
                                ChangeTable(1);
                                break;
                            case ConsoleKey.F3:
                                ChangeTable(2);
                                break;
                            case ConsoleKey.F4:
                                ChangeTable(3);
                                break;
                            case ConsoleKey.F5:
                                ChangeTable(4);
                                break;
                            case ConsoleKey.F6:
                                ChangeTable(5);
                                break;
                            case ConsoleKey.F7:
                                ChangeTable(6);
                                break;
                            case ConsoleKey.F8:
                                ChangeTable(7);
                                break;
                            case ConsoleKey.F9:
                                ChangeTable(8);
                                break;
                            case ConsoleKey.F10:
                                ChangeTable(9);
                                break;
                        }
                        game.DisplayGame();
                    }
                    else if (userKey.Key == ConsoleKey.UpArrow || userKey.Key == ConsoleKey.DownArrow || userKey.Key == ConsoleKey.Enter)
                    {
                        GameKey(userKey);
                    }
                    if (!game.EndGame)
                    {
                        game.DisplayMenu();
                    }
                }
            }
        }

        private void PrintTableNumber()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2, 1);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("Стол: {0}", activeTable + 1);
            Console.ResetColor();
            string message = "Для перекючения между столами Alt + F1...F10";
            Console.SetCursorPosition(Console.WindowWidth / 2 - message.Length / 2, Console.WindowHeight - 1);
            Console.Write(message);
        }

        /// <summary>
        /// Cменить текущий стол 
        /// </summary>
        /// <param name="table">Номер стола, к которому переходим</param>
        private void ChangeTable(int table)
        {
            if (activeTable == table)
            {
                return;
            }
            SaveGame();
            LoadGame(table);
        }

        /// <summary>
        /// Сохранить состояние игры в поток
        /// </summary>
        private void SaveGame()
        {
            gameStates[activeTable].Position = 0;
            BF.Serialize(gameStates[activeTable], game);
            GameKey -= game.MenuAction;
        }

        /// <summary>
        /// Загрузить состояние игры из потока
        /// </summary>
        /// <param name="table">Номер стола, на котором проходит игра</param>
        private void LoadGame(int table)
        {
            activeTable = table;
            gameStates[table].Position = 0;
            game = (Game)BF.Deserialize(gameStates[table]);

            //Если игра не начата - начнём

            if (game.FirstGame == true)
            {
                game.BeginGame();
            }

            GameKey += game.MenuAction;
        }
    }
}