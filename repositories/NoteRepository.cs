using System.Data;
using System.Data.SqlClient;
using Dapper;

public class NoteRepository
{
    private readonly string _connectionString;
    private readonly UserService _userService;
    public NoteRepository(IConfiguration con, UserService userService)
    {
        _connectionString = con.GetConnectionString("DefaultConnection");
        _userService = userService;
    }
    private IDbConnection getConnection()
    {
        return new SqlConnection(_connectionString);
    }
    private string sqlWhere()
    {
        return " where user_id='" + _userService.GetUserId() + "'";
    }
    private string sqlWhere(int noteId)
    {
        return sqlWhere() + " and id=" + noteId;
    }
    public Task<IEnumerable<NoteModel>> getNotes()
    {
        string sql = "select id, title, created_at from tb_note" + sqlWhere();
        return getConnection().QueryAsync<NoteModel>(sql);
    }
    public Task<NoteModel> getNote(int id)
    {
        string sql = "select * from tb_note" + sqlWhere(id);
        return getConnection().QueryFirstOrDefaultAsync<NoteModel>(sql);
    }
    public async Task<bool> createNote(NoteRequestModel note)
    {
        string sql = @"insert into tb_note(title, content, user_id) values(@title, @content, @user_id)";
        int row = await getConnection().ExecuteAsync(sql, new { note.title, note.content, user_id = _userService.GetUserId() });
        return row > 0;
    }
    public async Task<bool> updateNote(NoteRequestModel note)
    {
        string sql = @"update tb_note set title=@title, content=@content, updated_at=getdate()" + sqlWhere(note.id);
        int row = await getConnection().ExecuteAsync(sql, new { note.id, note.title, note.content });
        return row > 0;
    }
    public async Task<bool> deleteNote(int id)
    {
        string sql = "delete from tb_note" + sqlWhere(id);
        int row = await getConnection().ExecuteAsync(sql);
        return row > 0;
    }
    public async Task<bool> isNoteExist(int id)
    {
        string sql = @"select top 1 id from tb_note where id=@id";
        return await getConnection().QueryFirstOrDefaultAsync<NoteModel>(sql, new { id }) != null;
    }
}