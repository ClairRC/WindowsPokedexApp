using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp.UI.bg
{
    
    /*
     * This class is for basically the area to place the Pokemon sprite and provide variety.
     * CURRENTLY this background is what holds the image of the pokemon to be drawn in the back.
     * This is really bad, and I am aware of it. I am going to keep it as is for now, but this is definitely 
     * next on the chopping block for things to fix.
     */
    internal class BackgroundBox : UserControl
    {
        //Pokedex Entry to get Sprite info.
        public PokedexEntry Entry { get; set; }

        //Colors for this element
        private Color backBox = Color.FromArgb(255, 255, 155, 7);
        private Color frontBox = Color.FromArgb(255, 72, 74, 69);

        //Default constructor
        public BackgroundBox(PokedexEntry? entry) { this.Entry = entry; this.DoubleBuffered = true; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            //Fill rectangles. Nothin to see here.
            using (Brush brush = new SolidBrush(backBox))
            {
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                g.FillRectangle(brush, rect);

                ((SolidBrush)brush).Color = frontBox;
                rect = new Rectangle(0, 5, this.Width, this.Height - 10);
                g.FillRectangle(brush, rect);
            }

            //If entry and its sprite exists.
            if (Entry != null && Entry.Sprite != null)
            {
                //Place Pokemon image
                Rectangle imgRect = new Rectangle(
                    this.Width / 10,
                    -3,
                    this.Height * Entry.Sprite.Width / 80,
                    this.Height * Entry.Sprite.Width / 80
                );

                g.DrawImage(Entry.Sprite, imgRect);
            }
        }
    }
}
