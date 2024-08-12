using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    public Camera cam;
    public Camera renderCam;
    public GameObject panel;

    public TMP_Text temp;

    float maxZoom = 3.0f;
    float minZoom = 5.0f;
    float delta = 1f;
    float maxRot = 10f;

    string fileName="my_r_";
    public RenderTexture renderTx;
    public RawImage rawImg;
    string path;

    bool isRotationMode = false;

    //C:\Users\USER\AppData\LocalLow\DefaultCompany\My_R

    void GetImage() {
        //Texture2D texture2D = new Texture2D(renderTx.width, renderTx.height, TextureFormat.ARGB32, false);
        Texture2D texture2D = new Texture2D(renderTx.width, renderTx.height, TextureFormat.ARGB4444, false);
        RenderTexture.active = renderTx;
        texture2D.ReadPixels(new Rect(0, 0, renderTx.width, renderTx.height),0,0);
        texture2D.Apply();

        path = Application.persistentDataPath + "/" + fileName + DateTime.Now.ToString("yyyyMMddhhmmss")+ ".png";
        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }

    void SetImage() {
        Texture2D texture2D= new Texture2D(renderTx.width, renderTx.height);

        //path = Application.persistentDataPath + "/" + fileName + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
        byte[] bytes = texture2D.EncodeToPNG();

        texture2D.LoadImage(bytes);
            texture2D.Apply();
        rawImg.texture = texture2D;
    }

    IEnumerator RenderProccess() {
        renderCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        GetImage();
        yield return new WaitForSeconds(0.1f);
        rawImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        SetImage();
        yield return new WaitForSeconds(0.3f);
        renderCam.gameObject.SetActive(false);
    }

    void GetSetImage_btn() {
        StartCoroutine(RenderProccess());
    }

    private void Start()
    {
        rawImg.gameObject.SetActive(false);
    }

    private void Update()
    {
        float x = Input.mousePosition.x - 1920 / 2;
        float y = Input.mousePosition.y - 1080 / 2;

        //if (Math.Abs(camera.transform.localPosition.x) <= 5.3f && Math.Abs(camera.transform.localPosition.y) <= 3.0f)

        float camX = cam.transform.localPosition.x==0?0:cam.transform.localPosition.x / Math.Abs(cam.transform.localPosition.x);
        float camY = cam.transform.localPosition.y == 0 ? 0 : cam.transform.localPosition.y / Math.Abs(cam.transform.localPosition.y);
        cam.transform.localPosition = new Vector3(x * 0.01f, y * 0.01f, cam.transform.localPosition.z);

        if (camX != 0.0&& Math.Abs(cam.transform.localPosition.x) > 3.5f)
            cam.transform.localPosition = new Vector3(3.5f * camX, cam.transform.localPosition.y, cam.transform.localPosition.z);
        if (camY != 0.0&&Math.Abs(cam.transform.localPosition.y) > 2.0f)
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, 2.0f * camY, cam.transform.localPosition.z);


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        temp.text = x + ", " + y + "     " + scroll;

        cam.orthographicSize += scroll * delta;

        if (Input.GetMouseButtonDown(1))
        {
            isRotationMode = !isRotationMode;
            Debug.Log(isRotationMode);
        }

        if (isRotationMode)
        {
            
            cam.transform.eulerAngles += new Vector3(0, 0, scroll * 10);
            //Debug.Log(cam.transform.localEulerAngles.z + "/" + maxRot);
            if (cam.transform.localEulerAngles.z > maxRot) cam.transform.localEulerAngles = new Vector3(0, 0, scroll * maxRot);
            if(cam.transform.localEulerAngles.z < -maxRot) cam.transform.localEulerAngles = new Vector3(0, 0, scroll * -maxRot);
        }
        else {//Æò¼Ò
            if (cam.orthographicSize > minZoom) cam.orthographicSize = minZoom;
            if (cam.orthographicSize < maxZoom) cam.orthographicSize = maxZoom;

        }


        if (Input.GetMouseButtonDown(0))
        {
            //screenshot
            panel.SetActive(false);
            //ÂûÄ¬
            //¼Ò¸® Àç»ý
            //ÃÔ¿µ ½ºÅ©¸³Æ®
            renderCam.orthographicSize = cam.orthographicSize;
            GetSetImage_btn();
            Debug.Log("ÂûÄ¬!");

            panel.SetActive(true);
        }

    }

    private void OnEnable()
    {
        cam.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        cam.gameObject.SetActive(false);
    }

}
