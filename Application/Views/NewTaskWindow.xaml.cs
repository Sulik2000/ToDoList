using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Application.Views;

public partial class NewTaskPage : Window
{
    private ICommand _addTask;
    public ICommand AddTask
    {
        get { return _addTask; }
        set { _addTask = value; }
    }
    public NewTaskPage()
    {
        InitializeComponent();
    }

    private void AddTaskClick(object sender, RoutedEventArgs e)
    {
        AddTask.Execute(null);
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}