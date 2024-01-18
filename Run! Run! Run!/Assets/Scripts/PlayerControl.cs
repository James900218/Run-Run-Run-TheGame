using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public DirectorScript director;
    public Transform orientation;

    float healthPoints = 100;
    public bool isHit = false;
    public GameObject damageUI;

    float horizontalInput;
    float verticalInput;

    Rigidbody rb;

    Vector3 moveDirection;

    //keybinds
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode crouch = KeyCode.LeftControl;

    // run,walk,crouch
    public float moveSpeed;

    public float walkSpeed; // players base speed when inputting a movement key

    public float runSpeed;  // players speed when moving with an inputting for "Sprint" key
    bool isSprinting = false;
    public float runDuration; // time in seconds, player can run for before becoming tired
    private float runTimer;

    public float slowWalkSpeed;  // players speed when crouching
    public bool isCrouching = false;

    // ground checking for jump
    public float playerHeight;
    public LayerMask isGround;
    bool grounded;
    public float groundDrag;

    // Jumping
    public float jumpForce;
    public float jumpCooldown;
    public float airMultplier;
    bool readyToJump = true;

    // objective counter
    public int numKeys;

    // audio
    public AudioSource movementSFX;
    public AudioSource damageSFX;
    public AudioSource keysSFX;
    Vector3 currentPosition;
    Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        runTimer = runDuration;
        currentPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();
        CheckHealth();

        // ground check
        // bool set true when raycast from players feet hits isGround layer
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, isGround);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }

        if (Input.GetKey(sprint) && !isCrouching)
        {
            isSprinting = true;

            runTimer -= Time.deltaTime;
        }
        else
        {
            isSprinting = false;
            runTimer = runDuration;
        }

        if (Input.GetKey(crouch))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        FootStepSFX();
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on Ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in Air
        if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultplier, ForceMode.Force);
        }

    }

    private void SpeedControl()
    {
        // get 2D velocity
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (isSprinting)
        {
            moveSpeed = runSpeed;
        }
        else if (isCrouching)
        {
            moveSpeed = slowWalkSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        // cap velocity if needed
        if (flatVelocity.magnitude > moveSpeed)
        {
           // normalize and multiply by desired speed
           Vector3 VelocityLimit = flatVelocity.normalized * moveSpeed;
           // set new velocity
           rb.velocity = new Vector3(VelocityLimit.x, rb.velocity.y, VelocityLimit.z);
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    public void CheckHealth()
    {
        if (isHit == true)
        {
            damageSFX.PlayDelayed(0.5f);
            healthPoints -= 50;
            damageUI.gameObject.SetActive(true);
            isHit = false;
        }

        if (healthPoints <= 0)
        {
            Debug.Log("DEATH");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }

    }   

    public void AddToKeyCount()
    {
        // when called, add to key count
        numKeys++;
        keysSFX.Play();
        // if all keys are found, end the game
        if (numKeys >= 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        director.BuffEnemy();
    }

    public void FootStepSFX()
    {
        newPosition = this.transform.position;

        if (Vector3.Distance(currentPosition, newPosition) >= 3 && grounded)
        {
            movementSFX.Play();
            currentPosition = newPosition;
            //Debug.Log("play footstepSFX");
        }
    }
}
