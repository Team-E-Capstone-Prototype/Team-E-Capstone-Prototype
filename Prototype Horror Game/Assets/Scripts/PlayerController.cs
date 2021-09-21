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

    // Camera
    public Camera m_PlayerCamera; // First Person Camera
    public float m_RotationSpeed = 2.0f;// Rptation Speed of Camera
    public float m_CameraVerticalAngle = 0.0f; // Vertical Angle of Camera
    public float m_CameraHorizontalAngle = 0.0f; // Vertical Angle of Camera
    Vector3 m_PlayerRotation = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        // Fetch components on the same gameObject
        m_Controller = new MouseKeyPlayerController();
        m_CharacterController = GetComponent<CharacterController>();

        // Cursor setup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCharacterMovement();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnLeftMouseClick();
        }
    }

    void FixedUpdate()
    {
    }

    public bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, m_MaxDistance); // or 
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
        // Set PlayerRotation to mouse input
        m_PlayerRotation += m_Controller.GetLookInput() * m_RotationSpeed;
        m_PlayerRotation.z = 0.0f;

        // Clamp Player Rotation to (-90, 90)
        m_PlayerRotation.y = Mathf.Clamp(m_PlayerRotation.y, -90.0f, 90.0f);

        // Get a rotation based on mouse input around (0, 1, 0)
        Quaternion xQuat = Quaternion.AngleAxis(m_PlayerRotation.x, Vector3.up);

        // Get a rotation based on mouse input around (-1, 0, 0)
        Quaternion yQuat = Quaternion.AngleAxis(m_PlayerRotation.y, Vector3.left);

        // Change the players camera rotation based on Quaternion
        m_PlayerCamera.transform.localRotation = xQuat * yQuat;
    }

    public void MoveCharacter()
    {
        // Set targetDirection to movement input
        Vector3 targetDirection = m_Controller.GetMoveInput();

        // Set targetDirection to world space of player camera based on movement input
        targetDirection = m_PlayerCamera.transform.TransformDirection(targetDirection);

        targetDirection.y = 0.0f;

        // Change the players position based on the targetDirection
        transform.position += targetDirection * m_Speed * Time.deltaTime;
    }

    void OnLeftMouseClick()
    {
        Ray interactionRay = m_PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;

        if (Physics.Raycast(interactionRay, out interactionInfo, 100))
        {
            if (interactionInfo.collider != null)
            {
                GameObject hitObject = interactionInfo.collider.gameObject;

                if (hitObject.tag == "Interactable Object")
                {
                    hitObject.transform.position = transform.position;
                }
            }
        }

    }
}