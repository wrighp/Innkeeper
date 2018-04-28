using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[Serializable]
public class StatBlockUIData : PageData
{
    public string text;
}

public class StatBlockForm : PageObject
{

    public Transform testLayout;
    public string fullPath;
    public string pageName;
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

    /// <summary>
    /// Call to create UI elements from StatBlockUIData
    /// Passing in data with an empty string creates based on template
    /// </summary>
    /// <param name="data">All the data needed to specify the page type</param>
    public override void BuildPage(PageData data)
    {
        StatBlockUIData uiData = (StatBlockUIData)data;
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
        }
        else
        {
            sourceText = uiData.text;
        }

        List<LineData> rows = StatBlockParser.StringToLineData(sourceText, stringWeight, numWeight, checkWeight);

        Transform layoutGroup = testLayout;
        GameObject.Instantiate(lineSegmentUI, layoutGroup);
        //Iterate through every row of lie data, creating the specified prefab, filling its contents with the user defined data
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
                        obj.GetComponent<InputField>().text = text;
                        obj.GetComponent<InputField>().onEndEdit.AddListener(delegate { SaveStatblock(); });
                        break;
                    case WordType.NumInput:
                        obj = (GameObject)GameObject.Instantiate(numInputUI, lineSpacer);
                        obj.GetComponent<InputField>().text = text;
                        obj.GetComponent<InputField>().onEndEdit.AddListener(delegate { SaveStatblock(); });
                        break;
                    case WordType.Checked:
                        obj = (GameObject)GameObject.Instantiate(checkboxUIOn, lineSpacer);
                        obj.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SaveStatblock(); });
                        break;
                    case WordType.Unchecked:
                        obj = (GameObject)GameObject.Instantiate(checkboxUI, lineSpacer);
                        obj.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SaveStatblock(); });
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
    }

    /// <summary>
    /// Net code to create a page packet for the current page form
    /// </summary>
    public void PrepareStatPacket() {
        if (pageName == "") return;
        print("Prepare stat packet");
        PagePacket packet = new PagePacket();
        packet.name = pageName;
        print("PageName: " + pageName);
        packet.pageType = PagePacket.PageType.StatBlockUI;
        packet.data = SerializationManager.SerializeObject(this.CreateStatBlockUIData());
        packet.destroy = false;
        
        //Get the local player and have the object send the data
        foreach (ClientController cc in GameObject.FindObjectsOfType<ClientController>()) {
            if (cc.isLocalPlayer) {
                cc.CmdSendPagePacket(packet);
                break;
            }
        }
    }

    /// <summary>
    /// Event Listener, Serialize and save the current statblock to the system
    /// </summary>
    public void SaveStatblock()
    {
        StatBlockUIData sbd = CreateStatBlockUIData();
        SerializationManager.SaveObject(fullPath, sbd);
    }

    /// <summary>
    /// Call to get stat block data from ui elements
    /// </summary>
    /// <returns>Return a statblockuidata that can be converted to string</returns>
    public StatBlockUIData CreateStatBlockUIData()
    {
        StatBlockUIData uiData = new StatBlockUIData();
        List<LineData> rows = new List<LineData>();
        //Loop through UI elements and convert to list of linedata
        
        //Iterate trhouh each row of elements
        for (int i = 0; i < transform.childCount; ++i)
        {
            LineData lD = new LineData();
            List<string> words = new List<string>();
            Transform t = testLayout.GetChild(i);

            //Iterate through each element addings its information to the statblockuidata
            for (int j = 0; j < t.childCount; ++j)
            {
                Transform child = t.GetChild(j);
                switch (child.tag)
                {
                    case "Input":
                        {

                            lD.forms.Add(WordType.StringInput);
                            words.Add(child.GetComponent<InputField>().text);
                            lD.totalWeight += stringWeight;
                            lD.weights.Add(stringWeight);
                            break;
                        }
                    case "NumInput":
                        {
                            lD.forms.Add(WordType.NumInput);
                            words.Add(child.GetComponent<InputField>().text);
                            lD.totalWeight += numWeight;
                            lD.weights.Add(numWeight);
                            break;
                        }
                    case "Text":
                        {
                            lD.forms.Add(WordType.String);
                            words.Add(child.GetComponent<Text>().text);
                            lD.totalWeight += stringWeight;
                            lD.weights.Add(stringWeight);
                            break;
                        }
                    case "ToggleOn":
                        {
                            lD.forms.Add(WordType.Checked);
                            words.Add("on");
                            lD.totalWeight += checkWeight;
                            lD.weights.Add(checkWeight);
                            break;
                        }
                    case "ToggleOff":
                        {
                            lD.forms.Add(WordType.Unchecked);
                            words.Add("off");
                            lD.totalWeight += checkWeight;
                            lD.weights.Add(checkWeight);
                            break;
                        }
                    default:
                        {
                            Debug.LogError("Invalid type passed to CreateStatBlockUiData");
                            break;
                        }
                }

            }
            lD.words = words.ToArray();
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
