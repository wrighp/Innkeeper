using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBlockForm : MonoBehaviour {

    public string Path;


	// Use this for initialization
	void Start () {
	/* rows holds a list object of lineData structs
         * For each line in the statblock file passed to the parser,
         * a lineData struct is created which stores
         *      words:       array of strings separated by tabs,
         *                   modified to remove ` marks and [] brackets
         *      weights:     a list object of ints containing the weights
         *                   corresponding to the strings in words
         *                   e.g. words[i] has a weight weights[i]
         *      forms:       a list object of bools containing whether
         *                   or not the corresponding string in words
         *                   contained in an editable field
         *                   e.g. words[i] having a field is forms[i]
         *      totalWeight: the total weight of the line
         *                   NOTE: totalWeight = -1 indicates an empty
         *                   space
         */
        List<lineData> rows = StatBlockParser.ReadData(Path);
        foreach(lineData obj in rows)
        {

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
