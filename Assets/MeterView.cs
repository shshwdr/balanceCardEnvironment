using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeterView : MonoBehaviour
{
    public Transform resultParent;

    public Transform targetParent;

    public Image fill;
    // Start is called before the first frame update
    void Start()
    {
        var resultImages = resultParent.GetComponentsInChildren<Image>();
        var resultTexts = resultParent.GetComponentsInChildren<TMP_Text>();
        var targetTexts =  targetParent.GetComponentsInChildren<TMP_Text>();
        
    }

    public void UpdateView()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
