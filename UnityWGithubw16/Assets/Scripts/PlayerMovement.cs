using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Animator anim;
    private int Horizontal = 0;
    public float MovementSpeed = 5f;
    public float Gravity = 1f;
    public CharacterController charController;
    private FacingDirection _playerFacingDirection;
    public float jumpHeight = 0f;
    public bool isJumping = false;
    private float degree = 0;
    
    public FacingDirection cmdFacingDirection
    {
        set { _playerFacingDirection = value; }
    }


    private void MoveCharacter(float moveFactor)
    {
        Vector3 transition = Vector3.zero;
        if(_playerFacingDirection == FacingDirection.Front)
        {
            transition = new Vector3(Horizontal * moveFactor, -Gravity * moveFactor, 0f);
        }
        else if(_playerFacingDirection == FacingDirection.Right)
        {
            transition = new Vector3(0f, -Gravity * moveFactor, Horizontal * moveFactor);
        }
        else if(_playerFacingDirection == FacingDirection.Back)
        {
            transition = new Vector3(-Horizontal * moveFactor, -Gravity * moveFactor, 0f);
        }
        else if(_playerFacingDirection == FacingDirection.Left)
        {
            transition = new Vector3(0f, -Gravity * moveFactor, -Horizontal * moveFactor);
        }
        if (isJumping)
        {
            transform.Translate(Vector3.up * jumpHeight * Time.deltaTime);
        }
        charController.SimpleMove(transition);
    }

	public IEnumerator JumpingWait()
    {
        yield return new WaitForSeconds(0.35f);
        isJumping = false;
    }
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis("Horizontal") < 0)
        {
            Horizontal = -1;
        }else if (Input.GetAxis("Horizontal") > 0)
        {
            Horizontal = 1;
        }
        else
        {
            Horizontal = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space)&& !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpingWait());
        }
        if (anim)
        {
            anim.SetInteger("Horizontal", Horizontal);
            float moveFactor = MovementSpeed * Time.deltaTime * 10f;
            //MoveCharacter
            MoveCharacter(moveFactor);

        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8 * Time.deltaTime);
		

	}
    public void updateToFacingdirection(FacingDirection newDirection, float angle)
    {
        _playerFacingDirection = newDirection;
        degree = angle;
    }
}
