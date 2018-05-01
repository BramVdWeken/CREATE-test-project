using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.Linq;
using System;

public class PlacesApi : MonoBehaviour
{

    public GameObject CarouselImagePrefab;

    public GameObject Carousel;

    public Text QueryText;
    public GameObject InputFieldPanel;
    public GameObject UIElements;
    public GameObject SidePanel;
    public GameObject LookAtOverwriteTarget;

    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenSearchWindow()
    {
        Carousel.GetComponent<Carousel>().Clear();
        SidePanel.GetComponent<SidePanel>().ClosePanel();
        InputFieldPanel.SetActive(true);
        UIElements.SetActive(false);
        QueryText.text = "";
    }

    public void StartNewSearch()
    {
        StopAllCoroutines();
        StartCoroutine(NewSearch(QueryText.text));
        InputFieldPanel.SetActive(false);
        UIElements.SetActive(true);
    }

    public IEnumerator NewSearch(string query)
    {
        //remove the old images
        Carousel.GetComponent<Carousel>().Clear();

        //get the data from the api
        query = query.Replace(' ', '+');
        string url = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + query + "&key=AIzaSyAYQhMIz-fDmymfvuez8xpwDPgNN7e7xw8";
        WWW response = new WWW(url);
        yield return response;

        //print(response.text);

        //create json object from the response json text
        JObject JsonResponse = JObject.Parse(response.text);

        //continue if we got good data
        if (JsonResponse["status"] != null && JsonResponse["status"].Value<string>() == "OK")
        {
            //find out how many images we will show in the carousel
            int counter = 0;
            int imageCount = Carousel.GetComponent<Carousel>().MaxImages;
            if (JsonResponse["results"].HasValues)
            {
                int count = JsonResponse["results"].Count();
                if (count < imageCount) imageCount = count;
            }

            //fill the carousel with the results
            foreach (JToken result in JsonResponse["results"])
            {
                counter++;
                if (counter <= imageCount)
                {
                    //create carousel image object
                    GameObject newObj = GameObject.Instantiate(CarouselImagePrefab);
                    CarouselImage carouselImageScript = newObj.GetComponent<CarouselImage>();
                    newObj.GetComponent<LookAtTargetAxis>().SetTarget(Carousel.transform);
                    newObj.GetComponent<LookAtTargetAxis>().SetOverwriteTarget(LookAtOverwriteTarget.transform);

                    string title = "";
                    string description = "";
                    string photo = "";
                    int height = 0;
                    int width = 0;

                    //get the title
                    if (result["name"] != null) title = result["name"].Value<string>();

                    //get some description
                    if(result["formatted_address"] != null) description += "Adress:\n\n" + result["formatted_address"].Value<string>() + "\n\n";
                    if (result["opening_hours"] != null && result["opening_hours"].HasValues)
                    {
                        description += "Open?\n\n";
                        if (result["opening_hours"]["open_now"].Value<bool>()) description += "Yes";
                        else description += "No";
                        description += "\n\n";
                    }
                    if (result["rating"] != null) description += "Rating:\n\n" + result["rating"].Value<float>();
                    
                    //get the reference string of the first photo
                    if (result["photos"] != null && result["photos"].HasValues)
                    {
                        JToken jsonPhoto = result["photos"].First;
                        photo = jsonPhoto["photo_reference"].Value<string>();
                        height = jsonPhoto["height"].Value<int>();
                        width = jsonPhoto["width"].Value<int>();
                    }

                    //send the data to the carousel image object
                    carouselImageScript.SetVariables(title, description, photo, height, width);

                    //set it at the correct position in the carousel
                    newObj.transform.parent = Carousel.transform;

                    double distanceFromCenter = 2.3;
                    double angle = (Math.PI * 2 / imageCount) * (counter - 1);

                    double x = Math.Cos(angle) * distanceFromCenter;
                    double y = Math.Sin(angle) * distanceFromCenter;

                    newObj.transform.localPosition = new Vector3((float)x, 0, (float)y);

                    //add the image object to the carousel
                    Carousel.GetComponent<Carousel>().CarouselImages.Add(newObj);
                }

            }
        }

    }
}
