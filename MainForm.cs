using PokeApiNet;
using WindowsPokedexApp.UI;
using WindowsPokedexApp.UI.bg;

namespace WindowsPokedexApp
{
    /*
     * Main form that holds the UI
     * I've decided to split the UI elements and the event handling in different classes.
     * I felt this class was becoming too spaghetti (as well as the rest of the project..)
     */
    public partial class MainForm : Form
    {
        //Fields
        private List<Control> mainUI = new List<Control>(); //List of UI elements
        private List<PokedexEntry> pokemonList = new List<PokedexEntry>(); //List of ALL pokemon
        private List<PokedexEntry> matchingPokemon = new List<PokedexEntry>(); //List of matching pokemon

        private Dictionary<string, PokedexEntry> pokemonMap = new Dictionary<string, PokedexEntry>();

        private PokeApiClient client = new PokeApiClient();

        private int pageNum = 1; //Page number to allow changing pages
        private int currentSelected = 1; //Keeps track of what pokemon is clicked on

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

        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            /*
             * I do have logic for resizing, but it does require the UI to be
             * completely rebuilt currently, and lining it all up was quite
             * annoying, so for now I am keeping this unable to be resized, but I may
             * make this better in the future.
             */
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            await DataHandler.GetPokedexEntries(client, pokemonList, pokemonMap);

            for (int i = 0; i < pokemonList.Count; i++)
            {
                //Matchin pokemon defaults to everything.
                matchingPokemon.Add(pokemonList[i]);
            }
            buildUI();
        }

        //This is called when the window gets resized
        private void MainForm_Resize(object sender, EventArgs e)
        {
            rebuildUI();
        }

        //This is called when one of the arrows gets clicked
        private void Arrow_OnClick (object sender, EventArgs e)
        {
            if (sender is LeftArrow)
            {
                //Loops pageNum if needed
                if (--pageNum <= 0)
                    pageNum = (matchingPokemon.Count / 5) + 1;

                Logic.UpdateNameCards(matchingPokemon, pokemonNameCards, pageNum, currentSelected);
            }

            if(sender is RightArrow)
            {
                if (++pageNum >= (matchingPokemon.Count / 5) + 2)
                    pageNum = 1;

                Logic.UpdateNameCards(matchingPokemon, pokemonNameCards, pageNum, currentSelected);
            }
        }

        //This is called when the card is clicked on
        private void PokemonNameCard_OnClick (object sender, EventArgs e)
        {
            //Return if the object clicked is already selected
            if (((PokemonNameCard)sender).IsSelected || ((PokemonNameCard)sender).Pokemon == null) 
            {
                return;
            }
            else
            {
                currentSelected = ((PokemonNameCard)sender).Pokemon.Id;
                Logic.UpdateInfoBox(pokemonList[currentSelected-1], bgBox, infoBox); //Needs pokemon and Controls to be updated
                Logic.UpdateNameCards(matchingPokemon, pokemonNameCards, pageNum, currentSelected);
            }
        }

        //This gets the string from the serach bar and executes the linq query
        private void SearchBar_UpdateInput(string input)
        {
            var lq =
                from name in pokemonMap.Keys
                where name.StartsWith(input.ToLower())
                select pokemonMap[name];

            matchingPokemon = lq.ToList();
            pageNum = 1;
            Logic.UpdateNameCards(matchingPokemon, pokemonNameCards, pageNum, currentSelected);
        }

        //This will scroll the Cards or whatever when the scroll wheel gets scrolled
        private void OnScroll(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (++pageNum >= (matchingPokemon.Count / 5) + 2)
                    pageNum = 1;

                Logic.UpdateNameCards(matchingPokemon, pokemonNameCards, pageNum, currentSelected);
            }

            else if (e.Delta < 0)
            {
                if (--pageNum <= 0)
                    pageNum = (matchingPokemon.Count / 5) + 1;

                Logic.UpdateNameCards(matchingPokemon, pokemonNameCards, pageNum, currentSelected);
            }
        }

        //Method that places all the UI elements for the first time
        private void buildUI()
        {
            //Adds scroll event to allow scrolling through entries
            this.MouseWheel += OnScroll;

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
                PokemonNameCard newCard = new PokemonNameCard(pokemonList[i]);

                if (i == 0)
                    newCard.IsSelected = true;

                newCard.Location = locationToPlace;
                newCard.Size = size;
                newCard.Click += PokemonNameCard_OnClick;

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
            sb.UpdateInput += SearchBar_UpdateInput;
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

            Logic.UpdateInfoBox(pokemonList[0], bgBox, infoBox); //Sets the information and sprite
        }

        //Rebuilds UI when window is resized

        /*
         * Okay so like off the record I know this is a really bad way to do this, because whenever the 
         * window is resized, it removes the control and then recreates it.
         * I tried OH HOW I TRIED to do this better. I created a wrapper class to hold the size and width,
         * but then the problem became updating THAT, and I tried using delegates to pass in a function to do that,
         * and that ALMOST worked, but when I added stuff in that for loop, they would all be put into the same
         * location when it got resized. I am SURE that there is a way to do this. HOWEVER there are not very many UI
         * elements that need to be updated. Currently I am
         * not going to worry about this since I am not allowing for it to
         * be resized normally, so FOR NOW I am going to keep it this way. In the 
         * future I will attempt to make this better if i make this app resizeable.
         * I am going to keep it here though just in case.
         */
        private void rebuildUI()
        {
            foreach (Control control in mainUI)
            {
                Controls.Remove(control);
            }

            mainUI.Clear();
            pokemonNameCards.Clear();
            buildUI();
        }
    }
}