using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp.UI
{
    //Left arrow to scroll things
    internal class LeftArrow : UserControl
    {
        //Color
        Color arrowColor = Color.FromArgb(255, 170, 170, 170);
        Color gradientColor = Color.FromArgb(255, 100, 100, 100);

        public LeftArrow() { }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            Point[] verticies = {
                new Point(15, 5),
                new Point(5, this.Height/2),
                new Point(15, this.Height-5)
            };

            //Draw the box. Mimics the backgroudn cuz I can't make it transparent.
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (Brush brush = new LinearGradientBrush(rect, Color.Black, Color.Black, LinearGradientMode.Vertical))
            {
                ColorBlend cb = new ColorBlend();

                Color[] colors = { gradientColor, Color.Black, Color.Black };
                float[] positions = { 0F, 0.5F, 1F };

                cb.Colors = colors;
                cb.Positions = positions;
                ((LinearGradientBrush)brush).InterpolationColors = cb;

                g.FillRectangle(brush, rect);
            }

            //Draws the arrow
            using (Pen pen = new Pen(arrowColor, 10))
            {
                g.DrawLines(pen, verticies);
            }
        }
    }
}
