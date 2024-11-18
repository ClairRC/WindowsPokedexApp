using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp
{
    //Delegate to pass updated info to UI class from logic when page is updated
    internal class PageInfoUpdateEventArgs : EventArgs
    {
        //Properties
        public List<PokedexEntry?>? NewPokemon { get; private set; } //New pokemon to be displayed
        public int CurrentSelectedPokemon { get; private set; } //Currently selected pokemon

        //Constructor
        public PageInfoUpdateEventArgs(List<PokedexEntry?> newPokemon, int currentSelectedPokemon)
        {
            NewPokemon = newPokemon;
            CurrentSelectedPokemon = currentSelectedPokemon;
        }
    }
}
