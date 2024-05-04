using System;
using System.Reflection;
using EventDispacther.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventDispacther.Dispatcher;

public class EventDispatcher : IEventDispatcher
{
    private readonly Dictionary<Type, List<Delegate>> _eventListeners = new Dictionary<Type, List<Delegate>>();

    public void AddListener<TEvent>(Action<TEvent> listener) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);

        if (!_eventListeners.ContainsKey(eventType))
        {
            _eventListeners[eventType] = new List<Delegate>();
        }

        _eventListeners[eventType].Add(listener);
    }

    public void Dispatch<TEvent>(TEvent @event) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
       
        if (_eventListeners.TryGetValue(eventType, out var eventListener))
        {
            foreach (var listener in eventListener.ToList())
            {
                ((Action<TEvent>)listener)(@event);
            }
        }
    }

    public void RegisterEventSubscriber(object eventSubscriber)
    {
       
        if(!ImplementsEventSubscriberInterface(eventSubscriber.GetType()))
        {
            return;
        }
        var eventTypes = GetEventTypes(eventSubscriber.GetType());

        foreach (var eventType in eventTypes)
        { 
            if (!_eventListeners.ContainsKey(eventType))
            { 
                _eventListeners[eventType] = new List<Delegate>();
            }
            _eventListeners[eventType].Add( CreateEventHandlerDelegate(eventSubscriber, eventType));
        }
    }

    public void RegisterEventSubscribers(Assembly assembly)
    {
      
        var types = assembly.GetTypes();
        var eventSubscriberTypes = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && ImplementsEventSubscriberInterface(type));

        foreach (var eventSubscriberType in eventSubscriberTypes)
        {
            var eventSubscriber = Activator.CreateInstance(eventSubscriberType);
           RegisterEventSubscriber(eventSubscriber!);
        }
    }

    public void RemoveListener<TEvent>(Action<TEvent> listener) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        if (_eventListeners.ContainsKey(eventType))
        {
            _eventListeners[eventType].Remove(listener);
        }
    }


    private bool ImplementsEventSubscriberInterface(Type type)
    {
        return Array.Exists(type.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventSubscriber<>));
    }

    private IEnumerable<Type> GetEventTypes(Type eventSubscriberType)
    {
        var eventSubscriberInterface = typeof(IEventSubscriber<IEvent>);
        return eventSubscriberType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition().Name == eventSubscriberInterface.Name)
            .Select(i => i.GetGenericArguments()[0]);
    }

    private Delegate CreateEventHandlerDelegate(object eventSubscriber, Type eventType)
    {
        var handleEventMethod = typeof(IEventSubscriber<>).MakeGenericType(eventType)
                 .GetMethod("HandleEvent");
     
        return Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(eventType), eventSubscriber, handleEventMethod!);
    }
}
