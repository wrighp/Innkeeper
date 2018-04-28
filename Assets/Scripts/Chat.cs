using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public GameObject text;
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private ScrollRect rect;

    /// <summary>
    /// Add the message recieved from the server to the chat pane
    /// </summary>
    /// <param name="message">text of the recieved message</param>
    internal void AddMessage(string message)
    {
        GameObject next = Instantiate(text,container.transform);
        next.GetComponent<Text>().text = message;
        Invoke("ScrollDown", .1f);
    }
    /// <summary>
    /// Send the message in the input field
    /// </summary>
    /// <param name="input">Input field user added the text to</param>
    public virtual void SendMessage(InputField input) { }

    /// <summary>
    /// Scroll the chat pane down inorder to display new messages when they appear
    /// </summary>
    private void ScrollDown()
    {
        if (rect != null)
            rect.verticalScrollbar.value = 0;
    }
}