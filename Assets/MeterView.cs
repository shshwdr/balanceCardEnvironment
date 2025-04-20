using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeterView : MonoBehaviour
{
    public Transform resultParent;

    public Transform targetParent;

    public Transform targetProgressParent;
    
    Image[] resultImages;
    Image[] targetImages;
    TMP_Text[] resultTexts;
    TMP_Text[] targetTexts;

    public bool isIndustry;
    // Start is called before the first frame update
    void Start()
    {
        resultImages = resultParent.GetComponentsInChildren<Image>();
            targetImages = targetProgressParent.GetComponentsInChildren<Image>();
        resultTexts = resultParent.GetComponentsInChildren<TMP_Text>();
        targetTexts =  targetParent.GetComponentsInChildren<TMP_Text>();
        
        EventPool.OptIn("IndustryChanged", UpdateView);
        EventPool.OptIn("NatureChanged", UpdateView);
        
    }

    public void UpdateViewForStartOfTurn()
    {
        var currentTurnReq = CSVLoader.Instance.turnRequirementDict[GameManager.Instance.Turn];
        var rewardList = isIndustry?currentTurnReq.industryReward:currentTurnReq.natureDisaster;
        for (int i = 0; i < resultTexts.Length; i++)
        {
            if (rewardList[i] == -1)
            {
                resultTexts[i].text = "DIE";
            }
            else
            {
                
                resultTexts[i].text =  rewardList [i].ToString();
            }
        }
        var reqList = isIndustry?currentTurnReq.industryReq:currentTurnReq.natureReq;
        for (int i = 0; i < targetTexts.Length; i++)
        {
            {
                
                targetTexts[i].text = reqList[i].ToString();
            }
        }
        UpdateView();
        
        
    }

    public string currentResult = "";
    public void UpdateView()
    {
        
        var currentTurnReq = CSVLoader.Instance.turnRequirementDict[GameManager.Instance.Turn];
        var reqList = isIndustry?currentTurnReq.industryReq:currentTurnReq.natureReq;
        var rewardList = isIndustry?currentTurnReq.industryReward:currentTurnReq.natureDisaster;
        var currentValue = isIndustry? GameManager.Instance.Industry:GameManager.Instance.Nature;
        bool firstFinished = true;
        for (int i = rewardList.Count-1;i>=0;i--)
        {
            int prevReq = i ==0 ? (isIndustry?0:reqList[i]-20) : reqList[i-1];
            
            if (currentValue < reqList[i])
            {
                resultImages[i].color = Color.black;
                targetImages[i].fillAmount = (currentValue- prevReq)/(float)(reqList[i] - prevReq);
                if (i == 0 && firstFinished)
                {
                    
                    resultImages[i].color = Color.red;
                    currentResult = resultTexts[i].text;
                }
            }

            else if (currentValue >= reqList[i])
            {
                if (firstFinished)
                {
                    if (i == 0)
                    {
                        resultImages[i].color = Color.red;
                        currentResult = resultTexts[i].text;
                    }
                    else
                    {
                        
                        resultImages[i].color = Color.green;
                        currentResult = resultTexts[i].text;
                    }
                    firstFinished = false;
                }
                else
                {
                    
                    resultImages[i].color = new Color(0.5f,1,0.5f);
                }
                
                targetImages[i].fillAmount = (currentValue- prevReq)/(float)(reqList[i] - prevReq);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
