namespace Karia.Api.ResourceParameters
{
    public class ExpertsResourceParameters
    {
        public string SearchQuery { get; set; }
        public bool IsMaster { get; set; }
        public bool IsHaveVehicle { get; set; }
        public int PageNumber { get; set; } = 1;
        
        private const int MaxSize = 20;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxSize) ? MaxSize : value;
        }

        public string OrderBy { get; set; } = "name";
    }
}