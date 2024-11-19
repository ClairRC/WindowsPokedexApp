using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PokeApiNet;
using WindowsPokedexApp.Events;

namespace WindowsPokedexApp
{
    /*
     * This class is just responsible for grabbing the pokemon from the API
     */
    internal class DataHandler
    {

        /*
         * I changed this a lot so that now instead of taking in the lists and adding to it, it loads the pokemon and then
         * fires an event to communicate which pokemon it is.
         * I've also changed the sprites from being loaded locally to being loaded asyncronously from the API in the
         * PokedexEntryClass
         * I am still doing this in batches because of the load time, although the loadtime issues are now mostly fixed.
         * (Although this does introduce a liiittle extra bugginess but the i
         */
        //Number of Pokemon being included (All of them, currently)
        const int NUM_POKEMON = 1025;
        const int BATCH_SIZE = 40; //Loading in batches to save CPU usage and (hopefully) reduce load time

        //API client
        //I decided this should live here because this is the class that is responsible for API calls
        private PokeApiClient client = new PokeApiClient();

        //Event
        public event EventHandler<ListUpdatedEventArgs> ListUpdated; //Gets fired when pokemon list gets updated so Logic class can update UI

        //This will create a list of all Pokemon for the main program to use.
        public async Task GetPokedexEntries()
        {

            int i = 1;
            while(i <= NUM_POKEMON)
            {
                List<Task<Pokemon>> tasks = new List<Task<Pokemon>>();

                Task<Pokemon> pokemon;

                for (int j = 0; j < BATCH_SIZE; j++)
                {
                    if (i > NUM_POKEMON)
                        break;

                    pokemon = client.GetResourceAsync<Pokemon>(i);

                    tasks.Add(pokemon);
                    
                    i++;
                }

                for (int k = 0; k < BATCH_SIZE; k++)
                {
                    Pokemon newPokemon = await tasks[k];

                    ListUpdated.Invoke(this, new ListUpdatedEventArgs(new PokedexEntry(newPokemon.Id, newPokemon)));
                }
            }
        }
    }
}
