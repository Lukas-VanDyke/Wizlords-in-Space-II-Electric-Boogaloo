using UnityEngine;
using System.Collections;

public class p2control : MonoBehaviour {
	//public GameObject eyeOfNewt; //  item 0
	//public GameObject featherOfBird; // item 1
	//public GameObject herbOfEnlightenment; // item 2
	public GameObject gc;
    public GameObject wintext;

    private int[] inventory;

	[HideInInspector]
	public bool facingRight = true;
	[HideInInspector]
	public bool jump = false;

	public AudioClip dudSound;
	public AudioClip fireSound;
	public AudioClip darkSound;
	public AudioClip iceSound;
	public AudioClip windSound;
	public AudioClip jumpSound;
	public float moveForce = 50f;
	public float maxSpeed = 5f;
	public float jumpForce = 100f;
	public Transform groundCheck;
	private int next = 0;

	public Animator slot0;
	public Animator slot1;
	public Animator slot2;

	private Animator anim;
	private Rigidbody2D rb2d;

	private bool grounded = false;    

	private bool onFire = false; // FIRE
	public int fireTime = 300;

	private bool onIce = false; // ICE
	public int iceTime = 300;

	private bool windy = false; // WIND
	public int windTime = 500;
	Vector2 wind;

	private bool darkness = false; // DARK
	public int darkTime = 50;
	public GameObject dark;

	private int spellTime = 0;

    public bool lost = false;
    public bool won = false;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		inventory = new int[3];
	}

	// Update is called once per frame
	void Update()
	{


		grounded = Physics2D.Linecast(this.transform.position, groundCheck.position,1 << LayerMask.NameToLayer("Ground"));

		if (Input.GetKeyDown (KeyCode.UpArrow) && grounded) {
			jump = true;
			SoundManager.instance.PlaySingle (dudSound);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow) && grounded) {

		if (inventory[0] != 0 && inventory[1] != 0 && inventory[2] != 0)
		{
			if (inventory[0] == inventory[1] && inventory[1] == inventory[2])
			{
				anim.SetBool("cast2", true);
				gc.GetComponent<GameController>().cast(1, inventory[0]);
			}
			else if (inventory[0] != inventory[1] && inventory[1] != inventory[2] && inventory[2] != inventory[0])
			{
				anim.SetBool("cast2", true);
				gc.GetComponent<GameController>().cast(1, 4);
			}
			else
			{
				anim.SetBool("cast3", true);
					SoundManager.instance.PlaySingle (jumpSound);
			}
			clear(); // delete    
		}
		else
		{
			anim.SetBool("cast1", true);
		}

	}
	}
	void FixedUpdate()
	{
        Debug.Log("p2: " + won);

        slot0.SetInteger ("pickUpValue", inventory[0]);
		slot1.SetInteger ("pickUpValue", inventory[1]);
		slot2.SetInteger ("pickUpValue", inventory[2]);

		float h = 0;
		if (Input.GetKey (KeyCode.RightArrow)) {
			h = 1.0f;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			h = -1.0f;
		}
		anim.SetFloat("Speed", Mathf.Abs(h));
		anim.SetFloat("vMovement",this.GetComponent<Rigidbody2D>().velocity.y);
		anim.SetBool("grounded",grounded);

		if (!onFire && !lost)
		{
			if (h * rb2d.velocity.x < maxSpeed) {
				rb2d.AddForce (Vector2.right * h * moveForce);
			}

			if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
				rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

			if (h > 0 && !facingRight)
				Flip();
			else if (h < 0 && facingRight)
				Flip();

			if(!onIce)
			{
				rb2d.velocity = new Vector2(0, rb2d.velocity.y);
			}else{
				spellTime++;
				if(spellTime > iceTime){
					spellTime = 0;
					onIce =false;
					anim.SetBool("stillIce",false);
				}
			}
			if (windy)
			{
				Debug.Log("wind");

				if (spellTime % 100 == 0)
				{
					if (Random.Range(0, 100) < 51)
					{
						wind = (Vector2.right * moveForce * 0.25f);
					}
					else
					{
						wind = (Vector2.left * moveForce * 0.25f);
					}
				}
				rb2d.AddForce(wind);
				spellTime++;
				if (spellTime > windTime)
				{
					spellTime = 0;
					windy = false;
				}

			}

			if (jump)
			{
				anim.SetTrigger("Jump");
				if (!onIce) {
					rb2d.AddForce (new Vector2 (0f, jumpForce));
				} else {
					rb2d.AddForce(new Vector2(0f, 0.75f*jumpForce));
				}
				jump = false;
			}
		}else if (onFire) //on fire effects
		{
			if(spellTime % 25 < 12){
				if(!facingRight)Flip();
				rb2d.transform.position = rb2d.transform.position + new Vector3(0.1f,0,0);
			}
			else{
				if(facingRight)Flip();
				rb2d.transform.position = rb2d.transform.position - new Vector3(0.1f,0,0);
			}

            if (won)
            {
                wintext.SetActive(true);
            }

            spellTime++;
			if(spellTime > fireTime){
				spellTime = 0;
				onFire = false;
				anim.SetBool("stillFire",false);
                // WON!!!
                if (won)
                {
                    Application.LoadLevel("menu");
                }
			}
		}
		if (darkness)
		{
			spellTime++;
			if (spellTime > darkTime)
			{
				darkness = false;
				dark.SetActive(false);
				spellTime = 0;
			}
		}


	}

	public void clear(){
		inventory [0] = 0;
		inventory [1] = 0;
		inventory [2] = 0;
	}

	public void pickup(GameObject aitim){
		if (aitim.CompareTag("e")){
			inventory [next] = 1;
		}else if(aitim.CompareTag("f")){
			inventory [next] = 2;
		}else{
			inventory [next] = 3;
		}
		next++;
		next = next%3;
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void castSpell(int spellID)
	{
		// Fire testing
		switch (spellID)
		{
		case 1: // darkness
			darkness = true;
			dark.SetActive (true);
			SoundManager.instance.PlaySingle (darkSound);
			break;
		case 2: // wind
			windy = true;
			SoundManager.instance.PlaySingle (windSound);
			break;
		case 3: // fire
			anim.SetTrigger("onFire");
			anim.SetBool("stillFire", true);
			SoundManager.instance.PlaySingle (fireSound);
			onFire = true;
			break;
		case 4: // ice
			anim.SetTrigger("onIce");
			anim.SetBool("stillIce", true);
			SoundManager.instance.PlaySingle (iceSound);
			onIce = true;
			break;

		}
	}

}
