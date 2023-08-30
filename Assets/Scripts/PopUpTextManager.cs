using UnityEngine;
using UnityEngine.UI;

public class PopUpTextManager : MonoBehaviour
{
    [SerializeField] private Text PopUpText;
    private Animator PopUpAnimator;

    private void Start()
    {
        PopUpAnimator = PopUpText.gameObject.GetComponent<Animator>();    
    }

    public void PlayText(string text)
    {
        PopUpText.gameObject.SetActive(true);
        PopUpText.text = text;
        PopUpAnimator.Play("PopUpTextIntro");
    }
}
