using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


// ����������� ����� ����� �����䳿 � ���������
public class CastsceneAnimator : MonoBehaviour
{
    [SerializeField] Animator castsceneAnim; // ������� � ����� ���������� ����� ��� ��������
    private cameraMovement camera;
    private GameObject Player;
    private GameObject Clown;

    private void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player").First();
        Clown = GameObject.FindGameObjectsWithTag("Enemy").First();
        camera = FindObjectOfType<cameraMovement>();
    }

    /// <summary>
    /// ������������ ���� �����
    /// </summary>
    /// <exception cref="Exception"></exception>
    public IEnumerator Play(Animator animCast, string titleAnimation, Transform cameraPoint)
    {
        if (animCast != null)
        {
            GameFreeze(true);

            // ����� ����� ��� ���������
            castsceneAnim.Play("StartCastscene");
            camera.ChangeTarget(cameraPoint.position);
            yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

            // ������������ ���������
            animCast.Play(titleAnimation);
            yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

            // ������� ����� ��� ���������
            camera.FollowPlayer();
            castsceneAnim.Play("StopCastscene");
            yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

            GameFreeze(false);
        }
        else
        {
            throw new Exception("Castscene animator is null");
        }
    }

    private void GameFreeze(bool freezed) // TODO: ���������� �� ������
    {
        Player.GetComponent<Movement>().enabled = !freezed;
        Clown.GetComponent<NavMeshAgent>().enabled = !freezed;
        Clown.GetComponent<BoxCollider2D>().enabled = !freezed;
    }

    // ����� ��� ���������� ���������
    public void StopCastscene(Animator animCast, string titleAnimation)
    {
        animCast.StopPlayback();
        castsceneAnim.Play("StopCastscene");
    }
}
