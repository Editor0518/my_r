using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "StandingSprite")]
public class StandingSpriteManager : ScriptableObject
{

    [System.Serializable]
    public class CharacterSpr
    {
        public string name;
        public Sprite sprite;
    }

    [System.Serializable]
    public class CharacterSprList
    {
        //public string characterName;
        public List<CharacterSpr> chapter1;
        public List<CharacterSpr> chapter2;
        public List<CharacterSpr> chapter3;
        public List<CharacterSpr> chapter4;
        public List<CharacterSpr> chapter5;
    }

    //public List<CharacterSpr> enjolrasSpr;
    //public List<CharacterSpr> grantaireSpr;

    public CharacterSprList enjolrasSpr;
    public CharacterSprList grantaireSpr;
    public CharacterSprList combeferreSpr;
    public CharacterSprList courfeyracSpr;
    public CharacterSprList jolySpr;
    public CharacterSprList lamarqueSpr;
    public CharacterSprList musichettaSpr;

    int iChapter = 1;//나중에 findsprite의 파라미터로 옮기기
    public Sprite FindSprite(string chName, string face)
    {
        Sprite spr = null;
        CharacterSprList sprListWhole;
        List<CharacterSpr> sprList;

        switch (chName)
        {
            case "GRANTAIRE":
                sprListWhole = grantaireSpr;
                break;
            case "ENJOLRAS":
                sprListWhole = enjolrasSpr;
                break;
            case "COMBEFERRE":
                sprListWhole = combeferreSpr;
                break;
            case "X":
                return null;
            case "COURFEYRAC":
                sprListWhole = courfeyracSpr;
                break;
            case "LAMARQUE":
                sprListWhole = lamarqueSpr;
                break;

            case "JOLY":
                sprListWhole = jolySpr;
                break;
            case "MUSICHETTA":
                sprListWhole = musichettaSpr;
                break;
            default:
                sprListWhole = new();
                break;
        }

        switch (iChapter)
        {
            case 1:
                sprList = sprListWhole.chapter1;
                break;
            case 2:
                sprList = sprListWhole.chapter2;
                break;
            default:
                sprList = new();
                break;
        }
        if (face.Replace(" ", "").Equals("")) face = "DEFAULT";
        for (int i = 0; i < sprList.Count; i++)
        {
            if (sprList[i].name.Equals(face))
            {
                spr = sprList[i].sprite;
                break;
            }
        }

        return spr;
    }
}
