using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using WindowsPokedexApp.UI;
using WindowsPokedexApp.UI.bg;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WindowsPokedexApp
{
    /*
     * This will hold some functions to change stuff when
     * certain events fire.
     */
    internal class Logic
    {
        //Private fields
        private List<PokedexEntry> pokemonList = new List<PokedexEntry>(); //List of ALL pokemon
        private List<PokedexEntry> matchingPokemon = new List<PokedexEntry>(); //List of matching pokemon

        private Dictionary<string, PokedexEntry> pokemonMap = new Dictionary<string, PokedexEntry>();

        private PokeApiClient client = new PokeApiClient();

        private int pageNum = 1; //Page number to allow changing pages
        private int currentSelected = 0; //Keeps track of what pokemon is clicked on

        //Event handling stuff
        public event EventHandler<PageInfoUpdateEventArgs> PageInfoUpdate;
        public event EventHandler<PokedexEntryEventArgs>? OnNewSelectedPokemon;

        public async Task InitializeData()
        {
            await DataHandler.GetPokedexEntries(client, pokemonList, pokemonMap);

            for (int i = 0; i < pokemonList.Count; i++)
            {
                //Matchin pokemon defaults to everything.
                matchingPokemon.Add(pokemonList[i]);
            }
        }

        //Called on initial load. Invokes whatever is needed for initial state
        public void OnInitialLoad(object sender, EventArgs e)
        {
            OnPageChange(sender, new PageChangeEventArgs(0));
            PokemonNameCard_OnClick(sender, new PokedexEntryEventArgs(pokemonList[0]));
        }

        //Called when page is changed
        public void OnPageChange(object sender, PageChangeEventArgs e)
        {
            //Changes page number depending on which direction it gets changed
            //It is 0 if the page isn't changed but page info needs to be updated
            if (e.PageChangeDirection > 0)
            {
                if (++pageNum >= (matchingPokemon.Count / 5) + 2)
                    pageNum = 1;
            }

            if (e.PageChangeDirection < 0)
            {
                if (--pageNum <= 0)
                    pageNum = (matchingPokemon.Count / 5) + 1;
            }

            PageInfoUpdate.Invoke(this, new PageInfoUpdateEventArgs(GetUpdatedPokemon(), currentSelected));
        }

        //When the name card gets clicked, this updates the information and passes it back to the UI
        public void PokemonNameCard_OnClick(object? sender, PokedexEntryEventArgs? e)
        {
            if (e.Pokemon == null || e.Pokemon.Id == currentSelected)
                return;

            currentSelected = e.Pokemon.Id;

            OnNewSelectedPokemon!.Invoke(this, new PokedexEntryEventArgs(e.Pokemon));
            PageInfoUpdate.Invoke(this, new PageInfoUpdateEventArgs(null, currentSelected));
        }

        //This gets the string from the serach bar and executes the linq query
        public void SearchBar_UpdateInput(string input)
        {
            var lq =
                from name in pokemonMap.Keys
                where name.StartsWith(input.ToLower())
                select pokemonMap[name];

            matchingPokemon = lq.ToList();
            pageNum = 1;

            PageInfoUpdate.Invoke(this, new PageInfoUpdateEventArgs(GetUpdatedPokemon(), currentSelected));
        }

        //Gets updated list of Pokemon to be displayed.
        //Called whenever the page changes or query gets linq'ed
        private List<PokedexEntry?> GetUpdatedPokemon()
        {
            //List of pokemon to pass to the UI
            List<PokedexEntry?> pokemonToDisplay = new List<PokedexEntry?>();

            for (int i = 0; i < FormController.NUM_POKEMON_PER_PAGE; i++)
            {
                int indexToAdd = i + (FormController.NUM_POKEMON_PER_PAGE * (pageNum - 1));

                if (indexToAdd >= matchingPokemon.Count)
                    pokemonToDisplay.Add(null);

                else
                    pokemonToDisplay.Add(matchingPokemon[indexToAdd]);
            }

            return pokemonToDisplay;
        }
    }
}
