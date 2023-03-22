using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LocalNote_Assign2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ViewModels.NotesViewModel NotesViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            
            //Instantiate NotesViewModel
            this.NotesViewModel = new ViewModels.NotesViewModel();
        }

        //Event handler for edited notes
        private void NoteContentBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotesViewModel.SelectedNoteContent = NoteContentBox.Text;
        }

        //Event handler for about button click
        private void About_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }
    }
}
