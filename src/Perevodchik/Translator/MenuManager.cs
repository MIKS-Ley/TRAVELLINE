using System;
using System.Collections.Generic;

namespace Perevodchik
{
    public static class MenuManager
    {
        public static void ShowMainMenu()
        {
            string[] options = { "Переводчик", "Редактор словаря", "Помощь", "Выход" };
            Action[] actions =
            {
                Translator.StartTranslator,
                ShowDictionaryEditorMenu,
                Editor.DisplayHelpMenu,
                () => Environment.Exit(0)
            };

            Action customCode = () =>
            {
                Console.Clear();
                Console.Title = "Translator";
                Editor.DisplayHeader( "Выполнил: Клыков Михаил.\nВыберите действие:" );
            };

            CreateCustomMenu(
                menuItems: options,
                actions: actions,
                selectedColor: ConsoleColor.Green,
                title: null,
                customCodeBeforeRender: customCode
            );
        }

        public static void ShowDictionaryEditorMenu()
        {
            string[] options = { "Удалить словарь", "Очистить словарь", "Добавить слова", "Просмотр словаря", "Выход" };
            Action[] actions = {
        () => DictionaryEditor.DeleteDictionary(Translator.RussianToEnglish, Translator.EnglishToRussian),
        () => DictionaryEditor.ClearDictionary(Translator.RussianToEnglish, Translator.EnglishToRussian),
        AddNewWordInteraction,
        DictionaryEditor.DisplayDictionary,
        ShowMainMenu
    };

            Action customCode = () => Editor.SiteName();
            CreateCustomMenu(
                menuItems: options,
                actions: actions,
                customCodeBeforeRender: customCode
            );
        }

        private static void AddNewWordInteraction()
        {
            Console.Clear();
            Editor.SiteName();

            var word = GetUserInput( "Введите новое слово: " );
            if ( string.IsNullOrWhiteSpace( word ) ) return;

            if ( Translator.RussianToEnglish.ContainsKey( word ) || Translator.EnglishToRussian.ContainsKey( word ) )
            {
                Console.WriteLine( "Это слово уже существует в словаре.", ConsoleColor.Yellow );
                Editor.WaitForContinue();
                return;
            }

            var translation = GetUserInput( "Введите перевод: " );
            if ( string.IsNullOrWhiteSpace( translation ) ) return;

            if ( Translator.AddNewWord( word, prompt =>
            {
                Console.Write( prompt );
                return Console.ReadLine();
            } ) )
            {
                Console.WriteLine( "Слово добавлено в словарь.", ConsoleColor.Green );
            }
            else
            {
                Console.WriteLine( "Не удалось добавить слово.", ConsoleColor.Red );
            }

            Editor.WaitForContinue();
        }

        private static string GetUserInput( string prompt )
        {
            Console.Write( prompt );
            return Console.ReadLine()?.Trim();
        }

        public static void CreateCustomMenu(
            string[] menuItems,
            Action[] actions,
            ConsoleColor selectedColor = ConsoleColor.Green,
            ConsoleColor defaultColor = ConsoleColor.Gray,
            string title = null,
            bool loopMenu = true,
            string pointer = "> ",
            string unselectedPointer = "  ",
            Action customCodeBeforeRender = null )
        {
            if ( menuItems == null || menuItems.Length == 0 )
                throw new ArgumentException( "Кнопки не могут быть пустыми" );

            if ( actions != null && menuItems.Length != actions.Length )
                throw new ArgumentException( "Количество кнопок должно быть равно количеству методов" );

            int selectedIndex = 0;
            Console.CursorVisible = false;

            do
            {
                Console.Clear();

                customCodeBeforeRender?.Invoke();

                if ( !string.IsNullOrEmpty( title ) )
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine( title + "\n" );
                    Console.ForegroundColor = defaultColor;
                }

                for ( int i = 0; i < menuItems.Length; i++ )
                {
                    if ( i == selectedIndex )
                    {
                        Console.Write( pointer );
                        Console.ForegroundColor = selectedColor;
                    }
                    else
                    {
                        Console.Write( unselectedPointer );
                        Console.ForegroundColor = defaultColor;
                    }

                    Console.WriteLine( menuItems[ i ] );
                    Console.ResetColor();
                }

                var key = Console.ReadKey( true ).Key;

                switch ( key )
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = ( selectedIndex - 1 + menuItems.Length ) % menuItems.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = ( selectedIndex + 1 ) % menuItems.Length;
                        break;
                    case ConsoleKey.Enter:
                        if ( actions != null && actions.Length > selectedIndex )
                        {
                            Console.Clear();
                            actions[ selectedIndex ]?.Invoke();

                            if ( !loopMenu ) return;
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            } while ( true );
        }
    }
}