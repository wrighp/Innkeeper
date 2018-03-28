using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

public enum WordType
{
    StringInput,
    NumInput,
    Checked,
    Unchecked,
    String
}

public enum ListType
{
    Start,
    End,
    None
}

public class LineData
{
    public string[] words;
    public List<int> weights;
    public List<WordType> forms;
    public ListType listing = ListType.None;
    public int totalWeight;

    public int stringWeight;
    public int numWeight;
    public int checkWeight;

    public LineData(string input, int sw, int nw, int cw)
    {
        stringWeight = sw;
        numWeight = nw;
        checkWeight = cw;
        totalWeight = 0;

        words = input.Split('\t');
        weights = new List<int>();
        forms = new List<WordType>();
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 1 && words[i][0] == '/' && words[i][1] == '/')
            {
                Debug.Log("Comment");
                break;
            }
            else if (words[i].Length > 0 && words[i][0] == '\n')
            {
                Debug.Log("Newline");
                break;
            }
            else if (i == 0 && words[i].Length > 0 && words[i][0] == '#')
            {
                Debug.Log("Hash");
                totalWeight = -1;
                break;
            }
            else if (i == 0 && words[i].Length > 0 && words[i][0] == '{')
            {
                Debug.Log("List Start");
                listing = ListType.Start;
                break;
            }
            else if (i == 0 && words[i].Length > 0 && words[i][0] == '}')
            {
                Debug.Log("List End");
                listing = ListType.End;
                break;
            }

            bool ticked = false;

            if (words[i].Length > 0 && words[i][0] == '`')
            {
                ticked = true;
                words[i] = words[i].Substring(1);
            }

            if (words[i].Length > 0 && words[i][0] == '[' && words[i][(words[i].Length - 1)] == ']')
            {
                forms.Add(WordType.NumInput);
                words[i] = words[i].Substring(1, words[i].Length - 2);
            }
            else if (words[i].Length > 0 && words[i][0] == '<' && words[i][(words[i].Length - 1)] == '>')
            {
                forms.Add(WordType.StringInput);
                words[i] = words[i].Substring(1, words[i].Length - 2);
            }
            else if (words[i].Length > 0 && words[i][0] == '(' && words[i][(words[i].Length - 1)] == ')')
            {
                words[i] = words[i].Substring(1, words[i].Length - 2);
                if (words[i].Contains("*"))
                {
                    forms.Add(WordType.Checked);
                }
                else
                {
                    forms.Add(WordType.Unchecked);
                }
                words[i] = "";
            }
            else
            {
                forms.Add(WordType.String);
            }

            if (!ticked)
            {
                switch (forms[forms.Count - 1])
                {
                    case WordType.NumInput:
                        {
                            weights.Add(numWeight);
                            break;
                        }

                    case WordType.StringInput: 
                        {
                            weights.Add(stringWeight);
                            break;
                        }

                    case WordType.Checked:
                        {
                            weights.Add(checkWeight);
                            break;
                        }

                    case WordType.Unchecked:
                        {
                            weights.Add(checkWeight);
                            break;
                        }

                    case WordType.String:
                        {
                            weights.Add(stringWeight);
                            break;
                        }
                }
            }
            else
            {
                weights.Add(numWeight);
            }
            totalWeight += weights[i];
            Debug.Log(words[i] + weights[i] + forms[i]);
        }
        Debug.Log("totalweight: " + totalWeight);
    }

};

public static class StatBlockParser
{
    public static List<LineData> ReadData(TextAsset textAsset, int sw, int nw, int cw)
    {
        List<LineData> rows = new List<LineData>();
        using (StringReader sr = new StringReader(textAsset.ToString()))
        {
            string line;
            while((line = sr.ReadLine()) != null)
            {
                rows.Add(new LineData(line, sw, nw, cw));
            }
        }

        return rows;
    }
}
