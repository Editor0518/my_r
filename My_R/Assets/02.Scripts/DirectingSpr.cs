using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DirectingSpr", menuName = "ScriptableObjects/DirectingSpr", order = 1)]
public class DirectingSpr : ScriptableObject
{
    [System.Serializable]
    public struct DSprite
    {
        public string name;
        public Sprite sprite;
    }


    public List<DSprite> miniCutscenes;



    public Sprite FindCutSprite(string cutName)
    {
        for (int i = 0; i < miniCutscenes.Count; i++)
        {
            if (miniCutscenes[i].name.Equals(cutName))
            {
                return miniCutscenes[i].sprite;
            }
        }
        return null;
    }


}
