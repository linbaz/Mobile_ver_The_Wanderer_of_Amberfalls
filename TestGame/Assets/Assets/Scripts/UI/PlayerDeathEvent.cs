using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathEvent : UnityEvent
{
}

public static class GameEvents
{
    public static PlayerDeathEvent OnPlayerDeath = new PlayerDeathEvent();
}
