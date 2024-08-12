using System.Collections.Generic;
using UnityEngine;
public enum SpeechMode
{
    TALK, QUESTION
};
public class SpeechSchedule : MonoBehaviour
{
    [System.Serializable]
    public class SpeechItem
    {
        public string speech;//1, 8, 9

        public SpeechMode mode;
        public int startIndex = 3;
        public SpeechItem(string speech, SpeechMode mode, int startIndex)
        {
            this.speech = speech;
            this.mode = mode;
            this.startIndex = startIndex;
        }

    }
    public List<SpeechItem> speechList = new List<SpeechItem>();

}
