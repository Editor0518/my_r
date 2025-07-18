// StandingManager.cs
using UnityEngine;

public class StandingManager : MonoBehaviour
{
    public SpriteRenderer[] standingImages;
    public StandingSpriteManager spriteFinder;

    public void UpdateStanding(Block line)
    {
        for (int i = 0; i < standingImages.Length; i++)
        {
            string[] spriteData = line.standing[i].Split('_');
            if (spriteData.Length < 2)
                spriteData = new string[] { "", "" };

            Sprite sprite = spriteFinder.FindSprite(spriteData[0], spriteData[1]);
            standingImages[i].sprite = sprite;
            standingImages[i].enabled = sprite != null;
            standingImages[i].color = (line.focus == i) ? Color.white : new Color32(163, 164, 168, 255);
        }
    }
}