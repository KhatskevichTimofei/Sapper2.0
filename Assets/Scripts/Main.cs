using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private int _healthCount = Menu.NumberOfAttempts;

    private Difficulty difficulty = Menu.Difficulty;

    private int _numberOfCellsInWidth;
    private int _numberOfCellsInHeight;
    private int _bombCount;
    private int _maxHealth;
    private int[,] _values;
    private int[,] _valuesSave;
    private int _resultOfTheGame;
    private int _goodCellsCount;

    private float _delayEndGame = 3;

    private bool _timerEnable;



    [SerializeField] private GameObject[] _cellPrefabs;
    [SerializeField] private List<GameObject> _health;
    [SerializeField] private GameObject _gameBoard;
    [SerializeField] private GameObject _numberOfBombsText;
    [SerializeField] private GameObject _heartsPrefab;
    [SerializeField] private GameObject _endGamePanel;

    private GameObject[,] _openedÑellPrefab;
    private GameObject[,] _closedCellPrefab;

    [SerializeField] private Sprite _proxiMinesSprite;
    [SerializeField] private Sprite _closedCellSignSprite;
    [SerializeField] private Sprite _closedCellSprite;

    [SerializeField] private Text _endGameResultText;

    [SerializeField] private AudioClip _mineExplosion;
    [SerializeField] private AudioSource _allAudio;

    private void Start()
    {
        if (difficulty == Difficulty.Easy)
        {
            _numberOfCellsInWidth = 16;
            _numberOfCellsInHeight = 16;
        }
        else if (difficulty == Difficulty.Normal)
        {
            _numberOfCellsInWidth = 32;
            _numberOfCellsInHeight = 16;
        }
        else if (difficulty == Difficulty.Hard)
        {
            _numberOfCellsInWidth = 32;
            _numberOfCellsInHeight = 32;
        }

        _bombCount = (_numberOfCellsInWidth * _numberOfCellsInHeight) / 10;
        int k = _bombCount;
        _maxHealth = _healthCount;
        NumberOfBombs(_bombCount);
        _openedÑellPrefab = new GameObject[_numberOfCellsInWidth, _numberOfCellsInHeight];
        _closedCellPrefab = new GameObject[_numberOfCellsInWidth, _numberOfCellsInHeight];

        _values = new int[_numberOfCellsInWidth, _numberOfCellsInHeight];
        _valuesSave = new int[_numberOfCellsInWidth, _numberOfCellsInHeight];


        if (_maxHealth < _health.Count)
        {
            _health.RemoveRange(0, _health.Count - _maxHealth);

        }
        for (int i = 0; i < _health.Count; i++)
        {
            _health[i].SetActive(true);
        }


        for (int i = 0; i < _numberOfCellsInWidth; i++)
        {
            for (int j = 0; j < _numberOfCellsInHeight; j++)
            {
                _values[i, j] = 0;
            }
        }


        while (k > 0)
        {
            int i = Random.Range(0, _numberOfCellsInWidth);
            int j = Random.Range(0, _numberOfCellsInHeight);
            if (_values[i, j] >= 0)
            {
                _values[i, j] = -10;
                if (i > 0) _values[i - 1, j]++;
                if (j > 0) _values[i, j - 1]++;
                if (i < _numberOfCellsInWidth - 1) _values[i + 1, j]++;
                if (j < _numberOfCellsInHeight - 1) _values[i, j + 1]++;
                if ((i > 0) && (j > 0)) _values[i - 1, j - 1]++;
                if ((i > 0) && (j < _numberOfCellsInHeight - 1)) _values[i - 1, j + 1]++;
                if ((i < _numberOfCellsInWidth - 1) && (j > 0)) _values[i + 1, j - 1]++;
                if ((i < _numberOfCellsInWidth - 1) && (j < _numberOfCellsInHeight - 1)) _values[i + 1, j + 1]++;

                k--;
            }

        }


        for (int i = 0; i < _numberOfCellsInWidth; i++)
        {
            for (int j = 0; j < _numberOfCellsInHeight; j++)
            {
                _openedÑellPrefab[i, j] = Instantiate(_cellPrefabs[0], Vector3.zero, Quaternion.identity, transform);
                RectTransform rtOpenedCellPrefab = _openedÑellPrefab[i, j].GetComponent<RectTransform>();
                rtOpenedCellPrefab.anchorMin = new Vector2(i * 1.0f / _numberOfCellsInWidth, j * 1.0f / _numberOfCellsInHeight);
                rtOpenedCellPrefab.anchorMax = new Vector2((i + 1) * 1.0f / _numberOfCellsInWidth, (j + 1) * 1.0f / _numberOfCellsInHeight);
                rtOpenedCellPrefab.offsetMin = Vector2.zero;
                rtOpenedCellPrefab.offsetMax = Vector2.zero;
                Text cellValue = rtOpenedCellPrefab.GetChild(0).GetComponent<Text>();
                Image proxiMines = ImageGiver(_openedÑellPrefab[i, j]);

                _closedCellPrefab[i, j] = Instantiate(_cellPrefabs[1], Vector3.zero, Quaternion.identity, transform);
                RectTransform rtClosedCellPrefab = _closedCellPrefab[i, j].GetComponent<RectTransform>();
                rtClosedCellPrefab.anchorMin = new Vector2(i * 1.0f / _numberOfCellsInWidth, j * 1.0f / _numberOfCellsInHeight);
                rtClosedCellPrefab.anchorMax = new Vector2((i + 1) * 1.0f / _numberOfCellsInWidth, (j + 1) * 1.0f / _numberOfCellsInHeight);
                rtClosedCellPrefab.offsetMin = Vector2.zero;
                rtClosedCellPrefab.offsetMax = Vector2.zero;
                ImageGiver(_closedCellPrefab[i, j]);


                if (_values[i, j] == 0)
                {
                    cellValue.text = "";
                    _goodCellsCount++;
                }
                else if (_values[i, j] < 0)
                {
                    proxiMines.sprite = _proxiMinesSprite;

                }
                else if (_values[i, j] > 0)
                {
                    cellValue.text = _values[i, j].ToString();
                    _goodCellsCount++;
                }
                _openedÑellPrefab[i, j].SetActive(false);
                _valuesSave[i, j] = _values[i, j];

            }
        }
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            EndGame(_resultOfTheGame = 0);
        }

        if (_timerEnable)
        {
            _delayEndGame -= Time.deltaTime;
            if (_delayEndGame <= 0)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }


    public void OnPointerDown(BaseEventData data)
    {
        RectTransform rtGameBoard = _gameBoard.GetComponent<RectTransform>();

        PointerEventData pntr = (PointerEventData)data;
        int i = (int)(pntr.position.x / rtGameBoard.rect.width * _numberOfCellsInWidth);
        int j = (int)(pntr.position.y / rtGameBoard.rect.height * _numberOfCellsInHeight);


        if (Input.GetMouseButton(1) && !(_openedÑellPrefab[i, j].activeSelf))
        {
            Image closedCellSign = ImageGiver(_closedCellPrefab[i, j]);

            if (closedCellSign.sprite == _closedCellSprite)
            {
                closedCellSign.sprite = _closedCellSignSprite;
                _values[i, j] = 100;
                NumberOfBombs(_bombCount -= 1);
            }

            else if (closedCellSign.sprite == _closedCellSignSprite)
            {
                closedCellSign.sprite = _closedCellSprite;
                _values[i, j] = _valuesSave[i, j];
                NumberOfBombs(_bombCount += 1);
            }
        }

        if (Input.GetMouseButtonDown(0) && _values[i, j] < 50)
        {
            OpenCell(i, j);
        }
    }


    private void OpenCell(int i, int j)
    {

        if (_openedÑellPrefab[i, j].activeSelf)
        {
            return;
        }

        _openedÑellPrefab[i, j].SetActive(true);

        if (ImageGiver(_openedÑellPrefab[i, j]).sprite == _proxiMinesSprite)
        {
            _allAudio.PlayOneShot(_mineExplosion, 0.1f);
            _bombCount -= 1;

            NumberOfBombs(_bombCount);

            if (_healthCount <= _health.Count)
            {
                _health[_health.Count - _healthCount].SetActive(false);
                _healthCount--;
                Debug.Log(_healthCount);
            }
            if (_healthCount == 0)
            {
                EndGame(_resultOfTheGame = -1);
            }
        }

        if (ImageGiver(_openedÑellPrefab[i, j]).sprite != _proxiMinesSprite)
        {
            _goodCellsCount--;
            Debug.Log(_goodCellsCount);
        }

        if (_goodCellsCount == 0)
        {
            EndGame(_resultOfTheGame = 1);
        }

        if (_values[i, j] > 50)
        {
            return;
        }
        else if (_values[i, j] == 0)
        {
            if (i > 0 && _values[i - 1, j] < 50) OpenCell(i - 1, j);
            if (j > 0 && _values[i, j - 1] < 50) OpenCell(i, j - 1);
            if (i < _numberOfCellsInWidth - 1 && _values[i + 1, j] < 50) OpenCell(i + 1, j);
            if (j < _numberOfCellsInHeight - 1 && _values[i, j + 1] < 50) OpenCell(i, j + 1);
            if ((i > 0) && (j > 0) && (_values[i - 1, j - 1] < 50)) OpenCell(i - 1, j - 1);
            if ((i > 0) && (j < _numberOfCellsInHeight - 1) && (_values[i - 1, j + 1] < 50)) OpenCell(i - 1, j + 1);
            if ((i < _numberOfCellsInWidth - 1) && (j > 0) && (_values[i + 1, j - 1] < 50)) OpenCell(i + 1, j - 1);
            if ((i < _numberOfCellsInWidth - 1) && (j < _numberOfCellsInHeight - 1) && (_values[i + 1, j + 1] < 50)) OpenCell(i + 1, j + 1);
        }
        _closedCellPrefab[i, j].SetActive(false);
    }


    private void NumberOfBombs(int bombCountClosed)
    {
        Text numberOfBombsText = _numberOfBombsText.GetComponent<Text>();
        numberOfBombsText.text = bombCountClosed.ToString();
    }


    private Image ImageGiver(GameObject prefbs) => (prefbs.GetComponent<Image>());


    private void EndGame(int resultGame)
    {
        if (resultGame == 1)
        {
            _timerEnable = true;
            _endGamePanel.SetActive(true);
            _endGameResultText.text = "You Win";
        }
        if (resultGame == 0)
        {
            SceneManager.LoadScene("Menu");
        }
        if (resultGame == -1)
        {
            _timerEnable = true;
            _endGamePanel.SetActive(true);
            _endGameResultText.text = "You Lose";
        }
    }
}
