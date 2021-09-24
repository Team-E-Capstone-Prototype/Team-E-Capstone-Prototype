using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSound : MonoBehaviour
{
    public AudioClip m_FirstAudioClip;
    public AudioClip m_SecondAudioClip;
    public AudioEmitter m_AEmitter;


    public float MaxAudioRange = 50f;
    public float maxForce = 5;

    bool m_AudioToggle = true;

    // Start is called before the first frame update
    void Start()
    {
        m_AEmitter = GetComponent<AudioEmitter>();
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
                m_AEmitter.EmitSound(m_FirstAudioClip, volume, volume * MaxAudioRange);
            }
            else
            {
                m_AudioToggle = true;
                m_AEmitter.EmitSound(m_SecondAudioClip, volume, volume * MaxAudioRange);
            }
        }
    }
}