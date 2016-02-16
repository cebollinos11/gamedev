using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {

    float margin = 1/3f;
    public float speed;
    
    Vector2 screenSize;

	// Use this for initialization
	void Start () {

        screenSize = new Vector2(Screen.width, Screen.height);


	}

    void MoveHorizontal(int direction) {

        
        transform.Translate(Vector3.right * Time.deltaTime * speed * direction);
    }

    void MoveVertical(int direction)
    {
        float y = transform.position.y;
        transform.Translate(Vector3.forward * Time.deltaTime * speed * direction);

        transform.position = Vector3.Scale(transform.position, new Vector3(1, 0, 1)) + new Vector3(0, 1, 0) * y;
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKey(KeyCode.Mouse2)) //dont move if right mouse pressed
        {

            Vector3 mousePos = Input.mousePosition;

            if (mousePos.x < margin * screenSize.x)
            {
                MoveHorizontal(-1);
            }

            if (mousePos.x > (1 - margin) * screenSize.x)
            {
                MoveHorizontal(1);
            }


            if (mousePos.y < margin * screenSize.y)
            {
                MoveVertical(-1);
            }

            if (mousePos.y > (1 - margin) * screenSize.y)
            {
                MoveVertical(1);
            }
        }
	}
}
