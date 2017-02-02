namespace TamagochiAPI.DAL.SQLite.Scripts
{
	internal class DBScripts
	{
		internal const string CreateUsersTable = 
			"CREATE TABLE IF NOT EXISTS `users` ( `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `name` TEXT NOT NULL, `last_login` TEXT )";

		internal const string CreateAnimalsTable =
			"CREATE TABLE IF NOT EXISTS `animals` ( `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `name` TEXT, `type` INTEGER NOT NULL, `owner_id` " +
			"INTEGER NOT NULL, `happines_level` INTEGER DEFAULT 0, `hungry_level` INTEGER DEFAULT 0, FOREIGN KEY(`owner_id`) REFERENCES users(id) )";
	}
}