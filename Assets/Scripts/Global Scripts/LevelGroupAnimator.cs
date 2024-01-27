using System.Linq;
using UnityEngine;

public class LevelGroupAnimator : MonoBehaviour 
{ 
    private CastsceneAnimator castsceneAnimator; // убедитесь, что эта ссылка присвоена в инспекторе Unity
    private GameObject[] buttons;


    private void Start()
    {
        castsceneAnimator = FindObjectOfType<CastsceneAnimator>();

        if (castsceneAnimator != null)
        {
            StartCoroutine(castsceneAnimator.Play(transform, 1.5F));
            CastsceneAnimator.onCameraFocused += HandleUnityEvent;
            buttons = GetComponentsInChildren<ButtonPressLogic>(true).Select(component => component.gameObject).ToArray();
        }
        else
        {
            Debug.LogError("Animator is not assigned!");
        }
    }

    private void HandleUnityEvent()
    {
        foreach (GameObject btn in buttons)
        {
            btn.SetActive(true);
        }
    }
}

