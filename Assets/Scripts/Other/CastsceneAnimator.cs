using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ����������� ����� ����� ���������� ������� ��� �������
/// </summary>
public class CastsceneAnimator : MonoBehaviour
{
    public delegate void MyEventHandler();         // ������� �� ��䳿
    public static MyEventHandler onCameraFocused;  // ���� ��� ����������� ���������

    [SerializeField] Animator castsceneAnim; // ������� � ����� ���������� ����� ��� ��������
    private cameraMovement cameraScript;  // ��������� �� c���� �� ������     
    private GameObject Player;            // ��������� �� ������
    private GameObject Clown;             // ��������� �� ������


    private void Awake()
    {
        Player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        Clown = GameObject.FindGameObjectsWithTag("Enemy").FirstOrDefault();
        cameraScript = FindObjectOfType<cameraMovement>();
    }

    // ��������� ������ �� ��������� � �� �����������
    public IEnumerator Play(Transform cameraPoint, float waitingTime)
    {
        GameFreeze(true);

        // ����� ����� ��� ���������
        castsceneAnim.Play("StartCastscene");
        cameraScript.SetTarget(cameraPoint.position);
        yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

        // ������������ ���������
        onCameraFocused?.Invoke();
        yield return new WaitForSeconds(waitingTime);

        // ������� ����� ��� ���������
        cameraScript.FollowPlayer();
        castsceneAnim.Play("StopCastscene");
        yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

        GameFreeze(false);
    }

    // ������� ������ � ������ 
    private void GameFreeze(bool freezed) // TODO: ���������� �� ������
    {
        if ( Player != null)
        {
            Player.GetComponent<PlayerJump>().enabled = !freezed;
        }
        else throw new Exception ("[CastsceneAnimator\\GameFreeze] - Clown not found!");

        if( Clown != null)
        {
            Clown.GetComponent<EnemyClown>().enabled = !freezed;
            Clown.GetComponent<NavMeshAgent>().enabled = !freezed;
        } 
        else throw new Exception("[CastsceneAnimator\\GameFreeze] - Player not found!");
    }

    // ����������� ��������
    public void StopCastscene(Animator animCast, string titleAnimation)
    {
        animCast.StopPlayback();
        castsceneAnim.Play("StopCastscene");
    }
}
