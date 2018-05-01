using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselImage : MonoBehaviour
{

    private string Title = "";
    private string Description = "";
    private string Photo = "";
    private int PhotoHeight = 0;
    private int PhotoWidth = 0;
    private Vector3 mousePosWhenClicked = Vector3.zero;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SetTextureFromStream()
    {
        //use photo api to find the image and set it as the texture
        string url = "https://maps.googleapis.com/maps/api/place/photo?photoreference=" + Photo + "&sensor=false&maxheight=" + PhotoHeight + "&maxwidth=" + PhotoWidth + "&key=AIzaSyAYQhMIz-fDmymfvuez8xpwDPgNN7e7xw8";
        WWW imageResponse = new WWW(url);
        yield return imageResponse;
        GetComponent<Renderer>().material.SetTexture("_MainTex", imageResponse.texture);
    }

    public void SetVariables(string title, string description, string photo, int photoHeight, int photoWidth)
    {
        Title = title;
        Description = description;
        Photo = photo;
        PhotoHeight = photoHeight;
        PhotoWidth = photoWidth;

        if (photo != "")
        {
            StartCoroutine(SetTextureFromStream());
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(Random.Range(0.4f, 0.55f), Random.Range(0.4f, 0.55f), Random.Range(0.75f, 0.85f));
        }
    }

    private void CheckIfNotDragging()
    {
        if (!Input.GetMouseButton(0))
        {

        }
    }

    void OnMouseDown()
    {
        mousePosWhenClicked = Input.mousePosition;
    }

    void OnMouseUp()
    {
        if ((Input.mousePosition - mousePosWhenClicked).magnitude < 10) //prevent opening panel when trying to drag
        {
            //when clicked open the side panel and fill it with the correct information
            SidePanel sidePanelScript = GameObject.FindGameObjectWithTag("SidePanel").GetComponent<SidePanel>();
            sidePanelScript.SetInformation(Title, Description);
            sidePanelScript.OpenPanel();

            //scroll the carousel to the clicked image
            gameObject.transform.parent.gameObject.GetComponent<Carousel>().GoToImage(gameObject);
        }

    }

}
