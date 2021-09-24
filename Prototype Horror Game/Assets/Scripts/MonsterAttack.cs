using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    LineRenderer m_Line;

    public float m_MaxDistance = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Line = GetComponent<LineRenderer>();
        m_Line.positionCount = 5;
    }

    // Update is called once per frame
    void Update()
    {
        DetectTarget();
    }

    public void DetectTarget()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, m_MaxDistance) && hit.transform.tag == "Player")
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            m_Line.enabled = true;

            m_Line.SetPosition(0, transform.position);

            for (int i = 1; i < m_Line.positionCount - 1; i++)
            {
                Vector3 pos = Vector3.Lerp(transform.position, hit.transform.position, i / 4.0f);

                pos.x += Random.Range(-0.4f, 0.4f);
                pos.y += Random.Range(-0.4f, 0.4f);

                m_Line.SetPosition(i, pos);
            }

            m_Line.SetPosition(4, hit.transform.position);

        }
        else
        {
            m_Line.enabled = false;
        }
    }
}