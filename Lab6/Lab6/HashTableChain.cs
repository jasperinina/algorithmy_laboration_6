using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class ListNode<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
        public ListNode<TKey, TValue> Next;
        public ListNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Next = null;
        }
    }

    public class HashTableChain<TKey, TValue>
    {
        private const int Size = 1000; // Размер таблицы
        private ListNode<TKey, TValue>[] Elements;
        private HashMethod hashMethod;
        public enum HashMethod // список всех хеш-функций
        {
            HashDivision,
            HashMultiplication,
            HashBitShift,
            HashByPi,
            HashFibonacci
        }

        // Конструктор, принимающий вариант хеш-функции
        public HashTableChain(HashMethod hashMethod)
        {
            Elements = new ListNode<TKey, TValue>[Size];
            this.hashMethod = hashMethod;
        }

        // Вставка элемента
        public void Insert(TKey key, TValue value)
        {
            // Вычисление индекса в зависимости от выбранного метода хеширования
            int index = GetHash(key) % Size;

            // Проверка наличия узла с тем же ключом
            ListNode<TKey, TValue> current = Elements[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    // Обновляем значение, если ключ уже существует
                    current.Value = value;
                    return;
                }
                current = current.Next;
            }

            // Создаем новый элемент
            ListNode<TKey, TValue> newNode = new ListNode<TKey, TValue>(key, value);
            newNode.Next = Elements[index]; // Добавляем его в начало списка
            Elements[index] = newNode; // Обновляем элемент в массиве
        }

        // Удаление элемнета
        public void Delete(TKey key)
        {
            int index = GetHash(key);
            ListNode<TKey, TValue> current = Elements[index];
            ListNode<TKey, TValue> previous = null;

            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    if (previous == null)
                    {
                        // Удаляем первый элемент в цепочке
                        Elements[index] = current.Next;
                    }
                    else
                    {
                        // Удаляем элемент в середине или в конце цепочки
                        previous.Next = current.Next;
                    }
                    return;
                }
                previous = current;
                current = current.Next;
            }

            throw new Exception("Key not found.");
        }

        // Поиск элемента
        public TValue Search(TKey key)
        {
            int index = GetHash(key);
            ListNode<TKey, TValue> current = Elements[index];

            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    return current.Value;
                }
                current = current.Next;
            }

            throw new Exception("Key not found.");
        }

        private int GetHash(TKey key)
        {
            switch (hashMethod)
            {
                case HashMethod.HashDivision:
                    return HashDivision(key);

                case HashMethod.HashMultiplication:
                    return HashMultiplication(key);

                case HashMethod.HashBitShift:
                    return HashBitShift(key);

                case HashMethod.HashByPi:
                    return HashByPi(key);

                case HashMethod.HashFibonacci:
                    return HashFibonacci(key);

                default:
                    throw new InvalidOperationException("Неизвестная хеш-функция");
            }
        }



        private int HashDivision(TKey key)
        {
            // формула с интернета: key % size
            if (key == null)
            {
                return 0;
            }

            if (key is int intKey)
            {
                return Math.Abs(intKey) % Size;
            }

            if (key is string stringKey)
            {
                return Math.Abs(stringKey.Length) % Size;
            }

            return Math.Abs(key.GetHashCode()) % Size;
        }

        private int HashMultiplication(TKey key)
        {
            // h(K)=[Size * ((C * K) % 1)] - формула из интернета

            double C = 0.123487654; // рандомная константа, лежащая в [0, 1]

            if (key == null)
            {
                return 0;
            }

            if (key is int intKey)
            {
                return (int)(Size * ((intKey * C) % 1));
            }

            if (key is string stringKey)
            {
                return (int)(Size * ((stringKey.Length * C) % 1));
            }

            int hash = key.GetHashCode();
            return (int)(Size * ((hash * C) % 1));
        }

        // Собственная хеш-функция (С битовым сдвигом)
        private int HashBitShift(TKey key)
        {
            if (key is int intKey)
            {
                return (intKey << 3) ^ intKey % Size;
            }

            if (key is string stringKey)
            {
                return (stringKey.Length << 3) ^ stringKey.Length % Size;
            }

            int hash = key.GetHashCode();
            return (hash << 3) ^ hash % Size;
        }

        // Собственная хеш-функция (С числом пи)
        private int HashByPi(TKey key)
        {
            double pi = Math.PI;
            if (key is int intKey)
            {
                return (int)(intKey * pi) % Size;
            }

            if (key is string stringKey)
            {
                return (int)(stringKey.Length * pi) % Size;
            }

            int hash = key.GetHashCode();
            return (int)(hash * pi) % Size;
        }

        // Собственная хеш-функция (С числом Фибоначчи)
        private int HashFibonacci(TKey key)
        {
            double C = (Math.Sqrt(5) - 1) / 2; // золотое сечение

            if (key is int intKey)
            {
                return (int)(Size * ((intKey * C) % 1));
            }

            if (key is string stringKey)
            {
                return (int)(Size * ((stringKey.Length * C) % 1));
            }

            int hash = key.GetHashCode();
            return (int)(Size * ((hash * C) % 1));
        }

        // Метод для подсчета коэффициента заполнения
        public double GetLoadFactor()
        {
            int filledSlots = 0;
            for (int i = 0; i < Size; i++)
            {
                if (Elements[i] != null)
                {
                    filledSlots++;
                }
            }
            return (double)filledSlots / Size;
        }

        // Метод для нахождения длины самой длинной цепочки
        public int GetLongestChainLength()
        {
            int longestChain = 0;
            for (int i = 0; i < Size; i++)
            {
                int currentChainLength = GetChainLength(Elements[i]);
                if (currentChainLength > longestChain)
                {
                    longestChain = currentChainLength;
                }
            }
            return longestChain;
        }

        // Метод для нахождения длины самой короткой цепочки
        public int GetShortestChainLength()
        {
            int shortestChain = int.MaxValue; // Начальная длина цепочки
            bool hasChains = false; // Переменная для отслеживания наличия цепочек

            for (int i = 0; i < Size; i++)
            {
                int currentChainLength = GetChainLength(Elements[i]);
                if (currentChainLength > 0)
                {
                    hasChains = true; // Если есть хотя бы одна цепочка
                    if (currentChainLength < shortestChain)
                    {
                        shortestChain = currentChainLength;
                    }
                }
            }

            return hasChains ? shortestChain : 0; // Возвращаем 0, если нет цепочек
        }

        // Вспомогательный метод для подсчета длины цепочки
        private int GetChainLength(ListNode<TKey, TValue> node)
        {
            int length = 0;
            while (node != null)
            {
                length++;
                node = node.Next;
            }
            return length;
        }
    }
}
