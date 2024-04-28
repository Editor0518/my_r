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
    public bool isOn = true;
    public int name_ch;
    public string sentence;//��ü Ÿ�����ؾ��ϴ� ����
    public string current;//���� Ÿ���� ���� ����
    public List<Word> currentWord = new List<Word>();

    [Header("Type Setting")]
    public TMP_Text content;
    public DialogueManager dialogueManager;

    public float autoDelay = 1.0f;
    public bool isTyping = false;


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

    public void StartTyping(string sentence, int name_ch, bool isOn) {

        StopCoroutine("Typing");

        this.sentence = sentence;
        this.name_ch = name_ch;
        this.isOn = isOn;

        StartCoroutine("Typing");

    }

    public void StopTyping(){
        if (sentence.Contains("<skip>")) return;
        StopCoroutine("Typing");

        current = this.sentence;
        /*if (current.Contains("<skip>")) {
            current = current.Replace("<skip>", "");

            content.text = current;
            isTyping = false;

            dialogueManager.ChangeDialogue();
        }*/
        
        content.text = current;
        isTyping = false;
    }

    IEnumerator Typing() {
        WaitForSeconds wait = new(0.07f);
        isTyping = true;
        Debug.Log(isTyping);
        current = "";
        for (int i = 0; i< sentence.Length; i++) {
            if (sentence[i].Equals('<'))
            {
                if (sentence[i + 1].Equals('s'))
                { //<skip>
                    for (; !sentence[i].Equals('>'); i++) ;
                    dialogueManager.ChangeDialogue();
                    break;//�� ��� for������ ���

                }
                else {
                    Debug.Log(current+"����" + i);
                    for (; !sentence[i].Equals('>'); i++) ;
                        //current += sentence[i]; //;
                    current = sentence[..i];
                    
                    content.text = current;
                    Debug.Log(current + "��ȣ �߰�" + i);
                }

                yield return wait;
            }
            current += sentence[i];
            content.text = current;
            
            if (isOn&&!sentence[i].Equals(" ") && !sentence[i].Equals("��") && !sentence[i].Equals("?") && !sentence[i].Equals("!") && !sentence[i].Equals(".") && !sentence[i].Equals(",")) {
                SoundManager.instance.PlayVoice(name_ch);
            }
            //

            yield return wait;
        }
        isTyping = false;
        yield return null;
    }
}
