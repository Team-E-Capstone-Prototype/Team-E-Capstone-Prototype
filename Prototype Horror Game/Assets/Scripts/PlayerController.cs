using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Controllers
    MouseKeyPlayerController m_Controller;          // Player Input Controls
    CharacterController m_CharacterController;      // Character Controller

    // Player
    public float m_Speed = 10.0f;                   // Movement Speed
    private Vector3 m_JumpVelocity;                 // Jump Velocity
    private bool m_isGrounded = true;               // Is Player Grounded?
    private float m_JumpHeight = 1.0f;              // Jump Height
    private float gravityValue = -9.81f;            // Gravity
    public GameObject m_Hand;                       // Player Hand

    // Camera
    public Camera m_PlayerCamera;                   // First Person Camera
    public float m_RotationSpeed = 2.0f;            // Rptation Speed of Camera
    public float m_CameraVerticalAngle = 0.0f;      // Vertical Angle of Camera
    public float m_CameraHorizontalAngle = 0.0f;    // Vertical Angle of Camera

    // Raycasts
    Ray interactionRay;                             // Interaction Ray
    RaycastHit interactionInfo;                     // Structure used to get information back from a raycast
    GameObject hitObject;                           // Object Hit with Raycast
    GameObject objectInHand;                        // Object in Player Hand

    // Player

    // Health
    public int m_MaxHealth = 100;                   // Max Health of Player
    public int m_CurrentHealth;                     // Current Health of Player
    public HealthBar m_HealthBar;                   // UI Health Bar

    // Sanity
    public int m_MaxSanity = 20;                    // Max Sanity of Player
    public float m_CurrentSanity;                   // Current Sanity of Player
    public SanityBar m_SanityBar;                   // UI Sanity Bar

    // Misc
    bool m_isObjectHeld = false;                    // Is Object Held?
    private GameObject lastSelectedObject = null;   // Last Selected Object
    private Color lastSelectedColor;                // Last Selected Object Color
    int objectHoldingSpeed;                         // Speed of Held Object

    AudioSource m_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Player Health
        {
            m_CurrentHealth = m_MaxHealth;
            m_HealthBar.SetMaxHealth(m_MaxHealth);

            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
        }

        // Player Sanity
        {
            m_CurrentSanity = m_MaxSanity;
            m_SanityBar.SetMaxSanity(m_MaxSanity);

            m_CurrentSanity = Mathf.Clamp(m_CurrentSanity, 0, m_MaxSanity);
        }

        // Fetch components on the same gameObject
        {
            m_Controller = new MouseKeyPlayerController();
            m_CharacterController = GetComponent<CharacterController>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        m_AudioSource.enabled = false;

        // Cursor setup
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleCharacterMovement();

        HighlightObjects();

        ResetHighlightedObject();

        CheckCharacterInput();

        // If Hit Object is Enemy...
        if (hitObject != null && hitObject.tag == "Enemy")
        {
            // Calculate Sanity Damage
            float sanityDamage = 1 * Time.deltaTime;

            // Lose Sanity based on Sanity Damage
            LoseSanity(sanityDamage);
        }

        // Update Health
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);

        // Update Sanity
        m_CurrentSanity = Mathf.Clamp(m_CurrentSanity, 0, m_MaxSanity);
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

    public void CheckCharacterInput()
    {
        // If Fire1 is pressed, interact with object
        if (Input.GetButton("Fire1"))
        {
            OnLeftMouseClick();
        }

        // If Mouse Left Cicked is pressed, Drop Object in Hand
        if (Input.GetMouseButtonUp(0))
        {
            if (m_isObjectHeld)
            {
                DropObject();
            }
        }

        // If X is pressed, Current Health takes 10 damage
        if (Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(10);
        }

        // If C is pressed, increase Current Health to 20
        if (Input.GetKeyDown(KeyCode.C))
        {
            HealPlayer(20);
        }

        // If M is pressed, reset Current Health to 100
        if (Input.GetKeyDown(KeyCode.M))
        {
            m_CurrentHealth = 0;
            HealPlayer(100);
        }

        // If T is pressed, reset Current Sanity to 20
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_CurrentSanity = 0;
            GainSanity(20);
        }
    }

    // CameraRotation is called when rotating character
    public void CameraRotation()
    {
        // Horizontal character rotation
        {
            // Rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3(0f, (m_Controller.GetLookInput().x * m_RotationSpeed * 1.0f), 0f), Space.Self);
        }

        // Vertical camera rotation
        {
            // Add vertical inputs to the camera's vertical angle
            m_CameraVerticalAngle -= m_Controller.GetLookInput().y * m_RotationSpeed;

            // Limit the camera's vertical angle to min/max
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -90.0f, 90.0f);

            // Apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            m_PlayerCamera.transform.localRotation = Quaternion.Euler(m_CameraVerticalAngle, 0.0f, 0.0f);
        }
    }

    // MoveCharacter is called when moving character
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

    // JumpAndGravity is called when character jumping and dealing with gravity
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

    // OnLeftMouseClick is called when interacting with objects
    void OnLeftMouseClick()
    {
        // If Hit Object is a Movable Door or Interactable Object...
        if (hitObject.tag == "Moveable Door" || hitObject.tag == "Interactable Object")
        {
            // If Object Held is false...
            if (m_isObjectHeld == false)
            {
                Debug.Log("Trying Pickup");

                // Try picking up object 
                TryPickup();
            }
            //hitObject.transform.position = transform.position;
            else
            {
                Debug.Log("Holding");

                // Hold Object
                holdObject();
            }
        }
    }

    // HighlightObjects is called when looking at an object
    void HighlightObjects()
    {
        //interactionRay = m_PlayerCamera.ScreenPointToRay(Input.mousePosition);
        interactionRay = m_PlayerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(interactionRay, out interactionInfo, 100))
        {
            if (interactionInfo.collider.gameObject != null)
            {
                hitObject = interactionInfo.collider.gameObject;
                if (hitObject.tag == "Interactable Object" || hitObject.tag == "Moveable Door")
                {
                    if (lastSelectedObject != null)
                    {
                        ResetObjectColor();
                    }

                    lastSelectedObject = hitObject;

                    lastSelectedColor = lastSelectedObject.GetComponent<Renderer>().material.color;

                    if (hitObject.name == "LightBox" || hitObject.name == "ModerateBox" || hitObject.name == "HeavyBox")
                    {
                        // Temporary demonstration hack to keep example objects from highlighting
                    }
                    else
                    {
                        hitObject.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    // Display Hand Grab UI
                    this.GetComponent<UIAppear>().ShowHandGrabUI();
                }
            }
        }
    }

    // ResetHighlightedObject is called when resetting a hightlighted object
    void ResetHighlightedObject()
    {
        if (hitObject != null && hitObject.tag != "Interactable Object")
        {
            if (lastSelectedObject != null)
            {
                ResetObjectColor();

                // Hide Hand Grab UI
                this.GetComponent<UIAppear>().HideHandGrabUI();

            }
        }
    }

    // TryPickup is called when attempting to pickup an object
    void TryPickup()
    {
        if (m_isObjectHeld == false)
        {
            Ray playerAim = m_PlayerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(playerAim, out hitInfo, 3.0f))
            {
                objectInHand = hitInfo.collider.gameObject;

                //if (objectInHand.tag == "Moveable Door")

                m_isObjectHeld = true;
                objectInHand.GetComponent<Rigidbody>().useGravity = true;
                objectInHand.GetComponent<Rigidbody>().freezeRotation = false;

            }
        }
    }

    // holdObject is called when holding an object
    void holdObject()
    {
        Ray playerAim = m_PlayerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 nextPos = m_PlayerCamera.transform.position + playerAim.direction * 3f;
        Vector3 currPos = objectInHand.transform.position;

        float heldObjectMass = objectInHand.GetComponent<Rigidbody>().mass;

        // Temporary hack for objects to have varying weight
        if (heldObjectMass >= 58.0f)
        {
            objectHoldingSpeed = 0;
        }
        else if (heldObjectMass >= 40.0f)
        {
            objectHoldingSpeed = 2;
        }
        else if (heldObjectMass >= 22.0f)
        {
            objectHoldingSpeed = 4;
        }
        else if (heldObjectMass >= 4.0f)
        {
            objectHoldingSpeed = 8;
        }
        else
        {
            objectHoldingSpeed = 10;
        }

        objectInHand.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * objectHoldingSpeed;
    }

    // DropObject is called when dropping an object
    void DropObject()
    {
        Debug.Log("Dropping");
        m_isObjectHeld = false;
        objectInHand.GetComponent<Rigidbody>().useGravity = true;
        objectInHand.GetComponent<Rigidbody>().freezeRotation = false;
        objectInHand = null;
    }

    // ResetObjectColor is called when resetting the last selected object color
    public void ResetObjectColor()
    {
        lastSelectedObject.GetComponent<Renderer>().material.color = lastSelectedColor;
        lastSelectedObject = null;
    }

    // TakeDamage is called when player takes damage
    void TakeDamage(int damage)
    {
        m_CurrentHealth -= damage;

        m_HealthBar.SetHealth(m_CurrentHealth);
    }

    // HealPlayer is called when player heals
    void HealPlayer(int healAmount)
    {
        m_CurrentHealth += healAmount;

        m_HealthBar.SetHealth(m_CurrentHealth);
    }

    // LoseSanity is called when player losses sanity
    void LoseSanity(float damage)
    {
        m_CurrentSanity -= damage;

        m_SanityBar.SetSanity(m_CurrentSanity);
    }

    // GainSanity is called when player gains sanity
    void GainSanity(int sanityGained)
    {
        m_CurrentSanity += sanityGained;

        m_SanityBar.SetSanity(m_CurrentSanity);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Audio Zone")
        {
            m_AudioSource.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Audio Zone")
        {
            m_AudioSource.enabled = false;
        }
    }
}