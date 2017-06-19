using SQLite;

namespace UsersManagementSystem.Models
{
    class UsersRepository
    {
        SQLiteConnection connection;

        public UsersRepository(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public bool CheckUserByUsername(string username)
        {
            User user = connection.Table<User>().Where(u => u.Username == username).FirstOrDefault();
            return user != null;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            User user = connection.Table<User>().Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            return user ?? null;
        }

        public User AddUser(string username, string password)
        {
            if (!CheckUserByUsername(username))
            {
                User user = new User()
                {
                    ID = 0,
                    Username = username,
                    Password = password
                };
                connection.Insert(user);
                return user;
            }

            else
            {
                return null;
            }
        }
    }
}