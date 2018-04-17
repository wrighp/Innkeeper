using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public GameObject text;
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private ScrollRect rect;

    internal void AddMessage(string message)
    {
        GameObject next = Instantiate(text,container.transform);
        next.GetComponent<Text>().text = message;
        //just a hack to jump a frame and scrolldown the chat
        Invoke("ScrollDown", .1f);
    }

    public virtual void SendMessage(InputField input) { }

    private void ScrollDown()
    {
        if (rect != null)
            rect.verticalScrollbar.value = 0;
    }
}