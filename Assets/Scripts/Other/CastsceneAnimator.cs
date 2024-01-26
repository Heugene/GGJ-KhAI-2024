using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


// ����������� ����� ����� �����䳿 � ���������
public class CastsceneAnimator : MonoBehaviour
{
    [SerializeField] Animator castsceneAnim; // ������� � ����� ���������� ����� ��� ��������
    private cameraMovement camera;
    private GameObject Player;
    private GameObject Clown;
    public delegate void MyEventHandler();
    public static MyEventHandler onCameraFocused;

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

    private void GameFreeze(bool freezed) // TODO: ���������� �� ������
    {
        if (Player == null || Clown == null)
        {
            Debug.LogError("Player or Clown not found!");
            return;
        }

        Player.GetComponent<PlayerJump>().enabled = !freezed;
        Clown.GetComponent<EnemyClown>().enabled = !freezed;
    }

    // ����� ��� ���������� ���������
    public void StopCastscene(Animator animCast, string titleAnimation)
    {
        animCast.StopPlayback();
        castsceneAnim.Play("StopCastscene");
    }
}
