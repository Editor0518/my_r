using UnityEngine;

public class Minigame_SpeechPlayer : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = transform;
    }

    void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));


        player.transform.position = new Vector3(point.x > 0 ? 2 : -2, point.y > 0 ? 2 : -2, 0);

    }
}
