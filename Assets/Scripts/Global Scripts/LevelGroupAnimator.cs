using System.Linq;
using UnityEngine;

public class LevelGroupAnimator : MonoBehaviour 
{ 
    private CastsceneAnimator castsceneAnimator; // убедитесь, что эта ссылка присвоена в инспекторе Unity
    private Transform[] objects;


    private void Start()
    {
        castsceneAnimator = FindObjectOfType<CastsceneAnimator>();

        if (castsceneAnimator != null)
        {
            StartCoroutine(castsceneAnimator.Play(transform, 1.5F));
            CastsceneAnimator.onCameraFocused += HandleUnityEvent;
            objects = GetComponentsInChildren<Transform>(true).Where(t => t != transform).ToArray();

        }
        else
        {
            Debug.LogError("Animator is not assigned!");
        }
    }

    private void HandleUnityEvent()
    {
        foreach (Transform btn in objects)
        {
            btn.gameObject.SetActive(true);
        }
    }
}

