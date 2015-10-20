using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game21
{
    [Serializable]
    class Deck
    {
        //======================= Поля =======================

        private int amount;
        public ArrayList cards;

        //======================= Методы =======================

        public Deck()
        {
            cards = new ArrayList();
            setAmount();
        }

        /// <summary>
        /// Cоздать заполненную колоду по порядку
        /// </summary>
        /// <param name="filled">true - заполненная колода; false - пустая колода</param>
        public Deck(bool filled)
        {
            cards = new ArrayList();
            setAmount();
            if (filled)
            {
                fillDeck();
            }
        }

        /// <summary>
        /// Установить количество карт в колоде, согласно данным из CardData
        /// </summary>
        private void setAmount()
        {
            if (CardData.face.Length != CardData.point.Length)
            {
                Console.WriteLine("Ошибка. Невозможно создать колоду. Неверные данные.");
                return;
            }
            int faceCount = CardData.face.Length;
            int suitCount = CardData.suit.Length;
            amount = faceCount * suitCount;
        }

        /// <summary>
        /// Заполнить колоду картами (по порядку)
        /// </summary>
        public void fillDeck()
        {
            string currentFace, currentSuit;
            Card currentCard;
            cards.Clear();
            int faceCardCount = CardData.face.Length;
            for (int i = 0; i < amount; i++)
            {
                currentFace = CardData.face[i % faceCardCount];
                currentSuit = CardData.suit[i / faceCardCount];
                currentCard = new Card(currentFace, currentSuit);
                cards.Add(currentCard);
            }
        }

        /// <summary>
        /// Перетасовать колоду
        /// </summary>
        public void shuffleDeck()
        {
            if (cards.Count == amount && amount != 0)
            {
                Random rand = new Random();
                int sourceIndex, destIndex;
                Card tmpCard;
                for (sourceIndex = 0; sourceIndex < amount; sourceIndex++)
                {
                    destIndex = rand.Next(0, amount);
                    tmpCard = (Card)cards[sourceIndex];
                    cards[sourceIndex] = cards[destIndex];
                    cards[destIndex] = tmpCard;
                }
            }
            else
            {
                Console.WriteLine("Ошибка. Данную колоду нельзя перетасовать.");
            }
        }

        /// <summary>
        /// "Взять" первую карту
        /// </summary>
        /// <returns></returns>
        public Card popCard()
        {
            Card firstCard = null;
            if (cards.Count != 0)
            {
                firstCard = (Card)cards[0];
                cards.RemoveAt(0);
            }
            else
            {
                Console.WriteLine("Ошибка. Колода пуста.");
            }
            return firstCard;
        }

        /// <summary>
        /// Добавить карту в колоду
        /// </summary>
        /// <param name="card">ссылка на объект карты</param>
        public void pushCard(Card card)
        {
            if (card != null)
                cards.Add(card);
        }

        /// <summary>
        /// Очистить колоду
        /// </summary>
        public void clearDeck()
        {
            if (cards.Count != 0)
                cards.Clear();
        }

        /// <summary>
        /// Получить количество карт в колоде
        /// </summary>
        /// <returns>количество карт в колоде</returns>
        public int getCount()
        {
            return cards.Count;
        }

        /// <summary>
        /// Получить сумму очков карт в колоде
        /// </summary>
        /// <returns>сумма очков карт в колоде</returns>
        public int getPoints()
        {
            int points = 0;
            if (cards.Count != 0)
            {
                foreach (Card card in cards)
                {
                    points += card.Point;
                }
            }
            return points;
        }

        /// <summary>
        /// Вывести колоду в графическом режиме
        /// </summary>
        /// <param name="leftOrigin">начало отсчёта по координате x</param>
        /// <param name="topOrigin">начало отсчёта по координате y</param>
        /// <param name="cardOffset">смещение карт относительно друг друга</param>
        public void drawDeck(int leftOrigin, int topOrigin, int cardOffset)
        {
            if (cards.Count != 0)
            {
                int index = 0;
                int currentCardTop, currentCardLeft;
                foreach (Card card in cards)
                {
                    try
                    {
                        currentCardTop = topOrigin;
                        currentCardLeft = leftOrigin + index * cardOffset;
                        card.drawCard(currentCardLeft, currentCardTop);
                        index++;
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine("Ошибка: {0}", e.Message);
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Колода пуста");
            }
        }

        /// <summary>
        /// Установить состояние карт в колоде
        /// </summary>
        /// <param name="state">состояние карт (открыты/закрыты)</param>
        public void setCardState(Card.STATE state)
        {
            foreach (Card card in cards)
            {
                card.State = state;
            }
        }

        /// <summary>
        /// Напечатать колоду (текстовый режим)  (отладка)
        /// </summary>
        public void printDeck()
        {
            if (cards.Count != 0)
            {
                foreach (Card card in cards)
                {
                    Console.Write("{0}{1}\t", card.Face, card.Suit);
                }
            }
            else
            {
                Console.WriteLine("Колода пуста");
            }
        }
    }
}
