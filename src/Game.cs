using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game21
{
    [Serializable]
    class Game
    {
        private enum GAME_RESULT
        {
            NEXT = 0,
            PLAYER_WIN,
            COMPUTER_WIN,
            DRAW
        };
        private enum ACTION
        {
            TAKE_CARD,
            END_TURN
        }

        //======================= Поля =======================

        private bool endGame;
        public bool EndGame
        {
            get
            {
                return this.endGame;
            }
        }

        private bool firstGame;
        public bool FirstGame
        {
            get
            {
                return this.firstGame;
            }
        }

        public bool isGameOver
        {
            get
            {
                return money < bet;
            }
        }

        private bool firstTurn;
        private Deck deck = new Deck(true);
        private Deck playerCards = new Deck();
        private Deck computerCards = new Deck();
        private int money;
        private int bet;
        private bool endPlayerGame;
        private int playerWins;
        private int computerWins;
        private static string[] gameMenu = { "Взять карту", "Себе" };
        private int activeMenuItem;

        //======================= Методы =======================

        public Game()
        {
            deck.shuffleDeck();
            money = GameSettings.startMoney;
            bet = GameSettings.defaultBet;
            endGame = false;
            endPlayerGame = false;
            firstTurn = true;
            firstGame = true;
            playerWins = 0;
            computerWins = 0;
            activeMenuItem = 0;
        }

        public void MenuAction(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    activeMenuItem++;
                    if (activeMenuItem >= gameMenu.Length)
                    {
                        activeMenuItem = 0;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    activeMenuItem--;
                    if (activeMenuItem < 0)
                    {
                        activeMenuItem = gameMenu.Length - 1;
                    }
                    break;
                case ConsoleKey.Enter:
                    if (!this.EndGame)
                    {
                        GameAction();
                    }
                    else
                    {
                        BeginGame();
                        DisplayGame();
                    }
                    break;
            }
        }

        private void GameAction()
        {
            switch ((ACTION)activeMenuItem)
            {
                case ACTION.TAKE_CARD:
                    playerCards.pushCard(deck.popCard());
                    if (playerCards.getPoints() > 21)
                    {
                        goto case ACTION.END_TURN;
                    }
                    break;
                case ACTION.END_TURN:
                    endPlayerGame = true;
                    ComputerGame();
                    break;
            }
            DisplayGame();
        }

        /// <summary>
        /// Отобразить состояние игры
        /// </summary>
        public void DisplayGame()
        {
            Console.Clear();

            if (isGameOver)
            {
                string message = "Game Over";
                Console.SetCursorPosition(Console.WindowWidth / 2 - message.Length / 2, Console.WindowHeight / 2);
                Console.Write(message);
                return;
            }

            //карты игроков

            if (endPlayerGame)
            {
                computerCards.setCardState(Card.STATE.OPEN);
            }

            computerCards.drawDeck(GameSettings.xOrigin, GameSettings.yOrigin, GameSettings.cardOffset);
            playerCards.drawDeck(GameSettings.xOrigin, GameSettings.yOrigin + GameSettings.cardDistance, GameSettings.cardOffset);

            //текущее состояние игры

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("\nСостояние игры:\n");
            Console.ResetColor();

            this.PrintPointsMoneyBetScore();
            GAME_RESULT result = this.GameAnalysis();
            if (this.endGame)
            {
                this.DisplayGameState(result);
            }
        }

        private void DisplayGameState(GAME_RESULT result)
        {
            Console.SetCursorPosition(0, 33);
            switch (result)
            {
                case GAME_RESULT.PLAYER_WIN:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nВы выиграли");
                    break;
                case GAME_RESULT.COMPUTER_WIN:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nКомпьютер выиграл");
                    break;
                case GAME_RESULT.DRAW:

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nНичья");
                    break;
                default:
                    break;
            }
            Console.ResetColor();
            Console.WriteLine("Для продолжения нажмите Enter");
        }

        /// <summary>
        /// Отобразить игрвое пользовательское меню
        /// </summary>
        public void DisplayMenu()
        {
            if (isGameOver) { return; }

            int index = 0;

            Console.SetCursorPosition(GameSettings.menuPositionLeft, GameSettings.menuPositionTop);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Ваше действие: ");
            Console.ResetColor();

            foreach (string menuItem in gameMenu)
            {
                Console.CursorLeft = GameSettings.menuPositionLeft;
                Console.CursorTop = GameSettings.menuPositionTop + index + 1;
                if (index == this.activeMenuItem)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine("{0}", menuItem);
                Console.ResetColor();
                index++;
            }
        }

        /// <summary>
        /// Начало игры: раздать по 2 карты игрокам
        /// </summary>
        public void BeginGame()
        {
            //обнуление

            activeMenuItem = 0;
            endGame = false;
            endPlayerGame = false;
            firstTurn = true;
            deck.fillDeck();
            deck.shuffleDeck();
            playerCards.clearDeck();
            computerCards.clearDeck();

            //раздать карты игроку

            Card tempCard;
            //игроку
            tempCard = deck.popCard();
            playerCards.pushCard(tempCard);
            tempCard = deck.popCard();
            playerCards.pushCard(tempCard);

            //раздать карты компьютеру

            tempCard = deck.popCard();
            computerCards.pushCard(tempCard);
            tempCard = deck.popCard();
            computerCards.pushCard(tempCard);
            computerCards.setCardState(Card.STATE.CLOSE);

            firstTurn = false;
            firstGame = false;
        }

        /// <summary>
        /// Игра компьютера
        /// </summary>
        private void ComputerGame()
        {
            int computerPoints = computerCards.getPoints();
            int playerPoints = playerCards.getPoints();
            if (playerPoints > 21)
            {
                return;
            }
            while (computerPoints <= GameSettings.maxComputerPoints && computerPoints < playerPoints)
            {
                computerCards.pushCard(deck.popCard());
                computerPoints = computerCards.getPoints();
            }
        }

        /// <summary>
        /// Анализ игры на текущий момент
        /// </summary>
        /// <returns>текущее состояние игры</returns>
        private GAME_RESULT GameAnalysis()
        {
            int playerPoints = playerCards.getPoints();
            int computerPoints = computerCards.getPoints();
            GAME_RESULT result = GAME_RESULT.NEXT;
            //анализ после раздачи по 2 карты
            if (firstTurn)
            {
                if (playerPoints > 21 && computerPoints > 21)
                {
                    BeginGame();
                }
                else if (playerPoints > 21 || computerPoints == 21)
                {
                    result = GAME_RESULT.COMPUTER_WIN;
                    computerWins++;
                    money -= bet;
                    endGame = true;
                }
                else if (playerPoints == 21 || computerPoints > 21)
                {
                    result = GAME_RESULT.PLAYER_WIN;
                    playerWins++;
                    money += bet * GameSettings.prizeFactor;
                    endGame = true;
                }
                else if (playerPoints == 21 && computerPoints == 21)
                {
                    result = GAME_RESULT.DRAW;
                    endGame = true;
                }
            }
            //анализ игры после ходов
            else if (endPlayerGame)
            {
                if (playerPoints > 21 || computerPoints == 21 || (computerPoints > playerPoints && computerPoints <= 21))
                {
                    result = GAME_RESULT.COMPUTER_WIN;
                    computerWins++;
                    money -= bet;
                    endGame = true;
                }
                else if (computerPoints > 21 || playerPoints > computerPoints)
                {
                    result = GAME_RESULT.PLAYER_WIN;
                    playerWins++;
                    money += bet * GameSettings.prizeFactor;
                    endGame = true;
                }
                else if (computerPoints == playerPoints)
                {
                    result = GAME_RESULT.DRAW;
                    endGame = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Вывести очки игроков, ставку, деньги
        /// </summary>
        private void PrintPointsMoneyBetScore()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nИгрок:\t{0}", playerCards.getPoints());
            Console.WriteLine("Деньги:\t{0}", money);
            Console.WriteLine("Ставка:\t{0}", bet);
            Console.WriteLine("Счёт:\t{0} : {1}", playerWins, computerWins);
            if (endPlayerGame)
            {
                Console.WriteLine("\nКомп:\t{0}", computerCards.getPoints());
            }
        }
    }
}