using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Prism.Commands;

namespace ConventionalCommitForm
{
    public class ConventionalCommitFormViewModel
        : ConventionalCommitViewModel,
        IDataErrorInfo
    {
        private const string _commitHistoryFilename = "CommitHistory.xml";

        public static int MaxWidth => 72;
        public static IList<string> Types { get; }

        static ConventionalCommitFormViewModel()
        {
            Types = new[] { "fix", "feat​", "docs​", "refactor​", "test", "chore​", "build​", "ci​", "style", "perf​", "cleanup" };
        }

        private int _headerWidth;
        private string fullMessage;
        private ICollection<string> scopes;
        private ICollection<string> footers;
        private IList<ConventionalCommitViewModel> history;

        public ConventionalCommitFormViewModel()
        {
            Type = Types.First();
            scopes = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            footers = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            history = new ObservableCollection<ConventionalCommitViewModel>();

            WindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
            WindowClosingCommand = new DelegateCommand(OnWindowClosing);
            CopyToClipboardCommand = new DelegateCommand(CopyToClipboard);
            SetPreviousCommitMessageCommend = new DelegateCommand(() => NavigateHistory(-1), CanSetPreviousCommitMessage);
            SetNextCommitMessageCommand = new DelegateCommand(() => NavigateHistory(+1), CanSetNextCommitMessage);
        }

        public ICommand WindowClosingCommand { get; }
        public ICommand WindowLoadedCommand { get; }
        public ICommand CopyToClipboardCommand { get; }
        public DelegateCommand SetPreviousCommitMessageCommend { get; }
        public DelegateCommand SetNextCommitMessageCommand { get; }

        public ICollection<string> Scopes
        {
            get { return scopes; }
            private set => SetProperty(ref scopes, value);
        }

        public ICollection<string> Footers
        {
            get { return footers; }
            private set => SetProperty(ref footers, value);
        }

        public IList<ConventionalCommitViewModel> History
        {
            get { return history; }
            private set => SetProperty(ref history, value);
        }

        public int HeaderWidth
        {
            get => _headerWidth;
            private set => SetProperty(ref _headerWidth, value);
        }

        public string FullMessage
        {
            get => fullMessage;
            private set => SetProperty(ref fullMessage, value);
        }

        public string Error => "....";

        public string this[string name]
        {
            get
            {
                if (name == nameof(HeaderWidth) &&
                    HeaderWidth > MaxWidth)
                {
                    return $"Width > {MaxWidth}";
                }

                return string.Empty;
            }
        }

        private void OnWindowLoaded()
        {
            var history = Load<List<ConventionalCommitViewModel>>(_commitHistoryFilename);
            if (history.Any())
            {
                history.AddRange(history);

                var lastCommit = history.First();
                InitUiFromCommitMessage(lastCommit);
            }

            SetNextCommitMessageCommand.RaiseCanExecuteChanged();
            SetPreviousCommitMessageCommend.RaiseCanExecuteChanged();

            UpdateScopesAndFooters();
        }

        private void UpdateScopesAndFooters()
        {
            Scopes = Unique(History.Select(cm => cm.Scope));
            Footers = Unique(History.Select(cm => cm.Footer));
        }

        private void InitUiFromCommitMessage(ConventionalCommitViewModel commitMessage)
        {
            Type = commitMessage.Type;
            Scope = commitMessage.Scope;
            Description = commitMessage.Description;
            Body = commitMessage.Body;
            Footer = commitMessage.Footer;
        }

        public void CopyToClipboard()
        {
            Clipboard.SetText(FullMessage);

            AddCommitMessageToHistoryAtBeginning(this);
        }

        private void AddCommitMessageToHistoryAtBeginning(ConventionalCommitViewModel newCommitMessage)
        {
            if (history.Contains(newCommitMessage))
                return;

            history.Insert(0, newCommitMessage);

            SetNextCommitMessageCommand.RaiseCanExecuteChanged();
            SetPreviousCommitMessageCommend.RaiseCanExecuteChanged();
        }

        private void AddCommitMessageToHistoryUnique(ConventionalCommitViewModel newCommitMessage)
        {
            if (history.Contains(newCommitMessage))
                return;

            SetNextCommitMessageCommand.RaiseCanExecuteChanged();
            SetPreviousCommitMessageCommend.RaiseCanExecuteChanged();

            AddCommitMessageToHistoryAtBeginning(newCommitMessage);
        }

        private void Update()
        {
            FullMessage = this.ToString();
            HeaderWidth = FullMessage.CountWidth();
        }

        private bool CanSetNextCommitMessage()
        {
            return history.IndexOf(this) > 0;
        }

        private bool CanSetPreviousCommitMessage()
        {
            return history.IndexOf(this) < history.Count - 1;
        }

        private void NavigateHistory(int offset)
        {
            var currentCommitMessage = this;

            AddCommitMessageToHistoryUnique(currentCommitMessage);

            var currentIndex = history.IndexOf(currentCommitMessage);

            InitUiFromCommitMessage(history[currentIndex + offset]);

            SetNextCommitMessageCommand.RaiseCanExecuteChanged();
            SetPreviousCommitMessageCommend.RaiseCanExecuteChanged();
        }

        private void OnWindowClosing()
        {
            AddCommitMessageToHistoryAtBeginning(this);

            var limitedHistory = history
                .Take(100)
                .ToList();

            Save(_commitHistoryFilename, limitedHistory);
        }

        private static ICollection<string> Unique(IEnumerable<String> items) =>
            new SortedSet<string>(items, StringComparer.CurrentCultureIgnoreCase);

        private static void Save<T>(string path, T value)
        {
            using (var writer = new StreamWriter(path))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, value);
            }
        }

        private static T Load<T>(string path)
            where T : new()
        {
            if (!File.Exists(path))
                return new T();

            using (var reader = new FileStream(path, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}