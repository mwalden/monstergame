using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public GameObject gridSpace;

    public GameObject[,] spaces;
    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {
        spaces = new GameObject[x,y];
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            {
                Vector3 newPosition = new Vector3(i * 16f, j * 16f, 10f);
                GameObject space = Instantiate(gridSpace, transform);
                space.transform.localPosition = newPosition;
                space.GetComponent<GridSpace>().x = i;
                space.GetComponent<GridSpace>().y = j;
                spaces[i, j] = space;
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MarkTaken(float x, float y)
    {
        print("Taken : " + x + ", " + y);
        Collider2D hit = GetGridSpace(x, y);
        if (hit != null)
        {
            hit.GetComponent<DrawCube>().Taken();
            hit.GetComponent<GridSpace>().occupied = true;
        }
    }
    public void MarkFree(float x, float y)
    {
        Collider2D hit = GetGridSpace(x, y);
        if (hit != null)
        {
            hit.GetComponent<DrawCube>().Free();
            hit.GetComponent<GridSpace>().occupied = false;
        }
    }
    public bool isTaken(float x, float y)
    {
        Collider2D hit = GetGridSpace(x, y);
        if (hit != null)
        {
            return hit.GetComponent<GridSpace>().occupied;
        }
        return false;
    }

    public bool isValid(float x, float y)
    {
        Collider2D hit = GetGridSpace(x, y);
        return (hit != null);
        
    }

    public Vector2 GetRandomAdjacentSpace(float x, float y)
    {
        Collider2D hit = GetGridSpace(x, y);
        if (hit != null)
        {
            for (int i = 0; i < this.x; i++)
                for (int j = 0; j < this.y; j++)
                {
                    if (hit.gameObject.Equals(spaces[i, j]))
                    {
                        GameObject go = GetRandomSpaceAroundPoint(i, j);
                        return new Vector2(go.transform.position.x, go.transform.position.y);
                    }
                        
                }
        }
        return new Vector2(x,y);
    }
    

    public GameObject GetRandomSpaceAroundPoint(int i, int j)
    {
        Random rnd = new Random();
        print("starting at " + i + ", " + j);
        //int radius = 3;
        int totalRadius = 3;
        List<GameObject> gos = new List<GameObject>();
        for (int radius =0; radius < totalRadius;radius++)
            for (int x = i - radius; x <= i + radius; x++)
                for (int y = j - radius; y <= j + radius; y++)
                {
                    if (x < 0 || y < 0)
                        continue;
                    if (x >= this.x || y >= this.y)
                        continue;
                    gos.Add(spaces[x, y]);
                    //print("Adding " + x + ", " + y);
                }
        int roll1 = Random.Range(0, gos.Count - 1);
        int roll2 = Random.Range(0, gos.Count - 1);
        int choice = Mathf.Min(roll1, roll2);
        return gos[roll1];
    }
    
    //bug here when position is at 0,0
    public List<GameObject> GetSpacesAroundPoint(Vector2 position)
    {
        Collider2D coll = GetGridSpace(position.x, position.y);
        MarkTaken(position.x, position.y);
        if (coll == null)
            return null;
        int i = coll.gameObject.GetComponent<GridSpace>().x;
        int j = coll.gameObject.GetComponent<GridSpace>().y;
        
        List<GameObject> gos = new List<GameObject>();
        for (int x = i - 1; x <= i + 1; x++)
            for (int y = j - 1; y <= j + 1; y++)
            {
                if (x < 0 || y < 0)
                    continue;
                if (x > this.x || y > this.y)
                    continue;
                if (spaces[x, y].Equals(coll.gameObject))
                    continue;
                MarkTaken(spaces[x, y].transform.position.x, spaces[x, y].transform.position.y);
                gos.Add(spaces[x, y]);
            }
        return gos;
    }


    Collider2D GetGridSpace(float x, float y)
    {
        Vector2 pos = new Vector2(x, y);
        RaycastHit2D hit = Physics2D.Raycast(pos, -Vector2.up);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }
}
