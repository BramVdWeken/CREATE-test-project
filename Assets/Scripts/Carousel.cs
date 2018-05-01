using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carousel : MonoBehaviour
{
    public float MaxDragSpeed = 0.20f;
    public int MaxImages = 20;
    public List<GameObject> CarouselImages;

    private int currentSelectedImage = 0;
    private bool dragging = false;
    private float lastMousePosX;
    private float targetDragRotation;
    private bool ImageOrientationFrontFacing = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int imageCount = CarouselImages.Count;
        if (imageCount > 0)
        {
            //carousel spins to the active image
            if (!dragging)
            {
                targetDragRotation = 360 / CarouselImages.Count * currentSelectedImage + 90;
                Quaternion targetRotation = Quaternion.Euler(0, targetDragRotation, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            }

            //dragging the carousel
            if (Input.GetMouseButton(0))
            {
                if (!dragging)
                {
                    lastMousePosX = Input.mousePosition.x;
                    dragging = true;
                    return;
                }

                //get how fast we are dragging
                float currentMousePosX = Input.mousePosition.x;
                float mouseMovement = (lastMousePosX - currentMousePosX) / Screen.width;
                if (System.Math.Abs(mouseMovement) > MaxDragSpeed)
                {
                    if (mouseMovement > 0) mouseMovement = MaxDragSpeed;
                    else mouseMovement = -MaxDragSpeed;
                }

                //add to the place we will end up after we stop dragging
                if (mouseMovement < 0) targetDragRotation += mouseMovement * -mouseMovement * Time.deltaTime * 300000;
                else targetDragRotation += mouseMovement * mouseMovement * Time.deltaTime * 300000;
                if (targetDragRotation > 360) targetDragRotation -= 360; if (targetDragRotation < 0) targetDragRotation += 360;

                //figure out at what image we will end up at
                float sectionSize = (360 / CarouselImages.Count);
                float tempDragRotation = targetDragRotation - 90;
                if (tempDragRotation > 360) tempDragRotation -= 360; if (tempDragRotation < 0) tempDragRotation += 360;
                int targetSelectedImage = (int)(tempDragRotation / sectionSize + 0.5f);
                if (targetSelectedImage != currentSelectedImage)
                {
                    currentSelectedImage = targetSelectedImage;
                }

                //rotate the carousel
                Quaternion targetRotation = Quaternion.Euler(0, targetDragRotation, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);

                lastMousePosX = Input.mousePosition.x;
            }
            else dragging = false;

        }

    }

    public void Clear()
    {
        //remove all the carousel images
        foreach (var obj in CarouselImages)
        {
            Destroy(obj.GetComponent<Renderer>().material.mainTexture);
            GameObject.Destroy(obj, 0.1f);
        }
        CarouselImages.Clear();
        currentSelectedImage = 0;
    }

    public void ToggleOrientation()
    {
        ImageOrientationFrontFacing = !ImageOrientationFrontFacing;
        foreach (GameObject item in CarouselImages)
        {
            item.GetComponent<LookAtTargetAxis>().LookAtAxis = !ImageOrientationFrontFacing;
            item.GetComponent<LookAtTargetAxis>().LookAtTarget = ImageOrientationFrontFacing;
            item.GetComponent<LookAtTargetAxis>().UseDistanceTargetOverwrite = ImageOrientationFrontFacing;
        }
    }

    public void MaxImagesChanged(Slider slider)
    {
        MaxImages = (int)slider.value;
        GameObject.FindGameObjectWithTag("API").GetComponent<PlacesApi>().StartNewSearch();
    }

    public void GoToNextImage()
    {
        currentSelectedImage++;
        if (currentSelectedImage > CarouselImages.Count - 1) currentSelectedImage = 0;
    }

    public void GoToPreviousImage()
    {
        currentSelectedImage--;
        if (currentSelectedImage < 0) currentSelectedImage = CarouselImages.Count - 1;
    }

    public void GoToImage(GameObject imageObject)
    {
        int index = CarouselImages.IndexOf(imageObject);
        if (index >= 0) currentSelectedImage = index;


    }
}
