using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


// Представляє собою логіку взаємодії з анімаціями
public class CastsceneAnimator : MonoBehaviour
{
    [SerializeField] Animator castsceneAnim; // Аніматор в якому находяться рамки для кастсцен
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
    /// Проигрывание каст сцены
    /// </summary>
    /// <exception cref="Exception"></exception>
    public IEnumerator Play(Transform cameraPoint, float waitingTime)
    {
        GameFreeze(true);

        // Показ рамок для кастсцены
        castsceneAnim.Play("StartCastscene");
        camera.ChangeTarget(cameraPoint.position);
        yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

        // Проигрывание кастсцены
        onCameraFocused?.Invoke();
        yield return new WaitForSeconds(waitingTime);

        // Скрытие рамок для кастсцены
        camera.FollowPlayer();
        castsceneAnim.Play("StopCastscene");
        yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

        GameFreeze(false);
    }

    private void GameFreeze(bool freezed) // TODO: Переделать на ивенты
    {
        if (Player == null || Clown == null)
        {
            Debug.LogError("Player or Clown not found!");
            return;
        }

        Player.GetComponent<PlayerJump>().enabled = !freezed;
        Clown.GetComponent<EnemyClown>().enabled = !freezed;
    }

    // Метод для прерывания кастсцены
    public void StopCastscene(Animator animCast, string titleAnimation)
    {
        animCast.StopPlayback();
        castsceneAnim.Play("StopCastscene");
    }
}
