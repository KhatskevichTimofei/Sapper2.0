using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    int xCount = 16;
    int yCount = 16;
    int bombCount;
    int heathsCount;
    int[,] values;
    int[,] valuesSave;

    [SerializeField] private GameObject[] cellPrefab;
    [SerializeField] private GameObject[] heaths;
    [SerializeField] private GameObject gameBoard;
    [SerializeField] private GameObject numberOfBombs;
    
    private GameObject[,] openedÑellPrefab;
    private GameObject[,] closedCellPrefab;

    [SerializeField] private Sprite proxiMinesSprite;
    [SerializeField] private Sprite closedCellSignSprite;
    [SerializeField] private Sprite closedCellSprite;

    [SerializeField] private AudioClip mineExplosionClip;
    [SerializeField] private AudioSource allAudio;

    private void Start()
    {
        bombCount = (xCount * yCount) / 10;
        int k = bombCount;
        heathsCount = 3;

        NumberOfBombs(bombCount);

        openedÑellPrefab = new GameObject[xCount, yCount];
        closedCellPrefab = new GameObject[xCount, yCount];

        values = new int[xCount, yCount];
        valuesSave = new int[xCount, yCount];

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {
                values[i, j] = 0;
            }
        }
        while (k > 0)
        {
            int i = Random.Range(0, xCount);
            int j = Random.Range(0, yCount);
            if (values[i, j] >= 0)
            {
                values[i, j] = -10;
                if (i > 0) values[i - 1, j]++;
                if (j > 0) values[i, j - 1]++;
                if (i < xCount - 1) values[i + 1, j]++;
                if (j < yCount - 1) values[i, j + 1]++;
                if ((i > 0) && (j > 0)) values[i - 1, j - 1]++;
                if ((i > 0) && (j < yCount - 1)) values[i - 1, j + 1]++;
                if ((i < xCount - 1) && (j > 0)) values[i + 1, j - 1]++;
                if ((i < xCount - 1) && (j < yCount - 1)) values[i + 1, j + 1]++;

                k--;
            }

        }

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {
                openedÑellPrefab[i, j] = Instantiate(cellPrefab[0], Vector3.zero, Quaternion.identity, transform);
                RectTransform rtOpenedCellPrefab = openedÑellPrefab[i, j].GetComponent<RectTransform>();
                rtOpenedCellPrefab.anchorMin = new Vector2(i * 1.0f / xCount, j * 1.0f / yCount);
                rtOpenedCellPrefab.anchorMax = new Vector2((i + 1) * 1.0f / xCount, (j + 1) * 1.0f / yCount);
                rtOpenedCellPrefab.offsetMin = Vector2.zero;
                rtOpenedCellPrefab.offsetMax = Vector2.zero;
                Text cellValue = rtOpenedCellPrefab.GetChild(0).GetComponent<Text>();
                Image proxiMines = ImageGiver(openedÑellPrefab[i, j], i, j);

                closedCellPrefab[i, j] = Instantiate(cellPrefab[1], Vector3.zero, Quaternion.identity, transform);
                RectTransform rtClosedCellPrefab = closedCellPrefab[i, j].GetComponent<RectTransform>();
                rtClosedCellPrefab.anchorMin = new Vector2(i * 1.0f / xCount, j * 1.0f / yCount);
                rtClosedCellPrefab.anchorMax = new Vector2((i + 1) * 1.0f / xCount, (j + 1) * 1.0f / yCount);
                rtClosedCellPrefab.offsetMin = Vector2.zero;
                rtClosedCellPrefab.offsetMax = Vector2.zero;
                ImageGiver(closedCellPrefab[i, j], i, j);

                if (values[i, j] == 0)
                {
                    cellValue.text = "";
                }

                if (values[i, j] < 0)
                {
                    proxiMines.sprite = proxiMinesSprite;

                }
                if (values[i, j] > 0)
                {
                    cellValue.text = values[i, j].ToString();
                }
                openedÑellPrefab[i, j].SetActive(false);
                valuesSave[i, j] = values[i, j];

            }
        }
    }


    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void OnPointerDown(BaseEventData data)
    {
        RectTransform rtGameBoard = gameBoard.GetComponent<RectTransform>();

        PointerEventData pntr = (PointerEventData)data;
        int i = (int)(pntr.position.x / rtGameBoard.rect.width * xCount);
        int j = (int)(pntr.position.y / rtGameBoard.rect.height * yCount);
        if (Input.GetMouseButton(1) && !(openedÑellPrefab[i, j].activeSelf))
        {
            Image closedCellSign = ImageGiver(closedCellPrefab[i, j], i, j);

            if (closedCellSign.sprite == closedCellSprite)
            {
                closedCellSign.sprite = closedCellSignSprite;
                values[i, j] = 100;
                NumberOfBombs(bombCount -= 1);
            }
            else if (closedCellSign.sprite == closedCellSignSprite)
            {
                closedCellSign.sprite = closedCellSprite;
                values[i, j] = valuesSave[i, j];
                NumberOfBombs(bombCount += 1);
            }
        }
        if (Input.GetMouseButtonDown(0) && values[i, j] < 50)
        {
            OpenCell(i, j);
        }
    }


    private void OpenCell(int i, int j)
    {

        if (openedÑellPrefab[i, j].activeSelf)
        {
            return;
        }
        openedÑellPrefab[i, j].SetActive(true);
        if (ImageGiver(openedÑellPrefab[i, j], i, j).sprite == proxiMinesSprite)
        {
            allAudio.PlayOneShot(mineExplosionClip, 0.1f);
            bombCount -= 1;
            NumberOfBombs(bombCount);
            if (heathsCount > 0)
            {
                heaths[heaths.Length - heathsCount].SetActive(false);
                heathsCount--;
            }
            if (heathsCount == 0)
            {
                Application.Quit();
            }
            Debug.Log(bombCount);
        }
        if (values[i, j] > 50)
        {
            return;
        }
        closedCellPrefab[i, j].SetActive(false);
        if (values[i, j] == 0)
        {
            if (i > 0 && values[i - 1, j] < 50) OpenCell(i - 1, j);
            if (j > 0 && values[i, j - 1] < 50) OpenCell(i, j - 1);
            if (i < xCount - 1 && values[i + 1, j] < 50) OpenCell(i + 1, j);
            if (j < yCount - 1 && values[i, j + 1] < 50) OpenCell(i, j + 1);
            if ((i > 0) && (j > 0) && (values[i - 1, j - 1] < 50)) OpenCell(i - 1, j - 1);
            if ((i > 0) && (j < yCount - 1) && (values[i - 1, j + 1] < 50)) OpenCell(i - 1, j + 1);
            if ((i < xCount - 1) && (j > 0) && (values[i + 1, j - 1] < 50)) OpenCell(i + 1, j - 1);
            if ((i < xCount - 1) && (j < yCount - 1) && (values[i + 1, j + 1] < 50)) OpenCell(i + 1, j + 1);
        }


    }


    private void NumberOfBombs(int bombCountClosed)
    {
        Text numberOfBombsText = numberOfBombs.GetComponent<Text>();
        numberOfBombsText.text = bombCountClosed.ToString();
    }

    private Image ImageGiver(GameObject prefbs, int i, int j)
    {
        return (prefbs.GetComponent<Image>());
    }

}