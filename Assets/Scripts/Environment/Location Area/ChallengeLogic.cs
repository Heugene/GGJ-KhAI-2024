using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChallengeLogic : MonoBehaviour
{
    internal bool Solved { get; private set; }
    private GameObject player; // Ďëĺşđ
    
    // Îá'şęňč ńęđčďňłâ äë˙ çîí ç ęíîďęŕěč, ůîá ěîćíŕ áóëî ç íčő âčň˙ăóâŕňč ńňŕí ďđîőîäćĺíí˙ çîíč
    [SerializeField] private ButtonGroupLogic Area1; 
    [SerializeField] private ButtonGroupLogic Area2;
    [SerializeField] private ButtonGroupLogic Area3;
    [SerializeField] private ButtonGroupLogic Area4;
    [SerializeField] private Transform Lever;
    [SerializeField] private RandomSauceSpawner sauceSpawner;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().End += EndGame;
        player = GameObject.FindGameObjectWithTag("Player");

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
        // ßĘ ďîäîëŕňč ęëîóíŕ.ďíă Äćîäćîđĺôĺđĺíń.äćčďĺă, ńňŕđňóşěî ěŕëţâŕíí˙ ďĺíňŕăđŕěč ËĹŇŃÔŔĘ˛ÍĂÎÎÎÎÎÎÎÎ
        
        Lever.gameObject.SetActive(true);

        GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().Activation();

        // Ńňŕâčěî âĺëčęčé ÷ŕń łńíóâŕíí˙ ňđĺéëó, ůîá âńňčăíóňč íŕěŕëţâŕňč ďĺíňŕăđŕěó.
        player.GetComponentInChildren<TrailRenderer>().time = 45;

    }
    private void EndGame()
    {
        //Äłż ďłńë˙ çŕđŕőóâŕíí˙ ďĺíňŕăđŕěč

        // Ďîâĺđňŕşěî íŕçŕä ÷ŕń łńíóâŕíí˙ ňđĺéëó
        player.GetComponentInChildren<TrailRenderer>().time = 5;
    }
}
