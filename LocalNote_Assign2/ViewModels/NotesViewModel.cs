using LocalNote_Assign2.Commands;
using LocalNote_Assign2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote_Assign2.ViewModels
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        //Commands
        public AddCommand AddCommand { get; }
        public SaveCommand SaveCommand { get; }
        public EditCommand EditCommand { get; }
        public DeleteCommand DeleteCommand { get; }
        public ExitCommand ExitCommand { get; }

        //Booleans
        private Boolean _isReadOnly = false;
        public Boolean IsReadOnly 
        {
            get { return _isReadOnly; } 
            set { 
                if(value == _isReadOnly) { return; }
                _isReadOnly = value;
                
                //Invoke property changed event
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsReadOnly"));
            }
        }       

        //Search properties
        private string _search;
        public string Search
        {
            get { return _search; }
            set {
                if (value == _search) { return; }
                _search = value;

                //Call filtering/search method
                PerformFiltering();

                //Invoke property changed event
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Search"));
            }
        }

        //Property for setting command bar note title
        private string _comBarTitle;
        public string ComBarTitle
        {
            get { return _comBarTitle; }
            set
            {
                if (SelectedNote == null)
                {
                    _comBarTitle = "Untitled Note";
                }
                else
                {
                    _comBarTitle = value;
                }
            }
        }

        //Notes Collection
        public ObservableCollection<Note> NotesCollection { get; set; }

        //Notes list for filtering use
        private List<Note> _allNotes = new List<Note>();

        //Selected Note object
        private Note _selectedNote;

        //Selected Note title
        public string SelectedNoteTitle { get; set; }

        //Selected Note content
        public string SelectedNoteContent { get; set; }

        //Events
        public event PropertyChangedEventHandler PropertyChanged;

        //Constructor
        public NotesViewModel()
        {
            //Instantiate commands
            AddCommand = new AddCommand(this);
            SaveCommand = new SaveCommand(this);
            EditCommand = new EditCommand(this);
            DeleteCommand = new DeleteCommand(this);
            ExitCommand = new ExitCommand(this);

            //Create collection of notes
            NotesCollection = new ObservableCollection<Note>();

            //Load notes
            LoadNotes();
        }

        public Note SelectedNote 
        { 
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                
                if (value == null)
                {
                    SelectedNoteContent = null;
                    SelectedNoteTitle = null;
                    ComBarTitle = null;
                }
                else
                {
                    SelectedNoteContent = value.NoteContent;
                    SelectedNoteTitle = value.NoteTitle;
                    ComBarTitle = SelectedNoteTitle;
                }

                //Set IsReadOnly back to false when new note is selected
                if (IsReadOnly == false)
                {
                    IsReadOnly = true;
                }

                //Disable IsReadOnly if add button is clicked
                if (SelectedNote == null)
                {
                    IsReadOnly = false;
                }

                //Update Note Ttitle/Content when changed
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNoteContent"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNoteTitle"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ComBarTitle"));

                //Check if commands can/can't be exectued after a change
                AddCommand.Check_CanExecuteChanged();
                SaveCommand.Check_CanExecuteChanged();
                EditCommand.Check_CanExecuteChanged();
                DeleteCommand.Check_CanExecuteChanged();
            }
        }

        //Filtering method (Provided by Geoff Gillespie for this assignment (2022-02-23))
        private void PerformFiltering()
        {
            if (_search == null)
            {
                _search = "";
            }

            //If _filter has a value (ie. user entered something in filter textbox)
            //Lower-case and trim string
            var lowerCaseFilter = Search.ToLowerInvariant().Trim();

            //Use LINQ query to get all note model names that match filter text, as a list
            var result =
                _allNotes.Where(d => d.NoteTitle.ToLowerInvariant()
                .Contains(lowerCaseFilter))
                .ToList();

            //Get list of values in current filtered list that we want to remove
            //(ie. don't meet new filter criteria)
            var toRemove = NotesCollection.Except(result).ToList();

            //Loop to remove items that fail filter
            foreach (var x in toRemove)
            {
                NotesCollection.Remove(x);
            }

            var resultCount = result.Count;
            // Add back in correct order.
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > NotesCollection.Count || !NotesCollection[i].Equals(resultItem))
                {
                    NotesCollection.Insert(i, resultItem);
                }
            }
        }

        //Function to load notes from database
        public void LoadNotes()
        {
            //Check if there are any unwanted objects lingering
            if (_allNotes.Count != 0 || NotesCollection.Count != 0)
            {
                NotesCollection.Clear();
                _allNotes.Clear();
            }

            //Retrieve notes from db
            var databaseNotes = Repositories.DatabaseRepo.SelectNotes();

            //Add notes to collection/list
            foreach(var note in databaseNotes)
            {
                Note newNote = new Note(note.NoteTitle, note.NoteContent);

                _allNotes.Add(newNote);
                NotesCollection.Add(newNote);
            }
        }
    }
}
