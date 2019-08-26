using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    static bool musicIsPlaying = false;

    void Start()
    {
        if (!musicIsPlaying)
        {
            StartCoroutine("MusicLoop");
            musicIsPlaying = true;
        }
    }

   IEnumerator MusicLoop()
    {
        FindObjectOfType<AudioManager>().Play("MusicIntro");
        yield return new WaitForSeconds(65.75f);
        FindObjectOfType<AudioManager>().Stop("MusicIntro");
        FindObjectOfType<AudioManager>().Play("MusicLoop");
    }
}
