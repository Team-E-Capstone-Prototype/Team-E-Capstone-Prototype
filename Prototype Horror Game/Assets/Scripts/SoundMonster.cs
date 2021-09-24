using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoundMonster : MonoBehaviour
{
    [SerializeField]
    AudioDetector Ears;

    public NavMeshAgent NavAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        Ears.OnAudioDetect.AddListener(HandleAudioDetect);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HandleAudioDetect(AudioEmitter Audio)
	{
        NavAgent.SetDestination(Audio.gameObject.transform.position);
	}

}
