namespace Astrum.News.ViewModels;

public class NewsForm
{
    public List<PostForm> Posts { get; set; } = new();
    public List<BannerForm> Banners { get; set; } = new();
    public List<WidgetForm> Widgets { get; set; } = new();
}