namespace FiapCloudGames.Application.Results
{
    public class Pagination
    {
        public Pagination(int currPage, int itemsPerPage, int totalItems, int totalPages)
        {
            CurrPage = currPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

        public int CurrPage { get; private set; }
        public int ItemsPerPage { get; private set; }
        public int TotalItems { get; private set; }
        public int TotalPages { get; private set; }
    }
}
