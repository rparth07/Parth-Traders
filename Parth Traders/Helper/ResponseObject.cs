namespace Parth_Traders.Helper
{
    public class ResponseObject
    {
        public ResponseObject(string err, int statusCode)
        {
            this.Title = err;
            this.StatusCode = statusCode;
        }

        public string Title { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }
    }
}
