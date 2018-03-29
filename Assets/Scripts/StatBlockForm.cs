using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StatBlockForm : MonoBehaviour
{

    public Transform testLayout;


    public TextAsset text;
    public string savePath;
    public int stringWeight;
    public int numWeight;
    public int checkWeight;

    public GameObject lineSegmentUI;
    public GameObject textUI;
    public GameObject checkboxUI;
    public GameObject checkboxUIOn;
    public GameObject stringInputUI;
    public GameObject numInputUI;


    // Use this for initialization
    void Start()
    {
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
        List<LineData> rows = StatBlockParser.ReadData(text.text, stringWeight, numWeight, checkWeight);
        
        Transform layoutGroup = testLayout;
        GameObject.Instantiate(lineSegmentUI, layoutGroup);
        for (int i = 0, rowsCount = rows.Count; i < rowsCount; i++)
        {
            LineData row = rows[i];

            if (row.listing == LineData.ListType.Start || row.listing == LineData.ListType.End)
            {
                continue;
            }
            if (row.totalWeight == 0)
            {
                //If "//" then skip line spacing
                continue;
            }
            Transform lineSpacer = ((GameObject)GameObject.Instantiate(lineSegmentUI, layoutGroup)).transform;
            if (row.totalWeight == -1)
            {
                //If "#" then skip building words
                continue;
            }

            for (int j = 0; j < row.words.Length; j++)
            {

                var text = row.words[j];

                GameObject obj;
                switch (row.forms[j])
                {
                    case WordType.StringInput:
                        obj = (GameObject)GameObject.Instantiate(stringInputUI, lineSpacer);
                        obj.GetComponentsInChildren<Text>()[1].text = text;
                        break;
                    case WordType.NumInput:
                        obj = (GameObject)GameObject.Instantiate(numInputUI, lineSpacer);
                        obj.GetComponentsInChildren<Text>()[1].text = text;
                        break;
                    case WordType.Checked:
                        obj = (GameObject)GameObject.Instantiate(checkboxUIOn, lineSpacer);
                        break;
                    case WordType.Unchecked:
                        obj = (GameObject)GameObject.Instantiate(checkboxUI, lineSpacer);
                        break;
                    case WordType.String:
                        obj = (GameObject)GameObject.Instantiate(textUI, lineSpacer);
                        obj.GetComponent<Text>().text = text;
                        break;
                    default:
                        break;
                }
            }


        }

        StringBuilder sb = new StringBuilder();
        Debug.Log(rows.Count);
        for (int i = 0, rowsCount = rows.Count; i < rowsCount; i++)
        {
            LineData row = rows[i];
            if (row.listing == LineData.ListType.Start)
            {
                sb.Append("{\n");
            }
            else if (row.listing == LineData.ListType.End)
            {
                sb.Append("}\n");
            }
            else if (row.totalWeight == 0)
            {
                continue;
            }
            else if (row.totalWeight == -1)
            {
                sb.Append("#\n");
            }
            else
            {
                for (int w = 0, wordCount = row.words.Length; w < wordCount; w++)
                {
                    switch (row.forms[w])
                    {
                        case WordType.Unchecked:
                            {
                                sb.Append("()\t");
                                break;
                            }
                        case WordType.Checked:
                            {
                                sb.Append("(*)\t");
                                break;
                            }
                        case WordType.NumInput:
                            {
                                sb.Append("[" + row.words[w] + "]\t");
                                break;
                            }
                        case WordType.StringInput:
                            {
                                if (row.weights[w] == numWeight)
                                {
                                    sb.Append("`");
                                }
                                sb.Append("<" + row.words[w] + ">\t");
                                break;
                            }
                        case WordType.String:
                            {
                                if (row.weights[w] == numWeight)
                                {
                                    sb.Append("`");
                                }
                                sb.Append(row.words[w] + "\t");
                                break;
                            }
                    }

                    if (w == wordCount - 1)
                    {
                        sb.Append("\n");
                    }
                }
            }
        }

        string str = sb.ToString();
        Debug.Log(str);
        StreamWriter writer = new StreamWriter(savePath, true);
        writer.Write(str);
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
