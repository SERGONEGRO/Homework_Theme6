using System;
using System.IO;
using System.Text;
using System.IO.Compression;

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
            Console.ReadKey();
               

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
            Console.WriteLine("Создание файла с группами...\n");
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
                        Console.WriteLine("Сжатие файла завершено. Было: {0} байт  стало: {1} байт.",
                                          ss.Length * 2,
                                          ts.Length * 2);
                    }
                }
            }

            DateTime dateEnd = DateTime.Now;
            TimeSpan secs = dateEnd - dateBegin;

            Console.WriteLine($"Файл {output} создан за {secs.TotalSeconds:0.00} секунд. \n");
        }

        

    }
}
