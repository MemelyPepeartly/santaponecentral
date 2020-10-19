using System;
using System.Collections.Generic;
using System.Text;

namespace Santa.Logic.Objects.Information_Objects
{
    /// <summary>
    /// Logic object for possible pairings an auto assignment call might return
    /// </summary>
    public class PossiblePairingChoices
    {
        public Client sendingAgent { get; set; }
        public List<Client> potentialAssignments { get; set; }
    }
}
