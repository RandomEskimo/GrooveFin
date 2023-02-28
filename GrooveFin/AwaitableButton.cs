using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin
{
    class AwaitableButton : Button
    {
        private TaskCompletionSource TCS;
        
        public AwaitableButton() 
        {
            TCS = new TaskCompletionSource();
			Clicked += AwaitableButton_Clicked;
        }

		private void AwaitableButton_Clicked(object? sender, EventArgs e)
		{
            TCS.SetResult();
            TCS = new TaskCompletionSource();
		}

        public Task WaitForClickAsync()
        {
            return TCS.Task;
        }
	}
}
