using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

    public delegate void EventReciever(params object[] parameterContainer);
    private static Dictionary<GameEvent, EventReciever> _events;

    public static void AddEventListener(GameEvent eT, EventReciever listener)
    {
        if(_events == null)
        {
            _events = new Dictionary<GameEvent, EventReciever>();
        }
        if(!_events.ContainsKey(eT))
        {
            _events.Add(eT, null);
        }
        _events[eT] += listener;
    }

    public static void RemoveEventListener(GameEvent eT, EventReciever listener)
    {
        if(_events != null)
        {
            if(_events.ContainsKey(eT))
            {
                _events[eT] -= listener;
            }
        }
    }

    public static void DispatchEvent(GameEvent eT)
    {
        DispatchEvent(eT, null);
    }

    public static void DispatchEvent(GameEvent eT, params object[] paramsWrapper)
    {
        if(_events == null)
        {
            Debug.Log("No events suscribed");
            return;
        }
        if(_events.ContainsKey(eT))
        {
            if (_events[eT] != null)
                _events[eT](paramsWrapper);
        }
    }
}

public enum GameEvent
{
    Null,
    DARDO_SPAWN,
    DARDO_HIT,
    DARDO_DIE,
    SMALLABSORVABLE_REACHED,
    CAMERA_FIXPOS,
    BULLET_DARDO_SPAWN,
    BULLET_DARDO_DESTROY,
    BULLET_SCRAP_SPAWN,
    BULLET_SCRAP_DESTROY,
    ESCUPITAJO_SHOOT,
    ESCUPITAJO_BULLET_DESTROY,
    ESCUPITAJO_RECIEVE_DAMAGE,
    MIST_DIE,
    PLAYER_TAKE_DAMAGE,
    PLAYER_DIE,
    SECRET_COLLECTED,
    TRANSITION_DEATH_END
}
