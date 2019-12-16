﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public float restartLevelDelay = 1f;
    public int pointsPerFood = 10;      
    public int pointsPerSoda = 20;      
    public int wallDamage = 1;          


    private Animator animator;          
    private int food;                   


    //Start overrides the Start function of MovingObject
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    private void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        int vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;

        //if (Move(xDir, yDir, out hit))
        //{
        //    Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
        //}

        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;

        hitWall.DamageWall(wallDamage);

        //animator.SetTrigger("playerChop");
    }


    //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int loss)
    {
        //animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
}