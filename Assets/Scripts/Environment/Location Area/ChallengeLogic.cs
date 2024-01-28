using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChallengeLogic : MonoBehaviour
{
    internal bool Solved { get; private set; }
    private GameObject player; // Ďëĺşđ
    private EnemyClown clown;
    private NavMeshAgent mesh;

    // Îá'şęňč ńęđčďňłâ äë˙ çîí ç ęíîďęŕěč, ůîá ěîćíŕ áóëî ç íčő âčň˙ăóâŕňč ńňŕí ďđîőîäćĺíí˙ çîíč
    [SerializeField] private ButtonGroupLogic Area1; 
    [SerializeField] private ButtonGroupLogic Area2;
    [SerializeField] private ButtonGroupLogic Area3;
    [SerializeField] private ButtonGroupLogic Area4;
    [SerializeField] private GameObject hint;
    [SerializeField] private GameObject lightningStrick;
    [SerializeField] private GameObject cutsceneOfDeath;
    [SerializeField] private Transform Lever;
    [SerializeField] private RandomSauceSpawner sauceSpawner;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().End += EndGame;
        player = GameObject.FindGameObjectWithTag("Player");
        clown = GameObject.FindWithTag("Enemy").GetComponent<EnemyClown>();
        mesh = GameObject.FindWithTag("Enemy").GetComponent<NavMeshAgent>();

        Area1.LevelCompleted += Area1_CompleteActions;
        Area2.LevelCompleted += Area2_CompleteActions;
        Area3.LevelCompleted += Area3_CompleteActions;
        Area4.LevelCompleted += Area4_CompleteActions;

        Area2.gameObject.SetActive(false);
        Area3.gameObject.SetActive(false);
        Area4.gameObject.SetActive(false);
    }


    // Äîäŕňč ďĺđĺěčęŕíí˙ ęŕěĺđč, ŕíłěŕřęč, đîçáëîęóâŕíí˙ ńîóńłâ, âčäŕňč ńîóńč ăđŕâöţ


    // Äłż, ęîëč ďđîéřëč ďĺđřó çîíó ç ęíîďęŕěč
    private void Area1_CompleteActions()
    {
        Debug.Log("Area1 COMPLETED");
        sauceSpawner.SpawnMayonnaise();

        // Đîçáëîęóşěî çîíó 2
        Area2.gameObject.SetActive(true);

        //Ňĺńň
        //GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().Activation();

    }

    // Äłż, ęîëč ďđîéřëč äđóăó çîíó ç ęíîďęŕěč
    private void Area2_CompleteActions()
    {
        Debug.Log("Area2 COMPLETED");
        sauceSpawner.SpawnKetchup();
        sauceSpawner.SpawnMayonnaise();
        // Đîçáëîęóşěî çîíó 3
        Area3.gameObject.SetActive(true);
    }

    // Äłż, ęîëč ďđîéřëč ňđĺňţ çîíó ç ęíîďęŕěč
    private void Area3_CompleteActions()
    {
        Debug.Log("Area3 COMPLETED");
        sauceSpawner.SpawnKetchup();
        sauceSpawner.SpawnMayonnaise();
        sauceSpawner.SpawnCheese();


        // Đîçáëîęóşěî çîíó 4
        Area4.gameObject.SetActive(true);
    }

    // Äłż, ęîëč ďđîéřëč ÷ĺňâĺđňó çîíó ç ęíîďęŕěč
    private void Area4_CompleteActions()
    {
        Debug.Log("Area4 COMPLETED");
        sauceSpawner.SpawnKetchup();
        sauceSpawner.SpawnKetchup();
        
        Lever.gameObject.SetActive(true);

        GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().Activation();
        StartCoroutine(wait(3f));

        player.GetComponentInChildren<TrailRenderer>().time = 45;

    }
    public void EndGame()
    {
        clown.isDead = true;
        player.GetComponentInChildren<TrailRenderer>().time = 5;
        cutsceneOfDeath.SetActive(true);
        lightningStrick.SetActive(true);
        clown.enabled = false;
        mesh.enabled = false;
    }

    IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hint.SetActive(true);
    }
}
