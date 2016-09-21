using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Login.ListAdapters;
using Login.Models;
using SQLite;
using System.IO;
using System.Linq;
using static System.Environment;

namespace Login.Activities
{
    [Activity(Label = "Users")]
    public class UsersActivity : ListActivity
    {
        string pathToDatabase = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "db_login.db");
        User[] users;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Users);

            Button create = FindViewById<Button>(Resource.Id.create);

            PopulateListView();

            create.Click += delegate
            {
                StartActivity(typeof(EditActivity));
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
            Intent edit = new Intent(this, typeof(EditActivity));
            edit.PutExtra("userID", userID);
            StartActivity(edit);
        }

        protected void PopulateListView()
        {
            SQLiteConnection db = new SQLiteConnection(pathToDatabase);

            users = db.Table<User>().ToArray();

            ListAdapter = new UsersListAdapter(this, users);
        }
    }
}