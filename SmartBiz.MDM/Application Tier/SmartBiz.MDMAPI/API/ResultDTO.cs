namespace SmartBiz.MDMAPI
{
    public class ResultDTO<T>
    {
        public T[] Result { get; set; }

        public int TotalResultCount { get; set; }

        public int LocalResultCount { get; set; }
    }
}