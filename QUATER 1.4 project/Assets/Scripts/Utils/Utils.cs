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

        for (int i = 0; i < lenght; i++)
        {
            //Debug.Log(file[i]);
            if (String.Compare(toWrite[i], stringToReplace) == 0)
            {
                //Debug.Log("replacing");
                file[i] = words;
            }
        }
        reader.Close();
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

    public static int GetNumberAfterLastString(string path)
    {
        string text = ReadFromFile(path);

        StreamReader reader = new StreamReader(path);
        string[] file = reader.ReadToEnd().Split('\n', '\r', ' ');
        int latestNumber = 0;
        for (int i = 0; i < file.Length; i++)
        {
            //Debug.Log(file[i]);
            if (IsNumeric(file[i]))
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
        return Utils.GetLastNumberFromFile("Assets\\SaveInfo.txt");
    }

    public static void ResetLastLevel()
    {
        ReplaceLineFromFile("Assets\\SaveInfo.txt","on level: "+1,"on level: "+GetLastNumberFromFile("Assets\\SaveInfo.txt"));
    }

    public static void GetVolume()
    {

    }
    public static void SetVolume()
    {

    }
}
