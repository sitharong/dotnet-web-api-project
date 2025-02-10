using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/note")]
[ApiController]
[Authorize]
public class NoteController : ControllerBase
{
    // mix service code in here
    private readonly NoteRepository _noteService;

    public NoteController(NoteRepository noteRepo)
    {
        _noteService = noteRepo;
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
            var notes = await _noteService.getNotes();
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
            var note = await _noteService.getNote(id);
            if (note != null)
            {
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
            if (await _noteService.createNote(note))
            {
                return Ok(await _noteService.getNotes());
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
            if (!await _noteService.isNoteExist(note.id))
            {
                return responseNotFound();

            }
            if (await _noteService.updateNote(note))
            {
                return Ok(await _noteService.getNotes());
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
            if (!await _noteService.isNoteExist(id))
            {
                return responseNotFound();
            }
            if (await _noteService.deleteNote(id))
            {
                return Ok(await _noteService.getNotes());
            }
            throw new Exception("Error on deleting note");
        }
        catch (Exception ex)
        {
            return responseError500(ex.Message);
        }
    }

}