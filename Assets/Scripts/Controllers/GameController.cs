using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public AudioSource music1, music2, victoryMusic, gameOverMusic, fireSource;
    public RawImage gameOverImage;
    public Text countDownText;
    public GameObject instruction;
    public GameObject player;
    public GameObject fire;
    public GameObject potion;
    public GameObject endScreen;
    public BoxCollider secondFinishLine;
    public MeshRenderer slimeCore, slimeRim;
    public Material fireCore, fireRim, waterCore, waterRim;
    public int countDownValue = 3;

    private List<GameObject> respawnableObstacles;
    private AudioSource currentMusic;
    private Vector3 checkpoint;
    private bool isGameActive = false;
    private bool isGameFinished = false;
    private bool isPlayerDead = false;
    private bool areInstructionsActive = true;
    private bool reachedEnd = false;

    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        currentMusic = music1;
        checkpoint = player.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isGameActive && !isGameFinished && !countDownText.enabled && !areInstructionsActive)
            StartCoroutine(CountDown());
        else if (!isGameActive && isGameFinished)
        {

            ShowResults();
            if (Input.GetButtonDown("JumpButton"))
                SceneManager.LoadSceneAsync("MainMenu");

        }
        else if (isGameActive && !isGameFinished)
            return;
        else if (isPlayerDead && Input.GetButtonDown("JumpButton"))
            ManageRespawn();
        else if (areInstructionsActive && Input.GetButtonDown("JumpButton"))
            DeactivateInstructions();


    }

    private IEnumerator CountDown()
    {

        countDownText.enabled = true;
        countDownText.fontSize = 300;

        while (countDownValue > 0)
        {
            countDownText.text = countDownValue.ToString();
            yield return new WaitForSeconds(1f);
            countDownValue--;

        }
           
        countDownText.text = "YA!";
        isGameActive = true;
        currentMusic.Play();
        yield return new WaitForSeconds(0.2f);
        countDownText.enabled = false;

    }

    private void ShowResults()
    {

        countDownText.enabled = true;
        countDownText.text = "Press jump to return to the Main Menu";
        countDownText.rectTransform.position = new Vector3(countDownText.transform.position.x, -360f, countDownText.transform.position.z);
        countDownText.fontSize = 80;
        countDownText.color = Color.white;
        endScreen.SetActive(true);

}

    public bool IsGameRunning()
    {

        return isGameActive;

    }

    public void EndGame()
    {

        countDownText.enabled = true;
        isGameFinished = false;
        gameOverImage.enabled = true;
        isGameActive = false;
        isPlayerDead = true;
        currentMusic.Stop();
        gameOverMusic.Play();
        countDownText.text = "¡Game Over!\nPress spacebar to restart";
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
            currentMusic.Stop();
            victoryMusic.Play();
            StartSecondPart();            

        }
        victoryMusic.Play();

    }

    private void StartSecondPart()
    {
        countDownValue = 3;
        isGameActive = false;
        isPlayerDead = false;
        reachedEnd = true;
        fire.SetActive(true);
        potion.SetActive(false);
        fireSource.Play();
        //RespawnObstacles();
        checkpoint = new Vector3(-player.transform.transform.position.x, player.transform.transform.position.y, player.transform.transform.position.z);
        player.transform.transform.position = checkpoint;
        player.transform.transform.rotation = new Quaternion(player.transform.transform.rotation.x, 180, player.transform.transform.rotation.z, player.transform.transform.rotation.w);
        slimeRim.material = waterRim;
        slimeCore.material = waterCore;
        secondFinishLine.enabled = true;

    }

    public bool isFirstLoop()
    {

        return !reachedEnd;

    }

    public AudioSource getCurrentMusic()
    {

        return currentMusic;

    }

    public void ManageRespawn()
    {

        countDownText.enabled = false;
        countDownValue = 3;
        gameOverImage.enabled = false;
        isGameActive = false;
        isPlayerDead = false;
        player.transform.position = checkpoint;
        //RespawnObstacles();

    }

    public void AddRespawnableObstacle(GameObject obstable)
    {

        obstable.SetActive(false);
        respawnableObstacles.Add(obstable);

    }

    private void RespawnObstacles()
    {

        foreach(GameObject respawnable in respawnableObstacles)
        {

            respawnable.SetActive(true);

        }

        respawnableObstacles.Clear();

    }

    public void DeactivateInstructions()
    {

        instruction.SetActive(false);
        areInstructionsActive = false;

    }

}
