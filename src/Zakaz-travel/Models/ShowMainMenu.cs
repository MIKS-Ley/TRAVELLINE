namespace Zakaz_travel.Models
{
    public class PascalCase
    {
        public static void Menu()
        {
            var menuOperations = new[]
            {
                new MenuOperation("Начать", Editor.StartOrderProcess),
                new MenuOperation("Помощь", Editor.DisplayHelpMenu),
                new MenuOperation("Выход", () => Environment.Exit(0))
            };

            Action header = () =>
            {
                Editor.Zakaz();
                Console.Title = "Zakaz-travel.com";
                Console.WriteLine( "Выполнил: Клыков Михаил." );
                Console.WriteLine( "Выберите действие:" );
            };

            MenuOperation.CreateMenu(
                operations: menuOperations,
                customCodeBeforeRender: header,
                selectedColor: ConsoleColor.Blue
            );
        }
    }
}