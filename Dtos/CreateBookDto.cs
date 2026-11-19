namespace BookwormAPI.Dtos;

public class CreateBookDto
{
    public string Title { get; set; }
    public string Author { get; set; }

    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public string Genre { get; set; }
    public string Status { get; set; }
    public int UserId { get; set; }
}
