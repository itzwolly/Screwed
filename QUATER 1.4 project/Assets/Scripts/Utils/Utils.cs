using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Utils {

    public static void ChangeGameObjectColor(GameObject pGameObject, Color pFrom, Color pTo) {
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

    public static void ChangeGameObjectColorTo(GameObject pGameObject, Color pTo)
    {
       
       //pGameObject.transform.GetComponent<Renderer>().material.color = pTo;
        
    }

    public static void WriteToFile(string path,string words)
    {
        //Debug.Log("Written to file");
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

    public static void SaveStats(string path, string info)
    {
        if (ReadFromFile(path).Length > 1)
        {
            File.WriteAllText(path, String.Empty);
        }

        WriteToFile(path, info);
    }

    public static void GetStats(string path, out int timeinlevel,out bool completedwithoutdmg, out int totalshots, out int successfullheadshots, out int headshotkills, out int totalrangedkills, out int totalknives, out int successfullknives, out int knifekills, out int blockedshots, out int totalkills, out int secretsgathered)
    {
        timeinlevel = GetValueAfterString(path,"Timeinlevel:");
        if(GetValueAfterString(path, "Completedwithoutdmg:")==1)
        {
            completedwithoutdmg = true;
        }
        else
        {
            completedwithoutdmg = false;
        }
        totalshots = GetValueAfterString(path, "Totalshots:");
        successfullheadshots = GetValueAfterString(path, "Successfullheadshots:");
        headshotkills = GetValueAfterString(path, "Headshotkills:");
        totalrangedkills = GetValueAfterString(path, "Totalrangedkills:");
        totalknives = GetValueAfterString(path, "Totalknives:");
        successfullknives = GetValueAfterString(path, "Successfullknives:");
        knifekills = GetValueAfterString(path, "Knifekills:");
        blockedshots = GetValueAfterString(path, "Blockedshots:");
        totalkills = GetValueAfterString(path, "Totalkills:");
        secretsgathered = GetValueAfterString(path, "Secretsgathered:");
    }

    public static int GetSuccessfullShots(string path)
    {
        return GetValueAfterString(path, "Successfullshots:");
    }

    public static int GetSecretsGathered(string path)
    {
        return GetValueAfterString(path, "Secretsgathered:");
    }

    public static int GetTotalKills(string path)
    {
        return GetValueAfterString(path, "Totalkills:");
    }

    public static int GetBlockedShots(string path)
    {
        return GetValueAfterString(path, "Blockedshots:");
    }


    public static int GetKnifeKills(string path)
    {
        return GetValueAfterString(path, "Knifekills:");
    }

    public static int GetSuccessfullKnives(string path)
    {
        return GetValueAfterString(path, "Successfullknives:");
    }

    public static int GetTotalKnives(string path)
    {
        return GetValueAfterString(path, "Totalknives:");
    }

    public static int GetTotalRangedKills(string path)
    {
        return GetValueAfterString(path, "Totalrangedkills:");
    }

    public static int GetHeadshotKills(string path)
    {
        return GetValueAfterString(path, "Headshotkills:");
    }

    public static int GetSuccessfullHeadshots(string path)
    {
        return GetValueAfterString(path, "Successfullheadshots:");
    }

    public static int GetTotalShots(string path)
    {
        return GetValueAfterString(path, "Totalshots:");
    }

    public static bool GetCompletedWithoutDmg(string path)
    {
        if (GetValueAfterString(path, "Completedlevelwithoutdmg:") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int GetTimeInLevel(string path)
    {
        return GetValueAfterString(path, "Timeinlevel:");
    }

    public static void NextLevel(string statsPath,string info)
    {
        SaveStats(statsPath,info);
        Debug.Log("On to next level");
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
        Debug.Log(pString);
        int lastValue = GetValueAfterString(path, pString);
        string before = pString + " " + lastValue;
        string after = pString + " " + nextValue;
        //Debug.Log(before + "|" + after);
        //Debug.Log("|" + before +"|"+ after + "|");
        ReplaceLineFromFile(path,after,before);
    }
}
