using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class SoundMonster : MonoBehaviour
{

    enum AIState
	{
        Idle,
        Listening,
        Seeking,
        Mimic,
	}

    private List<AudioInfo> AudioInfoBuffer;
    private List<float> TimeBuffer;
    private Vector3 LastHeardPosition;
    private float TimeOfLastSound = 0;

    [SerializeField]
    AudioDetector Ears;

    [SerializeField]
    AudioSource MimicrySource;

    [SerializeField]
    float ListenTime = 3f;


    [SerializeField]
    float AIStopDistance = 3f;

    public NavMeshAgent NavAgent;

    AIState CurrentState;


    
    // Start is called before the first frame update
    void Start()
    {
        AudioInfoBuffer = new List<AudioInfo>();
        TimeBuffer = new List<float>();


        EnterState(AIState.Idle);

        Ears.OnAudioDetect.AddListener(HandleAudioDetect);
        NavAgent.stoppingDistance = AIStopDistance;
    }


    void EnterState(AIState newState)
	{
        switch (newState)
        {
            case AIState.Idle:
				{
                    CurrentState = AIState.Idle;
                    StartCoroutine(State_Idle());
				}
                break;
            case AIState.Listening:
				{


                    CurrentState = AIState.Listening;
                    StartCoroutine(State_Listening());
                }
                break;
            case AIState.Mimic:
				{


                    CurrentState = AIState.Mimic;
                    StartCoroutine(State_Mimic());
                }
                break;
            case AIState.Seeking:
				{
                    //start navigating to the sound
                    NavAgent.SetDestination(LastHeardPosition);
                    CurrentState = AIState.Seeking;
                    StartCoroutine(State_Seeking());
                }
                break;
        }
	}
    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleAudioDetect(AudioInfo Info)
	{





        LastHeardPosition = Info.emitter.transform.position;
        if (CurrentState == AIState.Mimic || CurrentState == AIState.Seeking)
		{
            //do nothing with the audio
            return;
		}
        else if (CurrentState == AIState.Listening)
		{


            //record the incomming sound data to the buffers
            TimeBuffer.Add(Time.time - TimeOfLastSound);
            TimeOfLastSound = Time.time;
            AudioInfoBuffer.Add(Info);

		}
        else if (CurrentState == AIState.Idle)
		{
            //if we're idle, start listening and record the first sound as well
            EnterState(AIState.Listening);
            AudioInfoBuffer.Add(Info);

            //we dont wait any time to play the first sound
            TimeOfLastSound = Time.time;
            TimeBuffer.Add(0);

        }
        

	}

    IEnumerator State_Idle()
	{
        //Debug.Log("Entered Idle State");
        yield return null;
	}
    IEnumerator State_Listening()
    {
        //Debug.Log("Entered Listening State");


        int ABufferSize = AudioInfoBuffer.Count;
        //clever timer
        for(float i = ListenTime; i > 0; i-= Time.deltaTime)
		{
            //wait till end of each new frame
            yield return new WaitForEndOfFrame();

            //if a new sound is heard, reset the timer
            if (ABufferSize != AudioInfoBuffer.Count)
			{
                ABufferSize = AudioInfoBuffer.Count;
                i = ListenTime;
			}


        }

        //mimic the sound
        EnterState(AIState.Mimic);

        yield return null;
    }
    IEnumerator State_Mimic()
    {
        //Debug.Log("Entered Mimic State");


        for (int i = 0; i < AudioInfoBuffer.Count; i++)
		{
            //pause for effect
            yield return new WaitForSecondsRealtime(TimeBuffer[i]);

            //play one shot
            MimicrySource.pitch = AudioInfoBuffer[i].Pitch;
            MimicrySource.PlayOneShot(AudioInfoBuffer[i].AClip, AudioInfoBuffer[i].Volume);
		}

        //flush buffers

        AudioInfoBuffer.Clear();
        TimeBuffer.Clear();

        EnterState(AIState.Seeking);

        yield return null;
    }
    IEnumerator State_Seeking()
    {
        //Debug.Log("Entered Seeking State");

        //just hold on now
        yield return new WaitForSecondsRealtime(1);

        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (NavAgent.remainingDistance < AIStopDistance + .5f)
            {
                break;
            }
        }


        //Debug.Log("Done Seeking!");
        EnterState(AIState.Idle);
        yield return null;
    }



}
