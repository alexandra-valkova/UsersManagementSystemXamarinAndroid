using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
using System.Linq;
using UsersManagementSystem.ListAdapters;
using UsersManagementSystem.Models;
using static System.Environment;

namespace UsersManagementSystem.Activities
{
    [Activity(Label = "Users", MainLauncher = true)]
    public class UsersActivity : ListActivity
    {
        User[] users;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (AuthenticationService.LoggedUser == null)
            {
                StartActivity(typeof(LoginActivity));
                Finish();
            }

            SetContentView(Resource.Layout.Users);

            Button create = FindViewById<Button>(Resource.Id.addUserButton);
            Button logout = FindViewById<Button>(Resource.Id.logoutButton);

            PopulateListView();

            create.Click += delegate
            {
                StartActivity(typeof(SaveUserActivity));
            };

            logout.Click += delegate
            {
                AuthenticationService.LogOut();
                StartActivity(typeof(LoginActivity));
                Finish();
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            PopulateListView();
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            int userID = users[position].ID;
            Intent edit = new Intent(this, typeof(SaveUserActivity));
            edit.PutExtra("userID", userID);
            StartActivity(edit);
        }

        protected void PopulateListView()
        {
            SQLiteConnection db = new SQLiteConnection(Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "db_login.db"));
            db.CreateTable<User>();
            users = db.Table<User>().ToArray();

            ListAdapter = new UsersListAdapter(this, users);
        }
    }
}