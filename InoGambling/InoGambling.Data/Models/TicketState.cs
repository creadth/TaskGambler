namespace InoGambling.Data.Models
{
    public enum TicketState : byte
    {
        None = 0,
        Created = 1,
        ToDo = 2,
        InProgress = 3,
        OnHold = 4,
        Verified = 5,
        Canceled = 6, //removed ticket
    }
}
