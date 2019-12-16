using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public int hp = 3;

    public void DamageWall(int loss)
    {
        hp -= loss;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
