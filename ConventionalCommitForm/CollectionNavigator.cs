using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConventionalCommitForm
{
    public class CollectionNavigator : Selector
    {
        static CollectionNavigator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionNavigator), new FrameworkPropertyMetadata(typeof(CollectionNavigator)));

            CommandManager.RegisterClassCommandBinding(typeof(CollectionNavigator), new CommandBinding(NavigationCommands.FirstPage, MoveFirst, CanMoveFirst));
            CommandManager.RegisterClassCommandBinding(typeof(CollectionNavigator), new CommandBinding(NavigationCommands.PreviousPage, MovePrevious, CanMovePrevious));
            CommandManager.RegisterClassCommandBinding(typeof(CollectionNavigator), new CommandBinding(NavigationCommands.NextPage, MoveNext, CanMoveNext));
            CommandManager.RegisterClassCommandBinding(typeof(CollectionNavigator), new CommandBinding(NavigationCommands.LastPage, MoveLast, CanMoveLast));
        }

        private static void CanMoveFirst(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private static void MoveFirst(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void CanMovePrevious(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private static void MovePrevious(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void CanMoveNext(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private static void MoveNext(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void CanMoveLast(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
        }

        private static void MoveLast(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
