using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

//This script manages 2D and 3D audio
public class AudioManager : MonoBehaviour
{
    public GameObject audioObject;
    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
        //Make this object DoNotDestroy
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //Set the various values for each sound within an audio source component
        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
            SetAudioSettings(s.source, s);
        }
    }

    //Used to select a random audio clip from an array passed in.
    public void PlayRandom(List<string> sounds, Transform playAt = null)
    {
        string soundToPlay = sounds[UnityEngine.Random.Range(0, sounds.Count)];

        Play(soundToPlay, playAt);
    }

    //Play an audio clip.
    public void Play(string sound, Transform playAt = null)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

        //If a Transform parameter is passed in, meaning we want to play a 3D sound at a certain position..
        if (playAt != null)
        {
            //Instantiate a gameobject and add an audio source component to it.
            GameObject audioInstance = Instantiate(audioObject, playAt.position, Quaternion.identity);
            AudioSource instanceSource = audioInstance.AddComponent<AudioSource>();

            //Set its parent to Instantiated Objects
            audioInstance.transform.SetParent(InstantiateParticles.instantiatedObjects.transform);

            //Tweak the audio source settings
            instanceSource.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            instanceSource.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
            SetAudioSettings(instanceSource, s);

            //Play it and destroy the object after 5 seconds.
            instanceSource.Play();
            Destroy(audioInstance, 5f);

            return; //Don't execute the following code
        }

        //Set the volume and pitch, then play the 2D sound.
        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        s.source.Play();
	}

    //Pause an audio clip.
    public void Pause(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Pause();
    }

    //Stop an audio clip.
    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    //Used to set the audio source settings on an AudioSource component.
    void SetAudioSettings(AudioSource source, Sound s)
    {
        source.clip = s.clip;
        source.loop = s.loop;

        source.playOnAwake = false;
        source.outputAudioMixerGroup = mixerGroup;

        //If we want to set the audio settings on an instantiated object, add 3D audio settings.
        if (source.name == "AudioObject(Clone)")
        {
            source.maxDistance = 35;
            source.rolloffMode = AudioRolloffMode.Custom;
            source.spatialBlend = 1;
            source.spread = 180;
        }
    }
}
