namespace Zakaz_travel.Models
{
    public class Order
    {
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public string? UserName { get; set; }
        public string? DeliveryAddress { get; set; }

        public void DisplayToConsole()
        {
            Decoration();
            Console.ForegroundColor = ConsoleColor.Blue;

            var orderInfo = new[]
            {
                $"- Название товара: {ProductName}",
                $"- Количество товара: {Quantity}",
                $"- Имя пользователя: {UserName}",
                $"- Адрес доставки: {DeliveryAddress}"
            };

            foreach ( var info in orderInfo )
            {
                Console.WriteLine( info );
            }

            Console.ResetColor();
        }

        private void Decoration()
        {
            Editor.Zakaz();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine( "\n                                             Оформление заказа" );
            Console.WriteLine( "                                           ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄" );
            Console.ResetColor();
        }
    }
}