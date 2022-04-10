using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;     
    [Header("Movement")]
    [SerializeField] private float mass = 2;               
    [SerializeField] private float maxVelocity = 100;      
    [SerializeField] private float acceleration = 100;     // how fast the player moves. dependant on friction
    [SerializeField] private float friction = 10;          // friction of the player when they are moving
    [SerializeField] private float airAcceleration = 10;   // acceleratin in the air. depends on airFriction
    [SerializeField] private float airFriction = 1;        // friction in the air while moving (effects downward movement)
    [Header("Jumping")]
    [SerializeField] private float jumpAcceleration = 300; // one-time force appplied upwards when jumping
    [Header("Crouching")]
    [SerializeField] private float crouchHeight = 1.25f;   // the new hight of player collider after crouching
    [SerializeField] private float crouchSpeed = 0.2f;     // how much the player moves per physics update when crouching
    [Header("Modifiers")]
    [SerializeField] private float walkModifier = 0.5f;    // acceleration is multiplied by this value when walking
    [SerializeField] private float crouchModifier = 0.5f;  // acceleration is multiplied by this value when crouching

    private bool isGrounded;  
    private float origHeight; // the original height of the collider
    private Rigidbody rb;     
    private CapsuleCollider co;       


    private void Awake()
    { 
        rb = GetComponent<Rigidbody>();
        co = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        rb.mass = mass; // mass does not effect movement. only interactions with physics objects
        origHeight = co.height;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(co.bounds.center, Vector3.down, co.bounds.extents.y + 0.1f, groundMask);
        //isGrounded = Physics.SphereCast(co.bounds.center, co.radius, Vector3.down, out RaycastHit hit, co.bounds.extents.y + 0.1f, groundMask);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        Move();
        Crouch();
    }

    private void Update()
    {
        Jump(); // called in Update because of "Input.GetKeyDown()"
        MovementModifiers();
    }

    
    private void Move()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 direction = Vector3.zero;                                // normalized vector representing the direction the player is moving. y = 0
        Vector3 velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // creating velocity variable based on player velocity. excludes y velocity

        // gathering positional data
        if (Input.GetKey(KeyCode.W))
        {
            direction += forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= right;
        }
        direction.y = 0;
        direction.Normalize();

        // ground controls
        if (isGrounded)
        {
            // acceleration
            rb.AddForce(direction * acceleration, ForceMode.Acceleration);
            // friction
            if (velocity.magnitude > 0)
            {
                rb.AddForce(-1 * velocity.normalized * friction * velocity.magnitude, ForceMode.Acceleration);
            }
        }
        // air control
        else if (!isGrounded)
        {
            // acceleration
            rb.AddForce(direction * airAcceleration, ForceMode.Acceleration);
            // friction
            if (velocity.magnitude > 0)
            {
                rb.AddForce(-1 * velocity.normalized * airFriction * velocity.magnitude, ForceMode.Acceleration);
            }
        }
    }
    
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpAcceleration, ForceMode.Acceleration);
        }
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && co.height > crouchHeight)
        {
            float prevHeight = co.height;
            co.height = Mathf.Max(co.height - crouchSpeed, crouchHeight);
            co.center += new Vector3(0, (prevHeight - co.height) / 2, 0);

            // if the player is grounded we move the body down instead of up
            if (isGrounded)
            {
                transform.position -= new Vector3(0, (prevHeight - co.height) / 2, 0) * 2;
            }
        }
        else if (!Input.GetKey(KeyCode.LeftControl) && co.height < origHeight && !Physics.SphereCast(transform.position, co.radius, Vector3.up, out RaycastHit hit, co.bounds.extents.y + 0.1f, groundMask))
        {
            float prevHeight = co.height;
            co.height = Mathf.Min(co.height + crouchSpeed, origHeight);
            co.center -= new Vector3(0, (co.height - prevHeight) / 2, 0);

            // if the player is ground we move the body up instead of down
            if (isGrounded)
            {
                transform.position += new Vector3(0, (co.height - prevHeight) / 2, 0) * 2;
            }
        }
    }

    // modifiers for movement variables go here 
    private void MovementModifiers()
    {
        // crouch modifier
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            acceleration = acceleration * crouchModifier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            acceleration = acceleration / crouchModifier;
        }

        // walk modifier
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            acceleration = acceleration * walkModifier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            acceleration = acceleration / walkModifier;
        }
    }
}