using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public string GameState = "game";
    public int second;
    public GameObject pool;
    public int score;
    [SerializeField] GameObject enemyCar;
    GameObject player;
    [SerializeField] Text gameOverText;

    public bool isPc;

    #region ui
    public void pc()
    {
        isPc = true;
        transform.Find("game").GetChild(0).GetChild(0).gameObject.SetActive(false);
        GameState = "menu";
    }
    public void mobile()
    {
        isPc = false;
        transform.Find("game").GetChild(0).GetChild(0).gameObject.SetActive(true);
        GameState = "menu";
    }
    #endregion


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(tickSecond());
    }

    public void play()
    {
        GameState = "game";
        second = 0;
        score = 0;
        StartCoroutine(tickSecond());
    }
    public void goMenu()
    {
        GameState = "menu";
    }

    private void LateUpdate()
    {
        switch (GameState)
        {
            case "menu":
                player.SetActive(true);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3, Time.deltaTime);
                break;
            case "game":
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 11, Time.deltaTime);
                break;
            case "gameOver":
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 6, Time.deltaTime);
                gameOverText.text = score.ToString();
                break;
            default: break;
        }
    }
    void Update()
    {
        
        foreach (Transform t in transform.GetComponentInChildren<Transform>())
        {
            if (t.name == GameState) t.gameObject.SetActive(true);
            else t.gameObject.SetActive(false);
        }
    }

    IEnumerator tickSecond()
    {
        while (GameState == "game")
        {
            yield return new WaitForSeconds(1);
            second++;
            if (second % (int)Random.Range(2,5) == 0)
            {
                if (pool.transform.childCount != 0)
                {
                    GameObject a = pool.transform.GetChild(0).gameObject;
                    a.transform.SetParent(transform.parent);
                    a.transform.position = new Vector3(
                        Random.Range(-3.5f,3.5f),
                        10.1f,
                        Random.Range(-3.5f, 3.5f)
                    );
                    a.transform.rotation = Quaternion.identity;
                    a.SetActive(true);
                }
                else
                {
                    GameObject a = Instantiate(enemyCar);
                    a.transform.position = new Vector3(
                        Random.Range(-3.5f, 3.5f),
                        10.1f,
                        Random.Range(-3.5f, 3.5f)
                    );
                    a.transform.rotation = Quaternion.identity;
                }
                second = 0;
            }
        }
    }
}
