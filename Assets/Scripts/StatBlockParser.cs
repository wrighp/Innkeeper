using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

struct lineData
{
    public string[] words;
    public List<int> weights;
    public List<bool> forms;
    public int totalWeight;

    public lineData(string input)
    {
        totalWeight = 0;

        words = input.Split('\t');
        weights = new List<int>();
        forms = new List<bool>();
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 1 && words[i][0] == '/' && words[i][1] == '/')
            {
                Debug.Log("Comment");
                break;
            }
            else if (i == 0 && words[i].Length > 0 && words[i][0] == '#')
            {
                Debug.Log("Hash");
                totalWeight = -1;
                break;
            }
            forms.Add(false);
            if (words[i].Length > 0 && words[i][0] == '`')
            {
                weights.Add(1);
                words[i] = words[i].Substring(1);
            }
            else if (words[i].Length > 0 && words[i][0] == '[' && words[i][(words[i].Length - 1)] == ']')
            {
                forms[i] = true;
                weights.Add(1);
                words[i] = words[i].Substring(1, words[i].Length - 2);
            }
            else weights.Add(3);
            totalWeight += weights[i];
            Debug.Log(words[i] + weights[i] + forms[i]);
        }
        Debug.Log("totalweight: " + totalWeight);
    }

};

public static class StatBlockParser
{
    public static List<lineData> ReadData(string filename)
    {
        string[] lines = System.IO.File.ReadAllLines(@filename);
        List<lineData> rows = new List<lineData>();
        foreach(string line in lines) {
            rows.Add(new lineData(line));
        }

        return rows;
    }
}