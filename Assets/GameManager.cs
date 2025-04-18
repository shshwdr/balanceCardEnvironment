using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        CSVLoader.Instance.Init();
        HandManager.Instance.Init();
        HandManager.Instance.InitDeck();
    }

    public int Turn => turn;
    private int turn = 1;

    private int industry;
    private int nature;
    private int gold;

    public int Industry
    {
        get => industry;
        set
        {
            industry = value;
            EventPool.Trigger("IndustryChanged");
        }
    }
    public int Nature
    {
        get => nature;
        set
        {
            nature = value;
            EventPool.Trigger("NatureChanged");
        }
    }
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            EventPool.Trigger("GoldChanged");
        }
    }
    public void StartNewTurn()
    {
        
        HandsView.Instance.DrawCard();
        foreach (var meterView in FindObjectsOfType<MeterView>())
        {
            meterView.UpdateViewForStartOfTurn();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Industry += 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Nature += 10;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Gold += 10;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
