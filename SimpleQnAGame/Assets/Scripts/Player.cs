using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : Photon.MonoBehaviour
{
    
    private BoxCollider2D boxCollider;
    private RaycastHit2D hit;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private Text playerNameText;



    public GameObject playerCamera;

    private Vector3 moveDelta;
    private Vector3 originalSize;

    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(photonView.isMine)
        {
            playerNameText.text = PhotonNetwork.playerName;
            playerCamera.SetActive(true);
            
        }
        else
        {
            playerNameText.color = Color.cyan;
            playerNameText.text = photonView.owner.NickName;
        }
    }
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSize = transform.localScale;
    }
    private void FixedUpdate()
    {
        if(photonView.isMine)
        {
            CheckInput();
        }
    }

    public void OnEnterAnswerBox(Answer answer)
    {
        if(photonView.isMine)
        { 
            GameManager.instance.SetAnswer(answer);
    
        }
    }

    public void OnExitAnswerBox()
    {
        if(photonView.isMine)
        {
            GameManager.instance.UnsetAnswer();
        }
    }

    private void CheckInput()
    {
        //Get input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //reset moveDelta
        moveDelta = new Vector3(x, y, 0);

        //flip sprite
        if (moveDelta.x > 0)
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }
        else if (moveDelta.x < 0)
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }

        if(moveDelta.x == 0 && moveDelta.y == 0)
        {
            photonView.RPC("NotRunningAnimation",PhotonTargets.AllBuffered);
        }
        else
        {
            photonView.RPC("IsRunningAnimation", PhotonTargets.AllBuffered);
        }
        //Make sure we can move in this direction, by casting a box there first. If the box returns null, then we are free to move!
        hit = Physics2D.BoxCast(transform.position,
                                boxCollider.size,
                                0,
                                new Vector2(0, moveDelta.y),
                                Mathf.Abs(moveDelta.y * Time.deltaTime),
                                LayerMask.GetMask("Actor", "Blocking"));

        if (!hit.collider)
        {
            //Move the player
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        //Make sure we can move in this direction, by casting a box there first. If the box returns null, then we are free to move!
        hit = Physics2D.BoxCast(transform.position,
                                boxCollider.size,
                                0,
                                new Vector2(moveDelta.x, 0),
                                Mathf.Abs(moveDelta.x * Time.deltaTime),
                                LayerMask.GetMask("Actor", "Blocking"));

        if (!hit.collider)
        {
            //Move the player
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }

    }

    [PunRPC]
    protected void FlipTrue()
    {
        spriteRenderer.flipX = true;
    }

    [PunRPC]
    protected void FlipFalse()
    {
        spriteRenderer.flipX = false;
    }
    [PunRPC]
    protected void IsRunningAnimation()
    {
        animator.SetBool("isRunning", true);
    }

    [PunRPC]
    protected void NotRunningAnimation()
    {
        animator.SetBool("isRunning", false);
    }
}
