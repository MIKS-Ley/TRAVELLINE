using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Perevodchik
{
    public static class Translator
    {
        public static Dictionary<string, string> RussianToEnglish = new Dictionary<string, string>();
        public static Dictionary<string, string> EnglishToRussian = new Dictionary<string, string>();
        public const string FilePath = "dictionary.txt";

        public static void LoadDictionary()
        {
            if ( File.Exists( FilePath ) )
            {
                foreach ( var line in File.ReadAllLines( FilePath ) )
                {
                    var parts = line.Split( ':' );
                    if ( parts.Length == 2 )
                    {
                        var word = parts[ 0 ].Trim();
                        var translation = parts[ 1 ].Trim();
                        RussianToEnglish[ word ] = translation;
                        EnglishToRussian[ translation ] = word;
                    }
                }
            }
            else
            {
                File.WriteAllText( FilePath, $"Файл был создан: {DateTime.Today:dd.MM.yyyy}\n" );
                Console.WriteLine( $"Файл '{FilePath}' был создан." );
            }
        }

        public static void StartTranslator()
        {
            while ( true )
            {
                Editor.SiteName();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine( "Выход клавиша -> '-'" );
                Console.Write( "Введите слово для перевода: " );
                var input = Console.ReadLine();

                if ( input == "-" ) break;

                foreach ( var word in input.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries ) )
                {
                    TranslateWord( word );
                }

                Editor.WaitForContinue();
            }
        }

        public static void TranslateWord( string word )
        {
            if ( RussianToEnglish.TryGetValue( word, out var translation ) )
            {
                Console.WriteLine( $"Перевод '{word}': {translation}" );
            }
            else if ( EnglishToRussian.TryGetValue( word, out var reverseTranslation ) )
            {
                Console.WriteLine( $"Перевод '{word}': {reverseTranslation}" );
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( $"Слово '{word}' не найдено в словаре." );
                Console.ResetColor();

                if ( Editor.AskToAddWord( word ) )
                {
                    AddNewWord( word );
                }
            }
        }
        public static bool AddNewWord( string word = null, Func<string, string> getTranslation = null )
        {
            getTranslation ??= prompt =>
            {
                Console.Write( prompt );
                return Console.ReadLine()?.Trim();
            };

            word ??= getTranslation( "Введите новое слово: " );
            var translation = getTranslation( "Введите перевод: " );

            if ( !RussianToEnglish.ContainsKey( word ) && !EnglishToRussian.ContainsKey( translation ) )
            {
                RussianToEnglish[ word ] = translation;
                EnglishToRussian[ translation ] = word;
                SaveWordToFile( word, translation );
                return true;
            }
            return false;
        }

        public static void SaveWordToFile( string word, string translation )
        {
            File.AppendAllText( FilePath, $"{word}:{translation}\n" );
        }
    }
}