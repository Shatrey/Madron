using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

public class Wall : MonoBehaviour, IDamagable
{
    public Sprite dmgSprite;
	public int hp = 4;
	public AudioClip chopSound1;
	public AudioClip chopSound2;

	private SpriteRenderer spriteRenderer;

    void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

    public void Damage(int damage)
    {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= damage;
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
