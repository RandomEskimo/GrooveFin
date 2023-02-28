using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin
{
    public class AbstractContentPage : ContentView
    {
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }
        public string? Title { get; set; }
    }
}
