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
    private Vector3 m_JumpVelocity; // Jump Velocity
    private bool m_isGrounded = true; // Is Player Grounded?
    private float m_JumpHeight = 1.0f; // Jump Height
    private float gravityValue = -9.81f; // Gravity

    // Camera
    public Camera m_PlayerCamera; // First Person Camera
    public float m_RotationSpeed = 2.0f;// Rptation Speed of Camera
    public float m_CameraVerticalAngle = 0.0f; // Vertical Angle of Camera
    public float m_CameraHorizontalAngle = 0.0f; // Vertical Angle of Camera
    Vector3 m_PlayerRotation = Vector3.zero;

    // Raycasts
    Ray interactionRay;
    RaycastHit interactionInfo;
    GameObject hitObject;

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

        HighlightObjects();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnLeftMouseClick();
        }

        ResetHighlightedObject();
    }

    void FixedUpdate()
    {
    }

    public void HandleCharacterMovement()
    {
        // Check if player is grounded
        m_isGrounded = m_CharacterController.isGrounded;

        // Set Jump Velocity to 0 if the player is grounded and the velocity is less than 0
        if (m_isGrounded && m_JumpVelocity.y < 0)
        {
            m_JumpVelocity.y = 0f;
        }

        // Character Movement
        MoveCharacter();

        // Character Rotation
        CameraRotation();

        // Character Jump and Gravity
        JumpAndGravity();
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

        // Change the players movement based on the targetDirection
        m_CharacterController.Move(targetDirection * m_Speed * Time.deltaTime);
    }


    public void JumpAndGravity()
    {
        // If player input for Jump is pressed and is grounded...
        if (m_Controller.IsJumping() && m_isGrounded)
        {
            // Get a Jump Velocity based on Jump Height and Gravity
            m_JumpVelocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * gravityValue);
        }

        // Gravity 
        m_JumpVelocity.y += gravityValue * Time.deltaTime;

        // Change the players movement based on the Jump Velocity
        m_CharacterController.Move(m_JumpVelocity * Time.deltaTime);
    }

    void OnLeftMouseClick()
    {
        if (hitObject.tag == "Interactable Object")
        {
            hitObject.transform.position = transform.position;
        }
    }

    void HighlightObjects()
    {
        interactionRay = m_PlayerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(interactionRay, out interactionInfo, 100))
        {
            if (interactionInfo.collider.gameObject != null)
            {
                hitObject = interactionInfo.collider.gameObject;
                if (hitObject.tag == "Interactable Object")
                {
                    hitObject.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
        }
    }

    void ResetHighlightedObject()
    {
        if (hitObject !=null && hitObject.tag != "Interactable Object")
        {
            GameObject selectedObject = GameObject.FindGameObjectWithTag("Interactable Object");

            selectedObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}