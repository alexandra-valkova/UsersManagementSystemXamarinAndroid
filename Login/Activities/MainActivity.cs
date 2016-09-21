using Android.App;
using Android.OS;
using Android.Widget;
using Login.Models;
using SQLite;
using System.IO;
using static System.Environment;

namespace Login.Activities
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/login")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);

            EditText username = FindViewById<EditText>(Resource.Id.username);
            EditText password = FindViewById<EditText>(Resource.Id.password);
            Button login = FindViewById<Button>(Resource.Id.login);
            Button register = FindViewById<Button>(Resource.Id.register);
            TextView errors = FindViewById<TextView>(Resource.Id.errors);
            
            string pathToDatabase = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "db_login.db");

            SQLiteConnection connection = new SQLiteConnection(pathToDatabase);
            connection.CreateTable<User>();

            login.Click += delegate
            {
                if (CheckUser(username.Text, password.Text, pathToDatabase))
                {
                    StartActivity(typeof(UsersActivity));
                }
                else
                {
                    errors.Text = "Username or password incorrect!";
                }
            };

            register.Click += delegate
            {
                if (AddUser(username.Text, password.Text, pathToDatabase))
                {
                    StartActivity(typeof(UsersActivity));
                }
                else
                {
                    errors.Text = "Username already exists!";
                }
            };
        }

        private bool CheckUser(string username, string password, string path)
        {
            SQLiteConnection db = new SQLiteConnection(path);
            User user = db.Table<User>().Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            return user != null ? true : false;
        }

        private bool AddUser(string username, string password, string path)
        {
            SQLiteConnection db = new SQLiteConnection(path);
            User userCheck = db.Table<User>().Where(u => u.Username == username).FirstOrDefault();
            if (userCheck == null)
            {
                User user = new User()
                {
                    ID = 0,
                    Username = username,
                    Password = password
                };
                db.Insert(user);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}