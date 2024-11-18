using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WindowsPokedexApp.UI.bg;

namespace WindowsPokedexApp.UI
{
    //Custom Search Bar cuz i want it to look less bad.
    //Uses my normal Bar thing 
    internal class PokemonSearch : BottomBar
    {
        //Properties
        public string Input { get => input; set => input = value; }

        public Color FontColor { get => fontColor; set => fontColor = value; }

        //Fields
        private string input;
        private Font textFont = new Font("Segoe UI", 18);
        private Color fontColor = Color.White;
        private Color focusedColor = Color.FromArgb(255, 100, 100, 100);
        private bool isFocused = false;

        //Event to get the input string, this is for my LINQ query.
        public delegate void updateInput(string input);
        public event updateInput UpdateInput;

        public PokemonSearch()
        {
            this.input = "Search";
            this.Enter += OnFocus;
            this.Leave += LoseFocus;
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (isFocused)
            {
                colorBg(new SolidBrush(focusedColor), g);
            }

            using (Brush brush = new SolidBrush(FontColor))
            {
                g.DrawString(Input, this.Font, brush, 10, 10);
            }
        }

        //Stuff to do when this is focused
        private void OnFocus(object sender, EventArgs e)
        {
            isFocused = true;
            this.KeyPress += OnTyping;
            this.Invalidate();
        }

        private void LoseFocus(object sender, EventArgs e)
        {
            isFocused = false;
            this.KeyPress -= OnTyping;
            this.Invalidate();
        }

        //Typing Logic (yaaay)
        private void OnTyping(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                if (input.Length > 0)
                    Input = Input.Substring(0, Input.Length - 1);
            }

            else
            {
                input += e.KeyChar; 
            }

            UpdateInput.Invoke(Input);
            this.Invalidate();
        }
    }
}
