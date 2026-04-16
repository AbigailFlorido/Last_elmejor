using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Movimiento: MonoBehaviour
{
    public float velocidad = 7f;
    //No importa tanto el valor de aqu� pero si es importante el punto y coma de esta parte
    public float fuerzaSalto = 7f;
    private Rigidbody2D rb;
    // rb es el nombre
	private bool enSuelo = true;
	
	public int slime;
	public int vidas = 3;
	
	public Image heart;
	public Image heart1;
	public Image heart2;
	
	public TMP_Text textoPuntos;
	
	public float tiempoDano = 0.5f;
	public float intervaloParpadeo = 0.1f;
	public float fuerzaEmpujeX = 4f;
	public float fuerzaEmpujeY = 3f;
	
	private bool recibiendoDano = false;
	private Color colorOriginal;
	
	private Animator animator;
	
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
	    spriteRenderer = GetComponent<SpriteRenderer>();
	    animator = GetComponent<Animator>();
	    colorOriginal = spriteRenderer.color;
	    ActualizarPuntos();
    }

    // Update is called once per frame
    void Update()
    {
        float movimientoX = Input.GetAxis("Horizontal");
        // se utiliza flotante porque no son enteros
        transform.Translate(movimientoX * velocidad * Time.deltaTime, 0f,0f);
        //translate es para mover

        if(movimientoX > 0)
        {
            spriteRenderer.flipX = false;
        }
        if(movimientoX < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            enSuelo = false;
            Debug.Log("Estas saltando");
        }
	    //animaciones
	    if(!enSuelo)
	    {
	    	animator.Play("Jump");
	    }
	    else if (movimientoX != 0)
	    {
	    	animator.Play ("Walk");
	    }
	    else
	    {
	    	animator.Play("Idle");
	    }
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hemos entrado");
        //Esto es para q salga como mensaje y sale en la consola de los mensajes, tmb hay que activarlo en el objeto como trigger y para comprobarlo pues sale en la consola
        //Tmb podemos actuar sobre etiquetas y pues cn el objeto le picamos y ah� viene para ponerle los tags, es importante q si lo usamos escribamos exactamente igual el nombre
      
	    if(other.name == "slime")
        {
	        // Debug.Log("Haz conseguido un slime");
		    slime = slime + 1;
		    ActualizarPuntos();
		    Debug.Log("Tienes: "+slime);
		    Destroy(other.gameObject);
        }
	    if(other.tag == ("eskeleton") && vidas > 0)
	    {
	    	QuitarVida();
	    	RecibirDano();
	    }
	    if (other.CompareTag("Enemy_head"))
	    {
	    	Enemigo enemigo = other.GetComponentInParent<Enemigo>();
	    	if (enemigo != null)
	    	{
	    		enemigo.Morir();
	    		rb.linearVelocity = new Vector2 (rb.linearVelocity.x, fuerzaSalto * 0.7f);
	    	}
	    }
    }
	    void RecibirDano()
	    {
	    	recibiendoDano = true;
	    	spriteRenderer.color = Color.red;
	    	float direccionEmpuje = spriteRenderer.flipX ? 1f : -1f;
	    	rb.linearVelocity = new Vector2 (direccionEmpuje * fuerzaEmpujeX, fuerzaEmpujeY);
	    	Invoke("VolverANormal", tiempoDano);
	    }
	void VolverANormal()
	{
		recibiendoDano = false;
		spriteRenderer.color = colorOriginal;
	}
	void ReiniciarNivel()
	{
		Scene escenaActual = SceneManager.GetActiveScene();
		SceneManager.LoadScene(escenaActual.name);
	}
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Debug.Log("Estas en el piso");
            enSuelo = true;
        }
	    if (collision.gameObject.CompareTag("Enemy") && vidas > 0 && !recibiendoDano)
	    {
	    	Debug.Log("Recibir daño del enemigo");
	    	RecibirDano();
	    	QuitarVida();
	    }
    }
	public void QuitarVida()
	{
		vidas --;
		if (vidas== 2)
		{
			heart2.enabled = false;
		}
		else if (vidas == 1)
		{
			heart1.enabled = false;
		}
		else if (vidas == 0)
		{
			heart.enabled = false;
			Debug.Log("Game over");
			Invoke("ReiniciarNivel", 0.5f);
		}
	}
	void ActualizarPuntos()
	{
		textoPuntos.text = "Slime" +slime;
	}
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Debug.Log("Estas fuera del piso");
            enSuelo = false;
        }
       
    }
    }

