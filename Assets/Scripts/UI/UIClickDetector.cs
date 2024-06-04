using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIClickDetector : MonoBehaviour
{
    void Update()
    {
        // Check if the left mouse button or the primary touch is pressed
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Perform a raycast to check if we clicked on a UI element
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };

            // Store the raycast results
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            // Process the raycast results
            foreach (var result in raycastResults)
            {
                Debug.Log("Clicked on: " + result.gameObject.name);
                // Add your custom logic here, e.g., call a method on the clicked object
            }
        }
    }
}
