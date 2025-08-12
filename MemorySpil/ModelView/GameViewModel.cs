using MemorySpil.Model;
using MemorySpil.Command;
using MemorySpil.Interface;
using MemorySpil.Repository;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace MemorySpil.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly IGameStatsRepository _repository;
        private readonly DispatcherTimer _gameTimer;
        private DateTime _gameStartTime;
        private string _playerName = string.Empty;
        private int _moveCount = 0;
        private string _gameTime = "00:00";
        private bool _isGameCompleted = false;
        private Card? _firstSelectedCard = null;
        private Card? _secondSelectedCard = null;
        private bool _isProcessingMove = false;

        public GameViewModel()
        {
            _repository = new FileGameStatsRepository();
            Cards = new ObservableCollection<Card>();
            HighScores = new ObservableCollection<GameStat>();

            // Initialize commands
            FlipCardCommand = new RelayCommand(FlipCard, CanFlipCard);
            NewGameCommand = new RelayCommand(StartNewGame);
            SaveStatsCommand = new RelayCommand(SaveStats, CanSaveStats);
            ShowHighScoresCommand = new RelayCommand(LoadHighScores);

            // Initialize timer
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _gameTimer.Tick += UpdateGameTime;

            // Start a new game
            StartNewGame();
        }

        public ObservableCollection<Card> Cards { get; set; }
        public ObservableCollection<GameStat> HighScores { get; set; }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public int MoveCount
        {
            get => _moveCount;
            private set
            {
                _moveCount = value;
                OnPropertyChanged();
            }
        }

        public string GameTime
        {
            get => _gameTime;
            private set
            {
                _gameTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsGameCompleted
        {
            get => _isGameCompleted;
            private set
            {
                _isGameCompleted = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Card? FirstSelectedCard
        {
            get => _firstSelectedCard;
            private set
            {
                _firstSelectedCard = value;
                OnPropertyChanged();
            }
        }

        public Card? SecondSelectedCard
        {
            get => _secondSelectedCard;
            private set
            {
                _secondSelectedCard = value;
                OnPropertyChanged();
            }
        }

        public ICommand FlipCardCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand SaveStatsCommand { get; }
        public ICommand ShowHighScoresCommand { get; }

        private void FlipCard(object? parameter)
        {
            if (_isProcessingMove || parameter is not Card card) return;

            card.IsFlipped = true;
            OnPropertyChanged(nameof(Cards));

            if (FirstSelectedCard == null)
            {
                FirstSelectedCard = card;
            }
            else if (SecondSelectedCard == null)
            {
                SecondSelectedCard = card;
                MoveCount++;

                _isProcessingMove = true;

                // Process the match with a delay to show both cards
                Task.Delay(1000).ContinueWith(t =>
                {
                    App.Current.Dispatcher.Invoke(() => ProcessMatch());
                });
            }
        }

        private void ProcessMatch()
        {
            if (FirstSelectedCard?.Symbol == SecondSelectedCard?.Symbol)
            {
                // Match found
                FirstSelectedCard.IsMatched = true;
                SecondSelectedCard.IsMatched = true;

                // Check if game is completed
                if (Cards.All(c => c.IsMatched))
                {
                    _gameTimer.Stop();
                    IsGameCompleted = true;
                }
            }
            else
            {
                // No match - flip cards back
                FirstSelectedCard.IsFlipped = false;
                SecondSelectedCard.IsFlipped = false;
            }

            // Reset selection
            FirstSelectedCard = null;
            SecondSelectedCard = null;
            _isProcessingMove = false;

            OnPropertyChanged(nameof(Cards));
            CommandManager.InvalidateRequerySuggested();
        }

        private bool CanFlipCard(object? parameter)
        {
            if (_isProcessingMove || parameter is not Card card)
                return false;

            return !card.IsFlipped && !card.IsMatched && SecondSelectedCard == null;
        }

        private void StartNewGame()
        {
            // Reset game state
            MoveCount = 0;
            GameTime = "00:00";
            IsGameCompleted = false;
            FirstSelectedCard = null;
            SecondSelectedCard = null;
            _isProcessingMove = false;

            // Create 8 pairs of cards
            var symbols = new[] { "🐶", "🐱", "🐭", "🐹", "🐰", "🦊", "🐻", "🐼" };
            var cards = new List<Card>();

            for (int i = 0; i < symbols.Length; i++)
            {
                // Create pair
                cards.Add(new Card { Id = i * 2, Symbol = symbols[i], IsFlipped = false, IsMatched = false });
                cards.Add(new Card { Id = i * 2 + 1, Symbol = symbols[i], IsFlipped = false, IsMatched = false });
            }

            // Shuffle cards
            var random = new Random();
            cards = cards.OrderBy(c => random.Next()).ToList();

            Cards.Clear();
            foreach (var card in cards)
            {
                Cards.Add(card);
            }

            // Start timer
            _gameStartTime = DateTime.Now;
            _gameTimer.Start();
        }

        private void UpdateGameTime(object? sender, EventArgs e)
        {
            var elapsed = DateTime.Now - _gameStartTime;
            GameTime = elapsed.ToString(@"mm\:ss");
        }

        private void SaveStats(object? parameter)
        {
            if (IsGameCompleted && !string.IsNullOrWhiteSpace(PlayerName))
            {
                var gameStat = new GameStat(
                    PlayerName,
                    MoveCount,
                    DateTime.Now - _gameStartTime,
                    DateTime.Now
                );

                try
                {
                    _repository.SaveGameStat(gameStat);
                    LoadHighScores(); // Refresh high scores
                    IsGameCompleted = false;
                }
                catch (Exception ex)
                {
                    // Handle error - in a real app, show message to user
                    Console.WriteLine($"Failed to save stats: {ex.Message}");
                }
            }
            
        }

        private bool CanSaveStats(object? parameter)
        {
            return IsGameCompleted && !string.IsNullOrWhiteSpace(PlayerName);
        }

        private void LoadHighScores()
        {
            try
            {
                var scores = _repository.FindByScore(0); // Get all, sorted by score
                HighScores.Clear();

                foreach (var score in scores.Where(s => s != null).Take(10))
                {
                    HighScores.Add(score!);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load high scores: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}