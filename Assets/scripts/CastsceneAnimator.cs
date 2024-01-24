using System;
using System.Collections;
using UnityEngine;

// Представляє собою логіку взаємодії з анімаціями
public class CastsceneAnimator : MonoBehaviour
{
    private Animator animator; // Аніматор в якому находяться рамки для кастсцен

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Проигрывание каст сцены
    /// </summary>
    /// <exception cref="Exception"></exception>
    public IEnumerator PlayCastscene(Animator animCast, string titleAnimation)
    {
        if (animCast != null)
        {
            // Показ рамок для кастсцены
            animator.Play("StartCastscene");

            // Проигрывание кастсцены
            animCast.Play(titleAnimation);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Скрытие рамок для кастсцены
            animator.Play("StopCastscene");
        }
        else
        {
            throw new Exception("Castscene animator is null");
        }
    }

    // Метод для прерывания кастсцены
    public void StopCastscene()
    {
        animator.Play("StopCastscene");
    }
}
