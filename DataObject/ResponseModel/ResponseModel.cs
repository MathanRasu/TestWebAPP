namespace DataObject.ResponseModel
{
    public class ResponseModel<T>
    {
        public List<T>? ListResult { get; set; } = new List<T>();
        public T? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string? StatusMessage { get; set; }
        public int StatusCode { get; set; }
        public string? ResponseMessage { get; set; }
        public string? Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
