using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelAttackMusic : MonoBehaviour
{
    public AudioSource audio_source;
    public AudioClip battle_music;
    //public AudioClip strong_music;
    //private int playingMusic = 0; // 0 - notPlaying, 1 - strong music, 2- battle music

    void Start()
    {
        
        //audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        //audio_source = GetComponent<AudioSource>();

        //if (TrackMovementTime.hasSquirrelAround && playingMusic != 2)
        //{
        //    audio_source.Stop();
        //    audio_source.clip = battle_music;
        //    audio_source.Play();
        //    playingMusic = 2;
        //} else {
        //    if (playingMusic != 1)
        //    {
        //        audio_source.Stop();
        //        audio_source.clip = strong_music;
        //        audio_source.Play();
        //        playingMusic = 1;
        //    }
        //}

        if (TrackMovementTime.hasSquirrelAround)
        {
            audio_source.PlayOneShot(battle_music);
        }
        else
        {
            if (!audio_source.isPlaying)
                audio_source.Play();
        }
    }
}
