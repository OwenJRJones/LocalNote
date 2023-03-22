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
    public class DeleteCommand : ICommand
    {
        private ViewModels.NotesViewModel _nvm;

        //Constructor
        public DeleteCommand(ViewModels.NotesViewModel NVM)
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
            if(_nvm.SelectedNote != null)
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
            //Instantiate new delete dialog and run asynchronously
            DeleteNoteDialog deleteNoteDialog = new DeleteNoteDialog();
            ContentDialogResult result = await deleteNoteDialog.ShowAsync();

            try
            {
                if (result == ContentDialogResult.Primary)
                {
                    Repositories.DatabaseRepo.DeleteNote(_nvm.SelectedNoteTitle);

                    _nvm.LoadNotes();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("A confirmation error occurred: " + ex.Message);
            }
        }
    }
}
