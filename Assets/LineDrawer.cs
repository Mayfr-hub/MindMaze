using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 currentPoint;
    
    private bool isDrawing = false;
    private int pointCount = 0;

    private Vector3[] positions = new Vector3[6]; //6 points in total

    void Start()
    {
        // Initialize the LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
       
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

                // Set the number of vertex positions
        lineRenderer.positionCount = 2;

        // Set the positions (start and end points)
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(1, 1, 1));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Start drawing line
            isDrawing = true;
            AddPoint();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }
    }

    void AddPoint(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Set z to 0 for 2D

        lineRenderer.positionCount = pointCount++;
        lineRenderer.SetPosition(pointCount-1, mousePos);
    }

}
