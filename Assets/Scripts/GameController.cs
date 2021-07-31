using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Other")]
    [SerializeField] Animator CountdownAnimator;

    [Space]
    [Header("Border")]
    [SerializeField] float speedBorder;  
    [SerializeField] Renderer top, bottom;
    Vector2 shiftBorder;

    [Space]
    [Header("Colored Wall")]
    [SerializeField] Transform baseObject;
    [SerializeField] float startSpeedColorWall;
    [SerializeField] float endSpeedColorWall;
    [SerializeField] float dxSpeedColorWall;
    float currentSpeedColorWall;

    [Space]
    [Header("Wall params")]
    float pointX;
    [SerializeField] float hWidthColorWall;
    [SerializeField] SpriteRenderer[] coloredWallSprites;
    [SerializeField] Collider2D[] coloredWallColliders;
    [SerializeField] Color[] baseColors;

    [Space]
    [Header("Background")]
    [SerializeField] Renderer background;
    [SerializeField] float backgroundSpeed;
    Vector2 backgroundShift;


    public void CountdownAnimatorPlay()
    {
        // Event. ButtonStart
        CountdownAnimator.Play("Countdown", -1, 0f);
    }

    void ColorShuffle()
    {
        int startIdx = Random.Range(0, int.MaxValue >> 1);

        int index;
        Color temp;

        for (int i = 0; i < baseColors.Length; i++)
        {
            index = (i + startIdx) % baseColors.Length;
            
            temp = baseColors[index];
            baseColors[index] = baseColors[i];
            baseColors[i] = temp;
            
            startIdx++;
        }
    }

    Color GetRandomColor() => baseColors[Random.Range(0, baseColors.Length)];

    void AddSpeedColorWall()
    {
        currentSpeedColorWall += dxSpeedColorWall;

        if(currentSpeedColorWall > endSpeedColorWall)
        {
            currentSpeedColorWall = endSpeedColorWall;
        }
    }

    void SetSpeedParam()
    {
        if(BirdScripts.instance)
        {
            BirdScripts.instance.addSpeedColorWall += AddSpeedColorWall;
        }
    }

    void Start()
    {
        SetSpeedParam();
        ColorShuffle();

        if (BirdScripts.instance)
        {
            BirdScripts.OnGetRandomColor += GetRandomColor;
            BirdScripts.OnColoredWallSetActive += ColoredWallSetActive;
        }

        shiftBorder = top.material.GetTextureOffset("_MainTex"); // Для bottom аналогичное значение
        backgroundShift = background.material.GetTextureOffset("_MainTex");

        // Ширина экрана в юнитах
        pointX = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        pointX = pointX / 2.0f + hWidthColorWall; // Половина ширины относительно нуля + сдвиг за пределы поля
    }

    void SetColorBases()
    {
        for (int i = 0; i < coloredWallSprites.Length; i++)
        {
            coloredWallSprites[i].color = baseColors[i];
        }
    }

    void ColoredWallSetActive(bool value)
    {
        for (int i = 0; i < coloredWallColliders.Length; i++)
        {
            coloredWallColliders[i].enabled = value;
        }
    }

    void ShiftBorders()
    {
        // Borders
        shiftBorder.x += speedBorder * Time.deltaTime;
        top.material.SetTextureOffset("_MainTex", shiftBorder);
        bottom.material.SetTextureOffset("_MainTex", -shiftBorder);
    }

    void ShiftBackground()
    {
        // Background
        backgroundShift.x += backgroundSpeed * Time.deltaTime;
        background.material.SetTextureOffset("_MainTex", backgroundShift);
    }

    void Update()
    {
        ShiftBorders();
        ShiftBackground();

        if (BirdScripts.instance && BirdScripts.instance.isAlive)
        {
            baseObject.position = Vector3.MoveTowards(baseObject.position, baseObject.position - baseObject.right, currentSpeedColorWall * Time.deltaTime);
        }
        else // При старте игры Base ставим в начало, или если погибли
        {
            currentSpeedColorWall = startSpeedColorWall;
            
            Vector3 startPos = baseObject.position;
            startPos.x = pointX;
            baseObject.position = startPos;
        }

        // Correct Position
        if(baseObject.position.x < -pointX)
        {
            Vector3 startPos = baseObject.position;
            startPos.x = pointX;
            baseObject.position = startPos;

            ColorShuffle();
            SetColorBases();
            ColoredWallSetActive(true);
        }
    }
}
