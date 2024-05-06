using System;
using BPL.Event.Dispatcher.Interfaces;

namespace BPL.Event.DispatcherTests
{
	public class NotificationEvent: IEvent
	{
		public int UserId { get;  set;  }
		public NotificationEvent(int userId)
		{
			UserId = userId;
		}
	}
}

