using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApiNet;

namespace WindowsPokedexApp
{
    /*
     * This class is just responsible for grabbing everything and putting it into
     * whatever data structure is required
     */
    internal class DataHandler
    {
        /*
         * Okay so I dunno how much you know about pokemon but this is not all of them
         * and there are a couple of reasons for that.
         * 
         * 1.) After this, they moved from using sprites to models, 
         * and I am no artist, but I know that fanmade sprites of those DO exist.
         * But I couldn't really find a place that I could get them all from without
         * downloading them together, only individually, and making a Python script to download 
         * them in MY opinion is out of the scope of this project
         * 
         * 2.) The UI I made is based on the Pokedex from these games
         * anyway, so it matches.
         * 
         * 3.) Even if I COULD download all the other ones online, even though
         * they are explicity free to use, I don't wanna use anything unofficial.
         * 
         * 4.) Without a database, I'd have to load in the thousands of sprites, which is just. Slow.
         * 
         * We can just pretend we're back in 2010
         */

        /*
         * Okay so the most annoying thing here is the SPRITES. It takes a while to do API calls
         * and to load in Sprites (Which I've included locally, just for variety I guess).
         * And so My Little Trick to get around this is to create all the tasks then wait for them.
         * I will be honest I did NOT know how async functions worked at all before starting this.
         * But I THINK this is probably how they're meant to be used. Instead of loading them one by one,
         * this class will start loading them at essentially the same time and load after they all finish.
         * Before using this solution, it took like. Forever to load.
         * It still takes awhile though, don't get me wrong. In hindsight I should probably
         * have used an external database
         */
        //Number of Pokemon being included
        const int NUM_POKEMON = 649;
        const int BATCH_SIZE = 40; //Loading in batches to save CPU usage and (hopefully) reduce load time

        //This will create a list of all Pokemon for the main program to use.
        public static async Task GetPokedexEntries(PokeApiClient client, List<PokedexEntry> list, Dictionary<string, PokedexEntry> map)
        {

            int i = 1;
            while(i <= NUM_POKEMON)
            {
                List<Task<Pokemon>> tasks = new List<Task<Pokemon>>();
                List<Task<Image>> sprites = new List<Task<Image>>();

                Image[] spritesList = new Image[BATCH_SIZE];
                Pokemon[] pokemonList = new Pokemon[BATCH_SIZE];

                Task<Pokemon> pokemon;
                Task<Image> sprite;

                for (int j = 0; j < BATCH_SIZE; j++)
                {
                    if (i > NUM_POKEMON)
                        break;

                    pokemon = client.GetResourceAsync<Pokemon>(i);
                    sprite = LoadSprites(i);

                    tasks.Add(pokemon);
                    sprites.Add(sprite);
                    
                    i++;
                }

                spritesList = await Task.WhenAll(sprites);
                pokemonList = await Task.WhenAll(tasks);

                for (int k = 0; k < pokemonList.Length; k++)
                {
                    list.Add(new PokedexEntry(pokemonList[k].Id, pokemonList[k], spritesList[k]));
                    map.Add(pokemonList[k].Name, new PokedexEntry(pokemonList[k].Id, pokemonList[k], spritesList[k]));
                }
            }
        }

        private static async Task<Image> LoadSprites(int id)
        {
            Image sprite = Image.FromFile("Sprites/ns/" + id + ".png");
            
            return sprite;
        }
    }
}
