using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace ConventionalCommitForm
{
    public class ConventionalCommitFormViewModel
        : BindableBase
        , IDataErrorInfo
    {
        private const string _commitHistoryFilename = "CommitHistory.xml";

        public static int MaxWidth => 72;

        private int _headerWidth;
        private string fullMessage;

        private CommitMessage message;

        public ConventionalCommitFormViewModel()
        {
            Types = new Types();
            Scopes = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            Footers = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            Messages = new ObservableCollection<CommitMessage>() ;

            WindowLoadedCommand = new DelegateCommand(Window_OnLoaded);
            WindowClosingCommand = new DelegateCommand(Window_OnClosing);
            CopyToClipboardCommand = new DelegateCommand(CopyToClipboard);

            var message = new CommitMessage();

            Messages.Add(message);

            Message = message;
        }

        public ICommand WindowClosingCommand { get; }
        public ICommand WindowLoadedCommand { get; }
        public ICommand CopyToClipboardCommand { get; }

        public ICollection<string> Types { get; }
        public ICollection<string> Scopes { get; }
        public ICollection<string> Footers { get; }
        public ICollection<CommitMessage> Messages { get; }

        public string Error => string.Empty;

        public int HeaderWidth
        {
            get => _headerWidth;
            private set => SetProperty(ref _headerWidth, value);
        }

        public string FullMessage
        {
            get => fullMessage;
            private set => SetProperty(ref fullMessage, value, OnFullMessageChanged);
        }

        public CommitMessage Message
        {
            get => message;
            set
            {
                if (message != null)
                {
                    message.PropertyChanged -= Message_PropertyChanged;
                }

                SetProperty(ref message, value);

                if (message != null)
                {
                    message.PropertyChanged += Message_PropertyChanged;
                }
            }
        }

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

        public void CopyToClipboard()
        {
            Clipboard.SetText(FullMessage);
        }

        private void Window_OnLoaded()
        {
            try
            {
                var previousHistory = Serializer.Load(_commitHistoryFilename);

                Update(previousHistory);

                Messages.AddRange(previousHistory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Load: {_commitHistoryFilename}");
            }
        }

        private void Window_OnClosing()
        {
            var limitedHistory = Messages
                .Take(100)
                .ToList();

            Serializer.Save(_commitHistoryFilename, limitedHistory);
        }

        private void Message_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            FullMessage = message.ToString();
        }

        private void OnFullMessageChanged()
        {
            HeaderWidth = FullMessage.CountWidth();
        }

        private void Update(IEnumerable<ICommitMessage> messages)
        {
            foreach (var message in messages)
                Update(message);
        }

        private void Update(ICommitMessage message)
        {
            Scopes.Add(message.Scope);
            Footers.Add(message.Footer);
        }
    }
}