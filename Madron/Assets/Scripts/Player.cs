using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Interfaces;

public class Player : MovingObject
{
    public float restartLevelDelay = 1f;
    public int pointsPerSmallHeart = 10;      
    public int pointsPerBigHeart = 20;      
    public int damage = 1;

    public Text energyText;
    
    public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1;
	public AudioClip drinkSound2;
	public AudioClip gameOverSound;

    private Animator animator;          
    private int energy;                   

    //Start overrides the Start function of MovingObject
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        energy = GameManager.instance.playerEnergyPoints;

        energyText.text = "Energy: " + energy;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerEnergyPoints = energy;
    }

    private void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0)
			vertical = 0;

        #else

		if (Input.touchCount > 0)
		{
			Touch myTouch = Input.touches[0];

			if (myTouch.phase == TouchPhase.Began)
			{
				touchOrigin = myTouch.position;
			} else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				Vector2 touchEnd = myTouch.position;
				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;
				touchOrigin.x = -1;
				if (Mathf.Abs(x) > Mathf.Abs(y))
					horizontal = x > 0 ? 1 : -1;
				else
					vertical = y > 0 ? 1 : -1;
			}
		}

        #endif

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<MonoBehaviour>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        energy--;
        energyText.text = "Energy: " + energy;

        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;

        if (Move (xDir, yDir, out hit)) {
			SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
		}

        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        if (component is IDamagable)
        {
            (component as IDamagable).Damage(damage);
            animator.SetTrigger("playerChop");
        }
    }

    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "SmallHeart")
        {
            energy += pointsPerSmallHeart;
            energyText.text = "+" + pointsPerSmallHeart + " Energy: " + energy;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "BigHeart")
        {
            energy += pointsPerBigHeart;
            energyText.text = "+" + pointsPerBigHeart + " Energy: " + energy;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseEnergy(int loss)
    {
        animator.SetTrigger("playerHit");
        energy -= loss;
        energyText.text = "-" + loss + " Energy: " + energy;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (energy <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
			SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}