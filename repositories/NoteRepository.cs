using System.Data;
using System.Data.SqlClient;
using Dapper;

public class NoteRepository
{
    private readonly string CONNECTION_STRING;
    public NoteRepository(IConfiguration con)
    {
        CONNECTION_STRING = con.GetConnectionString("DefaultConnection");
    }
    private IDbConnection getConnection()
    {
        return new SqlConnection(CONNECTION_STRING);
    }
    public Task<IEnumerable<NoteModel>> getNotes()
    {
        string sql = "select id, title, created_at from tb_note";
        return getConnection().QueryAsync<NoteModel>(sql);
    }
    public Task<NoteModel> getNote(int id)
    {
        string sql = @"select * from tb_note where id=@id";
        return getConnection().QueryFirstOrDefaultAsync<NoteModel>(sql, new { id });
    }
    public async Task<bool> createNote(NoteRequestModel note)
    {
        string sql = @"insert into tb_note(title, content) values(@title, @content)";
        int row = await getConnection().ExecuteAsync(sql, new { note.title, note.content });
        return row > 0;
    }
    public async Task<bool> updateNote(NoteRequestModel note)
    {
        string sql = @"update tb_note set title=@title, content=@content, updated_at=getdate() where id=@id";
        int row = await getConnection().ExecuteAsync(sql, new { note.id, note.title, note.content });
        return row > 0;
    }
    public async Task<bool> deleteNote(int id)
    {
        string sql = @"delete from tb_note where id=@id";
        int row = await getConnection().ExecuteAsync(sql, new { id });
        return row > 0;
    }
    public async Task<bool> isNoteExist(int id)
    {
        string sql = @"select top 1 id from tb_note where id=@id";
        return await getConnection().QueryFirstOrDefaultAsync<NoteModel>(sql, new { id }) != null;
    }
}