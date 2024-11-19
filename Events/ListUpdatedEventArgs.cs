using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp.Events
{
    //EventArg that is for whenever the pokemonList gets a new entry
    //Just holds the entry that has been added
    internal class ListUpdatedEventArgs : EventArgs
    {
        public PokedexEntry Pokemon;

        public ListUpdatedEventArgs(PokedexEntry updatedEntry) 
        { 
            this.Pokemon = updatedEntry;
        }
    }
}
