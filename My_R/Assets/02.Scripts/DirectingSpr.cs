using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "DirectingSpr", menuName = "ScriptableObjects/DirectingSpr", order = 1)]
public class DirectingSpr : ScriptableObject
{
    [System.Serializable]
    public struct DSprite
    {
        public string name;
        public Sprite sprite;
    }
    [System.Serializable]
    public struct DVolume
    {
        public string name;
        public GameObject volumePrefab;
    }

    [System.Serializable]
    public struct DVideo
    {
        public string name;
        public VideoClip video;
    }

    public List<DSprite> backgrounds;
    public List<DVideo> videos;
    public List<DSprite> miniCutscenes;
    public List<DSprite> cutscenes;

    public List<DVolume> volumeList;


    public VideoClip FindVideo(string name)
    {
        return videos.Find(x => x.name == name).video;
    }

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

    public Sprite FindCutscene(string name)
    {
        return cutscenes.Find(x => x.name == name).sprite;
    }


    public Sprite FindBackground(string name)
    {
        return backgrounds.Find(x => x.name == name).sprite;
    }

    public GameObject FindVolume(string name)
    {
        return volumeList.Find(x => x.name == name).volumePrefab;
    }



}
