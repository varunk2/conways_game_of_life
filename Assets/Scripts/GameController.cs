using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int SCREEN_WIDTH = 153;
    public static int SCREEN_HEIGHT = 114;
    public float updateRate = 0.1f;
    public bool start = true;

    [SerializeField] private Sprite _cellSprite;
    private int _off = 0;
    private int _on = 1;

    private CellController[,] grid = new CellController[SCREEN_WIDTH, SCREEN_HEIGHT];
    private int[,] states = new int[SCREEN_WIDTH, SCREEN_HEIGHT];
    private int[,] rules = new int[SCREEN_WIDTH, SCREEN_HEIGHT];

    void Start()
    {
        InitializeGrid();

        if (start) {
            RandomizeGrid();
            InvokeRepeating("UpdateStates", updateRate, updateRate);
        }
    }    

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) {
            start = start ? false : true;

            if (start) {
                RandomizeGrid();
                InvokeRepeating("UpdateStates", updateRate, updateRate);
            } else {
                CancelInvoke("UpdateStates");
            }

        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            //Apply the new mode per each cell
            for (int x = 0; x < SCREEN_WIDTH; x++) {
                for (int y = 0; y < SCREEN_HEIGHT; y++) {
                    grid[x, y].SetMode(0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            //Apply the new mode per each cell
            for (int x = 0; x < SCREEN_WIDTH; x++) {
                for (int y = 0; y < SCREEN_HEIGHT; y++) {
                    grid[x, y].SetMode(1);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            //Apply the new mode per each cell
            for (int x = 0; x < SCREEN_WIDTH; x++) {
                for (int y = 0; y < SCREEN_HEIGHT; y++) {
                    grid[x, y].SetMode(2);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            //Apply the new mode per each cell
            for (int x = 0; x < SCREEN_WIDTH; x++) {
                for (int y = 0; y < SCREEN_HEIGHT; y++) {
                    grid[x, y].SetMode(3);
                }
            }
        }
    }


    private void InitializeGrid() {
        for (int x = 0; x < SCREEN_WIDTH; x++) {
            for (int y = 0; y < SCREEN_HEIGHT; y++) {
                GameObject temp = new GameObject("Cell" + x + "_" + y);
                temp.AddComponent<CellController>().CreateCell(new int[] {x, y}, _cellSprite);
                grid[x, y] = temp.GetComponent<CellController>();
            }
        }
    }

    private void RandomizeGrid() {
        for (int x = 0; x < SCREEN_WIDTH; x++) {
            for (int y = 0; y < SCREEN_HEIGHT; y++) {
                grid[x, y].SetState(UnityEngine.Random.Range(0, 2), 0);
            }
        }
    }

    private void UpdateStates() {
        for (int x = 0; x < SCREEN_WIDTH; x++) {
            for (int y = 0; y < SCREEN_HEIGHT; y++) {
                CellController cell = grid[x, y];
                int cellState = cell.GetState();
                int state = cellState;
                int rule = cell.currentRl;
                int cellNeighbors = NeighborCount(x, y);

                if (cellState == 1 && cellNeighbors < 2) {
                    state = 0;
                    rule = 0;
                }
                if (cellState == 1 && (cellNeighbors == 2 || cellNeighbors == 3)) {
                    state = 1;
                    rule = 1;
                }
                if (cellState == 1 && cellNeighbors > 3) {
                    state = 0;
                    rule = 2;
                }
                if (cellState == 0 && cellNeighbors == 3) {
                    state = 1;
                    rule = 3;
                }

                states[x, y] = state;
                rules[x, y] = rule;
            }
        }

        for (int x = 0; x < SCREEN_WIDTH; x++) {
            for (int y = 0; y < SCREEN_HEIGHT; y++) {
                grid[x, y].SetState(states[x, y], rules[x, y]);
            }
        }
    }

    private int NeighborCount(int x, int y) {
        int neighborCount = 0;
        for(int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                int col = (x + i + SCREEN_WIDTH) % SCREEN_WIDTH;
                int row = (y + j + SCREEN_HEIGHT) % SCREEN_HEIGHT;

                neighborCount += grid[col, row].GetState();
            }
        }

        neighborCount -= grid[x, y].GetState();
        return neighborCount;
    }
}
