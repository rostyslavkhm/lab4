using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к исходной директории:");
        string sourceDir = Console.ReadLine();

        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine("Ошибка: Указанной исходной директории не существует.");
            Environment.ExitCode = 1;
            Console.WriteLine("программа завершила работу с кодом 1");
            return;
        }

        Console.WriteLine("Введите путь к целевой директории:");
        string targetDir = Console.ReadLine();

        if (!Directory.Exists(targetDir))
        {
            Console.WriteLine("Ошибка: Указанной целевой директории не существует.");
            Environment.ExitCode = 1;
            Console.WriteLine("программа завершила работу с кодом 1");
            return;
        }

        Console.WriteLine("Введите количество файлов для копирования:");
        if (!int.TryParse(Console.ReadLine(), out int filesToCopy))
        {
            Console.WriteLine("Неверный формат количества файлов.");
            Environment.ExitCode = 1;
            Console.WriteLine("программа завершила работу с кодом 1");
            return;
        }

        Console.WriteLine("Введите шаблон для поиска файлов (например, *.exe):");
        string searchPattern = Console.ReadLine();

        try
        {
            string[] files = Directory.GetFiles(sourceDir, searchPattern);
            int filesCopied = 0;

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                string destFile = Path.Combine(targetDir, fileInfo.Name);

                if ((fileInfo.Attributes & FileAttributes.Hidden) != 0)
                {
                    Console.WriteLine($"Файл {fileInfo.Name} является скрытым.");
                    continue;
                }
                if ((fileInfo.Attributes & FileAttributes.ReadOnly) != 0)
                {
                    Console.WriteLine($"Файл {fileInfo.Name} доступен только для чтения.");
                    continue;
                }
                if ((fileInfo.Attributes & FileAttributes.Archive) == 0)
                {
                    Console.WriteLine($"Файл {fileInfo.Name} не имеет атрибут архивации.");
                    continue;
                }

                File.Copy(file, destFile, true);
                Console.WriteLine($"Файл {fileInfo.Name} скопирован в {targetDir}");
                filesCopied++;

                if (filesCopied >= filesToCopy)
                    break;
            }

            if (filesCopied >= filesToCopy)
            {
                Console.WriteLine($"Копирование завершено. Скопировано {filesCopied} файлов из {filesToCopy} запланированных.");
                Environment.ExitCode = 0; 
                Console.WriteLine("Программа завершила работу с кодом 0");
            }
            else
            {
                Console.WriteLine($"Копирование завершено. Скопировано {filesCopied} файлов из {filesToCopy} запланированных.");
                Environment.ExitCode = 1; 
                Console.WriteLine("Программа завершила работу с кодом 1");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            Environment.ExitCode = 1; 
            Console.WriteLine("Программа завершила работу с кодом 1");
        }
    }
}
