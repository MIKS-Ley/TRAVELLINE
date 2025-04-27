using Accommodations.Commands;
using Accommodations.Dto;

namespace Accommodations;

public static class AccommodationsProcessor
{
    private static BookingService _bookingService = new();
    private static Dictionary<int, ICommand> _executedCommands = new();
    private static int s_commandIndex = 0;

    public static void Run()
    {
        Console.WriteLine( "Booking Command Line Interface" );
        Console.WriteLine( "Commands:" );
        Console.WriteLine( "'book <UserId> <Category> <StartDate> <EndDate> <Currency>' - to book a room" );
        Console.WriteLine( "'cancel <BookingId>' - to cancel a booking" );
        Console.WriteLine( "'undo' - to undo the last command" );
        Console.WriteLine( "'find <BookingId>' - to find a booking by ID" );
        Console.WriteLine( "'search <StartDate> <EndDate> <CategoryName>' - to search bookings" );
        Console.WriteLine( "'exit' - to exit the application" );

        string input;
        while ( ( input = Console.ReadLine() ) != "exit" )
        {
            try
            {
                ProcessCommand( input );
            }
            catch ( ArgumentException ex )
            {
                Console.WriteLine( $"Error: {ex.Message}" );
            }
        }
    }
    //����� ��� �� ������ � ��������� ������
    private static void ProcessCommand( string input )
    {
        string[] parts = input.Split( ' ' );
        string commandName = parts[ 0 ];

        switch ( commandName )
        {
            case "book":
                CommandBook( parts );
                break;

            case "cancel":
                CommandCancel( parts );
                break;

            case "undo":
                CommandUndo( parts );
                break;

            case "find":
                CommandFind( parts );
                break;

            case "search":
                CommandSearch( parts );
                break;

            default:
                Console.WriteLine( "Unknown command." );
                break;
        }
    }

    private static void CommandBook( string[] parts )
    {
        if ( parts.Length != 6 )
        {
            throw new ArgumentException( "Invalid number of arguments for booking." );
        }

        if ( !int.TryParse( parts[ 1 ], out int userID ) ) // �������� ID
        {
            throw new ArgumentException( "Invalid User ID." );
        }

        // �������� �� ���������� ������ ���
        var startDate = �onvectorDateTime( parts[ 1 ], "Wrong start date." );
        var endDate = �onvectorDateTime( parts[ 2 ], "Wrong end date." );
        // �������� ��� ���� ��������� ������ ���� �������
        if ( startDate >= endDate )
        {
            throw new ArgumentException( "Invalid date input" );
        }

        // ������� ������� ������
        if ( !Enum.TryParse( parts[ 5 ], true, out CurrencyDto currency ) )
        {
            string availableCurrency = string.Join( ", ", Enum.GetNames( typeof( CurrencyDto ) ) );
            throw new ArgumentException( $"Invalid currency: '{parts[ 5 ]}'. Use the available list of currencies: {availableCurrency.ToLower()}" );
        }

        BookingDto bookingDto = new()
        {
            UserId = userID,
            Category = parts[ 2 ],
            StartDate = startDate,
            EndDate = endDate,
            Currency = currency,
        };

        BookCommand bookCommand = new( _bookingService, bookingDto );
        bookCommand.Execute();
        _executedCommands.Add( ++s_commandIndex, bookCommand );
        Console.WriteLine( "Booking command run is successful." );

    }

    private static void CommandCancel( string[] parts )
    {
        if ( parts.Length != 2 )
        {
            throw new ArgumentException( "Invalid number of arguments for canceling." );
        }

        //������� ������� �����������
        if ( !Guid.TryParse( parts[ 1 ], out Guid bookingId ) )
        {
            throw new ArgumentException( "Invalid ID for booking." );
        }
        CancelBookingCommand cancelCommand = new( _bookingService, bookingId );
        cancelCommand.Execute();
        _executedCommands.Add( ++s_commandIndex, cancelCommand );
        Console.WriteLine( "Cancellation command run is successful." );

    }

    private static void CommandUndo( string[] parts )
    {
        // �������� �� ������ ����
        if ( _executedCommands.Count == 0 )
        {
            throw new InvalidOperationException( "User command history is empty" );
        }
        // �������� ������� ��� ������
        if ( s_commandIndex < 1 )
        {
            throw new ArgumentException( "�ancellation cannot be made" );
        }
        _executedCommands[ s_commandIndex ].Undo();
        _executedCommands.Remove( s_commandIndex );
        s_commandIndex--;
        Console.WriteLine( "Last command undone." );

    }

    private static void CommandFind( string[] parts )
    {
        if ( parts.Length != 2 )
        {
            throw new ArgumentException( "Invalid arguments for 'find'. Expected format: 'find <BookingId>'" );
        }
        //������� ������� �����������
        if ( !Guid.TryParse( parts[ 1 ], out Guid id ) )
        {
            Console.WriteLine( "Invalid id for booking." );
            return;
        }
        FindBookingByIdCommand findCommand = new( _bookingService, id );
        findCommand.Execute();
    }

    private static void CommandSearch( string[] parts )
    {
        if ( parts.Length != 4 )
        {
            throw new ArgumentException( "Invalid arguments for 'search'. Expected format: 'search <StartDate> <EndDate> <CategoryName>'" );
        }
        // �������� �� ����
        var startDate = �onvectorDateTime( parts[ 1 ], "Wrong start date." );
        var endDate = �onvectorDateTime( parts[ 2 ], "Wrong end date." );

        string categoryName = parts[ 3 ];
        SearchBookingsCommand searchCommand = new( _bookingService, startDate, endDate, categoryName );
        searchCommand.Execute();
    }

    private static DateTime �onvectorDateTime( string value, string errorMessage )
    {
        if ( !DateTime.TryParse( value, out DateTime dateTimeParsed ) )
        {
            throw new InvalidDataException( errorMessage );
        }
        return dateTimeParsed;
    }
}
