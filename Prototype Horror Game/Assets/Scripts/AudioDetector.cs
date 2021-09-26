using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioDetector : MonoBehaviour
{

    [HideInInspector]
    public UnityEvent<AudioInfo> OnAudioDetect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleDetection(AudioInfo info)
	{
        //just pass it to listeners for now
        OnAudioDetect.Invoke(info);
	}
}
