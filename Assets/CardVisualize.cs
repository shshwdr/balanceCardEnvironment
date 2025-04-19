using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CardVisualize : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{

    public Image image;

    public bool isDraggable = true;
    public TMP_Text text;
    public TMP_Text desc;
    
    //private GameObject selectionCircle;
    //public GameObject selectionCirclePrefab;


    public Transform hoverTrans;
    Vector3 startPos;
    private Vector3 hoverPos;

    public CardInfo cardInfo;
    public bool setPosition;
    public void Init(CardInfo info)
    {
        cardInfo = info;
        text.text = cardInfo.title;
        desc.text = cardInfo.desc;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setposition());
    }

    IEnumerator setposition()
    {
        yield return new WaitForSeconds(0.01f);
        setPosition = true;
        startPos = transform.position;
        hoverPos = hoverTrans.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        
        OnPlace();
        
        if (!isDraggable)
        {
            return;
        }

        // if (selectionCircle == null)
        // {
        //     
        //     selectionCircle = Instantiate(selectionCirclePrefab);
        // }

        //var radius = float.Parse(cardInfo.actions[1]);
        //selectionCircle.transform.localScale = Vector3.one * radius;
        //selectionCircle.SetActive(true);
        //PlayerControllerManager.Instance.StartDragging(selectionCircle,this);
    }

    public void OnPlace()
    {

        //FindObjectOfType<TutorialMenu>().StartCoroutine( FindObjectOfType<TutorialMenu>().FinishUseCard());
       // Debug.LogError("place");
        Collider2D[] results = new Collider2D[20]; // 假设最多检测 10 个碰撞体

        // 检测重叠
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;  // 允许触发器参与检测
       // int count = selectionCircle.GetComponent<Collider2D>().OverlapCollider(contactFilter, results);
        //sort by result's distance to selectionCicle
        // results = results.Where(x => x != null)
        //     .OrderBy(x => Vector3.Distance(x.transform.position, selectionCircle.transform.position)).ToArray();
        HandManager.Instance.useCard(cardInfo);
        bool foundTarget = false;
        

        
        
        //selectionCircle.SetActive(false);
        ExitCard();
        //Destroy(gameObject);
    }

    public void Cancel()
    {
        
        
        //selectionCircle.SetActive(false);
        ExitCard();
    }

    public bool OnDrag()
    {
        return true;
    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!setPosition|| !isDraggable)
        {
            return;
        }
        transform.position = hoverPos;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!setPosition || !isDraggable)
        {
            return;
        }

        if (PlayerControllerManager.Instance.currentDraggingCell == this)
        {
            
        }
        else
        {
            ExitCard();
        }
        
    }

    public void ExitCard()
    {
        if (this && transform)
        {
            
            transform.position = startPos;
        }
    }
}
