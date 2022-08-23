namespace Parth_Traders.Service.Filter
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string messsage, Exception innerException) : base(messsage, innerException) { }
    }
}
