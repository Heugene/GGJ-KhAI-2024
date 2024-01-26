using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


// Представляє собою логіку взаємодії з анімаціями
public class CastsceneAnimator : MonoBehaviour
{
    [SerializeField] Animator castsceneAnim; // Аніматор в якому находяться рамки для кастсцен
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
    /// Проигрывание каст сцены
    /// </summary>
    /// <exception cref="Exception"></exception>
    public IEnumerator Play(Animator animCast, string titleAnimation, Transform cameraPoint)
    {
        if (animCast != null)
        {
            GameFreeze(true);

            // Показ рамок для кастсцены
            castsceneAnim.Play("StartCastscene");
            camera.ChangeTarget(cameraPoint.position);
            yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

            // Проигрывание кастсцены
            animCast.Play(titleAnimation);
            yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

            // Скрытие рамок для кастсцены
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

    private void GameFreeze(bool freezed) // TODO: Переделать на ивенты
    {
        Player.GetComponent<Movement>().enabled = !freezed;
        Clown.GetComponent<NavMeshAgent>().enabled = !freezed;
        Clown.GetComponent<BoxCollider2D>().enabled = !freezed;
    }

    // Метод для прерывания кастсцены
    public void StopCastscene(Animator animCast, string titleAnimation)
    {
        animCast.StopPlayback();
        castsceneAnim.Play("StopCastscene");
    }
}
