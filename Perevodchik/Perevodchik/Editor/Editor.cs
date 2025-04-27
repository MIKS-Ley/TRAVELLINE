using Perevodchik;

public static class Editor
{
    public const string TranslaterText = @"
$$$$$$$$\                                         $$\             $$\                         
\__$$  __|                                        $$ |            $$ |                        
   $$ |    $$$$$$\   $$$$$$\  $$$$$$$\   $$$$$$$\ $$ | $$$$$$\  $$$$$$\    $$$$$$\   $$$$$$\  
   $$ |   $$  __$$\  \____$$\ $$  __$$\ $$  _____|$$ | \____$$\ \_$$  _|  $$  __$$\ $$  __$$\ 
   $$ |   $$ |  \__| $$$$$$$ |$$ |  $$ |\$$$$$$\  $$ | $$$$$$$ |  $$ |    $$ /  $$ |$$ |  \__|
   $$ |   $$ |      $$  __$$ |$$ |  $$ | \____$$\ $$ |$$  __$$ |  $$ |$$\ $$ |  $$ |$$ |      
   $$ |   $$ |      \$$$$$$$ |$$ |  $$ |$$$$$$$  |$$ |\$$$$$$$ |  \$$$$  |\$$$$$$  |$$ |      
   \__|   \__|       \_______|\__|  \__|\_______/ \__| \_______|   \____/  \______/ \__|      
                                                                                              ";

    public static void SiteName()
    {
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine( TranslaterText );
        Console.WriteLine();
        Console.ResetColor();
    }

    public static void DisplayHelpMenu()
    {
        SiteName();
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine( $@"
                            ��������� ����������.                               
 ������������! ���������� ������ ��������� ����������� � �������� ����          
 � �������� ����� �� ���������� � ��������.                                     
                                                                                
 ������� ���� ������ �� ������ �������, ������� �� ������ �������������.       
 �� ���������� ���� ��� �������� ��� ������������ ��� ��������� ��� �� ��������.
 ����� �� ������:                                                               
                                                                                
 - ������� �������                                                              
 - ��������� �����                                                              
 - ������� �������                                                              
 - ����� ����� ������� �� �����.                                                
 - ������� ���������� ���� ������������                                         " );
        Console.ResetColor();
        Console.WriteLine( "\n������� ����� ������� ��� ������..." );
        Console.ReadKey();
        Console.Clear();
    }

    public static bool AskToAddWord( string word )
    {
        Console.WriteLine( $"\n�������� ����� '{word}' � �������?" );

        string[] options = { " �� ", " ��� " };
        int selectedIndex = 0;
        int menuTop = Console.CursorTop;
        Console.CursorVisible = false;

        while ( true )
        {
            Console.SetCursorPosition( 0, menuTop );

            for ( int i = 0; i < options.Length; i++ )
            {
                if ( i == selectedIndex )
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write( options[ i ] );
                Console.ResetColor();
            }

            var key = Console.ReadKey( true ).Key;
            switch ( key )
            {
                case ConsoleKey.LeftArrow:
                    selectedIndex = Math.Max( 0, selectedIndex - 1 );
                    break;
                case ConsoleKey.RightArrow:
                    selectedIndex = Math.Min( options.Length - 1, selectedIndex + 1 );
                    break;
                case ConsoleKey.Enter:
                    Console.CursorVisible = true;
                    Console.SetCursorPosition( 0, menuTop + 1 );
                    return selectedIndex == 0;
            }
        }
    }

    public static void DisplayHeader( string text )
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine( text );
        Console.ResetColor();
    }

    public static void WaitForContinue()
    {
        Console.WriteLine( "\n������� Enter ����� ����������...", ConsoleColor.Green );
        Console.ReadLine();
        Console.ResetColor();
    }
}