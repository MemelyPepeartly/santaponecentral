using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Logic.Objects.Information_Objects
{
    /// <summary>
    /// Logic object for possible pairings an auto assignment call might return
    /// </summary>
    public class PossiblePairingChoices
    {
        public HQClient sendingAgent { get; set; }
        public List<HQClient> potentialAssignments { get; set; }
    }
}
