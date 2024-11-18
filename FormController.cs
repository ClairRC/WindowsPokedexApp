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

        //UI and Logic classes
        private Logic logic;
        private MainForm mainForm;

        public FormController(MainForm mainForm, Logic logic) 
        {
            this.mainForm = mainForm;
            this.logic = logic;
        }

        //Links events in th UI class to logic and vice versa
        public void WireEvents()
        {
            mainForm.PageChanged += logic.OnPageChange!;
            logic.PageInfoUpdate += mainForm.UpdateNameCards!;
            logic.OnNewSelectedPokemon += mainForm.UpdateInfoBox;
            mainForm.InitialLoad += logic.OnInitialLoad;
        }
    }
}
