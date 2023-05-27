using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class CarromStriker : MonoBehaviour
{

    //A simple class to move and strike the striker toward the desired direction (NOTE:Uses Old Input System)
    public static CarromStriker Instance { get; private set; }
    public static event System.Action OnStrike;
    [Header("UI Element")]
    [SerializeField] private Slider strikerSlider;//TODO:Refactor it into its own script to remove UI Dependency
    [Header("Transform")]
    [SerializeField] private Transform forcePoint;   //point from which force will applied toward the striker
    [SerializeField] private Transform strikerIndicator;
    [Space()]
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] Turn turnPlayer;

    //Decides whether force could be applied on Striker
    bool canApplyForce;
    //Decides whether player could strike or not 
    bool canStrike = true;
    Vector3 forceDirection;
    RaycastHit2D hit;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {

        transform.position = initialPosition;
        strikerSlider.onValueChanged.AddListener((value) =>
        {
            canApplyForce = false;
            strikerIndicator.localScale = Vector3.zero;
            transform.position = new Vector2(value, initialPosition.y);
        });

    }

    private void Update()
    {//Allow movement only when turn is of player
        if (turnPlayer == GameManager.Instance.CurrentTurn)
        {
            if (EventSystem.current.IsPointerOverGameObject() && !canApplyForce)
            {
                return;
            }
            if (!canStrike)
            {
                return;
            }
            strikerSlider.interactable = true;

            if (Input.GetMouseButton(0))
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward);

                //NOTE:We requires a whole screen collider to avoid any glitchy and awkward behaviour
                if (hit.collider != null)
                {
                    print(hit.transform.name);
                    if (hit.collider.tag == "Striker")
                    {
                        canApplyForce = true;
                    }

                    //we can use atan2 method too but while using lookAt (NOTE:keep your right to forward)
                    if (canApplyForce)
                    {
                        strikerIndicator.LookAt(hit.point);
                        Vector2 minMaxScale = new Vector2(0.5f, 5.2f);
                        float scaleMultiplier = Mathf.Clamp(Vector2.Distance(transform.position, hit.point), 0.5f, 5.2f);


                        strikerIndicator.localScale = Vector3.one * scaleMultiplier;
                    }





                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (canApplyForce)
                {


                    if (hit.collider == null) return;
                    if (hit.collider.tag != "Striker")
                    {

                        OnStrike?.Invoke();
                        forceDirection = transform.position - forcePoint.position;
                        float forceAmount = 100f;
                        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        GetComponent<Rigidbody2D>().AddForce(forceDirection * forceAmount, ForceMode2D.Impulse);

                        strikerIndicator.localScale = Vector3.zero;
                        canApplyForce = false;
                        canStrike = false;
                        strikerSlider.interactable = false;
                    }
                }
            }
        }
        else
        {
            strikerSlider.interactable = false;
            strikerIndicator.localScale = Vector3.zero;
        }
    }




    public void ResetSide(bool canSwitch)
    {


        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        StartCoroutine(MoveToInitial(0.5f, canSwitch));


    }

    public void Spawn()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        transform.eulerAngles = Vector3.zero;
        yield return StartCoroutine(ColorLerp(0.5f));
        GetComponent<CircleCollider2D>().enabled = true;
        canStrike = true;
    }
    IEnumerator ColorLerp(float duration)
    {

        SpriteRenderer spriteRender = GetComponentInChildren<SpriteRenderer>();

        float percent = 0f;
        float timer = 0f;
        spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, 0);

        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / duration;

            spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, Mathf.Lerp(0, 1, percent));

            yield return null;
        }

    }
    //duration denotes time required to moveToInitialPosition and fadeColor denotes whether striker would fade or not ,
    //if not fade,striker could be used again 

    IEnumerator MoveToInitial(float duration, bool fadeColor)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        SpriteRenderer spriteRender = GetComponentInChildren<SpriteRenderer>();
        Color initialColor = spriteRender.color;
        Vector3 currentPosition = (Vector2)transform.position;
        Vector3 targetPosition = initialPosition;


        float percent = 0f;
        float timer = 0f;
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / duration;
            if (fadeColor)
                spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, Mathf.Lerp(1, 0, percent));
            transform.position = Vector2.Lerp(currentPosition, targetPosition, percent);
            yield return null;
        }


        //TODO: have to refactor to new name ,not clear enough
        if (!fadeColor)
        {
            GetComponent<CircleCollider2D>().enabled = true;
            canStrike = true;

        }



    }
}
