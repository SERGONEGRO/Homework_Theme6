using System;
using System.IO;
using System.Text;
using System.IO.Compression;

/// Домашнее задание
///
/// Группа начинающих программистов решила поучаствовать в хакатоне с целью демонстрации
/// своих навыков. 
/// 
/// Немного подумав они вспомнили, что не так давно на занятиях по математике
/// они проходили тему "свойства делимости целых чисел". На этом занятии преподаватель показывал
/// пример с использованием фактов делимости. 
/// Пример заключался в следующем: 
/// Написав на доске все числа от 1 до N, N = 50, преподаватель разделил числа на несколько групп
/// так, что если одно число делится на другое, то эти числа попадают в разные руппы. 
/// В результате этого разбиения получилось M групп, для N = 50, M = 6
/// 
/// N = 50
/// Группы получились такими: 
/// 
/// Группа 1: 1
/// Группа 2: 2 3 5 7 11 13 17 19 23 29 31 37 41 43 47
/// Группа 3: 4 6 9 10 14 15 21 22 25 26 33 34 35 38 39 46 49
/// Группа 4: 8 12 18 20 27 28 30 42 44 45 50
/// Группа 5: 16 24 36 40
/// Группа 6: 32 48
/// 
/// M = 6
/// 
/// ===========
/// 
/// N = 10
/// Группы получились такими: 
/// 
/// Группа 1: 1
/// Группа 2: 2 7 9
/// Группа 3: 3 4 10
/// Группа 4: 5 6 8
/// 
/// M = 4
/// 
/// Участники хакатона решили эту задачу, добавив в неё следующие возможности:
/// 1. Программа считыват из файла (путь к которому можно указать) некоторое N, 
///    для которого нужно подсчитать количество групп
///    Программа работает с числами N не превосходящими 1 000 000 000
///   
/// 2. В ней есть два режима работы:
///   2.1. Первый - в консоли показывается только количество групп, т е значение M
///   2.2. Второй - программа получает заполненные группы и записывает их в файл используя один из
///                 вариантов работы с файлами
///            
/// 3. После выполения пунктов 2.1 или 2.2 в консоли отображается время, за которое был выдан результат 
///    в секундах и миллисекундах
/// 
/// 4. После выполнения пунта 2.2 программа предлагает заархивировать данные и если пользователь соглашается -
/// делает это.
/// 
/// Попробуйте составить конкуренцию начинающим программистам и решить предложенную задачу
/// (добавление новых возможностей не возбраняется)
///
/// * При выполнении текущего задания, необходимо документировать код 
///   Как пометками, так и xml документацией
///   В обязательном порядке создать несколько собственных методов

namespace Homework_Theme6_task1._1
{
    class Program
    {
        static string source = @"e:\Theme6_Number.txt";
        static string groupFile = @"e:\Theme6_Groups.txt";
        static string zipFile = @"e:\Theme6_Groups.txt.zip";

        static void Main(string[] args)
        {
            int n = EnterNumber(1, 1000000000);   //ввод числа от 1 до миллиарда
            Console.WriteLine("Кол-во чисел определено.\n");

            Console.WriteLine("Создание файла...\n");
            CreateNumberFile(n,source);

            //определяем кол-во групп
            var groupCount = Math.Ceiling(Math.Log(n, 2));

            Console.WriteLine("\nЕсли нужно вывести только количество групп, введите '1'.");
            if (Console.ReadLine().Equals("1"))   //Если ввели '1', выводим только группы:
            {
                Console.WriteLine($"\nВсего групп: {groupCount}\n");
            }

            else
            {   //создаем файл с группами
                CreateGroupsFile(n);
                
                Console.WriteLine("\nЕсли желаете заархивировать файл с группами, нажмите '1':\n");
                if (Console.ReadLine().Equals("1"))
                {
                    CreateZipFile(groupFile ,zipFile);
                }
                
            }
               

        }

        /// <summary>
        /// Ввод числа от 1 до миллиарда
        /// </summary>
        /// <returns>число</returns>
        static int EnterNumber(int min, int max)
        {
            Console.WriteLine("Введите количество чисел от 1 до 1_000_000_000");
            string s = Console.ReadLine();

            int number;
            
            bool success = int.TryParse(s, out number);
            if (success && number>=min && number <=max)
            {
                return number;
            }
            else
            {
                EnterNumber( min, max); 
            }
            return 0;
        }

        /// <summary>
        /// Создание файла с заданным числом
        /// </summary>
        /// <param name="number">Количество чисел в файле</param>
        static void CreateNumberFile(int number, string path)
        {
            StringBuilder sb = new StringBuilder(number); 

            sb.Append(number);
            //запись числа в файл
            File.WriteAllText(path, sb.ToString());

            Console.WriteLine($"Файл c числом {sb} создан.\n");
        }
        

        /// <summary>
        /// Создание файла с группами
        /// </summary>
        /// <param name="n">Число N</param>
        static void CreateGroupsFile (int n)
        {
            DateTime dateBegin = DateTime.Now;

            StringBuilder sb = new StringBuilder(n);
            int groupnumber = 1;

            sb.AppendFormat($"Группа №{groupnumber}:\n ");    //ввод первой группы
            for (int i = 1; i <= n; i++)
            {
                if (i == Math.Pow(2, groupnumber)) { sb.AppendFormat($"\n\nГруппа №{groupnumber + 1}:\n "); groupnumber++; }
                sb.AppendFormat("{0} ", i);
            }

            //запись массива чисел в строку
            File.WriteAllText(groupFile, sb.ToString());

            DateTime dateEnd = DateTime.Now;
            TimeSpan secs = dateEnd - dateBegin;

            Console.WriteLine($"Файл {groupFile} создан за {secs.TotalSeconds:0.00} секунд. \n");
        }

        /// <summary>
        /// Архивирует файл
        /// </summary>
        /// <param name="input">входящий файл</param>
        /// <param name="output">исходящий файл</param>
        static void CreateZipFile (string input,string output)
        {
            Console.WriteLine("Запуск архивации...");
            DateTime dateBegin = DateTime.Now;

            using (FileStream ss = new FileStream(input, FileMode.OpenOrCreate))
            {
                using (FileStream ts = File.Create(output))   // поток для записи сжатого файла
                {
                    // поток архивации
                    using (GZipStream cs = new GZipStream(ts, CompressionMode.Compress))
                    {
                        ss.CopyTo(cs); // копируем байты из одного потока в другой
                        //Console.WriteLine("Сжатие файла завершено. Было: {1} байт  стало: {2} байт.",
                        //                  ss.Length*2,
                        //                  ts.Length*2 );
                    }
                }
            }

            DateTime dateEnd = DateTime.Now;
            TimeSpan secs = dateEnd - dateBegin;

            Console.WriteLine($"Файл {output} создан за {secs.TotalSeconds:0.00} секунд. \n");
        }

        

    }
}
