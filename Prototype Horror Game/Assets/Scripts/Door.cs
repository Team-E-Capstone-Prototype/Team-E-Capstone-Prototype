using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator m_Door;                // Door Animator
    bool m_DoorAnimLock = true;     // Is Door Locked?

    // Start is called before the first frame update
    void Start()
    {
        // Fetch components on the same gameObject
        m_Door = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OpenDoor is called when opening door
    public void OpenDoor()
    {
        // If Door is Locked...
        if (m_DoorAnimLock)
        {
            // Unlock door
            m_DoorAnimLock = false;

            // Update Door Animator m_IsOpen
            m_Door.SetBool("m_IsOpen", true);
        }
    }
}