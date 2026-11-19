namespace BookwormAPI.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }

    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public string Genre { get; set; }

    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public string Status { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
