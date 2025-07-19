using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameRunaway : MonoBehaviour
    {
        public MinigameRunChoiceData choiceData;
        public Button leftButton;
        public Button middleButton;
        public Button rightButton;

        private HashSet<int> visited = new HashSet<int>();
        private int currentNode;
        private int fromNode;

        public void UpdateChoices(int current, int from)
        {
            currentNode = current;
            fromNode = from;

            visited.Add(from);

            var choices = choiceData.GetChoices(current, from);
            if (choices == null || choices.Count == 0)
            {
                // 막다른 길 - 유턴 처리
                leftButton.gameObject.SetActive(false);
                middleButton.gameObject.SetActive(false);
                rightButton.gameObject.SetActive(false);
                Debug.Log("Dead end. Only U-turn allowed.");
                return;
            }

            SetButton(leftButton, choices[0]);
            SetButton(middleButton, choices[1]);
            SetButton(rightButton, choices[2]);
        }

        void SetButton(Button button, int nodeId)
        {
            if (visited.Contains(nodeId) || nodeId == -1)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => MoveTo(nodeId));
            }
        }

        void MoveTo(int newNode)
        {
            Debug.Log($"Moved from {currentNode} to {newNode}");
            UpdateChoices(newNode, currentNode);
        }
    }
    