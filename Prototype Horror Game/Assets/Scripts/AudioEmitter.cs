using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInfo
{
	public AudioEmitter emitter;
	public AudioClip AClip;
	public float Volume;
	public float Pitch;

}

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

    public void EmitSound(AudioClip AClip, float Volume, float Radius)
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
			//float Dist = Vector3.Distance(gameObject.transform.position, Detector.gameObject.transform.position);
			//
			//if (Dist < Radius)
			//{
			//
			//	Vector3 EmitterPos = this.m_AudioSource.transform.position;
			//	Vector3 DetectorPos = Detector.transform.position;
			//	Vector3 Dir = (DetectorPos - EmitterPos).normalized;
			//
			//	int NumCollisions = Physics.RaycastAll(EmitterPos, Dir, 6 | 7).Length;
			//
			//	Debug.Log(NumCollisions + "      " + Time.time);
			//	if (Dist < Radius / NumCollisions)
			//	{

					//sound is detected
					AudioInfo newInfo = new AudioInfo();
					newInfo.emitter = this;
					newInfo.AClip = AClip;
					newInfo.Volume = Volume;
					newInfo.Pitch = m_AudioSource.pitch;
					Detector.HandleDetection(newInfo);
				//}
			//}
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
