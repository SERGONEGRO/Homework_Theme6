using System;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace Homework_Theme6_task1._1
{
    class Program
    {
        static string source = @"\Theme6_Number.txt";
        static string groupFile = @"\Theme6_Groups.txt";
        static string zipFile = @"\Theme6_Groups.txt.zip";
        static string dir = @"C:\\papka";

        static void Main(string[] args)
        {
            Console.WriteLine($"Введите папку для хранения файлов, например:{dir}");
            dir = Console.ReadLine();
            if (Directory.Exists(dir)==false) Directory.CreateDirectory(dir);

            //обновляем пути до папки
            source = dir + source;
            groupFile = dir + groupFile;
            zipFile = dir + zipFile;

            //ввод числа от 1 до миллиарда
            Console.WriteLine("Введите количество чисел от 1 до 1_000_000_000");
            int n = EnterNumber(1, 1000000000);   
            Console.WriteLine("Кол-во чисел определено.\n");

            Console.WriteLine("Создание файла...\n");
            CreateNumberFile(n,source);
            Console.WriteLine($"Файл c числом {source} создан.\n");

            //определяем кол-во групп
            var groupCount = Math.Ceiling(Math.Log(n, 2));

            Console.WriteLine("\nЕсли нужно вывести только количество групп, введите '1'.");
            if (Console.ReadLine().Equals("1"))   //Если ввели '1', выводим только группы:
            {
                Console.WriteLine($"\nВсего групп: {groupCount}\n");
            }

            else
            {   //создаем файл с группами
                DateTime dateBegin = DateTime.Now;
                Console.WriteLine("Создание файла с группами...\n");

                CreateGroupsFile(n);

                DateTime dateEnd = DateTime.Now;
                TimeSpan secs = dateEnd - dateBegin;

                Console.WriteLine($"Файл {groupFile} создан за {secs.TotalSeconds:0.00} секунд. \n");

                Console.WriteLine("\nЕсли желаете заархивировать файл с группами, нажмите '1':\n");
                if (Console.ReadLine().Equals("1"))
                {
                    Console.WriteLine("Запуск архивации...");

                    dateBegin = DateTime.Now;
                    CreateZipFile(groupFile ,zipFile);

                    dateEnd = DateTime.Now;
                    secs = dateEnd - dateBegin;

                    Console.WriteLine($"Файл {zipFile} создан за {secs.TotalSeconds:0.00} секунд. \n");
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
        static string CreateNumberFile(int number, string path)
        {
            StringBuilder sb = new StringBuilder(number); 

            sb.Append(number);
            //запись числа в файл
            File.WriteAllText(path, sb.ToString());
            return sb.ToString();
            
        }
        

        /// <summary>
        /// Создание файла с группами
        /// </summary>
        /// <param name="n">Число N</param>
        static void CreateGroupsFile (int n)
        {
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

        }

        /// <summary>
        /// Архивирует файл
        /// </summary>
        /// <param name="input">входящий файл</param>
        /// <param name="output">исходящий файл</param>
        static void CreateZipFile (string input,string output)
        {
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

           
        }

    }
}
