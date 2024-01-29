using System.Linq;
using System.Text;
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
            objects = GetComponentsInChildren<Transform>(true).Where(t => t != transform).ToArray();
            CastsceneAnimator.onCameraFocused += HandleUnityEvent;
            StartCoroutine(castsceneAnimator.Play(transform, 1.5F));
            //buttons = GetComponentsInChildren<ButtonPressLogic>(true).Select(component => component.gameObject).ToArray();
        }
        else
        {
            Debug.LogError("Animator is not assigned!");
        }
    }

    private void HandleUnityEvent()
    {
        foreach (Transform obj in objects)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(true);
            }
        }
    }
}

