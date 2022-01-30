using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public AudioSource music1, music2, victoryMusic, gameOverMusic;
    public RawImage gameOverImage;
    public Text countDownText;
    public GameObject player;
    public BoxCollider secondFinishLine;
    public MeshRenderer slimeCore, slimeRim;
    public Material fireCore, fireRim, waterCore, waterRim;
    public int countDownValue = 3;

    private string CurrentLevel = "Final";
    private bool isGameActive = false;
    private bool isGameFinished = false;
    private bool isPlayerDead = false;
    private bool reachedEnd = false;
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
            return;
        else if (isPlayerDead && Input.GetButtonDown("JumpButton"))
            SceneManager.LoadScene(CurrentLevel);


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

    public bool IsGameRunning()
    {

        return isGameActive;

    }

    public void EndGame()
    {

        isGameFinished = false;
        isGameActive = false;
        isPlayerDead = true;
        countDownText.enabled = true;
        gameOverImage.enabled = true;
        currentMusic.Stop();
        //gameOverMusic.Play();
        countDownText.text = "Te chocaste! Vuelve a empezar!\nPulsa barra espaciadora";
        countDownText.fontSize = 60;


    }

    public void ArriveDestination()
    {
        if(reachedEnd)
        {

            isGameFinished = true;
            isGameActive = false;
            currentMusic.Stop();

        } else
        {

            StartSecondPart();            

        }
        //victoryMusic.Play();

    }

    private void StartSecondPart()
    {
        isGameActive = false;
        countDownValue = 3;
        reachedEnd = true;
        player.transform.transform.position = new Vector3(-player.transform.transform.position.x, player.transform.transform.position.y, player.transform.transform.position.z);
        player.transform.transform.rotation = new Quaternion(player.transform.transform.rotation.x, 180, player.transform.transform.rotation.z, player.transform.transform.rotation.w);
        slimeRim.material = waterRim;
        slimeCore.material = waterCore;
        secondFinishLine.enabled = true;
        currentMusic = music2;
        currentMusic.Play();

    }

    public bool isFirstLoop()
    {

        return !reachedEnd;

    }

    public AudioSource getCurrentMusic()
    {

        return currentMusic;

    }

}
