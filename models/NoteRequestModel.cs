using System.ComponentModel.DataAnnotations;

public class NoteRequestModel
{
    public int id { set; get; }

    [Required]
    [MaxLength(10)]
    public string title { set; get; }

    [MaxLength(100)]
    public string content { set; get; }

}