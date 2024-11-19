using PokeApiNet;
using WindowsPokedexApp.UI;
using WindowsPokedexApp.UI.bg;

namespace WindowsPokedexApp
{
    /*
     * Main form that holds the UI
     */
    internal partial class MainForm : Form
    {
        //UI List to update if needed
        private List<Control> mainUI = new List<Control>(); //List of UI 

        //Logic, FormControll, and DataHandler instances to link everything together
        private Logic logic;
        private FormController controller;
        private DataHandler handler;

        //Instntiate UI Elements
        //I put this here so I could have a global reference to them.
        private Background bg = new Background();
        private BackgroundBox bgBox = new BackgroundBox(null);
        private BottomBar bar = new BottomBar();
        private LeftArrow la = new LeftArrow();
        private RightArrow ra = new RightArrow();
        private List<PokemonNameCard> pokemonNameCards = new List<PokemonNameCard>(); //List of just the Pokemon containers
        private PokemonSearch sb = new PokemonSearch();
        private TextBox infoBox = new TextBox();

        //Events
        public event EventHandler<PageChangeEventArgs> PageChanged;
        public event EventHandler InitialLoad; //Event for when UI gets loaded the first time to get everything in place

        //Constructor
        public MainForm()
        {
            InitializeComponent();
        }

        //Everything gets initialized when this gets loaded
        private async void MainForm_Load(object sender, EventArgs e)
        {
            /*
             * I initially included funcionality for resizing the windows, but lining things up was hard
             * and it was inefficient, so for the time being I am keeping this as a fixed window size.
             */
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            handler = new DataHandler();
            logic = new Logic();
            controller = new FormController(this, logic, handler);

            controller.WireEvents();
            logic.InitializeData(handler);
            buildUI();
        }

        //This is called when one of the arrows gets clicked
        private void Arrow_OnClick (object sender, EventArgs e)
        {
            if (sender is LeftArrow)
            {
                //Page change even for Logic update
                PageChanged.Invoke(this, new PageChangeEventArgs(-1));
            }

            if(sender is RightArrow)
            {
                PageChanged.Invoke(this, new PageChangeEventArgs(1));
            }
        }


        //This will scroll the Cards or whatever when the scroll wheel gets scrolled
        public async void OnMouseScroll(object sender, MouseEventArgs e)
        {
            if (e.Delta/2 > 0)
            {
                PageChanged.Invoke(this, new PageChangeEventArgs(1));
            }

            else if (e.Delta/2 < 0)
            {
                PageChanged.Invoke(this, new PageChangeEventArgs(-1));
            }
        }

        //Updates cards with updated pokemon list
        //If the list isn't updated, it only worries about updating which card is highlighted
        //Debatably I should have put these in separate methods, but since the functions would be
        //Essentially the same, I decided one was okay for now.
        public void UpdateNameCards(object? sender, UpdateDisplayedPokemonEventArgs e)
        {
            for (int i = 0; i < FormController.NUM_POKEMON_PER_PAGE; i++)
            {
                //Updates pokemon if pokemon list isn't null
                if(e.NewPokemon != null)
                    pokemonNameCards[i].Pokemon = e.NewPokemon[i];

                //If pokemon is not null AND is selected, then it gets marked as selected
                if (pokemonNameCards[i].Pokemon != null && pokemonNameCards[i].Pokemon.Id == e.CurrentSelectedPokemon)
                    pokemonNameCards[i].IsSelected = true;

                else
                    pokemonNameCards[i].IsSelected = false;

                pokemonNameCards[i].Invalidate();
            }
        }

        //Updates the part of the layout that gives info.
        //This is done when the cards get clicked on.
        public void UpdateInfoBox(object sender, UpdateSelectedPokemonEventArgs e)
        {
            bgBox.Entry = e.Pokemon;
            int totalStats = 0;

            foreach (PokemonStat stat in e.Pokemon.Stats)
            {
                totalStats += stat.BaseStat;
            }

            string[] lines =
            {
                "Height: " + e.Pokemon.Pkmn.Height/10.0 + "m",
                "Weight: " + e.Pokemon.Pkmn.Weight/10.0 + "kg",
                "",
                "HP: " + e.Pokemon.Pkmn.Stats[0].BaseStat,
                "Attack: " + e.Pokemon.Pkmn.Stats[1].BaseStat,
                "Defense: " + e.Pokemon.Pkmn.Stats[2].BaseStat,
                "Sp. Atk: " + e.Pokemon.Pkmn.Stats[3].BaseStat,
                "Sp. Def: " + e.Pokemon.Pkmn.Stats[4].BaseStat,
                "Speed: " + e.Pokemon.Pkmn.Stats[5].BaseStat,
                "Total: " + totalStats
            };

            infoBox.Lines = lines;

            bgBox.Invalidate();
            infoBox.Invalidate();
        }

        //Method that places all the UI elements for the first time
        private void buildUI()
        {
            //Adds scroll event to allow scrolling through entries
            this.MouseWheel += OnMouseScroll;

            //Add background
            bg.Location = new Point(0, 0);
            bg.Size = new Size(this.Width, this.Height);
            mainUI.Add(bg);
            Controls.Add(bg);
            bg.BringToFront();

            //Add other part of background idk
            bgBox.Location = new Point(0, this.Height / 7);
            bgBox.Size = new Size(this.Width, 3 * this.Height / 10);
            mainUI.Add(bgBox);
            Controls.Add(bgBox);
            bgBox.BringToFront();

            //Add bottom Bar
            bar.Location = new Point(0, 5 * this.Height / 6);
            bar.Size = new Size(this.Width, this.Height / 6);
            mainUI.Add(bar);
            Controls.Add(bar);
            bar.BringToFront();

            //Add nav arrows
            la.Location = new Point(this.Width / 2 + this.Height / 12, 5 * this.Height / 6);
            la.Size = new Size(this.Height / 12, this.Height / 12);
            la.Click += Arrow_OnClick;
            mainUI.Add(la);
            Controls.Add(la);
            la.BringToFront();

            ra.Location = new Point(this.Width - this.Height / 6, 5 * this.Height / 6);
            ra.Size = new Size(this.Height / 12, this.Height / 12);
            ra.Click += Arrow_OnClick;
            mainUI.Add(ra);
            Controls.Add(ra);
            ra.BringToFront();


            //Add Pokemon name cards
            for (int i = 0; i < 5; i++)
            {
                Point locationToPlace = new Point(this.Width / 2, this.Height / 10 + (i * this.Height / 8));
                Size size = new Size(this.Width / 2, this.Height / 10);
                PokemonNameCard newCard = new PokemonNameCard();

                if (i == 0)
                    newCard.IsSelected = true;

                newCard.Location = locationToPlace;
                newCard.Size = size;
                newCard.PokemonContainerClick += logic.PokemonNameCard_OnClick;

                pokemonNameCards.Add(newCard);
                mainUI.Add(newCard);
                Controls.Add(newCard);

                newCard.BringToFront();
            }

            //Search Bar
            sb.Font = new Font("Impact", this.Height / 20, FontStyle.Bold);
            sb.FontColor = Color.White;
            sb.Location = new Point(0, 5 * this.Height / 6);
            sb.Size = new Size(this.Width / 3, this.Height / 6);
            sb.UpdateInput += logic.SearchBar_UpdateInput;
            mainUI.Add(sb);
            Controls.Add(sb);
            sb.BringToFront();

            //Add box to display pokemon info
            //I really wish I could make this look Better. But alas, for now, this will do
            infoBox.BackColor = Color.FromArgb(255, 221, 19, 5);
            infoBox.Font = new Font("Impact", 18);
            infoBox.ForeColor = Color.White;
            infoBox.Multiline = true;
            infoBox.Location = new Point(this.Width/30, this.Height / 2);
            infoBox.Size = new Size(8 * this.Width / 20, this.Height / 4);
            infoBox.ReadOnly = true;
            infoBox.ScrollBars = ScrollBars.Vertical;

            mainUI.Add(infoBox);
            Controls.Add(infoBox);
            infoBox.BringToFront();

            PageChanged.Invoke(this, new PageChangeEventArgs(0)); //Sets the information and sprite
            InitialLoad.Invoke(this, new EventArgs()); //Initial load event
        }
    }
}