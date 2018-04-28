using UnityEngine;

public class ChatScroll : MonoBehaviour {

    private int open = 1;

    public float xOpen = 0f;
    public float yOpen = 0f;
    public float xClose= 0f;
    public float yClose = 400f;

    /// <summary>
    /// Called every frame, interpolate the chat in the correct direction
    /// </summary>
	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, Vector3.down * (yOpen + 400 * open), Time.deltaTime * 10f);
    }

    /// <summary>
    /// Event Listener, toggle the visibility of the chat
    /// </summary>
    public void Toggle()
    {
        open = 1 - 1 * open;
    }
}
