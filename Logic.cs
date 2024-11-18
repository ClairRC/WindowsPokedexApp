using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using WindowsPokedexApp.UI;
using WindowsPokedexApp.UI.bg;

namespace WindowsPokedexApp
{
    /*
     * This will hold some functions to change stuff when
     * certain events fire.
     */
    internal class Logic
    {
        //Updates cards with the given information.
        public static void UpdateNameCards(List<PokedexEntry> matchingPokemon, List<PokemonNameCard> cards, int pageNum, int currCard)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (i + (cards.Count * (pageNum - 1)) >= matchingPokemon.Count)
                {
                    cards[i].Pokemon = null;
                    cards[i].IsSelected = false;
                }

                else
                {
                    cards[i].Pokemon = matchingPokemon[i + (cards.Count * (pageNum - 1))];
                    if (cards[i].Pokemon.Id != currCard)
                        cards[i].IsSelected = false;
                    else
                        cards[i].IsSelected = true;
                }

                cards[i].Invalidate();
            }
        }

        //Updates the part of the layout that gives info.
        //This is done when the cards get clicked on.
        public static void UpdateInfoBox(PokedexEntry entry, BackgroundBox bgBox, TextBox textBox)
        {
            bgBox.Entry = entry;
            int totalStats = 0;

            foreach (PokemonStat stat in entry.Stats)
            {
                totalStats += stat.BaseStat;
            }

            string[] lines =
            {
                "Height: " + entry.Pkmn.Height/10.0 + "m",
                "Weight: " + entry.Pkmn.Weight/10.0 + "kg",
                "",
                "HP: " + entry.Pkmn.Stats[0].BaseStat,
                "Attack: " + entry.Pkmn.Stats[1].BaseStat,
                "Defense: " + entry.Pkmn.Stats[2].BaseStat,
                "Sp. Atk: " + entry.Pkmn.Stats[3].BaseStat,
                "Sp. Def: " + entry.Pkmn.Stats[4].BaseStat,
                "Speed: " + entry.Pkmn.Stats[5].BaseStat,
                "Total: " + totalStats
            };

            textBox.Lines = lines;

            bgBox.Invalidate();
            textBox.Invalidate();
        }
    }
}
