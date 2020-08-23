using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.Models.Board_Entry_Models
{
    /// <summary>
    /// Model for posting new board entries
    /// </summary>
    public class NewBoardEntryModel
    {
        public Guid entryTypeID { get; set; }
        public int threadNumber { get; set; }
        public int postNumber { get; set; }
        public string postDescription { get; set; }
    }
}
