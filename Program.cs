using System;
using System.IO;

namespace LB4
{
    class Program
    {

        static void Main(string[] args)
        {
            bool paramSubdir = false;
            bool paramHidden = false;
            bool paramRead = false;
            bool paramArchive = false;
            string paramPath = Directory.GetCurrentDirectory();
            string paramMask = "*.*";
            long totalSize = 0;

            // Descriptions
            if (args.Length == 0 || args[0].ToLower() == "/?")
            {
                Console.WriteLine("Синтаксис: FileSummarySizeUtility [/s] [/a] [/h] [/r] [/p:path] [/m:mask]");
                Console.WriteLine("/s - включити пiдрахунок файлiв у пiдкаталогах");
                Console.WriteLine("/a - включити файли з архiвним атрибутом");
                Console.WriteLine("/h - включити прихованi файли");
                Console.WriteLine("/r - включити файли тiльки для читання");
                Console.WriteLine("/p:path - вказати каталог для пiдрахунку");
                Console.WriteLine("/m:mask - вказати маску файлiв (Наприклад -> *.txt)");
                Environment.Exit(0);
            }

            // Params
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "/s")
                {
                    paramSubdir = true;
                }
                else if (args[i].ToLower() == "/a")
                {
                    paramArchive = true;
                }
                else if (args[i].ToLower() == "/h")
                {
                    paramHidden = true;
                }
                else if (args[i].ToLower() == "/r")
                {
                    paramRead = true;
                }
                else if (args[i].ToLower().StartsWith("/p:"))
                {
                    paramPath = args[i].Substring(3);
                }
                else if (args[i].ToLower().StartsWith("/m:"))
                {
                    paramMask = args[i].Substring(3);
                }
                else
                {
                    Console.WriteLine("Помилка! Невiдомий параметр: " + args[i]);
                    Environment.Exit(1);
                }
            }

            // Summary size file
            try
            {
                var files = Directory.GetFiles(paramPath, paramMask, paramSubdir ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    var attributes = File.GetAttributes(file);

                    if (paramArchive || !attributes.HasFlag(FileAttributes.Archive))
                    {
                        if (paramHidden || !attributes.HasFlag(FileAttributes.Hidden))
                        {
                            if (paramRead || !attributes.HasFlag(FileAttributes.ReadOnly))
                            {
                                totalSize += new FileInfo(file).Length;
                            }
                        }
                    }
                }

                Console.WriteLine("Загальний обсяг файлiв: " + totalSize + " байт");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка! Спробуйте ще раз! Повiдомлення: " + ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
