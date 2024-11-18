using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using PokeApiNet;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace WindowsPokedexApp.UI
{
    /*
     * This is a class to represent the little card that holds the
     * Pokemon name, sprite, and Pokedex number.
     */

    //I considered making an image in paint or something to use, but that's so lame.
    //This is my first time doing this so I am going to try to make it work out well.
    //I hardcode in a lot of stuff here to fit my current needs. I KNOW this is a bad practice.
    //Again though, since this is my first time and I don't currently have plans to expand this, 
    //in my opinion it'll suffice.
    internal class PokemonNameCard : UserControl
    {
        //Properties and Fields
        public PokedexEntry? Pokemon { get => pokemon; set => pokemon = value; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }

        private PokedexEntry? pokemon;

        private Color arrowGreen = Color.FromArgb(255, 104, 255, 0);
        private Color focusColor = Color.FromArgb(255, 0, 90, 0);
        private Color unFocusColor = Color.FromArgb(255, 20, 20, 20);
        private Color bgColor;
        private Color fontColor = Color.White;


        private bool isSelected = false; //Keeps track of if this card is selected

        private Font textFont = new Font("Segoe UI", 18);

        //Default constructor
        public PokemonNameCard(PokedexEntry? pokemon) { Pokemon = pokemon; this.DoubleBuffered = true; }

        //OnPaint function
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Get graphics from event handler
            Graphics g = e.Graphics;

            /*
             * Okay so like do NOT even get me started on this.
             * This shape is just a BORING rectangle. Who CARES
             * about a rectangle. I originally TRIED to use a fancier shape, but
             * as far as I can tell, I COULDNT make it transparent.
             * On top of that, I couldnt make this 
             * less opaque because the background is just WHITE.
             * I am SURE there MUST be a way around this. If I made the image outside of C#
             * and just imported it, this probably would be waaay easier so idk why I didn't do
             * that.
             */
            //Point array to define the shape of the card.
            Point[] shapePoints = {
                new Point(0, 0),
                new Point(this.Width, 0),
                new Point(this.Width, this.Height),
                new Point(0, this.Height)
            };
            
            //Fills background
            ColorBg(g, shapePoints);

            //Point Arrays for the green outlines
            Point[] leftArrow = {
                new Point(17, this.Height-7),
                new Point(7, this.Height-7),
                new Point(7, 7),
                new Point(17, 7)
            };

            Point[] rightArrow = {
                new Point(17 + this.Width/25 * this.Height/25, this.Height-7),
                new Point(27 + this.Width/25 * this.Height/25, this.Height-7),
                new Point(27 + this.Width/25 * this.Height/25, 7),
                new Point(17 + this.Width/25 * this.Height/25, 7)
            };

            //Draw arrows
            using (Pen pen = new Pen(arrowGreen, 5))
            {
                g.DrawLines(pen, leftArrow);
                g.DrawLines(pen, rightArrow);
            }

            //Only if pokemon is not null, draw the sprite and info.
            if (Pokemon != null)
            {
                //Place Pokemon images and stuff
                Rectangle imgRect = new Rectangle(
                    5,
                    -3,
                    this.Height * Pokemon.Sprite.Width / 80,
                    this.Height * Pokemon.Sprite.Width / 80
                );

                g.DrawImage(Pokemon.Sprite, imgRect);

                //Pokemon Info
                using (Brush brush = new SolidBrush(fontColor))
                {
                    g.DrawString(
                        Pokemon.Id + " " + Pokemon.Name.ToUpper(),
                        textFont,
                        brush,
                        new Point(this.Width / 5, this.Height / 4)
                    );
                }
            }
        }

        //Colors the background of the card
        private void ColorBg(Graphics g, Point[] points)
        {
            if (isSelected)
                bgColor = focusColor;
            else
                bgColor = unFocusColor;

            //Fill the card
            using (Brush brush = new SolidBrush(bgColor))
            {
                g.FillPolygon(brush, points);
            }
        }
    }
}
