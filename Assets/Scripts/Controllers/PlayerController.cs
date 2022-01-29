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
    private Rigidbody rbPlayer;
    private BoxCollider[] collidersPlayer;
    private Animator playerAnimator;
    private bool canMove = true;
    private bool canJumpOrSlide = true;
    private bool canHit = true;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {

        gameController = GameController.instance;

        rbPlayer = this.GetComponent<Rigidbody>();
        collidersPlayer = this.GetComponents<BoxCollider>();
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

            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + (movementSpeed * Time.deltaTime));

        }

    }

    private void ManageInput()
    {

        if (Input.GetButtonDown("JumpButton") && canJumpOrSlide)
        {
            JumpAction();

        } else if (Input.GetButtonDown("SlideButton") && canJumpOrSlide)
        {
            SlideAction();

        } else if (Input.GetButtonDown("HitButton") && canHit)
        {
            HitAction();

        } else if (Input.GetButtonDown("StopButton"))
        {
            canMove = !canMove;

        }

    }

    private void JumpAction()
    {
        canJumpOrSlide = !canJumpOrSlide;
        SelectAudio(jumpAudio, jumpAudioCode);
        playerAnimator.SetTrigger("Jump");
        rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(ActivateFlag(jumpForce*0.2f, false));

    }

    private void SlideAction()
    {

        canJumpOrSlide = !canJumpOrSlide;
        //slideAudio.Play();
        playerAnimator.SetTrigger("Slide");
        //this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y/slideSquish, this.transform.localScale.z);
        collidersPlayer[0].size = new Vector3(collidersPlayer[0].size.x, collidersPlayer[0].size.y / slideSquish, collidersPlayer[0].size.z); ;
        collidersPlayer[0].center = new Vector3(collidersPlayer[0].center.x, collidersPlayer[0].center.y / slideSquish, collidersPlayer[0].center.z);
        collidersPlayer[1].size = new Vector3(collidersPlayer[1].size.x, collidersPlayer[1].size.y / slideSquish, collidersPlayer[1].size.z); ;
        collidersPlayer[1].center = new Vector3(collidersPlayer[1].center.x, collidersPlayer[1].center.y / slideSquish, collidersPlayer[1].center.z);
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y / slideSquish, this.transform.position.z);
        StartCoroutine(ActivateFlag(slideTime, true));

    }

    private void HitAction()
    {

        canHit = !canHit;
        //hitAudio.Play();
        playerAnimator.SetTrigger("Hit");
        materialPlayer.color = Color.red;
        StartCoroutine(HitProcess(hitTime));

    }

    private void SelectAudio(AudioSource audioSource, string nameCode)
    {

        string pathFile = nameCode + (Random.Range(0, 3).ToString()) + ".ogg";
        audioSource.clip = (AudioClip)AssetDatabase.LoadAssetAtPath(nameCode + (Random.Range(0, 3).ToString()) + ".ogg", typeof(AudioClip));
        audioSource.Play();
        

    }

    private IEnumerator ActivateFlag(float waitTime, bool isSliding)
    {

        yield return new WaitForSeconds(waitTime);
        canJumpOrSlide = !canJumpOrSlide;

        if (isSliding)
        {

            //this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * slideSquish, this.transform.localScale.z);
            //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y * slideSquish, this.transform.position.z);
            collidersPlayer[0].size = new Vector3(collidersPlayer[0].size.x, collidersPlayer[0].size.y * slideSquish, collidersPlayer[0].size.z); ;
            collidersPlayer[0].center = new Vector3(collidersPlayer[0].center.x, collidersPlayer[0].center.y * slideSquish, collidersPlayer[0].center.z);
            collidersPlayer[1].size = new Vector3(collidersPlayer[1].size.x, collidersPlayer[1].size.y * slideSquish, collidersPlayer[1].size.z); ;
            collidersPlayer[1].center = new Vector3(collidersPlayer[1].center.x, collidersPlayer[1].center.y * slideSquish, collidersPlayer[1].center.z);

        }

    }

    private IEnumerator HitProcess(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        canHit = !canHit;
        materialPlayer.color = Color.cyan;

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JumpSlide")
        {
            gameController.EndGame();
        }
        else if (other.tag == "Hit" && canHit)
        {
            gameController.EndGame();
        }
        else if (other.tag == "Hit" && !canHit)
            return;
    }

}
