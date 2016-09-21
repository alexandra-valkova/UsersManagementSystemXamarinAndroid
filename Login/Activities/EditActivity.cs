using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Login.Models;
using SQLite;
using System.IO;
using static System.Environment;

namespace Login.Activities
{
    [Activity(Label = "Edit User")]
    public class EditActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Edit);

            EditText username = FindViewById<EditText>(Resource.Id.usernameEdit);
            EditText password = FindViewById<EditText>(Resource.Id.passwordEdit);
            EditText firstName = FindViewById<EditText>(Resource.Id.firstName);
            EditText lastName = FindViewById<EditText>(Resource.Id.lastName);
            CheckBox admin = FindViewById<CheckBox>(Resource.Id.admin);
            Button save = FindViewById<Button>(Resource.Id.save);
            Button delete = FindViewById<Button>(Resource.Id.delete);
            Button cancel = FindViewById<Button>(Resource.Id.cancel);
            TextView errors = FindViewById<TextView>(Resource.Id.errorsEdit);

            string pathToDatabase = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "db_login.db");
            SQLiteConnection db = new SQLiteConnection(pathToDatabase);

            User user = null;
            int userID = Intent.GetIntExtra("userID", 0);

            if (userID > 0) // Edit
            {
                user = db.Table<User>().Where(u => u.ID == userID).FirstOrDefault();

                username.Text = user.Username;
                password.Text = user.Password;
                firstName.Text = user.FirstName;
                lastName.Text = user.LastName;
                admin.Checked = user.IsAdmin;
            }

            save.Click += delegate
            {
                if (user == null) // Create
                {
                    user = new User();
                }

                User userCheck = db.Table<User>().Where(u => u.Username == username.Text && u.ID != user.ID).FirstOrDefault();

                if (userCheck == null)
                {
                    user.Username = username.Text;
                    user.Password = password.Text;
                    user.FirstName = firstName.Text;
                    user.LastName = lastName.Text;
                    user.IsAdmin = admin.Checked;

                    if (user.ID > 0)
                    {
                        db.Update(user);
                    }
                    else
                    {
                        db.Insert(user);
                    }
                    Finish();
                }
                else
                {
                    errors.Text = "Username already exists!";
                }
            };

            delete.Click += delegate
            {
                db.Delete(user);
                Finish();
            };

            cancel.Click += delegate
            {
                Finish();
            };
        }
    }
}