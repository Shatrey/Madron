using System.Collections;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager = null;

	void Awake () {
		if (GameManager.instance == null)
			Instantiate (gameManager);
	}
}
