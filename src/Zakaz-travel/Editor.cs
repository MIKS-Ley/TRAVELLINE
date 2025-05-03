using Zakaz_travel.Models;

public static class Editor
{
    public const string ZakazText = @"
 /$$$$$$$$           /$$                                     /$$                                              /$$
|_____ $$           | $$                                    | $$                                             | $$
     /$$/   /$$$$$$ | $$   /$$  /$$$$$$  /$$$$$$$$         /$$$$$$    /$$$$$$   /$$$$$$  /$$    /$$  /$$$$$$ | $$
    /$$/   |____  $$| $$  /$$/ |____  $$|____ /$$/ /$$$$$$|_  $$_/   /$$__  $$ /$$__  $$|  $$  /$$/ |____  $$| $$
   /$$/     /$$$$$$$| $$$$$$/   /$$$$$$$   /$$$$/ |______/  | $$    | $$  \__/| $$$$$$$$ \  $$/$$/   /$$$$$$$| $$
  /$$/     /$$__  $$| $$_  $$  /$$__  $$  /$$__/            | $$ /$$| $$      | $$_____/  \  $$$/   /$$__  $$| $$
 /$$$$$$$$|  $$$$$$$| $$ \  $$|  $$$$$$$ /$$$$$$$$          |  $$$$/| $$      |  $$$$$$$   \  $/   |  $$$$$$$| $$
|________/ \_______/|__/  \__/ \_______/|________/           \___/  |__/       \_______/    \_/     \_______/|__/";

    public static void DisplayHelpMenu()
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.WriteLine( $@"
                                                                                        
                                      Техподдержка.                                     
                                                                                        
Пункт 2.11.                        Оформления заказа.                                   
                                                                                        
Здравствуйте! В данной программе вы можете заказать товары с магазина Zakaz - travel.com
Для этого вам нужно указать: название товара, его количество, ваше имя и адрес доставки.
Пример предоставлен ниже.                                                               
                                                                                        
    - Название товара: Сумка                                                            
    - Количество товара: 1                                                              
    - Имя пользователя: Мария                                                           
    - Адрес доставки: г. Москва ул. Ленина д. 18 кв. 4                                  
                                                                                        
После произойдет подтверждение заказа, и ваш товар будет доставлен в ближайшие время.   
                                                                                        
                                           С наилучшими пожеланиями ваш Zakaz-travel!   
" );
        Console.ResetColor();
        Console.WriteLine( "\nНажмите любую клавишу для выхода..." );
        Console.ReadKey();
        Console.Clear();
        PascalCase.Menu();
    }

    public static void Zakaz()
    {
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.WriteLine( ZakazText );
        Console.ResetColor();
    }

    public static Order EditOrder( Order order )
    {
        string[] parameters = {
            "    - Название товара",
            "    - Количество товара",
            "    - Имя пользователя",
            "    - Адрес доставки",
            "    - Оформить заказ"
        };

        int fieldID = 0;

        while ( true )
        {
            Zakaz();
            Console.WriteLine( "Редактирование данных заказа:" );
            Console.WriteLine( "Вверх/Вниз - выбор пункта | Enter - выбрать" );
            Console.WriteLine( "--------------------------------------------------" );

            for ( int i = 0; i < parameters.Length; i++ )
            {
                if ( i == fieldID )
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write( "> " );
                }
                else
                {
                    Console.Write( "  " );
                }

                if ( i < parameters.Length - 1 )
                {
                    string meaning = GetFieldValue( i, order );
                    Console.WriteLine( $"{parameters[ i ]}: {meaning}" );
                }
                else
                {
                    Console.WriteLine( parameters[ i ] );
                }
                Console.ResetColor();
            }

            var key = Console.ReadKey( true );

            switch ( key.Key )
            {
                case ConsoleKey.Escape:
                    return order;

                case ConsoleKey.UpArrow when fieldID > 0:
                    fieldID--;
                    break;

                case ConsoleKey.DownArrow when fieldID < parameters.Length - 1:
                    fieldID++;
                    break;

                case ConsoleKey.Enter:
                    if ( fieldID == parameters.Length - 1 )
                    {
                        Console.WriteLine( ZakazText );
                        ShowConfirmation( order );
                        Console.WriteLine( "\nНажмите любую клавишу для завершения..." );
                        Console.ReadKey();
                        PascalCase.Menu();
                    }
                    else
                    {
                        string newValue = EditField( fieldID, parameters, order );

                        switch ( fieldID )
                        {
                            case 0: order.ProductName = newValue; break;
                            case 1:
                                if ( int.TryParse( newValue, out int quantity ) )
                                {
                                    order.Quantity = quantity;
                                }
                                break;
                            case 2: order.UserName = newValue; break;
                            case 3: order.DeliveryAddress = newValue; break;
                        }
                    }
                    break;
            }
        }
    }

    public static void ShowConfirmation( Order order )
    {
        Zakaz();
        DateTime todayDate = DateTime.Today;
        DateTime futureDate = todayDate.AddDays( 3 );
        Console.WriteLine( $"{order.UserName}! Ваш заказ {order.ProductName} в количестве {order.Quantity} оформлен!" +
            $"\nОжидайте доставку по адресу {order.DeliveryAddress} к {futureDate:dd.MM.yyyy}. Ваш Zakaz-travel.com!" );
    }

    private static string GetFieldValue( int fieldIndex, Order order )
    {
        return fieldIndex switch
        {
            0 => order.ProductName ?? "[не указано]",
            1 => order.Quantity?.ToString() ?? "[не указано]",
            2 => order.UserName ?? "[не указано]",
            3 => order.DeliveryAddress ?? "[не указано]",
            _ => ""
        };
    }

    private static string EditField( int fieldIndex, string[] fields, Order order )
    {
        string currentValue = GetFieldValue( fieldIndex, order );

        while ( true )
        {
            Zakaz();
            Console.WriteLine( $"РЕДАКТИРОВАНИЕ: {fields[ fieldIndex ].Trim()}" );
            Console.WriteLine( "--------------------------------------------------" );
            Console.WriteLine( $"Текущее значение: {currentValue}" );

            if ( fieldIndex == 1 )
            {
                Console.WriteLine( "Введите новое количество:" );
            }
            else
            {
                Console.WriteLine( "Введите новое значение (или нажмите Enter чтобы оставить текущее):" );
            }

            Console.Write( "> " );
            string? newValue = Console.ReadLine();

            if ( string.IsNullOrWhiteSpace( newValue ) )
            {
                return currentValue;
            }

            if ( fieldIndex == 1 )
            {
                if ( int.TryParse( newValue, out int quantity ) && quantity > 0 )
                {
                    return quantity.ToString();
                }
                Console.WriteLine( "Ошибка: Введите целое число больше 0!" );
                Console.ReadKey();
            }
            else
            {
                return newValue;
            }
        }
    }
    // Универсальный метод для отображения ошибки
    public static void ShowError( string message )
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine( message );
        Console.ResetColor();
        Console.WriteLine( "Нажмите любую клавишу для повторного ввода..." );
        Console.ReadKey();
    }
    public static void StartOrderProcess()
    {
        var order = CollectOrderInformation();
        ConfirmOrder( order );
    }

    private static Order CollectOrderInformation()
    {
        var order = new Order();
        var steps = new OrderStep[]
        {
                new OrderStep(
                    "Введите Название товара:",
                    value => order.ProductName = value,
                    value => !string.IsNullOrWhiteSpace(value),
                    "Ошибка: Название товара не может быть пустым."
                ),
                new OrderStep(
                    "Введите количество товара:",
                    value => order.Quantity = int.Parse(value),
                    value => int.TryParse(value, out int qty) && qty > 0,
                    "Ошибка: Введите целое число больше 0!"
                ),
                new OrderStep(
                    "Введите Имя пользователя:",
                    value => order.UserName = value,
                    value => !string.IsNullOrWhiteSpace(value),
                    "Ошибка: Имя пользователя не может быть пустым."
                ),
                new OrderStep(
                    "Введите Адрес доставки:",
                    value => order.DeliveryAddress = value,
                    value => !string.IsNullOrWhiteSpace(value),
                    "Ошибка: Адрес доставки не может быть пустым."
                )
        };

        foreach ( var step in steps )
        {
            ProcessOrderStep( order, step );
        }

        return order;
    }

    private static void ProcessOrderStep( Order order, OrderStep step )
    {
        bool isValid = false;
        while ( !isValid )
        {
            try
            {
                DisplayOrderScreen( order );
                Console.Write( step.Prompt );

                var input = Console.ReadLine()?.Trim() ?? string.Empty;

                if ( !step.Validate( input ) )
                    throw new ArgumentException( step.ErrorMessage );

                step.SetValue( input );
                isValid = true;
            }
            catch ( Exception ex )
            {
                ShowError( ex.Message );
            }
        }
    }

    private static void ConfirmOrder( Order order )
    {
        var options = new[]
        {
                new MenuOperation("Да", () => FinalizeOrder(order)),
                new MenuOperation("Нет", () => {EditOrder(order); PascalCase.Menu(); } )
            };

        Action confirmationHeader = () =>
        {
            Editor.Zakaz();
            Console.ResetColor();
            Console.WriteLine( $"Здравствуйте, {order.UserName}, вы заказали " +
                            $"{order.Quantity} {order.ProductName} на адрес " +
                            $"{order.DeliveryAddress}, все верно?" );
        };

        MenuOperation.CreateMenu(
            operations: options,
            customCodeBeforeRender: confirmationHeader,
            selectedColor: ConsoleColor.Blue
        );
    }

    private static void FinalizeOrder( Order order )
    {
        Editor.ShowConfirmation( order );
        Console.ReadKey();
        PascalCase.Menu();
    }
    private static void DisplayOrderScreen( Order order )
    {
        Console.Clear();
        Editor.Zakaz();
        order.DisplayToConsole();
    }
    public record OrderStep(
    string Prompt,
    Action<string> SetValue,
    Func<string, bool> Validate,
    string ErrorMessage
    );
}

