using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door m_Door;

    [SerializeField]
    private float m_reqMass;

    bool m_IsActive = true;

    [SerializeField]
    private float m_curMass = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (m_IsActive)
        {
            if (other.gameObject.tag == "Interactable Object")
            {
                Rigidbody objectRB = other.gameObject.GetComponent<Rigidbody>();

                m_curMass += objectRB.mass / 2.0f;
                
                if (m_curMass >= m_reqMass)
                {
                    m_IsActive = false;
                    m_Door.OpenDoor();
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
       if (m_IsActive)
        {
            if (other.gameObject.tag == "Interactable Object")
            {
                Rigidbody objectRB = other.gameObject.GetComponent<Rigidbody>();

                if (m_curMass > 0)
                {
                    m_curMass -= objectRB.mass / 2.0f;
                }

            }
        }
    }

}