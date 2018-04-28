using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

public enum WordType
{
    StringInput,
    NumInput,
    Checked,
    Unchecked,
    String
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

    public enum ListType
    {
        Start,
        End,
        None
    }

    public LineData() {
        weights = new List<int>();
        forms = new List<WordType>();
    }

    /// <summary>
    /// Parse a line data in order to create stat block ui data
    /// </summary>
    /// <param name="input">Text to be parsed</param>
    /// <param name="sw">Weight for strings</param>
    /// <param name="nw">Weight for numbers</param>
    /// <param name="cw">Weight for checkboxes</param>
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
                //Debug.Log("Comment");
                break;
            }
            else if (words[i].Length > 0 && words[i][0] == '\n')
            {
                //Debug.Log("Newline");
                break;
            }
            else if (i == 0 && words[i].Length > 0 && words[i][0] == '#')
            {
                //Debug.Log("Hash");
                totalWeight = -1;
                break;
            }
            else if (i == 0 && words[i].Length > 0 && words[i][0] == '{')
            {
                //Debug.Log("List Start");
                listing = ListType.Start;
                break;
            }
            else if (i == 0 && words[i].Length > 0 && words[i][0] == '}')
            {
                //Debug.Log("List End");
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
        }
    }

};

public static class StatBlockParser
{
    /// <summary>
    /// Convert a string into line data objects
    /// </summary>
    /// <param name="text">Text to be converted</param>
    /// <param name="sw">Weight of strings</param>
    /// <param name="nw">Weight of numbers</param>
    /// <param name="cw">Weight of checkboxes</param>
    /// <returns></returns>
    public static List<LineData> StringToLineData(string text, int sw, int nw, int cw)
    {
        List<LineData> rows = new List<LineData>();
        using (StringReader sr = new StringReader(text))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                rows.Add(new LineData(line, sw, nw, cw));
            }
        }
        return rows;
    }

    /// <summary>
    /// Convert line data to string
    /// </summary>
    /// <param name="rows">List of all rows in the page</param>
    /// <param name="sw">Weight of strings</param>
    /// <param name="numWeight">Weight of numbers</param>
    /// <param name="cw">Weight of checks</param>
    /// <returns>String conversion of line data</returns>
    public static string LineDataToString(List<LineData> rows, int sw, int numWeight, int cw)
    {
        StringBuilder sb = new StringBuilder();
        //Debug.Log(rows.Count);
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
        return sb.ToString();
    }
}
