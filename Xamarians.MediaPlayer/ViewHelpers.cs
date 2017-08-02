using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarians.MediaPlayer
{
    public static class ViewHelpers
    {
        public static void OnTapped(this View view, System.Action execute)
        {
            view.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(execute)
            });
        }
    }
}
