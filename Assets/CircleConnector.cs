using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleConnector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static int currentNumber = 1;
    public int circleNumber; // Circle number to determine order

    private GameManagerScript gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (circleNumber == currentNumber)
        {
            GetComponent<Image>().color = Color.green; // Change color to indicate it was clicked correctly
            Vector3 worldPosition = transform.position;
            gameManager.StartDragging(worldPosition);
            Debug.Log("Correct Circle Pressed: " + circleNumber);
        }
        else
        {
            Debug.Log("Incorrect Circle Pressed: " + circleNumber);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (circleNumber == currentNumber)
        {
            Vector3 worldPosition = transform.position;
            gameManager.StopDragging(worldPosition);
            gameManager.AddFinalPoint(worldPosition, true);
            Debug.Log("Correct Circle Released: " + circleNumber);
            currentNumber++;
            if (currentNumber > gameManager.circles.Length)
            {
                gameManager.EndGame();
            }
        }
        else
        {
            gameManager.AddFinalPoint(Vector3.zero, false);
            Debug.Log("Incorrect Circle Released: " + circleNumber);
        }
    }
}
