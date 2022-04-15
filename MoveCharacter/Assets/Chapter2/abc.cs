using UnityEngine;

public class abc : MonoBehaviour
{
    // Start is called before the first frame update
    float x;
    void Start()
    {
        x = transform.localPosition.x;
        //x = transform.position.x;
    }
	private void FixedUpdate()
	{
        Debug.Log("global : " + gameObject.transform.position);
        Debug.Log("local : " + gameObject.transform.localPosition);
        x += 0.01f;

        gameObject.transform.localPosition = new Vector3(x, transform.position.y, transform.position.z);
        //gameObject.transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
	// Update is called once per frame
	void Update()
    {
        // √‡πÊ«‚
        //transform.Translate(Vector3.right * Time.deltaTime);
    }
}
