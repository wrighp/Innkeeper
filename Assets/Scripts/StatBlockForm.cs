using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StatBlockForm : MonoBehaviour
{

    public Transform testLayout;

    public string name;
    public TextAsset textAsset;
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
    //void Start()
    //{
    //    CreateStatBlock(new StatBlockUIData());
    //}

    //public void CreateTestStatBlock()
    //{
    //    CreateStatBlock(new StatBlockUIData());
    //}

    /// <summary>
    /// Call to create UI elements from StatBlockUIData
    /// Passing in data with an empty string creates based on template
    /// </summary>
    /// <param name="uiData"></param>
    public void CreateStatBlock(StatBlockUIData uiData)
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
        string sourceText;
        if(uiData.text == null)
        {
            sourceText = textAsset.text;
            name = textAsset.name;
        }
        else
        {
            sourceText = uiData.text;
            name = uiData.name;
        }

        List<LineData> rows = StatBlockParser.StringToLineData(sourceText, stringWeight, numWeight, checkWeight);
        
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

        //Debug.Log(StatBlockParser.LineDataToString(rows, stringWeight, numWeight, checkWeight));
        
    }

    public void PrepareStatPacket() {
        print("Preparing packet");
        PagePacket packet = new PagePacket();
        packet.name = name;
        packet.pageType = PagePacket.PageType.StatBlockUI;
        packet.data = SerializationManager.SerializeObject(this.CreateStatBlockUIData());
        packet.destroy = false;
        
        foreach (ClientController cc in GameObject.FindObjectsOfType<ClientController>()) {
            if(cc.isLocalPlayer)
                cc.CmdSendPagePacket(packet);
        }
    }

    /// <summary>
    /// Call to get stat block data from ui elements
    /// </summary>
    /// <returns></returns>
    public StatBlockUIData CreateStatBlockUIData()
    {
        StatBlockUIData uiData = new StatBlockUIData();
        List<LineData> rows = new List<LineData>();
        //Loop through UI elements and convert to list of linedata

        for (int i = 0; i < testLayout.childCount; ++i)
        {
            LineData lD = new LineData();
            List<string> words = new List<string>();
            Transform t = testLayout.GetChild(i);

            for (int j = 0; j < t.childCount; ++j)
            {
                Transform child = t.GetChild(j);
                switch (child.tag) {
                    case "Input":
                        lD.forms.Add(WordType.StringInput);
                        words.Add(child.GetChild(1).GetComponent<Text>().text);
                        break;
                    case "NumInput":
                        lD.forms.Add(WordType.NumInput);
                        words.Add(child.GetChild(1).GetComponent<Text>().text);
                        break;
                    case "Text":
                        lD.forms.Add(WordType.String);
                        words.Add(child.GetComponent<Text>().text);
                        break;
                    case "ToggleOn":
                        lD.forms.Add(WordType.Checked);
                        words.Add("on");
                        break;
                    case "ToggleOff":
                        lD.forms.Add(WordType.Unchecked);
                        words.Add("off");
                        break;
                    default:
                        Debug.LogError("Invalid type passed to CreateStatBlockUiData");
                        break;
                }

            }

            //If empty line spacer set the total weight to 0
            if (t.childCount == 0)
            {
                lD.totalWeight = 0;
            }

            //Set weights
            lD.stringWeight = stringWeight;
            lD.numWeight = numWeight;
            lD.checkWeight = checkWeight;

            rows.Add(lD);
        }


        uiData.text = StatBlockParser.LineDataToString(rows, stringWeight, numWeight, checkWeight);
        return uiData;
    }


}
