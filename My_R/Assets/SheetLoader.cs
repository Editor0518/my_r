using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SheetLoader : MonoBehaviour
{
    public SheetData sheetData;
    string strSheetData;
    const string url = "https://docs.google.com/spreadsheets/d/1zLN0G9wqISQeQfYmFMTT0c_MJNNW0AlWNWzJupQrMYY/export?format=tsv&range=A2:L";

    IEnumerator Start()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isDone) strSheetData = www.downloadHandler.text;
            else Debug.Log("Error: " + www.error);
        }
        DisplayText();
        LoadInSheetData();
    }

    void DisplayText()
    {
        Debug.Log(strSheetData);

    }

    public void LoadInSheetData()
    {
        int branch = 0;
        string[] rows = strSheetData.Split('\n');
        for (int i = 0; i < rows.Length; i++)
        {

            string[] columns = rows[i].Replace("\r", "").Split('\t');
            if (columns[0].Equals(columns[2]) && columns[1].Equals(columns[7])) continue;
            if (int.TryParse(columns[0], out int newBranch))//branch 새로 생성
            {
                branch = newBranch;
                //branch	start_cmd	name	focus	left	center	
                //right	dialog	font	after_cmd	move
                StoryBlock storyBlock = new StoryBlock(branch);
                // Debug.Log(columns[0] + ", " + columns[1] + ", " + columns[2] + ", " + columns[3] + ", " + columns[4] + ", " + columns[5] + ", " + columns[6] + ", " + columns[7] + ", " + columns[8] + ", " + columns[9] + ", " + columns[10]);
                int focus = int.TryParse(columns[3], out int focusOn) ? focusOn : -1;
                Block block = new Block(columns[1], columns[2], focus, columns[4], columns[5], columns[6], columns[7], columns[8], columns[9], columns[10], columns[11]);
                storyBlock.AddBlock(block);
                sheetData.AddStoryBlock(storyBlock);

            }
            else
            {
                if (columns[0].Equals(""))
                { //하위 branch로
                    int focus = int.TryParse(columns[3], out int focusOn) ? focusOn : -1;
                    Block block = new Block(columns[1], columns[2], focus, columns[4], columns[5], columns[6], columns[7], columns[8], columns[9], columns[10], columns[11]);
                    sheetData.AddBlock(branch, block);
                }
                else continue; //주석. 처리X, 스킵
            }
        }

        GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>().StartDialogueManager();
    }

}
