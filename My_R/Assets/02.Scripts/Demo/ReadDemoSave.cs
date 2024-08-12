using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.AccessControl;

public class ReadDemoSave : MonoBehaviour
{
    //https://learn.microsoft.com/ko-kr/dotnet/api/system.io.filestream?view=net-8.0

    //static string path = @"C:\Program Files (x86)\Steam\steamapps\common\new folder test\demo test.txt";
    static string path = "\"C:\\Program Files (x86)\\Steam\\steamapps\\common\\new folder test\\demo test.txt\"";

    //static string path = @"C:\Users\USER\Downloads\demodemo\demo test.txt";
    string fileData;

    private void Awake()
    {

        try
        {

            if (!File.Exists(path))
            {
                Debug.Log("no file");
                return;
            }
            using (FileStream fRead = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                int readLen;

                while ((readLen = fRead.Read(b, 0, b.Length)) > 0)
                {
                    //System.Diagnostics.Debug.WriteLine(temp.GetString(b, 0, readLen));
                    fileData=temp.GetString(b, 0, readLen);
                }

                //System.Diagnostics.Debug.WriteLine("Done writing and reading data.");
                Debug.Log("Done reading data."+fileData);
            }
        }
        catch (Exception e)
        {
            //System.Diagnostics.Debug.WriteLine(e);
            Debug.Log(e);
        }
    }
   
}
