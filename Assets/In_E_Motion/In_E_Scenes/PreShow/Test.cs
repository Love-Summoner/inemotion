using UnityEngine;
using UnityEngine.UI;
using extOSC;

public class PopupController : MonoBehaviour
{
    public GameObject popupPanel; // Assign your UI Panel here
    public Text popupText; // Assign your UI Text here
    public string message = "Hello! This is a popup!";

    private bool isPopupVisible = false;

    [Header("OSC Settings")]
    public OSCReceiver Receiver;
    public string Address = "/inemotion/effect";
    void Start()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false); // Ensure it's hidden at the start
        }
        Receiver.Bind(Address, ReceivedMessage);
    }

    private void ReceivedMessage(OSCMessage message)
    {
        Debug.LogFormat("Received: {0}", message);
        TogglePopup();
    }

        void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Click
        {
            TogglePopup();
        }
    }

    void TogglePopup()
    {
        if (popupPanel != null && popupText != null)
        {
            isPopupVisible = !isPopupVisible;
            popupPanel.SetActive(isPopupVisible);
            popupText.text = message;
        }
    }
}
