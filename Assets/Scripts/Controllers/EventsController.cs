using UnityEngine;
using UnityEngine.UI;

public class EventsController : MonoBehaviour {

    // Handle the text style on buttons when the mouse is over it
    public void OnButtonEnter(Button button) {
        Text text = button.GetComponentInChildren<Text>();
        text.fontStyle = FontStyle.BoldAndItalic;
    }

    // Handle the text style on buttons when the mouse leave it
    public void OnButtonLeave(Button button) {
        Text text = button.GetComponentInChildren<Text>();
        text.fontStyle = FontStyle.Normal;
    }
}
