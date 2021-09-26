using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField]
    public NavMeshAgent Monster;

    bool m_IsActive = true;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_IsActive == true)
        {
            if (other.gameObject.tag == "Player")
            {
                Monster.GetComponent<FollowMonster>().Stunned();
                m_IsActive = false;
            }
        }
    }
}
