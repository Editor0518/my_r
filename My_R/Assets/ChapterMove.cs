using System.Collections.Generic;
using UnityEngine;

public class ChapterMove : MonoBehaviour
{
    public List<RectTransform> chapters;
    public List<RectTransform> behinds;

    public CharacterMove character;
    
    void Start()
    {
        
    }


    public void MoveOtherChapters(int index)
    {
        character.Move(chapters[index]);
    }
    
    public void MoveOtherBehinds(int index)
    {
        character.Move(behinds[index]);
    }
    
    
}
