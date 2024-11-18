using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp.UI.bg
{
    /*
     * This class represents the little bar on the bottom where 
     * some options are going to live
     */
    internal class BottomBar : UserControl
    {
        //Color for the gradient
        private Color gradientColor = Color.FromArgb(255, 100, 100, 100);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            //Fill box. nothin crazy
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (Brush brush = new LinearGradientBrush(rect, Color.Black, Color.Black, LinearGradientMode.Vertical))
            {
                ColorBlend cb = new ColorBlend();

                Color[] colors = { gradientColor, Color.Black, Color.Black };
                float[] positions = { 0F, 0.25F, 1F };

                cb.Colors = colors;
                cb.Positions = positions;
                ((LinearGradientBrush)brush).InterpolationColors = cb;

                colorBg(brush, g);
            }
        }

        protected void colorBg(Brush brush, Graphics g)
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            g.FillRectangle(brush, rect);
        }
    }
}
