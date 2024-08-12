using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;


    bool isPouring = false;
    Stream currentStream = null;

    void Update()
    {
        bool pourCheck = CalculatePourAngle(origin.forward, Vector3.up) > pourThreshold;

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if (!isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }

    void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();
    }
    void EndPour()
    {
        if (currentStream != null) currentStream.End();
        currentStream = null;
        streamPrefab.SetActive(false);
    }

    float CalculatePourAngle(Vector3 originForward, Vector3 up)
    {
        //return originForward.y * Mathf.Rad2Deg;
        return transform.forward.y * Mathf.Rad2Deg;
    }

    Stream CreateStream()
    {
        streamPrefab.SetActive(true);
        streamPrefab.transform.position = origin.position;
        streamPrefab.transform.rotation = Quaternion.identity;

        return streamPrefab.GetComponent<Stream>();
    }
}