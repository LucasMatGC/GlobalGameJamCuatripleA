using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject player;
    private Rigidbody rbPlayer;
    public Material materialPlayer;
    public float movementSpeed = 0.1f;
    public float jumpForce = 10.0f;

    private bool canMove = true;
    private bool canJumpOrSlide = true;
    private bool canHit = true;

    // Start is called before the first frame update
    void Start()
    {

        rbPlayer = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        MovePlayer();
        ManageInput();
        

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
            Debug.Log("Jumping!");
            JumpAction();

        } else if (Input.GetButtonDown("SlideButton") && canJumpOrSlide)
        {
            Debug.Log("Sliding!");
            SlideAction();

        } else if (Input.GetButtonDown("HitButton") && canHit)
        {
            Debug.Log("Hiting!");
            HitAction();

        } else if (Input.GetButtonDown("StopButton"))
        {
            Debug.Log("Stop or continue!");
            canMove = !canMove;

        }

    }

    private void JumpAction()
    {
        canJumpOrSlide = !canJumpOrSlide;
        rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(ActivateFlag(1, false));

    }

    private void SlideAction()
    {

        canJumpOrSlide = !canJumpOrSlide;
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y/2, this.transform.localScale.z);
        StartCoroutine(ActivateFlag(0.5f, true));

    }

    private void HitAction()
    {

        canHit = !canHit;
        materialPlayer.color = Color.red;
        StartCoroutine(HitProcess(0.5f));

    }

    private IEnumerator ActivateFlag(float waitTime, bool isSliding)
    {

        yield return new WaitForSeconds(waitTime);
        canJumpOrSlide = !canJumpOrSlide;

        if (isSliding)
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * 2, this.transform.localScale.z);

    }

    private IEnumerator HitProcess(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        canHit = !canHit;
        materialPlayer.color = Color.cyan;

    }

}
