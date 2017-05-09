using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Utils {

    public static void ChangeGameObjectColorTo(GameObject pGameObject, Color pFrom, Color pTo) {
        if (pGameObject.transform.GetComponent<Renderer>().material.color == pFrom) {
            pGameObject.transform.GetComponent<Renderer>().material.color = pTo;
        }
    }

    public static void ChangeGameObjectsColor(GameObject[] pGameObjects, Color pFrom, Color pTo) {
        foreach (GameObject obj in pGameObjects) {
            if (obj.transform.GetComponent<Renderer>().material.color == pFrom) {
                obj.transform.GetComponent<Renderer>().material.color = pTo;
            }
        }
    }
    public static void ChangeGameObjectColor(GameObject pGameObject, Color pTo)
    {
       
       pGameObject.transform.GetComponent<Renderer>().material.color = pTo;
        
    }

    public static void WriteToFile(string path,string words)
    {
        Debug.Log("Written to file");
        StreamWriter write = new StreamWriter(path,true);
        write.WriteLine(words);
        write.Close();
    }

    public static void ReplaceLineFromFile(string path, string words, string stringToReplace)
    {

        StreamReader reader = new StreamReader(path);

        //int lineNumber=0;
        string[] file = File.ReadAllText(path).Split('\n', '\r');
        string[] toWrite = new string[file.Length];
        int lenght = 0;
        /**/
        foreach (string s in file)
        {
            if (s != "")
            {
                toWrite[lenght] = s;
                lenght++;
            }
        }
        /**/
        reader.Close();


        for (int i = 0; i < lenght; i++)
        {
            //Debug.Log(toWrite[i] + "|" + stringToReplace+"|");
            if (String.Compare(toWrite[i], stringToReplace) == 0)
            {
                //Debug.Log("replacing");
                toWrite[i] = words;
            }
        }
        File.WriteAllText(path, String.Empty);


        StreamWriter writer = new StreamWriter(path);
        for (int i=0;i< lenght;i++)
        {
            writer.WriteLine(toWrite[i]);
        }
        writer.Close();
    }

    public static int GetLastNumberFromFile(string path)
    {
        string text = ReadFromFile(path);

        StreamReader reader = new StreamReader(path);
        string[] file = reader.ReadToEnd().Split('\n', '\r', ' ');
        int latestNumber = 0;
        int tempNr;
        for (int i = 0; i < file.Length; i++)
        {
            //Debug.Log(file[i]);
            if(IsNumeric(file[i]))
            {
                latestNumber = Convert.ToInt32(file[i]);
            }
        }
        reader.Close();

        return latestNumber;
    }

    public static bool IsNumeric(string text)
    {
        float value;
        return float.TryParse(text, out value);
    }

    public static string ReadFromFile(string path)
    {
        StreamReader read = new StreamReader(path);
        string file = read.ReadToEnd();
        read.Close();
        return file;
    }

    public static int LatestLevel()
    {
        return Utils.GetValueAfterString("Assets\\SaveInfo.txt","onlevel:");
    }

    public static void ResetLastLevel()
    {
        Debug.Log("resetting level");
        ReplaceLineFromFile("Assets\\SaveInfo.txt", "onlevel: " + 1, "onlevel: " + LatestLevel());
    }

    public static void NextLevel()
    {
        int _level = LatestLevel();
        Utils.ReplaceLineFromFile("Assets\\SaveInfo.txt", "onlevel: " + (_level + 1), "onlevel: " + _level);
    }

    public static int EffectVolume()
    {
        return Utils.GetValueAfterString("Assets\\SaveInfo.txt", "effectaudio:");
    }

    public static void ChangeEffectVolume(int value)
    {
        Utils.SetValueAfterString("Assets\\SaveInfo.txt","effectaudio:",value);
    }

    public static int MusicVolume()
    {
        return Utils.GetValueAfterString("Assets\\SaveInfo.txt", "musicaudio:");
    }

    public static void ChangeMusicVolume(int value)
    {
        Utils.SetValueAfterString("Assets\\SaveInfo.txt", "musicaudio:", value);
    }

    public static int GetValueAfterString(string path, string pString)
    {
        string text = ReadFromFile(path);

        StreamReader reader = new StreamReader(path);
        string[] file = reader.ReadToEnd().Split('\n', '\r', ' ');
        int latestNumber = 0;
        for (int i = 0; i < file.Length; i++)
        {
            if (string.Compare(file[i], pString) == 0)
            {
                if (IsNumeric(file[i+1]))
                {
                    latestNumber = Convert.ToInt32(file[i+1]);
                    i++;
                }
            }
        }
        reader.Close();

        return latestNumber;
    }

    public static void SetValueAfterString(string path, string pString, int nextValue)
    {
        int lastValue = GetValueAfterString(path, pString);
        string before = pString + " " + lastValue;
        string after = pString + " " + nextValue;
        //Debug.Log("|" + before +"|"+ after + "|");
        ReplaceLineFromFile(path,after,before);
    }
}
