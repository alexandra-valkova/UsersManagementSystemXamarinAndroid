using Android.App;
using Android.OS;
using Android.Widget;
using SQLite;
using System.IO;
using UsersManagementSystem.Models;
using static System.Environment;

namespace UsersManagementSystem.Activities
{
    [Activity(Label = "Login")]
    public class LoginActivity : Activity
    {
        EditText username, password;
        Button login, register;
        TextView errors;

        SQLiteConnection connection;
        UsersRepository repo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Login);

            username = FindViewById<EditText>(Resource.Id.usernameLogin);
            password = FindViewById<EditText>(Resource.Id.passwordLogin);
            login = FindViewById<Button>(Resource.Id.loginButton);
            register = FindViewById<Button>(Resource.Id.registerButton);
            errors = FindViewById<TextView>(Resource.Id.errorsLogin);

            connection = new SQLiteConnection(Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "db_login.db"));
            repo = new UsersRepository(connection);

            login.Click += Login_Click;

            register.Click += Register_Click;
        }

        private void Login_Click(object sender, System.EventArgs e)
        {
            if (CheckFieldsRequired())
            {
                AuthenticationService.LogIn(username.Text, password.Text);
                if (AuthenticationService.LoggedUser != null)
                {
                    StartActivity(typeof(UsersActivity));
                    Finish();
                }
                else
                {
                    errors.Text = "Username or password incorrect!";
                }
            }
        }

        private void Register_Click(object sender, System.EventArgs e)
        {
            if (CheckFieldsRequired())
            {
                AuthenticationService.Register(username.Text, password.Text);
                if (AuthenticationService.LoggedUser != null)
                {
                    StartActivity(typeof(UsersActivity));
                    Finish();
                }
                else
                {
                    errors.Text = "Username already exists!";
                }
            }
        }

        private bool CheckFieldsRequired()
        {
            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrEmpty(password.Text))
            {
                errors.Text = "Username and password are required!";
                return false;
            }

            return true;
        }
    }
}