using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public abstract class EventBase
{
	//All of this values are padded onto 32bits, equivalent to union in c++
	[FieldOffset(0)]
	public bool FlagValue;
	[FieldOffset(0)]
	public short ShortValue;
	[FieldOffset(0)]
	public int IntValue;
	[FieldOffset(0)]
	public float FloatValue;
}
public class EventManager
{
	public delegate void EventFunction<T>(T eventInfo);

	private Dictionary<Type, List<Delegate>> m_events;
	private Dictionary<Type, List<Delegate>> m_eventsToUnRegister;

	public EventManager()
	{
		m_events = new Dictionary<Type, List<Delegate>>();
		m_eventsToUnRegister = new Dictionary<Type, List<Delegate>>();
	}
	public void Send<T>(T data)
	{
		if(m_events.ContainsKey(typeof(T)))
		{
			foreach(EventFunction<T> function in m_events[typeof(T)])
				function.Invoke(data);			
		}
		else
			Debug.LogWarning("EventManager::Send: No registered receiver for type " + typeof(T).ToString());

		// In order to be able to UnRegister inside a registered function!
		BruteForceDelete();
	}
	public void Register<T>(EventFunction<T> subscribingFunction)
	{
		if(!m_events.ContainsKey(typeof(T)))
			m_events.Add(typeof(T), new List<Delegate>());
		m_events[typeof(T)].Add(subscribingFunction);
	}

	// If the afterFinishedSend flag is true, the event will be unregistered after send is complete
	// for all events. This is slow and should be avoided. Unregister events outside the function they
	// are registered with, to avoid this.
	public void UnRegister<T>(EventFunction<T> subscribingFunction, bool afterFinishedSend = false)
	{
		if(!m_events.ContainsKey(typeof(T)))
			return;

		for(int i = 0; i < m_events[typeof(T)].Count; ++i)
		{
			if(m_events[typeof(T)][i] == subscribingFunction as Delegate)
			{
				if(afterFinishedSend)
				{
					if(!m_eventsToUnRegister.ContainsKey(typeof(T)))
						m_eventsToUnRegister.Add(typeof(T), new List<Delegate>());
					m_eventsToUnRegister[typeof(T)].Add(subscribingFunction);		
					return;
				}
				m_events[typeof(T)].RemoveAt(i);
				if(m_events[typeof(T)].Count == 0)
					m_events.Remove(typeof(T));					
				return;
			}
		}
	}

	// Slow but enables us to unregister in registered functions aswell, use with caution!
	private void BruteForceDelete()
	{
		if(m_eventsToUnRegister.Count < 1)
			return;
		
		for(var e = m_events.GetEnumerator(); e.MoveNext();)	
		{				
			if(m_eventsToUnRegister.ContainsKey(e.Current.Key))
			{
				for(int j = 0; j < m_eventsToUnRegister[e.Current.Key].Count; ++j)
				{
					for(int k = 0; k < e.Current.Value.Count; ++k)
					{
						if(e.Current.Value[k] == m_eventsToUnRegister[e.Current.Key][j])
						{
							e.Current.Value.Remove(e.Current.Value[k]);
							--k;
						}
					}
				}
			}
		}
		m_eventsToUnRegister.Clear();	
	}
}

