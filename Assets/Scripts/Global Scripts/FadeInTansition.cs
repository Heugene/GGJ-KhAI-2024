using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInTansition : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void LoadScene(int sceneId) => SceneManager.LoadScene(sceneId);
    // ����� ������� ������� ��������
    public void PlayBegin(int sceneId) => StartCoroutine( BeginWithDelay() );
    // ����� �������� ��������������� �����
    public void PlayEnd(int sceneId) => StartCoroutine( EndWithDelay() );


    private IEnumerator BeginWithDelay()
    {
        animator.Play("FadeIn");
        yield return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    private IEnumerator EndWithDelay()
    {
        animator.Play("FadeInEnd");
        yield return animator.GetCurrentAnimatorStateInfo(0).length;
    }


}
