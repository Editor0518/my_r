using UnityEngine;

public class ChoiceWhole : MonoBehaviour
{
   public ChoiceManager choiceManager;

   public void ChoiceCloseIsDone()
   {
      choiceManager.SetIsChoiceClosed(true);
      this.gameObject.SetActive(false);
   }
   
}
