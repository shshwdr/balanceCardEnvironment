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

    public Dictionary<string, int> states = new Dictionary<string, int>();

    public void AddState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
        }
        else
        {
            states[key] = value;
        }
    }

    public int GetState(string key)
    {
        if (states.ContainsKey(key))
        {
            return states[key];
        }
        return 0;
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
            if(value!=industry)
            DamageNumbersManager.Instance.ShowResourceCollection(Hud.Instance.industryMeter, value-industry, DamageNumberType.industry);
            industry = value;
            EventPool.Trigger("IndustryChanged");
        }
    }
    public int Nature
    {
        get => nature;
        set
        {
            if(value!=nature)
            DamageNumbersManager.Instance.ShowResourceCollection( Hud.Instance.natureMeter, value - nature, DamageNumberType.nature);
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

    private Dictionary<string, int> charactersDict = new Dictionary<string, int>();

    public void AddCharacter(string key, int value)
    {
        if (charactersDict.ContainsKey(key))
        {
            charactersDict[key] += value;
        }
        else
        {
            charactersDict[key] = value;
        }
        EventPool.Trigger("CharacterChanged");
    }

    public int industryManCount => GetCharacter("industryMan");
    public int industryBoost => GetState("boostIndustry");
    public int natureManCount => GetCharacter("natureMan");
    public int natureBoost => GetState("boostNature");
    public int GetCharacter(string key)
    {
        if (charactersDict.ContainsKey(key))
        {
            return charactersDict[key];
        }
        return 0;
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
