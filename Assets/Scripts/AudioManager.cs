using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioClipsType
{
    onHighlight, onInteract, bossTeleport
}


public class AudioManager : Singleton<AudioManager>
{
    AudioSource mainAudioSource;

    public AudioClip DetectSound;
    public AudioClip mouseclick;

    public AudioClip music_bassline;
    public AudioClip music_physical;
    public AudioClip music_digital;
    public AudioClip music_alert;

    AudioSource a_bassline;
    AudioSource a_physical;
    AudioSource a_digital;
    AudioSource a_alert;
    [SerializeField] AudioClip crabStep;

    bool crabSoundEnabled;

    public static void PlayClip(AudioClip aClip)
    {
        Instance.mainAudioSource.PlayOneShot(aClip);

    }

    static IEnumerator EnableCrabSound()
    {
        yield return new WaitForSeconds(1f);
        Instance.crabSoundEnabled = true;
    }

    public static void PlayCraberino()
    {
        if (Instance.crabSoundEnabled)
        {
            Instance.mainAudioSource.PlayOneShot(Instance.crabStep);
            Instance.crabSoundEnabled = false;
            Instance.StartCoroutine(EnableCrabSound());
        }            

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

    AudioSource InitializeASource(AudioSource asource, AudioClip aclip)
    {
        asource = gameObject.AddComponent<AudioSource>();
        asource.loop = true;
        asource.clip = aclip;
        return asource;
    }

    IEnumerator LerpMusic(AudioSource asource, float from, float to)
    {

        asource.volume = from;

        float percent = 0f;




        do
        {

            asource.volume = Mathf.Lerp(from, to, percent);
            percent += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        } while (percent < 1.0f);

        asource.volume = to;

    }

    void turnOn(AudioSource asource)
    {
        //asource.volume = 1.0f;
        if (asource.volume != 1.0)
        {
            StartCoroutine(LerpMusic(asource, 0.0f, 1f));
        }
    }

    void turnOff(AudioSource asource)
    {
        asource.volume = 0.0f;
    }

    public static void TurnOffAll()
    {
        Instance.turnOff(Instance.a_alert);
        Instance.turnOff(Instance.a_bassline);
        Instance.turnOff(Instance.a_digital);
        Instance.turnOff(Instance.a_physical);
    }

    public static void HandleBackgroundMusic()
    {
        //Debug.Log("HANDLING MUSIC");
        FormsManager fm = GameObject.FindObjectOfType<FormsManager>();

        

        if(!fm.isSplitted)
        {
            Instance.turnOn(Instance.a_digital);
        }
        else
        {
            if (fm.curForm == 1)
                Instance.turnOn(Instance.a_digital);
            else
            {
                Instance.turnOff(Instance.a_digital);
            }
        }

        if (fm.curForm == 0)
            Instance.turnOn(Instance.a_physical);
        else
        {
            Instance.turnOff(Instance.a_physical);
        }

        bool playinvestigate = false;

        EnemyGuard[] enemies = GameObject.FindObjectsOfType<EnemyGuard>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].investigate)
                playinvestigate = true;


        }

        if (playinvestigate)
            Instance.turnOn(Instance.a_alert);
        else
        {
            Instance.turnOff(Instance.a_alert);
        }


    }

    // Use this for initialization
    void Start()
    {
        mainAudioSource = Instance.gameObject.AddComponent<AudioSource>();//"AudioSource"); //Instance.gameObject.AddComponent("AudioSource") as AudioSource;
        /*PlayBgSong(music_bassline);
        PlayBgSong(music_physical);*/

        a_bassline = InitializeASource(a_bassline, music_bassline);


        a_physical = InitializeASource(a_physical, music_physical);

        a_physical.volume = 0.0f;

        a_digital = InitializeASource(a_digital, music_digital);

        a_digital.volume = 0.0f;

        a_alert = InitializeASource(a_alert, music_alert);
        a_alert.volume = 0.0f;

        a_digital.Play();
        a_alert.Play();
        a_physical.Play();
        a_bassline.Play();

        HandleBackgroundMusic();

        crabSoundEnabled = true;

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            PlayClip(mouseclick);
        }

    }




}