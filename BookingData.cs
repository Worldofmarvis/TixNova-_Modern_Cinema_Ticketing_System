using System.Collections.Generic;

public class BookingData
{
    
    public string MovieName { get; set; }
    public string Cinema { get; set; }
    public string Schedule { get; set; }
    public List<SnackItem> Snacks { get; set; }
    public List<string> SelectedSeats { get; set; }

   
    public decimal TicketPrice { get; set; } = 250.00m;
    public int TicketQuantity { get; set; } = 1;
    public decimal TotalTicketPrice => TicketPrice * TicketQuantity;
    public decimal TotalSnacksPrice { get; set; }
    public decimal GrandTotal => TotalTicketPrice + TotalSnacksPrice;


    public static List<BookingData> AllBookings = new List<BookingData>();

    public BookingData()
    {
        Snacks = new List<SnackItem>();
        SelectedSeats = new List<string>();
    }
}

public class SnackItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal => Price * Quantity;
}