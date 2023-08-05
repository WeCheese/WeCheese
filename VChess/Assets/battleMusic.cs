using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleMusic : MonoBehaviour
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
        if (TrackMovementTime.hasSquirrelAround && !m_MyAudioSource.isPlaying)
        {
            Debug.Log("play battle");
            m_MyAudioSource.Play();
        }
        else if (!TrackMovementTime.hasSquirrelAround && m_MyAudioSource.isPlaying)
        {
            Debug.Log("stop battle");
            m_MyAudioSource.Stop();
        }
    }
}
