using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public AudioClip[] m_AudioClip;
    public AudioSource m_AudioSource;
    public AudioEmitter m_AEmitter;

    public Rigidbody m_Body;

    public float creakAngle = 15.0f; // Creak at set angle
    public float pitchControl = 0.1f; // Control pitch

    private float m_Angle = 0.0f;
    private float m_lastAngle = 0.0f;
    private float m_lastCreak = 0.0f;

    public float m_maxForce = 5.0f;
    public float m_MaxAudioRange = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch components on the same gameObject
        {
            m_AEmitter = GetComponent<AudioEmitter>();
            m_AudioSource = GetComponent<AudioSource>();
            m_Body = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CreakSounds();
    }

    public void CreakSounds()
    {
        float volume = CalculateVolume();

        // Set Angle to y rotation of the Door Rigidbody
        m_Angle = m_Body.rotation.eulerAngles.y;

        // If Door moved more than creakAngle...
        if (Mathf.Abs(m_Angle - m_lastAngle) > creakAngle)
        {
            // Update lastAngle
            m_lastAngle = m_Angle;

            // Calculate time from last creak
            float delta = Time.time - m_lastCreak;

            // Set lastCreak to amount of time in seconds that the game has been running for
            m_lastCreak = Time.time;

            // Increase pitch based on speed of door
            m_AudioSource.pitch = Mathf.Clamp((0.5f + pitchControl) / (0.5f + delta), 0.9f, 1.5f);

            m_AEmitter.EmitSound(m_AudioClip[Random.Range(0, m_AudioClip.Length)], volume, volume * m_MaxAudioRange);
        }
    }

    public float CalculateVolume()
    {
        float force = m_Body.angularVelocity.magnitude;

        float volume = 1.0f;

        if (force <= m_maxForce)
        {
            volume = Mathf.Pow(force / m_maxForce,1.5f);
        }

        return volume;
    }
}