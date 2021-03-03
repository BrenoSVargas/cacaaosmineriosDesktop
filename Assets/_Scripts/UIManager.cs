using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    GameObject _startPanelGO, _playPanelGO, _cardQuizGO, _answerPanelGO, _scoreGO, _answersParent, _winGO, _tipsContainer;
    Button _playBtn, _exit, _restart, _cardBtn, _playAgain;
    Image _quizImg;
    Text _textCard, _tittleQuiz, _wrongAnswer, _rightAnswer, _gameOverTxt;

    public Sprite frame1;

    private Button[] _answer = new Button[6];

    //Questions and Answers
    int answerInt = 7;
    int correctAnswer = 7;
    int correctAnswer2 = 7;
    int number = 0;
    Question quizUI;

    //Tips
    public Tips[] tips;
    private Button _getTips;
    private Button[] _tipsBtn = new Button[6];
    private Button selectedBtn;
    GameObject _panelTip;
    private Text _txtTip;
    private Image _imgTip;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += Load;
    }

    void Load(Scene scene, LoadSceneMode mode)
    {
        _startPanelGO = GameObject.Find("StartPanel");
        _playPanelGO = GameObject.Find("PlayPanel");
        _answerPanelGO = GameObject.Find("AnswerPanel");
        _answersParent = GameObject.Find("Answers");
        _cardQuizGO = GameObject.Find("CardsImg");
        _scoreGO = GameObject.Find("ScoreObj");
        _winGO = GameObject.Find("WinContainer");
        _panelTip = GameObject.Find("TipPanel");
        _quizImg = GameObject.Find("ImageQuiz").GetComponent<Image>();
        _tittleQuiz = GameObject.Find("QuizText").GetComponent<Text>();
        _wrongAnswer = GameObject.Find("WrongAnswer").GetComponent<Text>();
        _rightAnswer = GameObject.Find("RightAnswer").GetComponent<Text>();
        _textCard = GameObject.Find("TextCard").GetComponent<Text>();
        _gameOverTxt = GameObject.Find("GameOverText").GetComponent<Text>();



        _playBtn = GameObject.Find("PlayBtn").GetComponent<Button>();
        _cardBtn = GameObject.Find("Cards").GetComponent<Button>();
        _getTips = GameObject.Find("TipsBtn").GetComponent<Button>();
        _playAgain = GameObject.Find("PlayAgain").GetComponent<Button>();
        _exit = GameObject.Find("Exit").GetComponent<Button>();
        _restart = GameObject.Find("Restart").GetComponent<Button>();

        _answer[0] = GameObject.Find("Cobre").GetComponent<Button>();
        _answer[1] = GameObject.Find("Minerio").GetComponent<Button>();
        _answer[2] = GameObject.Find("Nenhum").GetComponent<Button>();
        _answer[3] = GameObject.Find("Manganes").GetComponent<Button>();
        _answer[4] = GameObject.Find("Niquel").GetComponent<Button>();
        _answer[5] = GameObject.Find("Fosfato").GetComponent<Button>();

        _tipsBtn[0] = GameObject.Find("TipMinerio").GetComponent<Button>();
        _tipsBtn[1] = GameObject.Find("TipCobre").GetComponent<Button>();
        _tipsBtn[2] = GameObject.Find("ReturnGame").GetComponent<Button>();
        _tipsBtn[3] = GameObject.Find("TipManganes").GetComponent<Button>();
        _tipsBtn[4] = GameObject.Find("TipNiquel").GetComponent<Button>();
        _tipsBtn[5] = GameObject.Find("TipFosfato").GetComponent<Button>();
        _txtTip = GameObject.Find("TextTip").GetComponent<Text>();
        _imgTip = GameObject.Find("TipImg").GetComponent<Image>();
        _tipsContainer =  GameObject.Find("Tips");

        _answer[0].onClick.AddListener(Cobre);
        _answer[1].onClick.AddListener(Minerio);
        _answer[2].onClick.AddListener(Nenhum);
        _answer[3].onClick.AddListener(Manganes);
        _answer[4].onClick.AddListener(Niquel);
        _answer[5].onClick.AddListener(Fosfato);

        _playBtn.onClick.AddListener(Play);
        _cardBtn.onClick.AddListener(CardPlay);
        _playAgain.onClick.AddListener(PlayAgain);
        _exit.onClick.AddListener(Exit);
        _restart.onClick.AddListener(PlayAgain);
        _getTips.onClick.AddListener(GetTips);
        _tipsBtn[2].onClick.AddListener(ReturnGame);

        _quizImg.enabled = false;
        _wrongAnswer.enabled = false;
        _rightAnswer.enabled = false;
        _gameOverTxt.enabled = false;
        _restart.interactable = false;
        _gameOverTxt.enabled = false;

        for (int i = 0; i < 6; i++)
        {
            if (i == 2)
                i++;

            _tipsBtn[i].onClick.AddListener(Tip);


        }

        _playAgain.gameObject.SetActive(false);
        _playPanelGO.SetActive(false);
        _answerPanelGO.SetActive(false);
        _cardQuizGO.SetActive(false);
        _getTips.gameObject.SetActive(false);
        _scoreGO.SetActive(false);
        _winGO.SetActive(false);
        _panelTip.SetActive(false);
        _cardBtn.interactable = false;



    }
    void Play()
    {
        _startPanelGO.SetActive(false);
        _playPanelGO.SetActive(true);
        _cardQuizGO.SetActive(true);
        _getTips.gameObject.SetActive(true);
        _getTips.enabled = true;
        _scoreGO.SetActive(true);
        _cardBtn.interactable = true;
        _restart.interactable = true;

        _cardBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, 0);
    }

    void PlayAgain()
    {
        _gameOverTxt.enabled = false;
        _playAgain.gameObject.SetActive(false);
        _answerPanelGO.SetActive(false);
        _startPanelGO.SetActive(false);
        _playPanelGO.SetActive(true);
        _cardQuizGO.SetActive(true);
        _getTips.enabled = true;
        _scoreGO.SetActive(true);
        _quizImg.enabled = false;
        _textCard.enabled = true;
        _cardBtn.GetComponent<Image>().enabled = true;
        _cardQuizGO.GetComponent<Image>().sprite = frame1;



        _cardBtn.interactable = true;

        QuestionManager.Instance.StartGame();
        ScoreManager.Instance.StartGame();
    }

    void CardPlay()
    {
        QuestionManager.Instance.StartQuestion(_cardBtn);
        _textCard.enabled = false;
        _playPanelGO.SetActive(false);
        _answerPanelGO.SetActive(true);
        _answersParent.SetActive(true);
        ActiveAnswers();
        _tittleQuiz.enabled = true;


    }

    public void DesableCards()
    {
        _cardBtn.GetComponent<Image>().enabled = false;
    }

    public void FormulateQuiz(Question quiz)
    {
        quizUI = quiz;
        _tittleQuiz.text = quiz.quiz;
        _cardQuizGO.GetComponent<Image>().sprite = quiz.frame;
        _quizImg.sprite = quiz.illustratation;
        _quizImg.GetComponent<RectTransform>().sizeDelta = quiz.sizeImg;
        _quizImg.enabled = true;
        correctAnswer = quiz.answer1;
        if (quiz.hasTwoAnswers)
        {
            correctAnswer2 = quiz.answer2;
        }
    }

    void CorrectQuestion(Button btn)
    {
        if (quizUI.hasTwoAnswers)
        {
            if (answerInt == correctAnswer || answerInt == correctAnswer2)
            {
                btn.interactable = false;
                number++;
                if (number >= 2)
                    StartCoroutine(RightAnswer());
                else
                {
                    _tittleQuiz.text = "Você acertou. Qual outro minério?";
                }
            }
            else
            {
                btn.interactable = false;
                _rightAnswer.enabled = false;
                _wrongAnswer.enabled = true;
                ScoreManager.Instance.ScoreSub();

            }
        }
        else
        {
            if (answerInt == correctAnswer)
            {
                btn.interactable = false;
                StartCoroutine(RightAnswer());
            }
            else
            {
                btn.interactable = false;
                _wrongAnswer.enabled = true;
                _rightAnswer.enabled = false;
                ScoreManager.Instance.ScoreSub();
            }
        }

        if (ScoreManager.Instance.score <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(GameOver());
        }
    }

    IEnumerator RightAnswer()
    {
        _wrongAnswer.enabled = false;
        _rightAnswer.enabled = true;
        yield return new WaitForSeconds(0.9f);
        number = 0;
        answerInt = 7;
        ActiveAnswers();
        QuestionManager.Instance.GetRandomQuestion();
    }

    IEnumerator GameOver()
    {
        _answersParent.SetActive(false);
        _tittleQuiz.enabled = false;
        _wrongAnswer.enabled = false;
        _getTips.interactable = false;

        yield return new WaitForSeconds(0.2f);


        _gameOverTxt.enabled = true;

        _playAgain.gameObject.SetActive(true);
    }

    public void Win()
    {
        _answersParent.SetActive(false);
        _tittleQuiz.enabled = false;
        _rightAnswer.enabled = false;

        _winGO.SetActive(true);
        _playAgain.gameObject.SetActive(true);
    }

    void ActiveAnswers()
    {
        for (int i = 0; i < 6; i++)
        {
            _answer[i].interactable = true;
        }
    }

    void Exit()
    {
        Application.Quit();
    }


    void Cobre() { answerInt = 0; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Minerio() { answerInt = 1; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Nenhum() { answerInt = 2; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Manganes() { answerInt = 3; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Niquel() { answerInt = 4; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }
    void Fosfato() { answerInt = 5; CorrectQuestion(_answer[answerInt].GetComponent<Button>()); }


    void GetTips()
    {
        _panelTip.SetActive(true);
        _answerPanelGO.SetActive(false);
        _imgTip.color = Color.clear;

    }

    void ReturnGame()
    {
        _panelTip.SetActive(false);
        _answerPanelGO.SetActive(true);
        _getTips.interactable = true;

    }

    void Tip()
    {
        if (selectedBtn)
        {
            selectedBtn.interactable = true;
        }
        for (int i = 0; i < 5; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == tips[i].nameTip)
            {
                _imgTip.color = Color.white;
                _txtTip.text = tips[i].txtTip;
                _txtTip.color = tips[i].colorTip;
                _imgTip.sprite = tips[i].imgTip;
                _tipsContainer.SetActive(false);



                Button n = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                selectedBtn = n;
                n.interactable = false;
                break;
            }
        }
    }



}
