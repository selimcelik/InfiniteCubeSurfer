using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    //Game Start Event
    public event Action onGameStarted;
    public void GameStarted()
    {
        if(onGameStarted != null)
        {
            onGameStarted();
        }
    }
    //Create Cube Event
    public event Action onCreateCubes;
    public void CreateCubes()
    {
        if(onCreateCubes != null)
        {
            onCreateCubes();
        }
    }
    //Collect Cube Event
    public event Action<GameObject> onCollectCubes;
    public void CollectCubes(GameObject g)
    {
        if(onCollectCubes != null)
        {
            onCollectCubes(g);
        }
    }
    //Jackpot Event
    public event Action onJackpotCubes;
    public void JackPotCubes()
    {
        if(onJackpotCubes != null)
        {
            onJackpotCubes();
        }
    }
}
