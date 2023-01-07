using System;
using System.Collections;
using System.Linq.Expressions;
using System.Numerics;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementPhysics : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration;
    public float maxSpeed;
    public float rotationSpeed;
    [Tooltip("Does the thing")]
    public float rotationSpeedIncrement = 175f;
    public float moveFOV;
    public float approachMoveFOVSpeed;
    public float leaveMoveFOVSpeed;
    public float slowDownSpeed;
    public bool handleSlopes = false;

    [Space(10)]
    public float jumpForce;
    public float jumpLungeForce;
    private bool _jumping;
    private bool _jumped;
    public bool freezeRotation;
    public float coyoteTime = 0.2f;
    private float _coyoteTimeCounter;

    [Space(10)]
    public float groundDrag;
    public float airDrag;
    public float airMultiplier;

    [Header("Ground Check")]
    public float playerHeight;
    public float heightError;
    public LayerMask groundCheck;
    private bool grounded;
    public float groundDistance = 0.4f;


    [Header("Other Required Fields")]
    public Transform orientation;
    public new CinemachineFreeLook camera;
    public Transform cameraDir;
    
    private float _defaultFOV;

    // Other Private fields
    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = freezeRotation;
        _defaultFOV = camera.m_Lens.FieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(transform.position - new Vector3(0, playerHeight / 2, 0), groundDistance,
            groundCheck);
        _jumping = Input.GetButtonDown("Jump");

        MyInput();

        LimitSpeed();
        
        if (handleSlopes)
            HandleSlopes();

        Jump();
        Drag();
    }

    private void FixedUpdate()
    {
        Move();

    }

    private void LateUpdate()
    {
        CoyoteTime();
    }


    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }


    private void Move()
    {


        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);

        if (moveDirection != Vector3.zero)
        {
            
            // Handles player rotation towards movement
            
            
            Quaternion toRotation = Quaternion.LookRotation( new Vector3(moveDirection.x, 0, moveDirection.z).normalized, Vector3.up);
            float rotationDifference = Quaternion.Angle(transform.rotation, toRotation);

            float rotationSpeedFinal =
                rotationSpeed /* Time.fixedDeltaTime*/ * (rotationDifference / rotationSpeedIncrement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeedFinal);
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, moveFOV, approachMoveFOVSpeed * Time.fixedDeltaTime);
        }
        else
        {
            camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, _defaultFOV, leaveMoveFOVSpeed * Time.fixedDeltaTime);
        }

        if (grounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * acceleration * rb.mass, ForceMode.Acceleration);
        }
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * acceleration * rb.mass, ForceMode.Acceleration);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * acceleration * rb.mass * airMultiplier, ForceMode.Acceleration);
        }
        
        // WHEN HANDLE SLOPES = TRUE, ERROR WITH TURNING, FIGURE OUT LATER //
        
        
        if (handleSlopes)
        // Rotate player based on ground
            if (slopeMoveDirection != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(slopeMoveDirection.normalized, slopeHit.normal);
            else
            {
                transform.rotation = Quaternion.LookRotation(transform.forward.normalized, slopeHit.normal);
            }
    }

    private void Drag()
    {
        if (grounded)
        {
            rb.drag = groundDrag;
        } 
        else
        {
            rb.drag = airDrag;
        }
    }

    private void LimitSpeed()
    {
        
        // Getting flat velocity
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Checking if travelling too fast
        if (flatVelocity.magnitude > maxSpeed && grounded)
        {
            Vector3 maxVelocity = flatVelocity.normalized * maxSpeed;
            Vector3 targetMax = new Vector3(maxVelocity.x, rb.velocity.y, maxVelocity.z);
            rb.velocity = Vector3.Slerp(rb.velocity, targetMax, slowDownSpeed * Time.deltaTime);
            //rb.velocity = new Vector3(maxVelocity.x, rb.velocity.y, maxVelocity.z);
        }
    }

    private void Jump()
    {
        
        // Checking if can jump
        if (_jumping && _coyoteTimeCounter > 0)
        {
            // Set coyote time to 0
            _coyoteTimeCounter = 0f;
            _jumped = true;
            
            // Applying jump force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (moveDirection != Vector3.zero)
            {
                // If player is moving apply boost in direction of movement
                //rb.AddForce(moveDirection.normalized * rb.velocity.magnitude * jumpLungeForce, ForceMode.Impulse);
                rb.velocity *= jumpLungeForce;
            }

            // Applying rotation only if freezeRotation = false
            Vector3 rotateAxis = Vector3.Cross(Vector3.up, moveDirection.normalized);
            rb.AddTorque(rotateAxis * 10000000, ForceMode.Impulse);
        }

        

    }

    private void CoyoteTime()
    {
        if (grounded && !_jumping)
        {
            _jumped = false;    
            _coyoteTimeCounter = coyoteTime;
        }
        else if (!_jumped)
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private RaycastHit slopeHit;
    private Vector3 slopeMoveDirection;
    
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight/2 + 0.5f))
        {
            return slopeHit.normal != Vector3.up && handleSlopes;
        }

        return false;
    }

    private void HandleSlopes()
    {
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    
}

