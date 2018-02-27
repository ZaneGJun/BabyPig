using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pld
{
    class PLDEventSystem : PLDSingleton<PLDEventSystem>
    {
        private Dictionary<string, PLDEvent> m_Events = new Dictionary<string, PLDEvent>();

        public void AddEventListener(string key, UnityAction<string, object> callback)
        {
            PLDEvent getEvent;
            if(m_Events.TryGetValue(key,out getEvent))
            {
                getEvent.AddListener(callback);
            }else
            {
                PLDEvent newEvent = new PLDEvent();
                newEvent.AddListener(callback);
                m_Events.Add(key, newEvent);
            }
        }

        public void RemoveEventListener(string key, UnityAction<string, object> callback)
        {
            PLDEvent getEvent;
            if(m_Events.TryGetValue(key, out getEvent))
            {
                getEvent.RemoveListener(callback);
            }
        }

        public void DispatchEvent(string key, object data = null)
        {
            PLDEvent getEvent;
            if(m_Events.TryGetValue(key, out getEvent))
            {
                getEvent.Invoke(key, data);
            }
        }
    }
}
