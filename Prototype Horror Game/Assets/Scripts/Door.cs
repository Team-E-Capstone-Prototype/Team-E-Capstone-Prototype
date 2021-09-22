using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator m_Door;
    bool m_DoorAnimLock = true;

    // Start is called before the first frame update
    void Start()
    {

        m_Door = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (m_DoorAnimLock)
        {
            m_DoorAnimLock = false;
            m_Door.SetBool("m_IsOpen", true);
        }
    }
}