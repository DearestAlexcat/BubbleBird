using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] Renderer background;

    [SerializeField] float speed;
    
    [SerializeField] float displayTime;

    Vector2 shift;

    void Start()
    {
        shift = background.material.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        shift.x += speed * Time.deltaTime;
        background.material.SetTextureOffset("_MainTex", shift);
    }
}
