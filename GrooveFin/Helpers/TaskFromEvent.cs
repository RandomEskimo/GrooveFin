using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.Helpers
{
	internal class TaskFromEvent
	{
		private TaskCompletionSource TCS;
		private Action<EventHandler> UnassignHandler;

		public Task Task => TCS.Task;

		public TaskFromEvent(Action<EventHandler> AssignHandler, Action<EventHandler> UnassignHandler)
		{
			TCS = new TaskCompletionSource();
			AssignHandler(OnHandler);
			this.UnassignHandler = UnassignHandler;
		}

		private void OnHandler(object? sender, EventArgs e)
		{
			TCS.SetResult();
			UnassignHandler(OnHandler);
		}
	}
}
