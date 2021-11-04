using System.Windows;

namespace ConventionalCommitForm
{
    /// <summary>
    /// Interaction logic for ConventionalCommitFormView.xaml
    /// </summary>
    public partial class ConventionalCommitFormView : Window
    {
        private ConventionalCommitFormViewModel _conventionalCommitFormViewModel = new ConventionalCommitFormViewModel();

        public ConventionalCommitFormView()
        {
            InitializeComponent();

            DataContext = new ConventionalCommitFormViewModel();
        }
    }
}
