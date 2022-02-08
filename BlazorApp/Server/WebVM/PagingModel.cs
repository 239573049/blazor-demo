namespace Web.WebVM
{
    public class PagingModel<T>
    {
        public int Count { get; set; }
        public T Data { get; set; }
        public PagingModel(T data, int count)
        {
            Data = data;
            Count = count;
        }
    }
}
