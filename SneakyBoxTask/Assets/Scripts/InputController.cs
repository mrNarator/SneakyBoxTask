﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    [SerializeField]
    private InputField length;
    [SerializeField]
    private float height;
    [SerializeField]
    private InputField width;
    [SerializeField]
    private GameObject tiles;
    [SerializeField]

    private GameObject floor;
    private GameObject eastWall;
    private GameObject northWall;
    private bool occupied;

    public void Start()
    {
        occupied = false;
    }
    public void Submit()
    {
        float x=0, y=0;
        try
        {
            x = float.Parse(length.text);
            y = float.Parse(width.text);
        }
        finally
        {
            if (x != 0 && y != 0)
            {
                generateNewRoom(x, y);
            }
        }
    }

    void generateNewRoom(float x, float y)
    {
        if (occupied)
        {
            Destroy(floor);
            Destroy(northWall);
            Destroy(eastWall);
        }

        occupied = true;
        floor = new GameObject("floor");
        eastWall = new GameObject("eastWall");
        northWall = new GameObject("northWall");
        for (int i = 0; i < x ; i++)
        {
            for (int j = 0; j < y ; j++)
            {
                GameObject floorTile = Instantiate(tiles, new Vector3(1*i, 1*j, 0), transform.rotation, floor.transform);
            }
        }
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject eastWallTile = Instantiate(tiles, new Vector3(1 * i, 1 * j, 0), transform.rotation, eastWall.transform);
            }
        }
        for (int i = 0; i < height ; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject northWallTile = Instantiate(tiles, new Vector3(1 * i, 1 * j, 0), transform.rotation, northWall.transform);
            }
        }

        northWall.transform.Translate(-0.5f, 0, 0.5f);
        eastWall.transform.Translate(0, -0.5f, 0.5f);
        northWall.transform.Rotate(0, -90, 0);
        eastWall.transform.Rotate(90, 0, 0);
    }
}