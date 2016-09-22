using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 5;
	private float moveSpeedStore;
	public float jumpForce = 10;
	public float speedMultiplier;
	public float speedIncreaseMilestone;
	public float speedIncreaseMilestoneStore;
	private float speedMilestoneCount;
	private float speedMilestoneCountStore;

	private float jumpTime = 0.25f; // max air time
	private float jumpTimeCounter;

	private bool stoppedJumping = true;

	private Rigidbody2D rb2;

	public bool grounded = false;
	public LayerMask groundRef;

	public Transform groundCheck;
	public float groundCheckRadius;

	private Animator anim;

	public GameManager gameManager;

	//private Collider2D collider2;
	// Use this for initialization
	void Start () {
		rb2 = GetComponent<Rigidbody2D> ();
		//collider2 = GetComponent<Collider2D> ();
		anim = GetComponent<Animator> ();
		jumpTimeCounter = jumpTime;
		speedMilestoneCount = speedIncreaseMilestone;

		// store all the stuff
		moveSpeedStore = moveSpeed;
		speedMilestoneCountStore = speedMilestoneCount;
		speedIncreaseMilestoneStore = speedIncreaseMilestone;
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundRef);

		if (transform.position.x > speedMilestoneCount) {
			speedMilestoneCount += speedIncreaseMilestone;
			moveSpeed *= speedMultiplier;
			speedIncreaseMilestone *= speedMultiplier;
		}

		rb2.velocity = new Vector2 (moveSpeed, rb2.velocity.y); // player speed is constant to the right

		if ((Input.GetKeyDown (KeyCode.UpArrow) || Input.GetMouseButtonDown (0)) && grounded) {
			rb2.velocity = new Vector2 (rb2.velocity.x, jumpForce);
			stoppedJumping = false;
		}

		if ((Input.GetKey (KeyCode.UpArrow) || Input.GetMouseButton (0)) && !stoppedJumping) {
			if (jumpTimeCounter > 0) {
				rb2.velocity = new Vector2 (rb2.velocity.x, jumpForce);
				jumpTimeCounter -= Time.deltaTime;
			}
		}

		if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetMouseButtonUp (0)) {
			jumpTimeCounter = 0;
			stoppedJumping = true;
		}

		if (grounded)
			jumpTimeCounter = jumpTime;

		anim.SetFloat ("speed", rb2.velocity.x); // can't run up
		anim.SetBool("grounded", grounded);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "killbox") {
			gameManager.RestartGame ();
			moveSpeed = moveSpeedStore;
			speedMilestoneCount = speedMilestoneCountStore;
			speedIncreaseMilestone = speedIncreaseMilestoneStore;
		}
	}
}
