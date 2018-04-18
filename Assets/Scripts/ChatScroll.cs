using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatScroll : MonoBehaviour {

    private int open = 1;

    public float xOpen = 0f;
    public float yOpen = 0f;
    public float xClose= 0f;
    public float yClose = 400f;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, Vector3.down * (yOpen + 400 * open), Time.deltaTime * 10f);
    }

    public void Toggle()
    {
        open = 1 - 1 * open;
    }
}
