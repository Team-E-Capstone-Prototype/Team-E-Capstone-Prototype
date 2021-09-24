using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitter : MonoBehaviour
{

    [SerializeField]
    AudioSource m_AudioSource;

    float VisualizerDrawTime = 2;

	float CurrentRadius = 0;
	Vector3 LastSoundPosition = Vector3.zero;

    float VisualizerTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		//handle visualize timer

		if (VisualizerTimer > 0)
		{
			VisualizerTimer -= Time.deltaTime;
		}

	}

    public void EmitSound(AudioClip AClip,float Volume,float Radius)
	{
        //play the actual sound
        m_AudioSource.PlayOneShot(AClip, Volume);

		//set visualizer info
		CurrentRadius = Radius;
		VisualizerTimer = VisualizerDrawTime;
		LastSoundPosition = transform.position;


        //notify detectors within range of the audio
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

	private void OnDrawGizmos()
	{
		if (VisualizerTimer > 0)
		{
			Gizmos.DrawWireSphere(LastSoundPosition, CurrentRadius);
		}
	}
}
