using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidePanel : MonoBehaviour
{
    public float OpenPosition;
    public float ClosedPosition;
    public short Direction = 1;
    public short Speed = 2;

    public Text TitleText;
    public Text DescriptionText;

    private bool moving = false;
    private bool closing = false;
    private bool opening = false;
    private RectTransform RectTransform;

    // Use this for initialization
    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        //open close animation
        if (moving)
        {
            if (opening)
            {
                RectTransform.anchoredPosition = new Vector3(RectTransform.anchoredPosition.x + Speed * Direction, RectTransform.anchoredPosition.y);
                if (Direction > 0)
                {
                    if (RectTransform.anchoredPosition.x >= OpenPosition)
                    {
                        RectTransform.anchoredPosition = new Vector3(OpenPosition, RectTransform.anchoredPosition.y);
                        moving = false; opening = false; closing = false;
                    }
                }
                else
                {
                    if (RectTransform.anchoredPosition.x <= OpenPosition)
                    {
                        RectTransform.anchoredPosition = new Vector3(OpenPosition, RectTransform.anchoredPosition.y);
                        moving = false; opening = false; closing = false;
                    }
                }
            }
            if (closing)
            {
                RectTransform.anchoredPosition = new Vector3(RectTransform.anchoredPosition.x - Speed * Direction, RectTransform.anchoredPosition.y);
                if (Direction > 0)
                {
                    if (RectTransform.anchoredPosition.x <= ClosedPosition)
                    {
                        RectTransform.anchoredPosition = new Vector3(ClosedPosition, RectTransform.anchoredPosition.y);
                        moving = false; opening = false; closing = false;
                    }
                }
                else
                {
                    if (RectTransform.anchoredPosition.x >= ClosedPosition)
                    {
                        RectTransform.anchoredPosition = new Vector3(ClosedPosition, RectTransform.anchoredPosition.y);
                        moving = false; opening = false; closing = false;
                    }
                }

            }
        }

    }

    public void SetInformation(string title, string description)
    {
        TitleText.text = title;
        DescriptionText.text = description;
    }

    public void ClosePanel()
    {
        moving = true;
        closing = true;
        opening = false;
    }

    public void OpenPanel()
    {
        moving = true;
        opening = true;
        closing = false;
    }

}
