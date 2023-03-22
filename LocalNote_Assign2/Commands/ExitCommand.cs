using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;

namespace LocalNote_Assign2.Commands
{
    public class ExitCommand : ICommand
    {
        private ViewModels.NotesViewModel _nvm;

        //Constructor
        public ExitCommand(ViewModels.NotesViewModel NVM)
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
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                CoreApplication.Exit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("application close failed: " + ex.Message);
            }
        }
    }
}
