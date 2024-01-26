using UnityEngine;

//TODO: ѕеределать на LevelGroupAnimator (должен вешатьс€ на ButtonGroup)
public class AnimationManager : MonoBehaviour 
{ 
    private CastsceneAnimator animator; // убедитесь, что эта ссылка присвоена в инспекторе Unity
    [SerializeField] Animator animCast;

    private void Start()
    {
        animator = FindObjectOfType<CastsceneAnimator>();
        //PlayCastScene("SpinBox", Point); // ѕроигрывание
    }

    public void PlayCastScene(string name, Transform cameraPoint)
    {
        if (animator != null) // добавьте проверку на null перед вызовом метода
        {
            StartCoroutine(animator.Play(animCast, name, cameraPoint));
        }
        else
        {
            Debug.LogError("Animator is not assigned!");
        }
    }
}

