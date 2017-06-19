using SQLite;
using System.IO;
using static System.Environment;

namespace UsersManagementSystem.Models
{
    class AuthenticationService
    {
        public static User LoggedUser { get; private set; }

        public static void LogIn(string username, string password)
        {
            UsersRepository repo = new UsersRepository(new SQLiteConnection(Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "db_login.db")));
            LoggedUser = repo.GetUserByUsernameAndPassword(username, password);
        }

        public static void Register(string username, string password)
        {
            UsersRepository repo = new UsersRepository(new SQLiteConnection(Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "db_login.db")));
            LoggedUser = repo.AddUser(username, password);
        }

        public static void LogOut()
        {
            LoggedUser = null;
        }
    }
}