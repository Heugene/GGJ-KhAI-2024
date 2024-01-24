using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

// ����������� ����� ����� �����䳿 � ����������
public class AnimationManager : MonoBehaviour
{
    private Animator animator; // �������� � ����� ���������� ����� ��� ��������

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// ������������ ���� �����
    /// </summary>
    /// <exception cref="Exception"></exception>
    public IEnumerator PlayCastscene(Animator animCast, string titleAnimation)
    {
        if (animCast != null)
        {
            // ����� ����� ��� ���������
            animator.Play("StartCastscene");

            // ������������ ���������
            animCast.Play(titleAnimation);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // ������� ����� ��� ���������
            animator.Play("StopCastscene");
        }
        else
        {
            throw new Exception("Castscene animator is null");
        }
    }

    // ����� ��� ���������� ���������
    public void StopCastscene()
    {
        animator.Play("StopCastscene");
    }
}
