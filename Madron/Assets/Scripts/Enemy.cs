using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

public class Enemy : MovingObject, IDamagable
{
    public int playerDamage;
    public int hp = 2;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);

        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //skip every second turn
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        hitPlayer.LoseFood(playerDamage);

        animator.SetTrigger("enemyAttack");

        SoundManager.instance.RandomizeSfx (enemyAttack1, enemyAttack2);
    }

    public void Damage(int damage)
    {
        // TODO: implement trigger
        // animator.SetTrigger("enemyAttacked");
        hp -= damage;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            GameManager.instance.ClearDiedEnemies();
        }
    }
}