using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Interview;

public interface ISqliteService
{
	Task<DataTable> ExecuteSqlAsync(string sql);
	event EventHandler<StateChangeEventArgs>? ConnectionStateChanged;
}

public class SqliteService : ISqliteService
{
	private static string DbDileName => Path.Combine(Directory.GetCurrentDirectory(),"..","..","..","..","DB", "interview.db3");
	public event EventHandler<StateChangeEventArgs>? ConnectionStateChanged;
	
	public async Task<DataTable> ExecuteSqlAsync(string sql)
	{
		await using SQLiteConnection sqLiteConnection = new ($"Data Source={DbDileName};Version=3;", true);
		sqLiteConnection.StateChange += (sender, args) => OnConnectionStateChanged(args);
		if(sqLiteConnection.State != ConnectionState.Open){
			ConnectionState oldState = sqLiteConnection.State;
			await sqLiteConnection.OpenAsync();
		}
		await using SQLiteCommand command = new (sql, sqLiteConnection);
		await using DbDataReader reader = await command.ExecuteReaderAsync();
		DataTable dataTable = new ();
		dataTable.Load(reader);
		await sqLiteConnection.CloseAsync();
		return dataTable;
	}

	private void OnConnectionStateChanged(StateChangeEventArgs args){
		ConnectionStateChanged?.Invoke(this, args);
	}
}