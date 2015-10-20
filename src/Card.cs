using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game21
{
    [Serializable]
    class Card
    {
        public enum STATE { OPEN = 1, CLOSE };

        //======================= Поля =======================

        private string face = "-";
        private string suit = "-";
        private int point = 0;
        private STATE state = STATE.OPEN;

        //======================= Свойства =======================

        public string Face
        {
            get
            {
                return face;
            }
            set
            {
                if (Array.IndexOf(CardData.face, value) != -1)
                {
                    face = value;
                }
            }
        }

        public string Suit
        {
            get
            {
                return suit;
            }
            set
            {
                if (Array.IndexOf(CardData.suit, value) != -1)
                {
                    suit = value;
                }
            }
        }

        public int Point
        {
            get
            {
                return point;
            }
            set
            {
                if (Array.IndexOf(CardData.point, value) != -1)
                {
                    point = value;
                }
            }
        }

        public STATE State
        {
            get
            {
                return state;
            }
            set
            {
                if (value == STATE.OPEN || value == STATE.CLOSE)
                {
                    state = value;
                }
            }
        }

        //======================= Методы =======================

        private Card() { }
        public Card(string face, string suite)
        {
            setValues(face, suite);
        }

        /// <summary>
        /// Установить параметры карты
        /// </summary>
        /// <param name="face">номинал</param>
        /// <param name="suite">масть</param>
        public void setValues(string face, string suit)
        {
            this.Face = face;
            this.Suit = suit;
            int pointIndex = Array.IndexOf(CardData.face, this.face);
            this.point = CardData.point[pointIndex];
        }

        /// <summary>
        /// Нарисовать карту в консоле (псевдографика)
        /// </summary>
        /// <param name="x">координата по x относительно верхнего левого угла окна в символах</param>
        /// <param name="y">координата по y относительно верхнего левого угла окна в символах</param>
        public void drawCard(int x, int y)
        {
            //проверка возможности отобразить карту
            if (this.Face == "-" || this.Suit == "-")
            {
                Console.WriteLine("Ошибка. Карту невозможно нарисвать. Не определены масть/номинал");
                return;
            }
            if (x < 0 || y < 0 || x > Console.WindowWidth || y > Console.WindowHeight)
            {
                Console.WriteLine("Ошибка. Карту невозможно нарисвать. Координаты выходят за окно");
                return;
            }
            //вывод карты в окно псевдографикой
            try
            {
                int imageIndex = Array.IndexOf(CardData.face, face);
                //получим массив строк изображения карты(для вывода с учётом цвета и координат)
                string[] splitedCardImage = CardData.cardImages[imageIndex].Split('\n');
                for (int i = 0; i < splitedCardImage.Length; i++)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.CursorLeft = x;
                    Console.CursorTop = y + i;
                    drawLine(splitedCardImage[i]);
                }
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        /// <summary>
        /// Вывести строку изображения карты карты (согласно цвету масти и состоянию)
        /// </summary>
        /// <param name="line">строка изображения карты</param>
        private void drawLine(string line)
        {
            switch (state)
            {
                case STATE.OPEN:
                    line = line.Replace("x", this.suit);
                    line = line.Replace("f", this.face);
                    for (int i = 0; i < line.Length; i++)
                    {
                        //установка цвета согласно масти карты
                        if (CardData.redSuite.Contains(suit))
                        {
                            if (!CardData.frameSymbols.Contains(line[i].ToString()))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                        }
                        Console.Write(line[i]);
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    break;
                case STATE.CLOSE:
                    line = line.Replace(" ", CardData.shirt);
                    line = line.Replace("x", CardData.shirt);
                    if (face.Length == 2)
                    {
                        line = line.Replace("f", CardData.shirt + CardData.shirt);
                    }
                    else
                    {
                        line = line.Replace("f", CardData.shirt);
                    }
                    Console.Write(line);
                    break;
                default:
                    Console.WriteLine("Ошибка. Состояне карты.");
                    break;
            }
        }
    }
}
