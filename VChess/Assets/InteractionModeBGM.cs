using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionModeBGM : MonoBehaviour
{
    AudioSource m_MyAudioSource;

    void Start()
    {
        //Fetch the AudioSource from the GameObject
        m_MyAudioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(TrackMovementTime.hasSquirrelAround);
        Debug.Log( m_MyAudioSource.isPlaying);
        if (TrackMovementTime.hasSquirrelAround && m_MyAudioSource.isPlaying)
        {
            Debug.Log("stop bgm ");
            m_MyAudioSource.Stop();
        }
        else if (!TrackMovementTime.hasSquirrelAround && !m_MyAudioSource.isPlaying)
        {
            Debug.Log("play bgm");
            m_MyAudioSource.Play();
        }
    }
}
