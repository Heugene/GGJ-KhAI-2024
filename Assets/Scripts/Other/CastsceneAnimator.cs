using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


// ����������� ����� ����� ���������� ������� ��� �������
public class CastsceneAnimator : MonoBehaviour
{
    public delegate void MyEventHandler();
    public static MyEventHandler onCameraFocused;

    [SerializeField] Animator castsceneAnim; // ������� � ����� ���������� ����� ��� ��������
    private cameraMovement camera;
    private GameObject Player;
    private GameObject Clown;


    private void Awake()
    {
        Player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        Clown = GameObject.FindGameObjectsWithTag("Enemy").FirstOrDefault();
        camera = FindObjectOfType<cameraMovement>();
    }

    /// <summary>
    /// ������������ ���� �����
    /// </summary>
    /// <exception cref="Exception"></exception>
    public IEnumerator Play(Transform cameraPoint, float waitingTime)
    {
        GameFreeze(true);

        // ����� ����� ��� ���������
        castsceneAnim.Play("StartCastscene");
        camera.ChangeTarget(cameraPoint.position);
        yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

        // ������������ ���������
        onCameraFocused?.Invoke();
        yield return new WaitForSeconds(waitingTime);

        // ������� ����� ��� ���������
        camera.FollowPlayer();
        castsceneAnim.Play("StopCastscene");
        yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

        GameFreeze(false);
    }

    // ������� ������ � ������ 
    private void GameFreeze(bool freezed) // TODO: ���������� �� ������
    {
        if ( Player == null)
        {
            Player.GetComponent<PlayerJump>().enabled = !freezed;
        }
        else throw new Exception ("CastsceneAnimator\\GameFreeze - Clown not found!");

        if( Clown == null)
        {
            Clown.GetComponent<EnemyClown>().enabled = !freezed;
            Clown.GetComponent<NavMeshAgent>().enabled = !freezed;
        } 
        else throw new Exception("CastsceneAnimator\\GameFreeze - Player not found!");
    }

    // ����� ��� ���������� ���������
    public void StopCastscene(Animator animCast, string titleAnimation)
    {
        animCast.StopPlayback();
        castsceneAnim.Play("StopCastscene");
    }
}
