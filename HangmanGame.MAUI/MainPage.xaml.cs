using System.ComponentModel;
using System.Diagnostics;

namespace HangmanGame.MAUI;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    #region UI Properties

    public string Spotlight
    {
        get => spotlight; set
        {
            spotlight = value;
            OnPropertyChanged();
        }
    }
    public string GameStatus
    {
        get => gameStatus;
        set
        {
            gameStatus = value;
            OnPropertyChanged();
        }
    }

    public List<char> Letters
    {
        get => letters;
        set
        {
            letters = value;
            OnPropertyChanged();
        }
    }
    public string Message
    {
        get => message;
        set
        {
            message = value;
            OnPropertyChanged();
        }
    }

    public string CurrentImage
    {
        get => currentImage;
        set
        {
            currentImage = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Fields
    List<string> words = new List<string>()
    {
        "PYTHON",
        "JAVASCRIPT",
        "MAUI",
        "CSHARP",
        "MONGODB",
        "SQL",
        "XAML",
        "WORD",
        "EXCEL",
        "POWERPOINT",
        "CODE",
        "HOTRELOAD",
    };
    List<char> guessed = new List<char>();
    string answer = "";
    private string spotlight;
    private List<char> letters = new List<char>();
    private string message;
    private string gameStatus;
    int mistakes = 0;
    int maxWrong = 6;
    private string currentImage = "img0.jpg";

    #endregion
    public MainPage()
    {
        InitializeComponent();
        Letters.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        BindingContext = this;
        PickWord();
        CalculateWord(answer, guessed);
    }


    #region Game Engine
    private void PickWord()
    {
        answer = words[new Random().Next(0, words.Count)];
        Debug.WriteLine(answer);
    }

    private void CalculateWord(string answer, List<char> guessed)
    {
        var temp = answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_'));
        Spotlight = string.Join(' ', temp);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn != null)
        {
            var letter = btn.Text;
            btn.BackgroundColor = Colors.Grey;
            HandleGuess(letter[0]);
        }
    }

    private void HandleGuess(char letter)
    {
        if (guessed.IndexOf(letter) == -1)
        {
            guessed.Add(letter);
        }
        if (answer.IndexOf(letter) >= 0)
        {
            CalculateWord(answer, guessed);
            CheckIfGameWon();
        }
        else if (answer.IndexOf(letter) == -1)
        {
            mistakes++;
            UpdateStatus();
            CheckIfGameLost();
            CurrentImage = $"img{mistakes}.jpg";
        }
    }

    private void UpdateStatus()
    {
        GameStatus = $"Errors : {mistakes} of {maxWrong} ";
    }

    private void CheckIfGameWon()
    {
        if (Spotlight.Replace(" ", "") == answer)
        {
            Message = "You Win !";
            DisabledLetters();
        }
    }

    private void CheckIfGameLost()
    {
        if (mistakes == maxWrong)
        {
            Message = "You Lost";
            DisabledLetters();
        }
    }

    private void DisabledLetters()
    {
        foreach (var children in LettersContainer.Children)
        {
            var btn = children as Button;
            if (btn != null)
            {
                btn.IsEnabled = false;
                btn.BackgroundColor = Colors.Grey;
            }
        }
    }
    private void EnabledLetters()
    {
        foreach (var children in LettersContainer.Children)
        {
            var btn = children as Button;
            if (btn != null)
            {
                btn.IsEnabled = true;
                btn.BackgroundColor = Color.FromArgb("#27374D");
            }
        }
    }
    private void btnReset_Clicked(object sender, EventArgs e)
    {
        mistakes = 0;
        guessed = new List<char>();
        CurrentImage = "img0.jpg";
        PickWord();
        CalculateWord(answer, guessed);
        Message = "";
        UpdateStatus();
        EnabledLetters();
    }
    #endregion


}

