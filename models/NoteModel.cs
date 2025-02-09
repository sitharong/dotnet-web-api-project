using System.ComponentModel.DataAnnotations;

public class NoteModel : NoteRequestModel
{

    public string created_at { get; }

    public string updated_at { get; }
}