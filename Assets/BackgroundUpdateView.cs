using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundUpdateView : MonoBehaviour
{
    public Transform industryParent;
    public Transform natureParent;
    public Image[] industryImages;
    public Image[] natureImages;
    // Start is called before the first frame update
    void Start()
    {
        
        EventPool.OptIn<int, bool>("meterUpdate",MeterUpdate);
        industryImages = industryParent.GetComponentsInChildren<Image>(true);
        natureImages = natureParent.GetComponentsInChildren<Image>(true);
    }

    void MeterUpdate(int index, bool isIndustry)
    {
        var images = isIndustry ? industryImages : natureImages;
        foreach (var image in images)
        {
            image.gameObject.SetActive(false); 
        }
        if (index>=3)
        {
            images[1].gameObject.SetActive(true);
        }else if (index >= 1)
        {
            images[0].gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
