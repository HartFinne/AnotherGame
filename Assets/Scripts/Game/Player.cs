using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 100f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform groundCheckTransform;

    [Header("Player Components")]
    [SerializeField] private Transform playerHorizontalLook;

    private Rigidbody rigidBody;
    [SerializeField] private Animator animator; // Animator reference
    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private float verticalInput;

    private int score = 0;
    [SerializeField] private Text textScore;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetPlayerInput();
        PlayerRotation();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        HandleJump();

    }

    /// <summary>
    /// Reads player input for movement and jumping
    /// </summary>
    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }
    }

    /// <summary>
    /// Handles player movement relative to rotation
    /// </summary>
    private void MovePlayer()
    {
        Vector3 moveDirection = playerHorizontalLook.forward * verticalInput + playerHorizontalLook.right * horizontalInput;
        rigidBody.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rigidBody.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    /// <summary>
    /// Handles player jumping logic
    /// </summary>
    private void HandleJump()
    {
        if (jumpKeyWasPressed)
        {
            if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
            {
                return;
            }

            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            animator.SetTrigger("jump"); // Trigger jump animation

            jumpKeyWasPressed = false;
        }

       
    }

    /// <summary>
    /// Handles player animations based on movement
    /// </summary>
    private void HandleAnimations()
    {
        bool isMovingForward = verticalInput > 0;
        bool isMovingBackward = verticalInput < 0;
        bool isMovingRight = horizontalInput > 0;
        bool isMovingLeft = horizontalInput < 0;


        animator.SetBool("jog", isMovingForward);
        animator.SetBool("jogback", isMovingBackward);
        animator.SetBool("leftwalk", isMovingLeft);
        animator.SetBool("rightwalk", isMovingRight);
        animator.SetBool("idle", !isMovingForward && !isMovingBackward && !isMovingRight && !isMovingLeft);
    }

    /// <summary>
    /// Rotates the player using Q and E keys
    /// </summary>
    private void PlayerRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            playerHorizontalLook.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            playerHorizontalLook.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Destroy(other.gameObject);
            score += 1;
            UpdateScoreUI();

            if (score >= 7)
            {
                SceneManager.LoadScene("End Screen");
            }
        }


    }

    private void UpdateScoreUI()
    {
        if (textScore != null)
        {
            textScore.text = "Score: " + score.ToString() + " / 7";
        }
    }

}
