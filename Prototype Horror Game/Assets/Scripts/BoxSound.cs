using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSound : MonoBehaviour
{
    public AudioClip m_FirstAudioClip;
    public AudioClip m_SecondAudioClip;
    public AudioEmitter m_AEmitter;

    public float MaxAudioRange = 40f;
    public float maxForce = 10f;
    public float MinForce = .5f;
    public float VolumeScale = 1.6f;

    bool m_AudioToggle = true;




    [Tooltip("Time on start where collisions are ignored")]
    public float m_startTimer = 1f;
    bool m_start = false;




    //it didnt work

    //[SerializeField]
    //float AccelerationTriggerSpeed = 1f;
    //[SerializeField]
    //int VelocityBufferSize = 3;
    //
    //
    //
    //List<float> VelocityBuffer;
    //int VelocityBufferPos = 0;




    // Start is called before the first frame update



    void Start()
    {
        //VelocityBuffer = new List<float>();
        //VelocityBuffer.Capacity = VelocityBufferSize;

        // Fetch components on the same gameObject
        m_AEmitter = GetComponent<AudioEmitter>();

        StartCoroutine(StartTimer());


    }

    // Update is called once per frame
    void Update()
    {
        //float Velocity = GetComponent<Rigidbody>().velocity.magnitude;
        //float Adv = 0;
        //
        //
        //
        //if (VelocityBuffer.Count >= VelocityBufferSize)
        //{
        //
        //    //calculate adveradge
        //    foreach (float vel in VelocityBuffer)
        //    {
        //        Adv += vel;
        //    }
        //
        //    Adv /= VelocityBuffer.Count;
        //
        //    //overrite old buffer pos after calculation
        //    VelocityBuffer[VelocityBufferPos] = Velocity;
        //
        //}
        //else
        //{ VelocityBuffer.Add(Velocity); }
        //
        //float acceleration = Mathf.Abs(Velocity - Adv);
        ////Debug.Log(acceleration);
        //if (acceleration > AccelerationTriggerSpeed)
		//{
        //    float volume = acceleration / maxForce;
        //    if (m_AudioToggle)
        //    {
        //        m_AudioToggle = false;
        //        m_AEmitter.EmitSound(m_FirstAudioClip, volume, volume * MaxAudioRange);
        //    }
        //    else
        //    {
        //        m_AudioToggle = true;
        //        m_AEmitter.EmitSound(m_SecondAudioClip, volume, volume * MaxAudioRange);
        //    }
        //}
        //
        //
        ////cycle through the buffer and write the velocity to each one
        //if (VelocityBufferPos >= VelocityBufferSize - 1)
        //    VelocityBufferPos = 0;
        //else
        //    VelocityBufferPos++;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.relativeVelocity.magnitude >= MinForce && m_start)
        {
            float force = collision.relativeVelocity.magnitude;

            float volume = force / maxForce;
        
            if (m_AudioToggle) 
            {
                m_AudioToggle = false;
                m_AEmitter.EmitSound(m_FirstAudioClip, Mathf.Pow(volume, VolumeScale),volume * MaxAudioRange);
            }
            else
            {
                m_AudioToggle = true;
                m_AEmitter.EmitSound(m_SecondAudioClip, Mathf.Pow(volume, VolumeScale), volume * MaxAudioRange);
            }
        }

    }
    IEnumerator StartTimer()
    {
        while (m_startTimer > 0f)
        {

            m_startTimer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        m_start = true;

        yield return null;
    }

}