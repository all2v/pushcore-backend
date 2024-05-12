namespace Astrum.Projects.ViewModels.Views
{
    public class ProductPaginationView
    {
        public List<ProductView> Products { get; set; }
        public int Index { get; set; }
        public bool NextExist { get; set; }
    }
}
