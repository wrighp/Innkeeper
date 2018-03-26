using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBlockForm : MonoBehaviour {

    public string Path;


	// Use this for initialization
	void Start () {
        List<lineData> rows = StatBlockParser.ReadData(Path);
        foreach(lineData obj in rows)
        {

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
