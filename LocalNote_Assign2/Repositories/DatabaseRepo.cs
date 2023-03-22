using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalNote_Assign2.Models;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace LocalNote_Assign2.Repositories
{
    public class DatabaseRepo
    {
        //Initialize database
        public async static void InitializeDatabase()
        {
            try
            {
                //Create database file in in Windows Storage
                await ApplicationData.Current.LocalFolder.CreateFileAsync("Notes.db", CreationCollisionOption.OpenIfExists);

                //Get full path to database
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Notes.db");

                //Open database connection
                using (SqliteConnection conn = new SqliteConnection($"Filename={dbPath}"))
                {
                    conn.Open();

                    //Define SQL command
                    String tableCommand = "CREATE TABLE IF NOT EXISTS NoteTable " +
                        "(NoteID INTEGER PRIMARY KEY, " +
                        "NoteTitle nvarchar(100) NOT NULL, " +
                        "NoteContent nvarchar(255));";

                    //Create command object
                    SqliteCommand cmd = new SqliteCommand(tableCommand, conn);

                    //Execute command
                    cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Database startup error occurred: " + ex.Message);
            }

        }

        //Get records from database
        public static ObservableCollection<Note> SelectNotes()
        {
            try
            {
                //Create collection for notes
                ObservableCollection<Note> notes = new ObservableCollection<Note>();

                //Create database connection
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Notes.db");

                using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
                {
                    //Open connection
                    db.Open();

                    //Create select command
                    SqliteCommand selectCommand = new SqliteCommand("SELECT NoteTitle, NoteContent FROM NoteTable", db);

                    //Execute command using data reader
                    SqliteDataReader query = selectCommand.ExecuteReader();

                    //Create note object from every retrieved record and add to list/collection
                    while (query.Read())
                    {
                        Note newNote = new Note(query.GetString(0), query.GetString(1));
                        notes.Add(newNote);
                    }

                    //Close connection
                    db.Close();
                }

                return notes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Note retrieval error occurred: " + ex.Message);
            }
            
            return null;
        }

        //Add new record to database
        public static void InsertNote(string noteTitle, string noteContent)
        {
            try
            {
                //Create database connection
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Notes.db");

                using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
                {
                    //Open connection
                    db.Open();

                    //Create insert command
                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    //Use parameterized query to sanitize input data (prevent SQL injection)
                    insertCommand.CommandText = "INSERT INTO NoteTable (NoteTitle, NoteContent) VALUES (@Title, @Content);";
                    insertCommand.Parameters.AddWithValue("@Title", noteTitle);
                    insertCommand.Parameters.AddWithValue("@Content", noteContent);

                    //Execute command
                    insertCommand.ExecuteReader();

                    //Close database connection
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Note retrieval error occurred: " + ex.Message);
            }
        }

        //Edit record in the database
        public static void UpdateNote(string noteTitle, string noteContent)
        {
            try
            {
                //Create database connection
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Notes.db");

                using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
                {
                    //Open connection
                    db.Open();

                    //Create insert command
                    SqliteCommand updateCommand = new SqliteCommand();
                    updateCommand.Connection = db;

                    //Use parameterized query to sanitize input data (prevent SQL injection)
                    updateCommand.CommandText = "UPDATE NoteTable SET NoteContent = @Content WHERE NoteTitle = @Title;";
                    updateCommand.Parameters.AddWithValue("@Title", noteTitle);
                    updateCommand.Parameters.AddWithValue("@Content", noteContent);

                    //Execute command
                    updateCommand.ExecuteReader();

                    //Close database connection
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Note retrieval error occurred: " + ex.Message);
            }
        }

        //Delete record from database
        public static void DeleteNote(string noteTitle)
        {
            try
            {
                //Create database connection
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Notes.db");

                using (SqliteConnection db = new SqliteConnection($"Filename={dbPath}"))
                {
                    //Open connection
                    db.Open();

                    //Create insert command
                    SqliteCommand deleteCommand = new SqliteCommand();
                    deleteCommand.Connection = db;

                    //Use parameterized query to sanitize input data (prevent SQL injection)
                    deleteCommand.CommandText = "DELETE FROM NoteTable WHERE NoteTitle = @Title;";
                    deleteCommand.Parameters.AddWithValue("@Title", noteTitle);

                    //Execute command
                    deleteCommand.ExecuteReader();

                    //Close database connection
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Note retrieval error occurred: " + ex.Message);
            }
        }
    }
}
