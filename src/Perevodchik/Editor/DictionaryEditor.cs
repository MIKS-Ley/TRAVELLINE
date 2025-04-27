using System;
using System.Collections.Generic;
using System.IO;

namespace Perevodchik
{
    public static class DictionaryEditor
    {
        private const string filePath = "dictionary.txt";

        public static void DeleteDictionary( Dictionary<string, string> russianToEnglish,
                                          Dictionary<string, string> englishToRussian )
        {
            try
            {
                File.Delete( filePath );
                russianToEnglish.Clear();
                englishToRussian.Clear();
                ConsoleExtensions( $"Файл {filePath} успешно удален.", ConsoleColor.Green );
            }
            catch ( Exception ex )
            {
                ConsoleExtensions( $"Ошибка: {ex.Message}", ConsoleColor.Red );
            }
            WaitForContinue();
        }

        public static void ClearDictionary( Dictionary<string, string> russianToEnglish,
                                         Dictionary<string, string> englishToRussian )
        {
            try
            {
                File.WriteAllText( filePath, string.Empty );
                russianToEnglish.Clear();
                englishToRussian.Clear();
                ConsoleExtensions( "Словарь очищен.", ConsoleColor.Green );
            }
            catch ( Exception ex )
            {
                ConsoleExtensions( $"Ошибка: {ex.Message}", ConsoleColor.Red );
            }
            WaitForContinue();
        }

        public static void DisplayDictionary()
        {
            try
            {
                if ( File.Exists( filePath ) )
                {
                    ConsoleExtensions( File.ReadAllText( filePath ), ConsoleColor.Green );
                }
                else
                {
                    ConsoleExtensions( "Словарь не найден.", ConsoleColor.Red );
                }
            }
            catch ( Exception ex )
            {
                ConsoleExtensions( $"Ошибка: {ex.Message}", ConsoleColor.Red );
            }
            WaitForContinue();
        }

        public static void WaitForContinue()
        {
            ConsoleExtensions( "\nНажмите Enter чтобы продолжить...", ConsoleColor.Green );
            Console.ReadLine();
        }
        public static void ConsoleExtensions( this string text, ConsoleColor color )
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine( text );
            Console.ForegroundColor = originalColor;
        }

    }
}