using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Random = UnityEngine.Random;

public class SheetLoader : MonoBehaviour
{
    public static SheetLoader instance;
    public SheetData sheetData;
    private string sheetRaw;

    // ✅ TSV 형식으로 수정
    private static string baseUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQl4ZXuHtvXSeldXyaYzkrBwbM55z5_V9h01dD8h79iOdxCyK0oUF2sHh_ealI90OS5W7OOHSuztA5V/pub?output=tsv&gid=";

    public static int chapter = 1;
    public static bool isLoading = false; 
    private string currentBackground = "";


    private static readonly string[] gids = { "0", "1101962320", "0", "0", "0" }; // 예시

    private void Awake() => instance = this;

    void Start() => StartLoadSheet();

    public void StartLoadSheet()
    {
        SheetData.instance.storyBlock = new List<StoryBlock>();
        StartCoroutine(LoadSheetCoroutine());
    }

    IEnumerator LoadSheetCoroutine()
    {
        string url = baseUrl + gids[chapter] + "&range=A2:P&nocache=" + DateTime.UtcNow.Ticks + Random.Range(0, 100000);

        using UnityWebRequest www = UnityWebRequest.Get(url);
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
                sheetRaw = www.downloadHandler.text;
            else
                Debug.LogError("Download error: " + www.error);
        }

        isLoading = true;
        Debug.Log("Load into Sheet Data...\n" + sheetRaw);
        sheetRaw = sheetRaw.Replace("\r", "");
        LoadIntoSheetData();

        yield return new WaitUntil(() => !isLoading);
        Debug.Log("Dialogue Master Start Dialogue");
        DialogueMaster.Instance.StartDialogue(chapter, 1);
    }

    void LoadIntoSheetData()
    {
        int currentBranch = 0;
        string[] rows = sheetRaw.Split('\n');

        foreach (var row in rows)
        {
            string[] cols = ParseTsvRow(row);
            if (cols.Length < 16) continue;

            if (int.TryParse(cols[0], out int newBranch))
            {
                currentBranch = newBranch;
                var block = CreateBlock(cols);
                var story = new StoryBlock(currentBranch);
                story.AddBlock(block);
                sheetData.AddStoryBlock(story);
            }
            else if ((cols[0].Equals("") && cols[1].Equals("") && cols[2].Equals("") && cols[11].Equals("") &&
                      cols[15].Equals("")))
            {
                //blank cell
                continue;
            }
            else if (!cols[0].StartsWith("//"))
            {
                var block = CreateBlock(cols);
                sheetData.AddBlock(currentBranch, block);
            }
        }

        isLoading = false;
    }

    Block CreateBlock(string[] cols)
    {
        int focus = int.TryParse(cols[3], out var f) ? f : -1;

        cols[4] = CombineFace(cols[4], cols[7]);
        cols[5] = CombineFace(cols[5], cols[8]);
        cols[6] = CombineFace(cols[6], cols[9]);

        string foundBackground = ExtractTaggedValue(ref cols[1], "background_");
        if (!string.IsNullOrEmpty(foundBackground))
        {
            currentBackground = foundBackground;
        }

        AppendTagTo(ref cols[14], ref cols[1], "CHOICE_");
        AppendTagTo(ref cols[14], ref cols[1], "MOVECMD_");

        if (cols[14].Contains("MINIGAME_") && !cols[1].Contains("isNoNext_false"))
            cols[1] = string.IsNullOrEmpty(cols[1]) ? "isNoNext_false" : cols[1] + ";isNoNext_false";

        return new Block(
            currentBackground,
            cols[1], cols[2], focus,
            cols[4], cols[5], cols[6],
            cols[11], cols[12], cols[13], cols[14], cols[15]
        );
    }

    // ✅ 간단한 탭 분할 함수
    static string[] ParseTsvRow(string row)
    {
        return row.Split('\t');
    }

    static string CombineFace(string part, string face) => string.IsNullOrEmpty(part) ? "" : part + "_" + face;

    static string ExtractTaggedValue(ref string cmd, string tag)
    {
        Match match = Regex.Match(cmd, $"{tag}([^;_]+)");
        if (match.Success)
        {
            cmd = cmd.Replace(tag + match.Groups[1].Value + ";", "");
            return match.Groups[1].Value;
        }
        return "";
    }

    static void AppendTagTo(ref string target, ref string source, string tag)
    {
        Match match = Regex.Match(source, $"{tag}([^;_]+)");
        if (match.Success)
        {
            string full = tag + match.Groups[1].Value;
            target = string.IsNullOrEmpty(target) ? full : target + ";" + full;
            source = source.Replace(full, "");
        }
    }
}
