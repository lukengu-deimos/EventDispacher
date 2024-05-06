# Event dispatcher
EventDispatcher is a package Inspired by Symfony: A Flexible Event Management System based on 
Levan Ostrovski's article on medium.com

# Understanding the EventDispatcher
The EventDispatcher, a potent element that facilitates effective event handling and communication across various components of an application, is at the core of event-driven architecture. The EventDispatcher acts as a focal point, managing events, alerting subscribers, and enabling decoupled communication.
The EventDispatcher is built around events. They indicate important system events like user actions or state changes. Developers can decouple components by encapsulating these occurrences as events and enabling indirect communication between them via the EventDispatcher.Implementing the EventDispatcher:

# How to use
## 1. Create an event as per the below example
```
    class Notification : IEvent
    {
        private readonly int UserId {get; private set;}

        public Notification(int userId)
        {
            this.UserId = userId;
        }
    }
```
## 2. Create EventSubscriber
```
    class NotificationSubscriber : IEventSubscriber<Notification>
    {
        public void HandleEvent(Notification @event)
        {
            Console.WriteLine("Notification event Intercepted");
        }
    }
```

## 3. Initialize EventDispatcher
```
/// Create new instance
EventDispatcher dispatcher = new EventDispatcher();
/// Pass Assembly information to automatically
/// find all subscribers
dispatcher.RegisterEventSubscribers(Assembly.GetExecutingAssembly());

dispatcher.Dispatch(new Notification(1998));
``` 
