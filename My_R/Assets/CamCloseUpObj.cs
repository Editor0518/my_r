using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CamCloseUpObj : MonoBehaviour
{
    public Minigame_Task minigame_Task;
    public float zoomSize = 3f;

    public Vector3 camMoveTrans = Vector3.back;

    float camZoomSize = 5f;
    Vector3 camReturnTrans;

    public GameObject toEnable;
    Button thisBtn;

    private void Start()
    {
        thisBtn = GetComponent<Button>();

    }

    public void CamCloseUpButton()
    {

        if (thisBtn != null) thisBtn.interactable = false;
        camReturnTrans = Camera.main.transform.position;
        camZoomSize = Camera.main.orthographicSize;
        StartCoroutine(CamZoom(zoomSize, camMoveTrans, true));
    }

    IEnumerator CamZoom(float zoom, Vector3 camTrans, bool toEnableActive)
    {
        WaitForSeconds wait = new WaitForSeconds(0.025f);
        int delta = 10;
        float x = (camTrans.x - Camera.main.transform.position.x) / delta; //Mathf.Lerp(Camera.main.transform.position.x, camTrans.x, 0.05f);
        float y = (camTrans.y - Camera.main.transform.position.y) / delta;//Mathf.Lerp(Camera.main.transform.position.y, camTrans.y, 0.05f);
        float minus = (Camera.main.orthographicSize - zoom) / delta;

        for (int i = 0; i < delta; i++)
        {
            Camera.main.transform.position += new Vector3(x, y, 0);
            Camera.main.orthographicSize -= minus;
            yield return wait;
        }

        if (toEnable != null) toEnable.SetActive(toEnableActive);

        Camera.main.transform.position = camTrans;
        Camera.main.orthographicSize = zoom;

        if (!toEnableActive)
        {

            if (minigame_Task != null) minigame_Task.AddMarkedDone();
            this.gameObject.SetActive(false);
        }
    }

    public void CamReturn()
    {
        toEnable.SetActive(false);
        StartCoroutine(CamZoom(camZoomSize, camReturnTrans, false));

    }
}