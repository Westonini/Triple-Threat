using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This version of AudioManager is present throughout all scenes and plays sound in 2D
public class GlobalAudioManager : AudioManager //Inherits from AudioManager
{
    public static GlobalAudioManager instance;

    protected override void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        base.Awake();
    }
}
