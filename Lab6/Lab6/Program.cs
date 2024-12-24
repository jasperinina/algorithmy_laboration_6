using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите задачу:");
                Console.WriteLine("1 - Хеш-таблицы с цепочками");
                Console.WriteLine("2 - Хеш-таблицы с открытой адресацией");
                Console.WriteLine("3 - Выход");
                Console.Write("Введите ваш выбор: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        TestHashTableChain();
                        break;
                    case "2":
                        TestHashTableOpenAddressing();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Повторите ввод.");
                        break;
                }
            }
        }

        static void TestHashTableOpenAddressing()
        {
            const int tableSize = 10000;
            const int numElements = 10000;
            var random = new Random();

            // Генерация 10000 уникальных ключей
            var keys = new HashSet<int>();
            while (keys.Count < numElements)
            {
                keys.Add(random.Next(1, int.MaxValue));
            }

            var keyList = new List<int>(keys);

            // Тестирование с различными стратегиями
            TestStrategy(new LinearProbing<int>(), "Linear Probing", keyList, tableSize);
            TestStrategy(new QuadraticProbing<int>(), "Quadratic Probing", keyList, tableSize);
            TestStrategy(new DoubleHashing<int>(), "Double Hashing", keyList, tableSize);
            TestStrategy(new CubeHashing<int>(), "Cube Hashing", keyList, tableSize);
            TestStrategy(new PseudoRandomHashing<int>(), "Pseudo Random Hashing", keyList, tableSize);
        }

        static void TestStrategy(IProbingStrategy<int> strategy, string strategyName, List<int> keys, int tableSize)
        {
            Console.WriteLine($"\nТестирование стратегии: {strategyName}");
            var hashTable = new HashTableOpenAddressing<int, string>(strategy);

            var stopwatch = Stopwatch.StartNew();

            // Вставка элементов
            foreach (var key in keys)
            {
                hashTable.Add(key, "Value" + key);
            }

            stopwatch.Stop();

            // Подсчёт длины самого длинного кластера
            var longestCluster = hashTable.GetLongestClusterLength();

            Console.WriteLine($"Время вставки: {stopwatch.ElapsedMilliseconds} мс");
            Console.WriteLine($"Самый длинный кластер: {longestCluster}");
        }

        static void TestHashTableChain()
        {
            int[] keys = new int[100000];
            Random random = new Random();

            for (int i = 0; i < 100000; i++)
            {
                keys[i] = random.Next(0, 10000000);
            }

            foreach (HashTableChain<int, string>.HashMethod method in Enum.GetValues(typeof(HashTableChain<int, string>.HashMethod)))
            {
                Console.WriteLine($"Хеш-функция: {method}");
                HashTableChain<int, string> hashTable = new HashTableChain<int, string>(method);

                var stopwatch = Stopwatch.StartNew();

                foreach (int key in keys)
                {
                    hashTable.Insert(key, $"Value {key}");
                }

                stopwatch.Stop();

                // Время вставки
                Console.WriteLine($"Время вставки: {stopwatch.ElapsedMilliseconds} мс");

                // Подсчет коэффициента заполнения
                Console.WriteLine($"Коэффициент заполнения: {hashTable.GetLoadFactor()}");

                // Длина самой длинной цепочки
                Console.WriteLine($"Длина самой длинной цепочки: {hashTable.GetLongestChainLength()}");

                // Длина самой короткой цепочки
                Console.WriteLine($"Длина самой короткой цепочки: {hashTable.GetShortestChainLength()}");

                Console.WriteLine();

            }
        }
    }
}