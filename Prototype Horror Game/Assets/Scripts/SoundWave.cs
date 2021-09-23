using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    [Tooltip("The frequency of a wave")]
    [Range(0, 10)]
    public float m_Frequency = 1.0f;

    [Tooltip("The amplitude of a wave")]
    [Range(0, 10)]
    public float m_Amplitude = 1.0f;

    [Tooltip("The direction of a wave")]
    [Range(-10, 10)]
    public float m_Direction = 1.0f;

    [Tooltip("The vertical offset of a wave")]
    [Range(0, 10)]
    public float m_yOffset = 0.0f;

    [Tooltip("Movement Speed of Object")]
    [Range(0, 10)]
    public float m_Speed = 2.0f;

    private float thetaStep = Mathf.PI / 32.0f;
    private float theta = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float newYPos = m_Direction * m_Amplitude * Mathf.Sin(theta * m_Frequency) + m_yOffset;
        float yStep = newYPos - transform.position.y;

        transform.Translate(new Vector3(m_Speed * Time.deltaTime, yStep));

        theta += thetaStep;
    }
}