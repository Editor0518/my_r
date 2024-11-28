using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "FontManager", menuName = "FontManager")]
public class FontManager : ScriptableObject
{
    public TMP_FontAsset defaultFont;
    public TMP_FontAsset drunkTalkFont;
    public TMP_FontAsset drunkSeriousFont;
    public TMP_FontAsset actingToneFont;
    public TMP_FontAsset madFont;


    public TMP_FontAsset GetFont(string fontName)
    {
        switch (fontName)
        {
            case "DEFAULT":
            case "":
                return defaultFont;
            case "DRUNKTALK":
                return drunkTalkFont;
            case "DRUNKSERIOUS":
                return drunkSeriousFont;
            case "ACTINGTONE":
                return actingToneFont;
            case "MAD":
                return madFont;
        }
        return null;
    }

    public float GetFontSize(string fontName)
    {
        switch (fontName)
        {
            case "DEFAULT":
            case "":
                return 37.5f;
            case "DRUNKTALK":
                return 49;
            case "DRUNKSERIOUS":
                return 39f;
            case "ACTINGTONE":
                return 36;
            case "MAD":
                return 36;
        }
        return 37.5f;
    }

    public float GetLineSpacing(string fontName)
    {
        switch (fontName)
        {
            case "DEFAULT":
            case "":
                return 0f;
            case "DRUNKTALK":
                return -15f;
            case "DRUNKSERIOUS":
                return 6f;
            case "ACTINGTONE":
                return 15f;
            case "MAD":
                return 0f;
        }
        return 0f;
    }

    public float GetCharacterSpacing(string fontName)
    {
        switch (fontName)
        {
            case "DEFAULT":
            case "":
                return 0f;
            case "DRUNKTALK":
                return 2f;
            case "DRUNKSERIOUS":
                return -1f;
            case "ACTINGTONE":
                return 0;// -15f;
            case "MAD":
                return 0f;
        }
        return 0f;
    }

    public float GetWordSpacing(string fontName)
    {
        switch (fontName)
        {
            case "DEFAULT":
            case "":
                return 0f;
            case "DRUNKTALK":
                return -5f;
            case "DRUNKSERIOUS":
                return -3.4f;
            case "ACTINGTONE":
                return -10f;
            case "MAD":
                return 0f;
        }
        return 0f;
    }
    public float GetAddPosY(string fontName)
    {
        switch (fontName)
        {
            case "DEFAULT":
            case "":
                return 0;
            case "DRUNKTALK":
                return 10f;
            case "DRUNKSERIOUS":
                return -4f;
            case "ACTINGTONE":
                return 0f;
            case "MAD":
                return 0f;
        }
        return 0f;
    }
}
