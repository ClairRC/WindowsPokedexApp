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
        public Image Sprite { get => sprite; private set => sprite = value; }
        
        public string Name { get => name; private set => name = value; }

        public List<PokemonStat> Stats { get => stats; private set => stats = value; }

        //Private fields
        private Pokemon pkmn;

        private int id;
        private float height;
        private float weight;

        private Image sprite;

        private string name;

        private List<PokemonStat> stats;

        public PokedexEntry(int id, Pokemon pkmn, Image sprite)
        {
            Pkmn = pkmn;
            Id = id;
            height = pkmn.Height;
            weight = pkmn.Weight;
            Stats = pkmn.Stats;
            Name = pkmn.Name;
            Sprite = sprite;
        }
    }
}
