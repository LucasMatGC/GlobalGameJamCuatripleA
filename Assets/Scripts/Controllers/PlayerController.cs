using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Rigidbody rbPlayer;
    private SphereCollider colliderPlayer;
    private bool canMove = true;
    private bool canJumpOrSlide = true;
    private bool canHit = true;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {

        gameController = GameController.instance;

        rbPlayer = this.GetComponent<Rigidbody>();
        colliderPlayer = this.GetComponent<SphereCollider>();

    }

    // Update is called once per frame
    void Update()
    {

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

            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + movementSpeed);

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
        //jumpAudio.Play();
        rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(ActivateFlag(jumpForce*0.2f, false));

    }

    private void SlideAction()
    {

        canJumpOrSlide = !canJumpOrSlide;
        //slideAudio.Play();
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y/slideSquish, this.transform.localScale.z);
        colliderPlayer.radius /= (slideSquish);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - (slideSquish*0.1f), this.transform.position.z);
        StartCoroutine(ActivateFlag(slideTime, true));

    }

    private void HitAction()
    {

        canHit = !canHit;
        //hitAudio.Play();
        materialPlayer.color = Color.red;
        StartCoroutine(HitProcess(hitTime));

    }

    private IEnumerator ActivateFlag(float waitTime, bool isSliding)
    {

        yield return new WaitForSeconds(waitTime);
        canJumpOrSlide = !canJumpOrSlide;

        if (isSliding)
        {

            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * slideSquish, this.transform.localScale.z);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (slideSquish * 0.1f), this.transform.position.z);
            colliderPlayer.radius = colliderPlayer.radius * (slideSquish);

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
            Debug.Log("Choque obstaculo!");
            gameController.EndGame();
        }
        else if (other.tag == "Hit" && canHit)
        {
            Debug.Log("Obstactulo sin destruir!");
            gameController.EndGame();
        }
        else if (other.tag == "Hit" && !canHit)
            //Objeto roto
            Debug.Log("Objeto roto!");
    }

}
