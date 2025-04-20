using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MenuBase
{
    public Button button;

    public TMP_Text text;

    public void ShowText(string t)
    {
        text.text = t;
        Show();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
