using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;

public class Hud : Singleton<Hud>
{
    public Transform industryMeter;
    public Transform natureMeter;

    public TMP_Text calculateForDayText;

    public TMP_Text energyText;

    public TMP_Text goldText;
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("TurnChanged",UpdateText);
        EventPool.OptIn("EnergyChanged",UpdateText);
        EventPool.OptIn("GoldChanged",UpdateText);
        
         
    }

    public void UpdateText()
    {
        calculateForDayText.text = $"Calculate Reward after {GameManager.Instance.turnInDay -  GameManager.Instance.Turn} turns";
        energyText.text = $"Energy: {GameManager.Instance.Energy}";
        goldText.text = $"Gold: {GameManager.Instance.Gold}";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
