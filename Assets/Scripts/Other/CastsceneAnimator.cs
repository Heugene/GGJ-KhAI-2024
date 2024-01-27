using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Представляє собою логіку відтворення анімацій для локацій
/// </summary>
public class CastsceneAnimator : MonoBehaviour
{
    public delegate void MyEventHandler();         // Делегат до події
    public static MyEventHandler onCameraFocused;  // Подія для програвання кастсцени

    [SerializeField] Animator castsceneAnim; // Аніматор в якому находяться рамки для кастсцен
    private cameraMovement cameraScript;  // Посилання на cкріпт до камери     
    private GameObject Player;            // Посилання на гравця
    private GameObject Clown;             // Посилання на клоуна


    private void Awake()
    {
        Player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
        Clown = GameObject.FindGameObjectsWithTag("Enemy").FirstOrDefault();
        cameraScript = FindObjectOfType<cameraMovement>();
    }

    // Переміщеня камери на кастсцену з її прогрванням
    public IEnumerator Play(Transform cameraPoint, float waitingTime)
    {
        GameFreeze(true);

        // Показ рамок для кастсцены
        castsceneAnim.Play("StartCastscene");
        cameraScript.SetTarget(cameraPoint.position);
        yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

        // Проигрывание кастсцены
        onCameraFocused?.Invoke();
        yield return new WaitForSeconds(waitingTime);

        // Скрытие рамок для кастсцены
        cameraScript.FollowPlayer();
        castsceneAnim.Play("StopCastscene");
        yield return new WaitForSeconds(castsceneAnim.GetCurrentAnimatorStateInfo(0).length);

        GameFreeze(false);
    }

    // Паузить гравця й клоуна 
    private void GameFreeze(bool freezed) // TODO: Переделать на ивенты
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

    // Переривання катсцени
    public void StopCastscene(Animator animCast, string titleAnimation)
    {
        animCast.StopPlayback();
        castsceneAnim.Play("StopCastscene");
    }
}
