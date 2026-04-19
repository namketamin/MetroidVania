using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float length;
    private float startPos;
    private void Awake()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void Start()
    {
        startPos=transform.position.x;
    }
    void Update()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);
        transform.position= new Vector3(startPos+distance, transform.position.y, transform.position.z);
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
