using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public AudioSource music1, music2, victoryMusic, gameOverMusic;
    public Text countDownText;

    public int countDownValue = 3;
    private bool isGameActive = false;
    private bool isGameFinished = false;
    private AudioSource currentMusic;

    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        currentMusic = music1;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isGameActive && !isGameFinished && !countDownText.enabled)
            StartCoroutine(CountDown());
        else if (!isGameActive && isGameFinished)
            ShowResults();
        else if (isGameActive && !isGameFinished)
            ProcessTime();
        
    }

    private IEnumerator CountDown()
    {

        countDownText.enabled = true;

        while (countDownValue > 0)
        {
            countDownText.text = countDownValue.ToString();
            yield return new WaitForSeconds(1f);
            countDownValue--;

        }
           
        countDownText.text = "YA!";
        isGameActive = true;
        currentMusic.Play();
        yield return new WaitForSeconds(1f);
        countDownText.enabled = false;

    }

    private void ShowResults()
    {

        countDownText.enabled = true;
        countDownText.text = "FIN";

    }

    private void ProcessTime()
    {


    }

    public bool IsGameRunning()
    {

        return isGameActive;

    }

    public void EndGame()
    {

        isGameFinished = false;
        isGameActive = false;
        countDownText.enabled = true;
        currentMusic.Stop();
        //gameOverMusic.Play();
        countDownText.text = "Te chocaste! Vuelve a empezar!";


    }

    public void ArriveDestination()
    {

        isGameFinished = true;
        isGameActive = false;
        currentMusic.Stop();
        //victoryMusic.Play();

    }

    public AudioSource getCurrentMusic()
    {

        return currentMusic;

    }

}
