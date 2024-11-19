using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp
{
    /*
     * Controller class that handlers passing information
     * between main form class and logic class.
     */
    internal class FormController
    {
        //Number of pokemon per page. I've put it here because both the form and logic need it
        public const int NUM_POKEMON_PER_PAGE = 5;

        //UI, Logic and DataHandler instances to wire everything together
        private Logic logic;
        private MainForm mainForm;
        private DataHandler handler;

        //Constructor
        //This links the main form and the logic classes so that events can be wired
        public FormController(MainForm mainForm, Logic logic, DataHandler handler) 
        {
            this.mainForm = mainForm;
            this.logic = logic;
            this.handler = handler;
        }

        //Links events in th UI class to logic and vice versa
        public void WireEvents()
        {
            mainForm.PageChanged += logic.MainForm_OnPageChange!;
            logic.UpdateDisplayedPokemon += mainForm.UpdateNameCards!;
            logic.UpdateSelectedPokemon += mainForm.UpdateInfoBox;
            mainForm.InitialLoad += logic.MainForm_OnInitialLoad;
            handler.ListUpdated += logic.PokemonList_OnUpdate;
        }
    }
}
