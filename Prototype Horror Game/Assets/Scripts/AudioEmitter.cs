using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitter : MonoBehaviour
{

    [SerializeField]
    AudioSource m_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EmitSound(AudioClip AClip,float Volume,float Radius)
	{
        //play the actual sound
        m_AudioSource.PlayOneShot(AClip, Volume);

        AudioDetector[] Detectors = FindObjectsOfType<AudioDetector>();

        foreach (var Detector in Detectors)
		{
            if(Vector3.Distance(gameObject.transform.position,Detector.gameObject.transform.position) < Radius)
			{
                //sound is detected
                Detector.HandleDetection(this);
			}
		}

	}
}
