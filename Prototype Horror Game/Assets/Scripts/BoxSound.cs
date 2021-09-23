using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSound : MonoBehaviour
{
    public AudioClip m_FirstAudioClip;
    public AudioClip m_SecondAudioClip;
    public AudioSource m_Audio;

    public float maxForce = 5;

    bool m_AudioToggle = true;

    // Start is called before the first frame update
    void Start()
    {
        m_Audio = GetComponent<AudioSource>();
        m_Audio.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1.0f)
        {
            float force = collision.relativeVelocity.magnitude;

            float volume = 1;

            if (force <= maxForce)
            {
                volume = force / maxForce;
            }

            if (m_AudioToggle)
            {
                m_AudioToggle = false;
                m_Audio.PlayOneShot(m_FirstAudioClip, volume);
            }
            else
            {
                m_AudioToggle = true;
                m_Audio.PlayOneShot(m_SecondAudioClip, volume);
            }
        }
    }
}