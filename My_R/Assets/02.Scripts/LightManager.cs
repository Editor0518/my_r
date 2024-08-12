using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static string currentLightGroup;

    [System.Serializable]
    public class LightGroup
    {
        public string lightGroupName;
        public Volume volume;
        public List<Light2D> lights = new List<Light2D>();

        public void SetLightActive(bool active)
        {
            foreach (var light in lights)
            {
                light.enabled = active;
            }
            volume.enabled = active;
        }

    }

    public List<LightGroup> lightGroups = new List<LightGroup>();

    public void SetLightGroupActive(string lightGroupName)
    {
        int count = 0;
        foreach (var lightGroup in lightGroups)
        {
            if (lightGroup.lightGroupName == lightGroupName)
            {
                lightGroup.SetLightActive(true);
                count++;
            }
            else if (lightGroup.lightGroupName == currentLightGroup)
            {
                lightGroup.SetLightActive(false);
                count++;
            }
            if (count >= 2) return;

        }
    }

    public void SetLightGroupDisactive()
    {
        foreach (var lightGroup in lightGroups)
        {
            lightGroup.SetLightActive(false);
        }
    }


}
