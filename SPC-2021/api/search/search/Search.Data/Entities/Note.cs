using System;
using System.Collections.Generic;

#nullable disable

namespace Search.Data.Entities
{
    public partial class Note
    {
        public Guid NoteId { get; set; }
        public Guid ClientId { get; set; }
        public string NoteSubject { get; set; }
        public string NoteContents { get; set; }

        public virtual Client Client { get; set; }
    }
}
