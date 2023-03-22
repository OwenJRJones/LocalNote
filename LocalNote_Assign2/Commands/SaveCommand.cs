using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace LocalNote_Assign2.Commands
{
    public class SaveCommand : ICommand
    {
        private ViewModels.NotesViewModel _nvm;

        //Constructor
        public SaveCommand(ViewModels.NotesViewModel NVM)
        {
            this._nvm = NVM;
        }

        public event EventHandler CanExecuteChanged;

        public void Check_CanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            if (_nvm.SelectedNote == null && _nvm.IsReadOnly == false)
            {
                return true;
            }
            else if (_nvm.SelectedNote != null && _nvm.IsReadOnly == true)
            {
                return false;
            }
            else if (_nvm.SelectedNote != null && _nvm.IsReadOnly == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void Execute(object parameter)
        {
            //Check if existing note was edited
            if (_nvm.SelectedNoteTitle != null)
            {
                try
                {
                    //Save the note
                    Repositories.DatabaseRepo.UpdateNote(_nvm.SelectedNoteTitle, _nvm.SelectedNoteContent);

                    //Show save confirmation
                    ContentDialog savedDialog = new ContentDialog()
                    {
                        Content = "Note saved successfully.",
                        Title = "Save Successful",
                        PrimaryButtonText = "OK"
                    };

                    await savedDialog.ShowAsync();

                    //Reload notes
                    _nvm.LoadNotes();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("File save error occurred: " + ex.Message);
                }
            }
            else
            {
                //Instantiate new save dialog and run asynchronously
                SaveNoteDialog saveNoteDialog = new SaveNoteDialog();
                ContentDialogResult result = await saveNoteDialog.ShowAsync();

                //Run code depending on users input (Save/Cancel)
                if (result == ContentDialogResult.Primary)
                {  
                    //Check if new note title is valid
                    if (saveNoteDialog.NewNoteTitle == null || saveNoteDialog.NewNoteTitle == "")
                    {
                        ContentDialog enterNameDialog = new ContentDialog()
                        {
                            Content = "Please provide a title for your note.",
                            Title = "Note Ttitle Empty",
                            PrimaryButtonText = "OK"
                        };

                        await enterNameDialog.ShowAsync();
                    }
                    else
                    {
                        try
                        {
                            //Check if file name already exists
                            bool fileExists = false;

                            //Loop through files in the folder
                            foreach (var note in _nvm.NotesCollection)
                            {
                                if (note.NoteTitle == saveNoteDialog.NewNoteTitle)
                                {
                                    fileExists = true;
                                    break;
                                }
                                else
                                {
                                    fileExists = false;
                                }
                            }

                            //If file exists, show name taken dialog, otherwise save note
                            if (fileExists == true)
                            {
                                //Show name taken dialog
                                ContentDialog nameTakenDialog = new ContentDialog()
                                {
                                    Content = "Please choose a different file name.",
                                    Title = "Note Already Exists",
                                    PrimaryButtonText = "OK"
                                };

                                await nameTakenDialog.ShowAsync();
                            }
                            else
                            {
                                //Save the note
                                Repositories.DatabaseRepo.InsertNote(saveNoteDialog.NewNoteTitle, _nvm.SelectedNoteContent);

                                //Show save confirmation
                                ContentDialog savedDialog = new ContentDialog()
                                {
                                    Content = "Note saved successfully.",
                                    Title = "Save Successful",
                                    PrimaryButtonText = "OK"
                                };

                                await savedDialog.ShowAsync();

                                //Reload notes
                                _nvm.LoadNotes();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("File save error occurred: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}
