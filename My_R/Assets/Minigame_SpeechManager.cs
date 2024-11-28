using System.Collections;
using TMPro;
using UnityEngine;

public class Minigame_SpeechManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        attack.gameObject.SetActive(false);
        StartCoroutine(Attack());
        StartCoroutine(Item());
    }

    public Transform attack;
    public GameObject attackPrev;
    public Transform item;

    public static int life = 3;
    public TMP_Text lifeText;

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.2f, 2.2f));
            float x = Random.Range(-2f, 2f);
            float y = Random.Range(-2f, 2f);

            attackPrev.transform.position = new Vector3(x > 0 ? 2f : -2f, y > 0 ? 2f : -2f, 0);
            attackPrev.SetActive(true);
            attack.position = attackPrev.transform.position;
            yield return new WaitForSeconds(0.5f);
            attack.gameObject.SetActive(true);
            attackPrev.SetActive(false);
            yield return new WaitForSeconds(1f);
            attack.gameObject.SetActive(false);
        }
    }

    IEnumerator Item()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 20f));
            float x = Random.Range(-2f, 2f);
            float y = Random.Range(-2f, 2f);

            item.transform.position = new Vector3(x > 0 ? 2f : -2f, y > 0 ? 2f : -2f, 0);
            item.gameObject.SetActive(true);
            yield return new WaitForSeconds(15f);
            item.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        lifeText.text = "Life: " + life;

        if (life <= 0)
        {
            Debug.Log("게임오버");
            lifeText.text = "Life: 0   GAME OVER!!!!";
            StopAllCoroutines();

        }

    }

}
