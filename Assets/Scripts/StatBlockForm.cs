using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBlockForm : MonoBehaviour {

    public TextAsset text;
    public int stringWeight;
    public int numWeight;
    public int checkWeight;

    // Use this for initialization
    void Start () {
        /* rows holds a list object of lineData objectss
         * For each line in the statblock file passed to the parser,
         * a lineData struct is created which stores
         *      words:       array of strings separated by tabs,
         *                   modified to remove ` marks and [] brackets
         *      weights:     a list object of ints containing the weights
         *                   corresponding to the strings in words
         *                   e.g. words[i] has a weight weights[i]
         *      forms:       a list object of WordType enums that correspond
         *                   to each string in words:
         *                      String:      a regular string
         *                      StringInput: an alphanumeric input
         *                      NumInput:    a numeric input
         *                      Checked:     a checked checkbox
         *                      Unchecked:   an unchecked checkbox
         *                   e.g. words[i] having a field is forms[i]
         *      listing:     a ListType enum that denotes if this line
         *                   of data is the start of a list, the end of
         *                   a list, or neither
         *      totalWeight: the total weight of the line
         *                   NOTE: totalWeight = -1 indicates an empty
         *                   space. totalWeight = 0 indicates this line
         *                   is not visible
         */
        List<LineData> rows = StatBlockParser.ReadData(text, stringWeight, numWeight, checkWeight);

        

        for (int i = 0, rowsCount = rows.Count; i < rowsCount; i++)
        {
            LineData row = rows[i];

            if(row.listing == LineData.ListType.Start)
            {

            }
           


            for(int j = 0; j < row.words.Length; j++)
            {

            }
            

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
