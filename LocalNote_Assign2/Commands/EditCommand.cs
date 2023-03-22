using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LocalNote_Assign2.Commands
{
    public class EditCommand : ICommand
    {
        private ViewModels.NotesViewModel _nvm;

        //Constructor
        public EditCommand(ViewModels.NotesViewModel NVM)
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
            if (_nvm.SelectedNote != null && _nvm.IsReadOnly == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            _nvm.IsReadOnly = false;
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            _nvm.SaveCommand.Check_CanExecuteChanged();
        }
    }
}
