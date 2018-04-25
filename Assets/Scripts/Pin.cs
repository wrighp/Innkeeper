using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pin : MonoBehaviour {

    public string reference;

	// Use this for initialization
	void Start () {
        PinManager pM = GameObject.FindObjectOfType<PinManager>();

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener((data) => { pM.Drag(gameObject); });
        trigger.triggers.Add(drag);

        EventTrigger.Entry press = new EventTrigger.Entry();
        press.eventID = EventTriggerType.PointerDown;
        press.callback.AddListener((data) => { DisplayReference(); });
        trigger.triggers.Add(press);

        EventTrigger.Entry release = new EventTrigger.Entry();
        release.eventID = EventTriggerType.PointerUp;
        release.callback.AddListener((data) => { pM.Release(gameObject); });
        trigger.triggers.Add(release);
    }
	
    public void DisplayReference()
    {
        print("asdasd");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
