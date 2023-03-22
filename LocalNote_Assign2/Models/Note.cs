using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalNote_Assign2.Models
{
    public class Note
    {
        public string NoteTitle { get; }
        public string NoteContent { get; }

        //Constructor
        public Note(string title, string content)
        {
            NoteTitle = title;
            NoteContent = content;
        }
    }
}
