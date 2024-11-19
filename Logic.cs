using PokeApiNet;
using System.Collections;
using System.Collections.Generic;
using WindowsPokedexApp.Events;

namespace WindowsPokedexApp
{
    /*
     * This will hold some functions to change stuff when
     * certain events fire.
     */
    internal class Logic
    {
        //Pokemon Data Structures
        private List<PokedexEntry> pokemonList = new List<PokedexEntry>(); //List of ALL pokemon
        private List<PokedexEntry> matchingPokemon = new List<PokedexEntry>(); //List of matching pokemon

        private Dictionary<string, PokedexEntry> pokemonMap = new Dictionary<string, PokedexEntry>();

        //Private logic values
        private int pageNum = 1; //Page number to allow changing pages
        private int currentSelected = 0; //Keeps track of what pokemon is clicked on


        //Events
        public event EventHandler<UpdateDisplayedPokemonEventArgs> UpdateDisplayedPokemon; //Update list of currently displayed pokemon
        public event EventHandler<UpdateSelectedPokemonEventArgs>? UpdateSelectedPokemon; //Update currently selected pokemon
        
        //Initializes pokemon data structures and gets information from API
        public void InitializeData(DataHandler dataHandler)
        {
            //I am now doing this on a separate thread and no longer awaiting it
            //This makes it every so slightly more buggy, but it doesn't take time to load anymore
            //And it doesn't get in the way of other control stuff.
            Task.Run(() =>
            {
                dataHandler.GetPokedexEntries();
            });
        }

        //Called on initial load. Invokes whatever is needed for initial state
        public void MainForm_OnInitialLoad(object sender, EventArgs e)
        {
            MainForm_OnPageChange(sender, new PageChangeEventArgs(0));
        }

        //Called when page is changed
        public void MainForm_OnPageChange(object sender, PageChangeEventArgs e)
        {
            //Changes page number depending on which direction it gets changed
            //It is 0 if the page isn't changed but page info needs to be updated
            if (e.PageChangeDirection > 0)
            {
                if (++pageNum > ((matchingPokemon.Count - 1) / FormController.NUM_POKEMON_PER_PAGE) + 1)
                    pageNum = 1;
            }

            if (e.PageChangeDirection < 0)
            {
                if (--pageNum <= 0)
                    pageNum = ((matchingPokemon.Count - 1) / FormController.NUM_POKEMON_PER_PAGE) + 1;
            }

            UpdateDisplayedPokemon.Invoke(this, new UpdateDisplayedPokemonEventArgs(GetUpdatedPokemonList(), currentSelected));
        }

        //When the name card gets clicked, this updates the information and passes it back to the UI
        public void PokemonNameCard_OnClick(object? sender, UpdateSelectedPokemonEventArgs? e)
        {
            //Return if pokemon is null
            if (e.Pokemon == null)
                return;

            //If pokemon is already selected, swap sprites
            //I'm not sure if this belongs in this class but it's suuuch an easy thing to implement so idk.
            if (e.Pokemon.Id == currentSelected)
                e.Pokemon.ToggleSprite();

            currentSelected = e.Pokemon.Id;

            UpdateSelectedPokemon!.Invoke(this, new UpdateSelectedPokemonEventArgs(e.Pokemon));
            UpdateDisplayedPokemon.Invoke(this, new UpdateDisplayedPokemonEventArgs(null, currentSelected));
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

            UpdateDisplayedPokemon.Invoke(this, new UpdateDisplayedPokemonEventArgs(GetUpdatedPokemonList(), currentSelected));
        }

        //This gets called whenever the list gets updated as it loads
        //This is so the logic can know what has been loaded and what information the UI needs
        /*
         * There is currently a bug where, since the list of pokemon are decided by the pokemon that match the current
         * query, and pokemon get added to that list as they are loaded, if you try to search for a pokemon, the results
         * will be invalidated by the pokemon continuing to get loaded in. I did some research on storing the linq query as
         * a global reference, and at least for the moment, I'm choosing to not look into that too much cuz it's a bit complicated.
         * So for now, the search feature is going to be a bit buggy as things get loaded in.
         * This is something I would like to fix sometime. The tradeoff is that it no longer takes 10-20 seconds to load
         * for the first time and loads the pokemon in the background. In general, I'd like to make this a bit more seamless,
         * but I think the tradeoff is worth it for now.
         */
        public void PokemonList_OnUpdate(object sender, ListUpdatedEventArgs e)
        {
            matchingPokemon.Add(e.Pokemon);
            pokemonList.Add(e.Pokemon);
            pokemonMap.Add(e.Pokemon.Name, e.Pokemon);

            UpdateDisplayedPokemon.Invoke(this, new UpdateDisplayedPokemonEventArgs(GetUpdatedPokemonList(), currentSelected));
        }

        //Gets updated list of Pokemon to be displayed.
        //Called whenever the page changes or query gets linq'ed
        private List<PokedexEntry?> GetUpdatedPokemonList()
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
