using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp
{
    //Wrapper class for the Pokemon class the API wrapper includes.
    //This is jsut so I can get exactly the data I need in one object.
    //Also so I don't have to deal with async functions everywhere.
    //Also it lets me add more stuff if I ever want to do that. Modular and what not.
    internal class PokedexEntry
    {
        //Public Properties
        public Pokemon Pkmn { get => pkmn; private set => pkmn = value; }

        public int Id { get => id; private set => id = value; }
        public float Height { get => height; private set => height = value; }
        public float Weight { get => weight; private set => weight = value; }
        public Image? Sprite { get => sprite; set => sprite = value; }

        public Image? SecondarySprite { get => secondarySprite; set => secondarySprite = value; } //Defaults to the shiny sprite

        public string Name { get => name; private set => name = value; }

        public List<PokemonStat> Stats { get => stats; private set => stats = value; }

        //Private fields
        private Pokemon pkmn;

        private int id;
        private float height;
        private float weight;

        private Image? sprite;
        private Image? secondarySprite;

        private string name;

        private List<PokemonStat> stats;

        //Constructor
        public PokedexEntry(int id, Pokemon pkmn)
        {
            Pkmn = pkmn;
            Id = id;
            height = pkmn.Height;
            weight = pkmn.Weight;
            Stats = pkmn.Stats;
            Name = pkmn.Name;
            LoadSprites();
        }

        /*
         * This function lets me load the images independently from the rest of the pokemon.
         * This should further improve pokemon loading times.
         * This was a whole dilemma, but I think allowing this class to handle it itself is ideal instead of awaiting
         * it all as they get loaded. It miiight be worse for CPU usage but it also is a lot faster sooo I dunno.
         * If my horrible computer can handle it im sure its fine? But we'll see..
         */
        private async void LoadSprites()
        {
            //Default Sprite
            string imageUrl = Pkmn.Sprites.FrontDefault;

            using (var client = new HttpClient())
            {
                byte[] imageData = await client.GetByteArrayAsync(imageUrl);

                using (var stream = new MemoryStream(imageData))
                {
                    Sprite = Image.FromStream(stream);
                }
            }

            //Shiny sprite
            imageUrl = Pkmn.Sprites.FrontShiny;

            using (var client = new HttpClient())
            {
                byte[] imageData = await client.GetByteArrayAsync(imageUrl);

                using (var stream = new MemoryStream(imageData))
                {
                    SecondarySprite = Image.FromStream(stream);
                }
            }
        }

        //Swaps the main and secondary sprites
        public void ToggleSprite()
        {
            Image temp = Sprite;
            Sprite = SecondarySprite;
            SecondarySprite = temp;
        }
    }
}
