using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{

    public GameObject player;
    public Material materialPlayer;
    public AudioSource jumpAudio, slideAudio, hitAudio;
    public float movementSpeed = 0.1f;
    public float jumpForce = 10.0f;
    public float hitTime = 1.0f;
    public float slideTime = 0.5f;
    public float slideSquish = 2;

    private string jumpAudioCode = "Assets/Audio/SFX/GGJ2022_SFX_Jump_", slideAudioCode = "Assets/Audio/SFX/GGJ2022_SFX_Slide_", hitAudioCode = "Assets/Audio/SFX/GGJ2022_SFX_Hit_";
    private Animator playerAnimator;
    private bool canMove = true;
    private bool canJumpOrSlide = true;
    private bool canHit = true;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {

        gameController = GameController.instance;
        playerAnimator = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController)
            gameController = GameController.instance;
        if (gameController.IsGameRunning())
        {
            MovePlayer();
            ManageInput();

        }
        

    }

    private void MovePlayer()
    {

        if (player && canMove)
        {
            if (gameController.isFirstLoop())
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + (movementSpeed * Time.deltaTime));
            else
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - (movementSpeed * Time.deltaTime));

        }

    }

    private void ManageInput()
    {

        if (Input.GetButtonDown("JumpButton") && canJumpOrSlide)
            JumpAction();

        else if (Input.GetButtonDown("SlideButton") && canJumpOrSlide)
            SlideAction();

        else if (Input.GetButtonDown("HitButton") && canHit)
            HitAction();

        //Only for Debug
        //else if (Input.GetButtonDown("StopButton"))
        //    canMove = !canMove;


    }

    private void JumpAction()
    {
        canJumpOrSlide = !canJumpOrSlide;
        SelectAudio(jumpAudio, jumpAudioCode);
        playerAnimator.SetTrigger("Jump");
        StartCoroutine(ActivateFlag(jumpForce*0.2f));

    }

    private void SlideAction()
    {

        canJumpOrSlide = !canJumpOrSlide;
        SelectAudio(slideAudio, slideAudioCode);
        playerAnimator.SetTrigger("Slide");
        StartCoroutine(ActivateFlag(slideTime));

    }

    private void HitAction()
    {

        canHit = !canHit;
        SelectAudio(hitAudio, hitAudioCode);
        playerAnimator.SetTrigger("Hit");
        materialPlayer.color = Color.red;
        StartCoroutine(HitProcess(hitTime));

    }

    private void SelectAudio(AudioSource audioSource, string nameCode)
    {
        
       // audioSource.clip = Resources.LoadAll("Audio/SFX/" + nameCode, typeof(AudioClip)).Cast<AudioClip>().ToArray()[(Random.Range(0, 3).ToString())];
        // (AudioClip)AssetDatabase.LoadAssetAtPath(nameCode + (Random.Range(0, 3).ToString()) + ".ogg", typeof(AudioClip));
        audioSource.Play();
        
    }

    private IEnumerator ActivateFlag(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        canJumpOrSlide = !canJumpOrSlide;

    }

    private IEnumerator HitProcess(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        canHit = !canHit;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JumpSlide")
            gameController.EndGame();

        else if ((other.tag == "Hit" && canHit))
            gameController.EndGame();

        else if (other.tag == "Hit" && !canHit)
        {

            other.gameObject.SetActive(false);
            //gameController.AddRespawnableObstacle(other.gameObject);
            return;

        }
    }

}
