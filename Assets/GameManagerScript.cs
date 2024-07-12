using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour
{
    public Image[] circles; // Array of circle UI elements
    public Text timerText; // Timer text UI element
    public LineRenderer lineRenderer; // LineRenderer component

    private float startTime;
    private bool gameStarted;
    private bool isDragging = false; // Flag to indicate if dragging
    private Vector3 startPoint; // Starting point of the line

    private List<Vector3> linePoints = new List<Vector3>(); // List to store points for all lines

    void Start()
    {
        RandomizePositions(); // Randomize the positions of circles
        StartGame(); // Start the game timer
        ResetLineRenderer(); // Initialize the LineRenderer to be empty
    }

    void Update()
    {
        if (gameStarted)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            timerText.text = minutes + ":" + seconds; // Update the timer text

            if (isDragging)
            {
                Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentMousePosition.z = 0; // Set z to 0 because we're in 2D
                DrawLine(currentMousePosition);
            }
        }
    }

    void RandomizePositions()
    {
        float minDistance = 100f; // Minimum distance between circles

        for (int i = 0; i < circles.Length; i++)
        {
            bool positionValid;
            Vector2 newPosition;

            do
            {
                positionValid = true;
                newPosition = new Vector2(Random.Range(-Screen.width / 2, Screen.width / 2),
                                          Random.Range(-Screen.height / 2, Screen.height / 2));

                for (int j = 0; j < i; j++)
                {
                    if (Vector2.Distance(newPosition, circles[j].rectTransform.anchoredPosition) < minDistance)
                    {
                        positionValid = false;
                        break;
                    }
                }
            } while (!positionValid);

            circles[i].rectTransform.anchoredPosition = newPosition;
        }
    }

    void StartGame()
    {
        startTime = Time.time;
        gameStarted = true;
    }

    void ResetLineRenderer()
    {
        linePoints.Clear();
        lineRenderer.positionCount = 0;
    }

    public void StartDragging(Vector3 point)
    {
        isDragging = true;
        startPoint = point;

        if (lineRenderer.positionCount == 0)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, startPoint);
        }
        else
        {
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, startPoint);
        }

        linePoints.Add(startPoint);
    }

    public void StopDragging(Vector3 endPoint)
    {
        isDragging = false;
    }

    private void DrawLine(Vector3 currentMousePosition)
    {
        if (isDragging && lineRenderer.positionCount > 1)
        {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentMousePosition);
        }
    }

    public void AddFinalPoint(Vector3 endPoint, bool isValid)
    {
        // Add the final point only if the mouse is released over the next image
        if (isDragging && isValid)
        {
            linePoints[linePoints.Count - 1] = endPoint;
            lineRenderer.SetPosition(linePoints.Count - 1, endPoint);
            linePoints.Add(endPoint);
            lineRenderer.positionCount = linePoints.Count;
        }
        else if (isDragging)
        {
            // If not valid, reset the last point to the start point
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, startPoint);
            linePoints.RemoveAt(linePoints.Count - 1);
            lineRenderer.positionCount = linePoints.Count;
        }
    }

    public void EndGame()
    {
        gameStarted = false;
        Debug.Log("Game Finished! Time: " + timerText.text);
    }
}
