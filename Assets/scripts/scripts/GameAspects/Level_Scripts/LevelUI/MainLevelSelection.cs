using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainLevelSelection : MonoBehaviour 
{
    #region properties

    public RectTransform content;
    public HorizontalLayoutGroup hlg;
    public int curr;
    public RectTransform sample;
    public float pos;
    public ScrollRect sr;
    private float speed;
    private bool isreached;
    private Vector3 targetSize;
    public TextMeshProUGUI Titleboard;
    public string[] txt;
    private float input;
   // private string selectedType = "";
    public Canvas MainCan;
    
    #endregion
    private void Start()
    {
        isreached = false;
    }

    private void Update()
    {
        if (MainCan.gameObject.activeInHierarchy) PositionSetUp(); 
        ControlByKeys();

    }

    private void PositionSetUp()
    {
        curr = Convert.ToInt32(content.localPosition.x / -850f);

        if (sr.velocity.magnitude < 200f && !isreached)
        {
            speed = 700f * Time.deltaTime;
            sr.velocity = Vector2.zero;
            content.localPosition = new Vector3(Mathf.MoveTowards(content.localPosition.x, 0 - (curr * (sample.rect.width + hlg.spacing)), speed),
                content.localPosition.y, content.localPosition.z);
            if (content.localPosition.x == 0 - curr * (sample.rect.width + hlg.spacing)) { isreached = true; Titleboard.text = txt[curr]; }
        }

        if (sr.velocity.magnitude > 200f)
        {
            isreached = false;
            speed = 0;
        }

    }

    private void ControlByKeys()
    {
        input = Input.GetAxis("Horizontal");

        sr.velocity = new Vector2(pos * -input, sr.velocity.y);
    }

   

   
}

