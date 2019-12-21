using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    // Start is called before the first frame update
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public BoardManager boardScript;
    public int playerEnergyPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

    void OnLevelWasLoaded (int index)
	{
		level++;

		InitGame ();
	}

	void InitGame()
	{
        doingSetup = true;

        levelImage = GameObject.Find ("LevelImage");
        levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
        levelText.text = "Level " + level;
        levelImage.SetActive (true);
        Invoke ("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene (level);
	}

    private void HideLevelImage()
    {
    	levelImage.SetActive (false);
    	doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "WASTED!!! \n You have passed" + level + " level(s)";
        levelImage.SetActive(true);
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (playersTurn || enemiesMoving || doingSetup)
           return;

       StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void ClearDiedEnemies()
    {
        enemies = enemies.Where(e => e.isActiveAndEnabled).ToList();
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        foreach (var enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
