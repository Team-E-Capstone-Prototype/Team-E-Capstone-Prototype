using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Controllers
    MouseKeyPlayerController m_Controller; // Player Input Controls
    CharacterController m_CharacterController; // Character Controller

    // Player
    public float m_Speed = 10.0f; // Movement Speed
    public float m_MaxDistance = 1.0f; // Max Distance of Checking Ground
    public float m_JumpForce = 1.5f; // Power of Jump

    // Camera
    public Camera m_PlayerCamera; // First Person Camera
    public float m_RotationSpeed = 200.0f;// Rptation Speed of Camera
    public float m_CameraVerticalAngle = 0.0f; // Vertical Angle of Camera
    public float m_CameraHorizontalAngle = 0.0f; // Vertical Angle of Camera

    Vector3 m_PlayerRotation = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        m_Speed = 10.0f;
        m_Controller = new MouseKeyPlayerController();
        m_CharacterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCharacterMovement();

    }

    void FixedUpdate()
    {
    }

    public bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, m_MaxDistance);
    }

    public void HandleCharacterMovement()
    {
        // Character Movement
        MoveCharacter();

        // Character Rotation
        CameraRotation();
    }

    public void CameraRotation()
    {
        m_PlayerRotation += m_Controller.GetLookInput() * m_RotationSpeed;
        m_PlayerRotation.z = 0.0f;

        m_PlayerRotation.y = Mathf.Clamp(m_PlayerRotation.y, -90.0f, 90.0f);
        Quaternion xQuat = Quaternion.AngleAxis(m_PlayerRotation.x, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(m_PlayerRotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;
    }

    public void MoveCharacter()
    {
        // Set targetDirection to movement input
        Vector3 targetDirection = m_Controller.GetMoveInput();

        // Set targetDirection to world space of player camera based on movement input
        targetDirection = m_PlayerCamera.transform.TransformDirection(targetDirection);

        targetDirection.y = 0.0f;

        // Change the players rotation based on the targetDirection
        transform.position += targetDirection * m_Speed * Time.deltaTime;
    }

}