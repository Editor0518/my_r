using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CamCloseUpObj : MonoBehaviour
{
    public Camera cam;
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
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
        // cam.GetComponent<Animator>().enabled = false;
    }

    public void CamCloseUpButton()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
        cam.GetComponent<Animator>().enabled = false;

        if (thisBtn != null) thisBtn.interactable = false;
        camReturnTrans = cam.transform.localPosition;
        camZoomSize = cam.orthographicSize;
        StartCoroutine(CamZoom(zoomSize, camMoveTrans, true));
    }

    IEnumerator CamZoom(float zoom, Vector3 camTrans, bool toEnableActive)
    {
        WaitForSeconds wait = new WaitForSeconds(0.025f);
        int delta = 1;
        Vector3 startPos = cam.transform.localPosition;
        float startSize = cam.orthographicSize;

        // cam.transform.localPosition = startPos + (camTrans - startPos);
        // cam.orthographicSize = startSize + (zoom - startSize);
        yield return wait;



        if (toEnable != null) toEnable.SetActive(toEnableActive);

        //  cam.transform.localPosition = camTrans;
        //  cam.orthographicSize = zoom;

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
