using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DotManager : MonoBehaviour
{
    public LineRenderer lineRenderer1;
    public LineRenderer lineRenderer2;
    public RectTransform canvasRect; // Reference to your Canvas RectTransform
    public List<Image> images; // List of your UI Images
    private int targetIndex;
    private float minDistance = 70f;  // Minimum distance between images
    private float canvasMargin = 50f;  // Minimum distance to edge of canvas

    private void Start()
    {
        // Ensure buttons are assigned in the correct order
        if (images.Count != 6)
        {
            Debug.LogError("There should be exactly 6 images.");
            return;
        }

        PlaceImagesRandomly();

        //initially, only allow dot with index 1 to be clicked
        targetIndex = 1;
    }

    private void Update()
    {
        if (targetIndex >= 2 && targetIndex < images.Count + 1)
        {
            DrawLineFromLastDot();
        }
        else
        {
            lineRenderer2.positionCount = 0;
        }
    }

    private void PlaceImagesRandomly()
    {
        foreach (Image image in images)
        {
            RectTransform imageRect = image.GetComponent<RectTransform>();
            bool validPositionFound = false;

            while (!validPositionFound)
            {
                // Generate random anchored position within canvas
                Vector3 randomPosition = new Vector3(
                    Random.Range(-canvasRect.rect.width / 2f + canvasMargin, canvasRect.rect.width / 2f - canvasMargin),
                    Random.Range(-canvasRect.rect.height / 2f + canvasMargin, canvasRect.rect.height / 2f - canvasMargin),
                    0
                );

                // Set anchoredPosition directly
                imageRect.anchoredPosition = randomPosition;

                validPositionFound = CheckIfPositionIsValid(imageRect);
            }
        }
    }

    private bool CheckIfPositionIsValid(RectTransform currentImageRect)
    {
        foreach (Image otherImage in images)
        {
            if (otherImage == currentImageRect.GetComponent<Image>()) continue; // Skip self

            RectTransform otherImageRect = otherImage.GetComponent<RectTransform>();

            // Check distance between centers of the RectTransforms
            if (Vector2.Distance(currentImageRect.anchoredPosition, otherImageRect.anchoredPosition) < minDistance)
            {
                return false;
            }
        }

        return true;
    }

    //memory leak management
    private void OnEnable()
    {
        DotClickHandler.OnDotClicked += HandleImageClick;
    }

    private void OnDisable()
    {
        DotClickHandler.OnDotClicked -= HandleImageClick;
    }

    private void HandleImageClick(int uniqueIndex)
    {
        // Debug.Log("DotManager detected click on image with index: " + uniqueIndex);
        if (uniqueIndex == targetIndex)
        {
            Image clickedImage = images[uniqueIndex - 1];
            clickedImage.color = Color.green;

            if (targetIndex >= 2)
            {
                DrawLineThroughDots();
            }

            targetIndex++;
        }
    }

    // Function to draw a line through all dots
    private void DrawLineThroughDots()
    {

        if (lineRenderer1 == null)
        {
            Debug.LogError("LineRenderer is not assigned.");
            return;
        }

        lineRenderer1.positionCount = targetIndex;
        for (int i = 0; i < lineRenderer1.positionCount; i++)
        {
            RectTransform imageRect = images[i].GetComponent<RectTransform>();
            // Convert the anchored position to world position
            Vector3 worldPos = canvasRect.TransformPoint(imageRect.anchoredPosition);
            // Debug.Log(worldPos);
            lineRenderer1.SetPosition(i, worldPos);
        }
    }

    private void DrawLineFromLastDot()
    {
        lineRenderer2.positionCount = 2;

        RectTransform imageRect = images[targetIndex - 2].GetComponent<RectTransform>();
        Vector3 dotWorldPos = canvasRect.TransformPoint(imageRect.anchoredPosition);

        Vector3 mousePosition = Input.mousePosition; // Get mouse position in screen coordinates
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition); // Convert to world coordinates

        lineRenderer2.SetPosition(0, dotWorldPos);
        lineRenderer2.SetPosition(1, mouseWorldPos);
    }

}
