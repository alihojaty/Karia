namespace Karia.Api.ResourceParameters
{
    public class CommentsResourceParameters
    {
        const int MaxSize = 10;
        private int _pageSize = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxSize) ? MaxSize : value;
        }

        public int PageNumber { get; set; } = 1;
    }
}