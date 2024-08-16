using System.Collections;
using UnityEngine;

public class ScreenEffect : MonoBehaviour
{
    public Camera cam;

    //진동

    public void StartVibrate()
    {
        //Handheld.Vibrate();
        Debug.Log("진동");
    }

    //화면 흔들림

    public void Flinch()
    {
        ScreenShake(0.15f, 0, 0.1f);
    }

    public void ScreenShake(float duration, float magnitudeX, float magnitudeY)
    {
        StartCoroutine(Shake(duration, magnitudeX, magnitudeY));
        Debug.Log("흔들기");
    }

    IEnumerator Shake(float duration, float magnitudeX, float magnitudeY)
    {
        Vector3 originalPos = cam.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitudeX;
            float y = Random.Range(-1f, 1f) * magnitudeY;

            cam.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;

        }

        cam.transform.localPosition = originalPos;


    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {

        }
    }
}
