using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    public struct Word {
        public string word;
        public string cmd;
    }

    [Header("Sentence")]
    public int name_ch;
    public string sentence;//전체 타이핑해야하는 문장
    public string current;//현재 타이핑 중인 문장
    public List<Word> currentWord = new List<Word>();

    [Header("Type Setting")]
    public TMP_Text content;


    void UpdateWordList() {
        for (int i = 0; i < sentence.Length; i++) {

            /*if (!sentence[i].Equals("<"))
            {
                Word w = new Word();
                w.word = sentence[i];
                w.cmd = "";
                currentWord.Add(w);
            }
            else {
                string tmp_cmd = sentence.Substring(i, sentence[i+1].Equals("/")?i+6:i + 5);
                if (tmp_cmd.Equals("<slow>")) {
                    
                }
            }*/
        }
    }

    public void StartTyping(string sentence, int name_ch) {
        StopCoroutine("Typing");

        this.sentence = sentence;
        this.name_ch = name_ch;

        StartCoroutine("Typing");
       
    }
    IEnumerator Typing() {
        WaitForSeconds wait = new(0.07f);
        current = "";
        for (int i = 0; i< sentence.Length; i++) {
            current += sentence[i];
            content.text = current;
            if (!sentence[i].Equals(" ") && !sentence[i].Equals("♡") && !sentence[i].Equals("?") && !sentence[i].Equals("!") && !sentence[i].Equals(".") && !sentence[i].Equals(",")) {
                SoundManager.instance.PlayVoice(name_ch);
            }
            //

            yield return wait;
        }
        yield return null;
    }
}
