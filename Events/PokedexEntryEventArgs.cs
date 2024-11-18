using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp
{
    //Event args that is used when the Pokemon container gets clicked on.
    internal class PokedexEntryEventArgs : EventArgs
    {
        //Properties
        public PokedexEntry? Pokemon {  get; private set; } //Pokemon that the card holds

        public PokedexEntryEventArgs(PokedexEntry? pokemon)
        {
            Pokemon = pokemon;
        }
    }
}
