using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
public class characterControl : MonoBehaviour
{
    // Karakterin hareketi icin gerekli parametreler
    Rigidbody2D rigidbodyC;
    Vector3 vector; 
    float horizontal = 0;
    bool jumpOnce = true;
    Animator animator;
    float waitingTime = 0;
    string currentAnimation = "idle";

    // Kamera ile karakter takibi icin gerekli parametreler
    GameObject cameraC; 
    Vector3 camLastPos;
    Vector3 camFirstPos;

    // UI parametreleri
    public Text liveText;
    int live = 100;
    public Image backgroundImage;
    float bImageCounter = 0;
    float mainMenuCounter = 0;

    // Can ve altýn icin parametreler
    GameObject chest;
    public Text coinText; 
    int coinCounter = 0;
   
    void Start()
    {
        Time.timeScale = 1;
        backgroundImage.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        chest = GameObject.FindGameObjectWithTag("giveLive");
        rigidbodyC = GetComponent<Rigidbody2D>();
        cameraC = GameObject.FindGameObjectWithTag("MainCamera");
        camFirstPos = cameraC.transform.position - transform.position;
        liveText.text = "LIVE : " + live;
        coinText.text = "" + coinCounter;
        if(SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("levelNo"))
        {
            PlayerPrefs.SetInt("levelNo", SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Update()
    {
        // Bilgisayara build edileceði zaman kullanýlan if (Input.GetKeyDown(KeyCode.Space) && jumpOnce){
        if (CrossPlatformInputManager.GetButtonDown("Jump") && jumpOnce)
        {
            rigidbodyC.AddForce(new Vector2(0, 800));
            rigidbodyC.gravityScale = 2;
            jumpOnce = false;
        }
    }
    void FixedUpdate()
    {
        charMovement();
        charAnimation();
        if(live <= 0)
        {
            backgroundImage.gameObject.SetActive(true);
            Time.timeScale = .4f; 
            liveText.enabled = false;
            bImageCounter += 0.03f;
            backgroundImage.color = new Color(0, 0, 0, bImageCounter);
            mainMenuCounter += Time.deltaTime; 
            if(mainMenuCounter > 1)
            {
                SceneManager.LoadScene("mainMenu");
            }
        }
    }

    // kamera olaylarýnda kullanýlmasý tercih ediliyor. 
    private void LateUpdate()
    {
        cameraControl();
    }
    void charMovement()
    {
        horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        vector = new Vector3(horizontal*10, rigidbodyC.velocity.y, 0);
        rigidbodyC.velocity = vector; 
    }

    // Nesne katý bir yere deðdiðinde aktif oluyor
    private void OnCollisionEnter2D(Collision2D col)
    {
        jumpOnce = true;    
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "bullet")
        {
            live -= 5;
        }
        else if (col.gameObject.tag == "mace" || col.gameObject.tag == "saw" )
        {
            live -= 10; 
        }
        else if (col.gameObject.tag == "finishLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
        }
        else if (col.gameObject.tag == "giveLive")
        {  
            if(live < 90)
            {
                live += 10;
            }
            else
            {
                live = 100; 
            }
            col.GetComponent<BoxCollider2D>().enabled = false; 
            chest.GetComponent<giveLife>().openChest(); 
            Destroy(col.gameObject, 0.7f);
        }
        else if (col.gameObject.tag == "coin")
        {
            coinCounter++;
            coinText.text = "" + coinCounter;
            Destroy(col.gameObject, 0.2f);
        }
        else if (col.gameObject.tag == "water")
        {
            transform.eulerAngles = new Vector3(0,0,70);
            live = 0; 
        }
       
        liveText.text = "LIVE : " + live;
    }
    void charAnimation()
    {
        if (jumpOnce)
        {
            waitingTime += Time.deltaTime;
            if(horizontal == 0 && waitingTime > 0.01f && currentAnimation != "idle")
            {
                animator.SetTrigger("idleTrigger");
                Debug.Log("idle tetiklendi \n current = idle");
                currentAnimation = "idle";

            }else if (horizontal > 0 && waitingTime > 0.01f && currentAnimation != "walkR")
            {
                Debug.Log("walkR tetiklendi \n current = walkR");
                animator.SetTrigger("walkTrigger");
                transform.localScale = Vector3.one;
                currentAnimation = "walkR";
            }
            else if(horizontal < 0 && waitingTime > 0.01f && currentAnimation != "walkL")
            {
                Debug.Log("walkL tetiklendi \n current = walkL");
                animator.SetTrigger("walkTrigger");
                transform.localScale = new Vector3(-1, 1, 1);
                currentAnimation = "walkL";
            }

            waitingTime = 0;
        }
        else
        {
            if(rigidbodyC.velocity.y > 0 && currentAnimation != "jump")
            {
                Debug.Log("jump tetiklendi \n current = jump");
                animator.SetTrigger("jumpTrigger");
                currentAnimation = "jump";
            }
            
            if(horizontal > 0)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
    }
    void cameraControl()
    {
        camLastPos = camFirstPos + transform.position;
        //kamera hareketlerinin yumuþatýlmasýný saðlar. 
        if(camLastPos.y > -12.3f && camLastPos.x > -26.4f)
        {
            cameraC.transform.position = Vector3.Lerp(cameraC.transform.position, camLastPos, 0.08f); 
        }
        
    }

 }
