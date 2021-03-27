using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Note_Models
{
    public class EditNoteContentsModel
    {
        public string noteSubject { get; set; }
        public string noteContents { get; set; }
    }
}
