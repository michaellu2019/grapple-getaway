using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public float speed = 5f;
	public float jumpSpeed = 5f;
	private float movement = 0f;
	private Rigidbody2D rb;
	public  Transform groundCheckPoint;
	public float groundCheckRadius;
	public LayerMask groundLayer;
	public LayerMask goalLayer;
	private bool touchingGround;
	private bool touchingGoal;
	public float score = 0;
	public float health = 100f;
	public bool enableMovementOnGrapple = true;
	// private Animator playerAnimation;

	public GrapplingGunController grappleGun;

	private float swingSpeed = 4f;

	private int numLeftSwings = 0;
	private int numRightSwings = 0;

	private const int MAX_SWINGS = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		// playerAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0) 
		{
			touchingGoal = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, goalLayer);
			if (touchingGoal) {
				SceneManager.LoadScene(0);	// Reset to title screen
			}

			touchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

			movement = Input.GetAxis("Horizontal");
			// Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			// Vector2 pos = new Vector2(transform.position.x, transform.position.y);
			// Vector2 dir = new Vector2((mousePos - pos).x, 0);
			// dir.Normalize();
			// transform.localScale = new Vector2(dir.x * Mathf.Abs(transform.localScale.x), transform.localScale.y);
			bool grappling = grappleGun.grappleRope.isGrappling;
			bool moveOnGrapple = enableMovementOnGrapple || (!enableMovementOnGrapple && !grappling);
			if ((movement > 0f || movement < 0f) && !grappling) {
				rb.velocity = new Vector2(movement * speed, rb.velocity.y);
			}

			if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && touchingGround)
			{
				rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
			}

			if (grappling && enableMovementOnGrapple) 
			{
				if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
				{
					grappleGun.desiredDistance += 0.05f;
				} else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				{
					grappleGun.desiredDistance -= 0.05f;
				}

				if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
				{
					rb.velocity -= new Vector2(swingSpeed * numLeftSwings, 0);
					numLeftSwings = Mathf.Min(numLeftSwings + 1, MAX_SWINGS);
					// Debug.Log("LEFT");
				} 
				if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
				{
					rb.velocity += new Vector2(swingSpeed * numRightSwings, 0);
					numRightSwings = Mathf.Min(numRightSwings + 1, MAX_SWINGS);
					// Debug.Log("RIGHT");
				}
				// Debug.LogFormat("Left {0} Right {1} \n", numLeftSwings, numRightSwings);
			} else {
				numLeftSwings = 0;
				numRightSwings = 0;
			}

			// Vector3 mousePosition = Input.mousePosition;
			// mousePosition.z = 10;
			// // mousePosition.z = gunPivot.position.z;
			// Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

			// Debug.Log("PLMs");
			// Debug.Log(mouseWorldPosition);
			// Debug.Log(transform.position);

			// playerAnimation.SetFloat("Movement", Mathf.Abs(movement));
	    }

    	// playerAnimation.SetBool("Alive", health > 0);
    	// playerAnimation.SetBool("OnGround", touchingGround);
    }
}
