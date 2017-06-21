using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using SQLite;
using UsersManagementSystem.Models;
using static System.Environment;

namespace UsersManagementSystem.Activities
{
    [Activity(Label = "Save User")]
    public class SaveUserActivity : Activity
    {
        EditText username, password, firstName, lastName;
        CheckBox admin;
        Button save, delete, cancel;
        TextView usernameLabel, passwordLabel, firstNameLabel, lastNameLabel, errors;

        User user = null;
        SQLiteConnection db = new SQLiteConnection(System.IO.Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "db_login.db"));

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SaveUser);

            username = FindViewById<EditText>(Resource.Id.usernameSave);
            password = FindViewById<EditText>(Resource.Id.passwordSave);
            firstName = FindViewById<EditText>(Resource.Id.firstNameSave);
            lastName = FindViewById<EditText>(Resource.Id.lastNameSave);
            admin = FindViewById<CheckBox>(Resource.Id.adminRadioButton);
            save = FindViewById<Button>(Resource.Id.saveButton);
            delete = FindViewById<Button>(Resource.Id.deleteButton);
            cancel = FindViewById<Button>(Resource.Id.cancelButton);
            usernameLabel = FindViewById<TextView>(Resource.Id.usernameLabel);
            passwordLabel = FindViewById<TextView>(Resource.Id.passwordLabel);
            firstNameLabel = FindViewById<TextView>(Resource.Id.firstNameLabel);
            lastNameLabel = FindViewById<TextView>(Resource.Id.lastNameLabel);
            errors = FindViewById<TextView>(Resource.Id.errorsSave);

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

            else // Create
            {
                delete.Enabled = false;
                delete.SetTextColor(Resources.GetColor(Resource.Color.disabledButtonTextColor));
            }

            save.Click += Save_Click;

            delete.Click += Delete_Click;

            cancel.Click += Cancel_Click;
        }


        private void Save_Click(object sender, System.EventArgs e)
        {
            ClearErrors();

            if (user == null) // Create
            {
                user = new User();
            }

            User userCheck = db.Table<User>().Where(u => u.Username == username.Text && u.ID != user.ID).FirstOrDefault();
            if (userCheck != null)
            {
                errors.Text = "Username already exists!";
                return;
            }

            if (CheckFields())
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
                Toast.MakeText(this, "User saved!", ToastLength.Short).Show();
            }

            return;
        }

        private void ClearErrors()
        {
            ClearFieldError(username, usernameLabel);
            usernameLabel.Text = "Username";
            ClearFieldError(password, passwordLabel);
            passwordLabel.Text = "Password";
            ClearFieldError(firstName, firstNameLabel);
            firstNameLabel.Text = "First Name";
            ClearFieldError(lastName, lastNameLabel);
            lastNameLabel.Text = "Last Name";
        }

        private bool CheckFields()
        {
            bool valid = true;

            if (string.IsNullOrWhiteSpace(username.Text))
            {
                usernameLabel.Text = "Username is required!";
                SetFieldError(username, usernameLabel);
                valid = false;
            }

            if (string.IsNullOrEmpty(password.Text))
            {
                passwordLabel.Text = "Password is required!";
                SetFieldError(password, passwordLabel);
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(firstName.Text))
            {
                firstNameLabel.Text = "First Name is required!";
                SetFieldError(firstName, firstNameLabel);
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(lastName.Text))
            {
                lastNameLabel.Text = "Last Name is required!";
                SetFieldError(lastName, lastNameLabel);
                valid = false;
            }

            return valid;
        }

        private void SetFieldError(EditText field, TextView label)
        {
            field.Background.SetColorFilter(Resources.GetColor(Resource.Color.errorsRed), PorterDuff.Mode.SrcIn);
            label.SetTextColor(Resources.GetColor(Resource.Color.errorsRed));
        }

        private void ClearFieldError(EditText field, TextView label)
        {
            field.Background.ClearColorFilter();
            label.SetTextColor(Resources.GetColor(Resource.Color.secondaryColor));
        }

        private void Delete_Click(object sender, System.EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Delete " + user.FirstName + " " + user.LastName);
            alert.SetMessage("Do you really want to delete this user?");
            alert.SetPositiveButton("Delete", (senderAlert, args) =>
            {
                db.Delete(user);
                Finish();
                Toast.MakeText(this, "User deleted!", ToastLength.Short).Show();
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void Cancel_Click(object sender, System.EventArgs e)
        {
            Finish();
        }
    }
}