﻿using System;
using BPL.Event.EventDispatcher.Interfaces;

namespace EventDispatcherTests;

public class NotificationEvent : IEvent
{
    public int userId { get; set; }

    public NotificationEvent(int userId)
    {
        this.userId = userId;
    }
}

