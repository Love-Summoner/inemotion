using UnityEngine;

public class AddBoxCollider2DToUI : MonoBehaviour
{
    void Start()
    {
        foreach (Transform child in transform) // Loop through all children in the Canvas
        {
            if (child.GetComponent<RectTransform>() && child.GetComponent<UnityEngine.UI.Image>())
            {
                if (!child.GetComponent<BoxCollider2D>()) // Check if the BoxCollider2D already exists
                {
                    BoxCollider2D collider = child.gameObject.AddComponent<BoxCollider2D>();

                    // Set collider size based on RectTransform
                    RectTransform rect = child.GetComponent<RectTransform>();
                    collider.size = rect.rect.size; // This gives the collider the same size as the UI element
                }
            }
        }
    }
}
