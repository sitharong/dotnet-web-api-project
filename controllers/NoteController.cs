using Microsoft.AspNetCore.Mvc;

[Route("api/note")]
[ApiController]
public class NoteController : ControllerBase
{
    // mix service code in here
    private readonly NoteRepository noteService;
    public NoteController(NoteRepository repo)
    {
        noteService = repo;
    }
    private void setResponseHeader()
    {
        Response.Headers.Append("Access-Control-Allow-Origin", "*");
    }
    private ObjectResult responseError500(string message)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, new
        {
            statusCode = 500,
            message
        });
    }
    private ObjectResult responseNotFound()
    {
        return NotFound(new
        {
            statusCode = 404,
            message = "Note does not exist."
        });
    }

    [HttpGet]
    public async Task<IActionResult> helloWorld()
    {
        System.Diagnostics.Debug.WriteLine("Hello World");
        return Ok("Hello World");
    }

    [HttpGet("list")]
    public async Task<IActionResult> getNotes()
    {
        try
        {
            var notes = await noteService.getNotes();
            setResponseHeader();
            return Ok(notes);
        }
        catch (Exception ex)
        {
            return responseError500(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> getNote(int id)
    {
        try
        {
            var note = await noteService.getNote(id);
            if (note != null)
            {
                setResponseHeader();
                return Ok(note);
            }
            return responseNotFound();

        }
        catch (Exception ex)
        {
            return responseError500(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> addNote(NoteRequestModel note)
    {
        try
        {
            if (await noteService.createNote(note))
            {
                setResponseHeader();
                return Ok(await noteService.getNotes());
            }
            throw new Exception("Error on creating note");

        }
        catch (Exception ex)
        {
            return responseError500(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> updateNote(NoteRequestModel note)
    {
        try
        {
            if (!await noteService.isNoteExist(note.id))
            {
                return responseNotFound();

            }
            if (await noteService.updateNote(note))
            {
                setResponseHeader();
                return Ok(await noteService.getNotes());
            }
            throw new Exception("Error on updating note");
        }
        catch (Exception ex)
        {
            return responseError500(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> deleteNote(int id)
    {
        try
        {
            if (!await noteService.isNoteExist(id))
            {
                return responseNotFound();
            }
            if (await noteService.deleteNote(id))
            {
                setResponseHeader();
                return Ok(await noteService.getNotes());
            }
            throw new Exception("Error on deleting note");
        }
        catch (Exception ex)
        {
            return responseError500(ex.Message);
        }
    }

}