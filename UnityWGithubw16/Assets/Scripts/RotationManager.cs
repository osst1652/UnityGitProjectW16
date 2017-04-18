using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    //get player movement

    private PlayerMovement playerMove;


    public FacingDirection facingDirection;

    public GameObject player;
    private float degree = 0;

    public Transform Level;
    public Transform Building;

    public GameObject InivisiCube;

    private List<Transform> InvisiList = new List<Transform>();

    private FacingDirection lastFacing;

    private float lastDepth = 0f;


    public float worldUnits = 1.000f;

    // Use this for initialization
    void Start()
    {
        facingDirection = FacingDirection.Front;
        playerMove = player.GetComponent<PlayerMovement>();
        UpdateLevelData(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMove.isJumping)
        {
            bool updateData = false;
            if (OnInvisiblePlatform())
            {
                if (movePlayerDepthToClosestPlatform())
                {

                    updateData = true;
                }
            }
            if (MoveToClosestPlatformToCamera())
            {
                updateData = true;
            }

            if (updateData)
            {
                UpdateLevelData(false);
            }

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (OnInvisiblePlatform())
            {
                movePlayerDepthToClosestPlatform();
            }
            lastFacing = facingDirection;
            facingDirection = rotateDirectionRight();
            degree -= 90f;
            UpdateLevelData(false);
            playerMove.updateToFacingdirection(facingDirection, degree);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (OnInvisiblePlatform())
            {
                movePlayerDepthToClosestPlatform();
            }

            lastFacing = facingDirection;
            facingDirection = rotateDirectionLeft();
            degree += 90f;
            UpdateLevelData(false);
            playerMove.updateToFacingdirection(facingDirection, degree);
        }
    }

    private void UpdateLevelData(bool forceRebuild)
    {
        if (!forceRebuild)
        {
            if (lastFacing == facingDirection && lastDepth == getPlayerDepth())
                return;
            foreach (Transform tr in InvisiList)
            {
                tr.position = Vector3.zero;
                Destroy(tr.gameObject);
            }
            InvisiList.Clear();
            float newDepth = 0f;

            newDepth = getPlayerDepth();
            createInvisicubesAtNewDepth(newDepth);
        }
    }

    private bool OnInvisiblePlatform()
    {
        foreach (Transform item in InvisiList)
        {
            if (Mathf.Abs(item.position.x - playerMove.transform.position.x) < worldUnits && Mathf.Abs(item.position.z - playerMove.transform.position.z) < worldUnits)
            {
                if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y > 0)
                {
                    return true;
                }
            }

        }
        return false;
    }

    private bool MoveToClosestPlatformToCamera()
    {
        bool moveCloser = false;
        foreach (Transform item in Level)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                if (Mathf.Abs(item.position.x - item.position.x) < worldUnits + 0.1f)
                {
                    if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y > 0 && !playerMove.isJumping)
                    {
                        if (facingDirection == FacingDirection.Front && item.position.z < playerMove.transform.position.z)
                        {
                            moveCloser = true;
                        }
                        if (facingDirection == FacingDirection.Back && item.position.z > playerMove.transform.position.z)
                        {
                            moveCloser = true;
                        }
                        if (moveCloser)
                        {
                            playerMove.transform.position = new Vector3(playerMove.transform.position.x, playerMove.transform.position.y, item.position.z);
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (Mathf.Abs(item.position.z - playerMove.transform.position.z) < worldUnits + 0.1f)
                {
                    if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y > 0 && !playerMove.isJumping)
                    {
                        if (facingDirection == FacingDirection.Right && item.position.x > playerMove.transform.position.x)
                        {
                            moveCloser = true;

                        }
                        if (facingDirection == FacingDirection.Left && item.position.x < playerMove.transform.position.x)
                        {
                            moveCloser = true;
                        }
                        if (moveCloser)
                        {
                            playerMove.transform.position = new Vector3(item.position.x, playerMove.transform.position.y, playerMove.transform.position.z);
                            return true;
                        }
                    }
                }
            }
        }




        return false;
    }

    private bool FindTransformInvisiList(Vector3 cube)
    {
        foreach (Transform item in InvisiList)
        {
            if (item.position == cube)
                return true;
        }
        return false;
    }
    private bool findTransformLevel(Vector3 cube)
    {
        foreach (Transform item in Level)
        {
            if (item.position == cube)
                return true;
        }
        return false;
    }
    private bool findTransformBuilding(Vector3 cube)
    {
        foreach (Transform item in Building)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (item.position.x == cube.x && item.position.y == cube.y && item.position.z < cube.z)
                    return true;
            }
            else if (facingDirection == FacingDirection.Back)
            {
                if (item.position.x == cube.x && item.position.y == cube.y && item.position.z > cube.z)
                    return true;
            }
            else if (facingDirection == FacingDirection.Right)
            {
                if (item.position.z == cube.z && item.position.y == cube.y && item.position.x > cube.x)
                    return true;
            }
            else
            {
                if (item.position.z == cube.z && item.position.y == cube.y && item.position.x < cube.x)
                    return true;
            }
        }
        return false;
    }

    private bool movePlayerDepthToClosestPlatform()
    {
        foreach (Transform item in Level)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                if (Mathf.Abs(item.position.x - playerMove.transform.position.x) < worldUnits + 0.1f)
                {
                    if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y > 0)
                    {
                        playerMove.transform.position = new Vector3(playerMove.transform.position.x, playerMove.transform.position.y, item.position.z);
                        return true;
                    }
                }
            }
            else
            {
                if (Mathf.Abs(item.position.z - playerMove.transform.position.z) < worldUnits + 0.1f)
                {
                    if (playerMove.transform.position.y - item.position.y <= worldUnits + 0.2f && playerMove.transform.position.y - item.position.y > 0)
                    {
                        playerMove.transform.position = new Vector3(item.position.x, playerMove.transform.position.y, playerMove.transform.position.z);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private Transform createInvisiCube(Vector3 position)
    {
        GameObject gameObj = Instantiate(InivisiCube) as GameObject;
        gameObj.transform.position = position;

        return gameObj.transform;
    }
    private void createInvisicubesAtNewDepth(float newDepth)
    {
        Vector3 tempCube = Vector3.zero;
        foreach (Transform child in Level)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                tempCube = new Vector3(child.position.x, child.position.y, newDepth);
                if (!FindTransformInvisiList(tempCube) && !findTransformLevel(tempCube) && !findTransformBuilding(child.position))
                {
                    Transform go = createInvisiCube(tempCube);
                    InvisiList.Add(go);
                }
            }
            else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
            {
                tempCube = new Vector3(newDepth, child.position.y, child.position.z);
                if (!FindTransformInvisiList(tempCube) && !findTransformLevel(tempCube) && !findTransformBuilding(child.position))
                {
                    Transform go = createInvisiCube(tempCube);
                    InvisiList.Add(go);
                }
            }
        }
    }
    public void returnToStart()
    {
        UpdateLevelData(true);
    }
    private float getPlayerDepth()
    {
        float closestPosition = 0f;
        if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
        {
            closestPosition = playerMove.transform.position.z;
        }
        else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
        {
            closestPosition = playerMove.transform.position.x;
        }

        return Mathf.Round(closestPosition);
    }
    private FacingDirection rotateDirectionRight()
    {
        int change = (int)(facingDirection);
        change++;
        if (change > 3)
            change = 0;
        return (FacingDirection)(change);
    }
    private FacingDirection rotateDirectionLeft()
    {
        int change = (int)(facingDirection);
        change--;
        if (change < 0)
            change = 3;
        return (FacingDirection)(change);
    }




}
public enum FacingDirection
{
    Front = 0,
    Right = 1,
    Back = 2,
    Left = 3

}