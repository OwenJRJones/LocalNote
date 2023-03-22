using LocalNote_Assign2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LocalNote_Assign2.Repositories
{
    //Manage file I/O
    public class NoteRepo
    {

        private ViewModels.NotesViewModel _nvm;

        //Constructor
        public NoteRepo(ViewModels.NotesViewModel NVM)
        {
            this._nvm = NVM;
        }

        //Declare folder location
        private static StorageFolder _notesFolder = ApplicationData.Current.LocalFolder;

        //File save function
        public async static void SaveNoteToFile(String noteTitle, String noteContent)
        {
            //File name
            String fileName = noteTitle;
           
            //File content
            String content = noteContent;

            //Save the file
            try
            {
                StorageFile noteFile = await _notesFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(noteFile, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("File save error occurred: " + ex.Message);
            }
        }

        //File delete function
        public async static void DeleteNoteFromFile(String noteTitle)
        {
            //File name
            String fileName = noteTitle;

            //Delete the file
            try
            {
                StorageFile storageFile = await _notesFolder.GetFileAsync(fileName);
                await storageFile.DeleteAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("File deletion error occurred: " + ex.Message);
            }
        }
    }
}
