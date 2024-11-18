using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Configuration;

namespace WindowsPokedexApp.UI.bg
{
    /*
     * This represents the background of the app.
     */

    //I am not sure what the best way to do this is but I'm just gonna do a few rectangles.
    //thank u nintendo for making a very good looking but basic ui.
    //I also am not sure if there is a Container class I should inherit from?
    internal class Background : UserControl
    {
        //Properties and Fields
        Color redBg = Color.FromArgb(255, 221, 19, 5);
        Color blackBg = Color.FromArgb(255, 29, 29, 29);

        //Default Constructor
        public Background() { }

        //OnPaint method
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            //Define bounds
            Rectangle fullRect = new Rectangle(0, 0, this.Parent.Width, this.Parent.Height);
            Rectangle topRect = new Rectangle(0, 0, this.Parent.Width, this.Parent.Height/4);
            Rectangle bottomRect = new Rectangle(0, this.Parent.Height/4, this.Parent.Width, 3*this.Parent.Height/4);

            //Fill Rectangles
            using (Brush brush = new SolidBrush(redBg))
            {
                g.FillRectangle(brush, bottomRect);

                ((SolidBrush)brush).Color = blackBg;

                g.FillRectangle(brush, topRect);
            }

            //Background hatching
            using (Pen pen = new Pen(Color.FromArgb(20, 0, 0, 0), 3))
            {
                for(int i = 0; i < this.Parent.Width; i+=this.Parent.Width/35)
                {
                    g.DrawLine(pen, new Point(i, 0), new Point(i, this.Parent.Height));
                    g.DrawLine(pen, new Point(0, i), new Point(this.Parent.Width, i));
                }
            }
        }
    }
}
