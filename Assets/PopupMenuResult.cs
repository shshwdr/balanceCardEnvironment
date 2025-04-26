using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuResult : MenuBase
{
    public TextPopup textPopup;
    public void ShowText(string text)
    {
        Show();
        textPopup.gameObject.SetActive(true);
        textPopup.text.text = text;

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
