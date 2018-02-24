namespace SampleApp
{
    public interface IBusContext
    {
        string CorrelationId { get; set; }
    }

    public class BusContext: IBusContext
    {
        public string CorrelationId { get; set; }
        public string SessionId { get; set; }
    }
}