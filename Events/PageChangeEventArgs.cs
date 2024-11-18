using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPokedexApp
{
    //EventArgs object which sends information about changing page
    internal class PageChangeEventArgs : EventArgs
    {
        //Fields
        public int PageChangeDirection { get; private set; } //Negative if previous page, positive if next

        public PageChangeEventArgs(int pageChange)
        {
            PageChangeDirection = pageChange;
        }
    }
}
