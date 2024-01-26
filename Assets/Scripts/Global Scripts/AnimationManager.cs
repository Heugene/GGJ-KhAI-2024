using UnityEngine;

//TODO: ���������� �� LevelGroupAnimator (������ �������� �� ButtonGroup)
public class AnimationManager : MonoBehaviour 
{ 
    private CastsceneAnimator animator; // ���������, ��� ��� ������ ��������� � ���������� Unity
    [SerializeField] Animator animCast;

    private void Start()
    {
        animator = FindObjectOfType<CastsceneAnimator>();
        //PlayCastScene("SpinBox", Point); // ������������
    }

    public void PlayCastScene(string name, Transform cameraPoint)
    {
        if (animator != null) // �������� �������� �� null ����� ������� ������
        {
            StartCoroutine(animator.Play(animCast, name, cameraPoint));
        }
        else
        {
            Debug.LogError("Animator is not assigned!");
        }
    }
}

