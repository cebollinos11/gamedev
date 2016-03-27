using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioClipsType
{
    onHighlight,onInteract,bossTeleport
}


public class AudioManager : Singleton<AudioManager>
{
    AudioSource mainAudioSource;
    

    public static void PlayClip(AudioClip aClip)
    {
        Instance.mainAudioSource.PlayOneShot(aClip);

    }

    public static void PlayBgSong(AudioClip bg)
    {

        //Instance.mainAudioSource.PlayOneShot(Instance.backgroundSongs[i]);
        Instance.mainAudioSource.clip = bg;
        Instance.mainAudioSource.loop = true;
        Instance.mainAudioSource.Play();

    }

 

    public static void StopAll()
    {
        Instance.mainAudioSource.Stop();
    }

    // Use this for initialization
    void Start()
    {
        mainAudioSource = Instance.gameObject.AddComponent<AudioSource>();//"AudioSource"); //Instance.gameObject.AddComponent("AudioSource") as AudioSource;



    }




}