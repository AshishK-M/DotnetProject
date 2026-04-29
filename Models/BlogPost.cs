namespace MyMvcApp.Models;

public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime PublishedOn { get; set; }
    public string Body { get; set; } = string.Empty;
}
