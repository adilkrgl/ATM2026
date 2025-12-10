namespace Artiligence.InvoiceSystem.Web.Models
{
    public enum DiscountType
    {
        None = 0,
        Amount = 1,
        Percent = 2
    }

    public enum PaymentStatus
    {
        Unpaid = 0,
        Partial = 1,
        Full = 2
    }

    public enum DeliveryStatus
    {
        NotDelivered = 0,
        BookedForDelivery = 1,
        Partial = 2,
        Full = 3
    }

    public enum PaymentMethod
    {
        Cash = 0,
        Card = 1,
        BankTransfer = 2,
        Finance = 3
    }

    public enum AddressType
    {
        Billing = 0,
        Delivery = 1,
        Other = 2
    }
}
