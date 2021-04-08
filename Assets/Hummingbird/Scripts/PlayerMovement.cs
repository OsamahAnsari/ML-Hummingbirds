using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    private float moveSpeed;
    public float runSpeed;
    public float sprintSpeed;
    
    public float slideSpeed;
    public float slideTimerMax;
    private float slideTimer = 0.0f;

    private float stunTimer = 0.0f;
    public float stunTimerMax;
    public float speedChangeTimerMax;
    private float speedChangeTimer = 0.0f;
    private bool speedingUp = false;
    private bool slowingDown = false;

    public float sprintTimerMax;
    private float sprintTimer = 0.0f;
    private bool isSprinting = false;

    private Vector3 jump;
    public float jumpVelocity;
    private bool isGrounded = false;
    public float fallMultiplier;

    private Rigidbody rb;

    private bool canMove = true;

    private bool isIdle = true;
    private bool isRunning = true;
    private bool isSliding = false;
    private bool isJumping = false;
    private bool isStunned = false;

    public GameObject flower;

    public TextMeshProUGUI displayText;

    public void FreezePlayer()
    {
        canMove = false;
    }

    public void UnfreezePlayer()
    {
        canMove = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        moveSpeed = runSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }
        
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isStunned = true;
            StartCoroutine(DisplayText("Stunned!"));
        }
    }

    IEnumerator DisplayText(string message)
    {
        displayText.text = message;
        yield return new WaitForSeconds(1f);
        displayText.text = "";
    }

    private void Update()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canMove)
        {
            rb.velocity += Vector3.up * jumpVelocity;
            //rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = true;
        }
        // Slide
        else if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && !isSliding && !isStunned && !isSprinting && canMove)
        {
            isSliding = true;
            slideTimer = 0.0f;
            transform.Rotate(Vector3.right, -90f);
        }
        // Sprint
        else if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !isSliding && !isStunned && !isSprinting && !slowingDown && canMove)
        {
            isSprinting = true;
            sprintTimer = 0.0f;
        }
        
        if (isSliding)
        {
            moveSpeed = slideSpeed;

            slideTimer += Time.deltaTime;
            if (slideTimer > slideTimerMax)
            {
                isSliding = false;
                transform.Rotate(Vector3.right, 90f);
                moveSpeed = runSpeed;
            }
        }

        if(isStunned)
        {
            stunTimer += Time.deltaTime;
            
            if (stunTimer > stunTimerMax)
            {
                speedingUp = true;
                isStunned = false;
                stunTimer = 0.0f;                
            }
            else
            {
                moveSpeed = 0f;
            }
        }
        
        if (isSprinting)
        {
            sprintTimer += Time.deltaTime;

            if (sprintTimer > sprintTimerMax)
            {
                slowingDown = true;
                isSprinting = false;
            }
            else
            {
                moveSpeed = sprintSpeed;
            }
            
        }

        if (speedingUp)
        {
            speedChangeTimer += Time.deltaTime;

            moveSpeed = Mathf.Lerp(0f, runSpeed, speedChangeTimer / speedChangeTimerMax);

            if (speedChangeTimer > speedChangeTimerMax)
            {
                speedChangeTimer = 0.0f;
                speedingUp = false;
            }
        }

        if (slowingDown)
        {
            speedChangeTimer += Time.deltaTime;

            moveSpeed = Mathf.Lerp(sprintSpeed, runSpeed, speedChangeTimer / speedChangeTimerMax);

            if (speedChangeTimer > speedChangeTimerMax)
            {
                speedChangeTimer = 0.0f;
                slowingDown = false;
            }
        }
    }

    private void LateUpdate()
    {
        animator.SetBool("Stunned", isStunned);
        animator.SetBool("Running", isRunning);
        animator.SetBool("Sliding", isSliding);
        animator.SetBool("Jumping", isJumping);
        animator.SetBool("Sprinting", isSprinting);
        animator.SetBool("Idle", isIdle);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (canMove)
        {
            float moveVertical = Input.GetAxis("Vertical");
            float moveHorizontal = Input.GetAxis("Horizontal");

            if (moveVertical != 0 || moveHorizontal != 0)
            {
                isRunning = true;
                isIdle = false;
            }
            else
            {
                isRunning = false;
                isIdle = true;
            }

            Vector3 targetDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);

            if (targetDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            }

            transform.Translate(targetDirection * moveSpeed * Time.deltaTime, Space.World);

            if (!isSliding && !isJumping)
            {
                flower.transform.localPosition = new Vector3(0f, flower.transform.localPosition.y, 0.3f);
            }
            else
            {
                flower.transform.localPosition = new Vector3(0f, flower.transform.localPosition.y, 0f);
            }
        }
        else
        {
            isRunning = false;
            isIdle = true;
        }
    }
}
