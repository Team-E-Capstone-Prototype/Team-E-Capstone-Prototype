using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //click mouse
		if (Input.GetMouseButtonDown(0))
		{
            //if it hits a sound emitter, make it emit sound
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
                AudioEmitter Emitter = hit.transform.gameObject.GetComponent<AudioEmitter>();
                if (Emitter)
				{
                    //Emitter.EmitSound(Volume);
				}
			}
		}
    }
}
