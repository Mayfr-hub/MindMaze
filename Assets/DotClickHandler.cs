using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro; // Include TextMeshPro namespace

public class DotClickHandler : MonoBehaviour, IPointerClickHandler
{
    public static event Action<int> OnDotClicked;
    private TextMeshProUGUI childText;
    private int uniqueIndex;



    private void Start()
    {
        childText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (childText != null)
        {
            string textContent = childText.text;
            if (int.TryParse(childText.text, out uniqueIndex))
            {
                // Debug.Log("Unique index is: " + uniqueIndex);
            }
            else
            {
                Debug.LogError("Text content is not a valid integer.");
            }
        }
        else
        {
            Debug.LogError("No Text component found in the child object.");
        }
    }

    //unity calls this function when the corresponding UI element is clicked.
    public void OnPointerClick(PointerEventData eventData)
    {
        OnDotClicked?.Invoke(uniqueIndex);
        // Debug.Log("Image clicked with index: " + uniqueIndex);
    }
}

