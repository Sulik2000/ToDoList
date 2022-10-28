using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Application.Models;
using Application.Views;

namespace Application.ViewModels
{
    public class MainVM
    {
        // Add new task
        public ICommand NewTaskCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    return true;
                }, x =>
                {
                    NewTaskPage taskPage = new NewTaskPage();
                    taskPage.Show();
                    taskPage.Focus();

                    taskPage.AddTask = new DelegateCommand(x =>
                    {
                        return true;
                    }, x =>
                    {
                        AddTask(taskPage);
                    });
                });
            }
        }
/// <summary>
/// This function adds new TaskView into StackPanel
/// </summary>
/// <param name="taskPage">The instance of window where where created new task</param>
        private void AddTask(NewTaskPage taskPage)
        {
            TaskView newTaskView = new TaskView()
            {
                Width = 600,
                Height = 170,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness() { Top = 10, Bottom = 10, Left = 10, Right = 10 },
                TaskName = taskPage.NameTaskText.Text,
                DeadlineText = taskPage.DateCalendar.SelectedDate.ToString(),
                Reliable = taskPage.ReliableText.Text
            };


            taskPage.Close();


            newTaskView.ID = MainModel.DataBase.Insert(newTaskView);

            newTaskView.DeleteTaskCommand = new DelegateCommand(x =>
            {
                return true;
            }, x =>
            {
                TaskView taskView = x as TaskView;

                (_currentPage.Content as StackPanel).Children.RemoveAt(taskView.Position);
                MainModel.DataBase.DeleteTask(taskView);
            });

            int index = (_currentPage.Content as StackPanel).Children.Add(newTaskView);
            ((_currentPage.Content as StackPanel).Children[index] as TaskView).Position = index;

        }

        public ICommand DeleteTaskCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    return true;
                }, x =>
                {
                    TaskView taskView = x as TaskView;
                    DeleteTasksFromView(taskView);
                    MainModel.DataBase.DeleteTask(taskView);
                });
            }
        }

        private void DeleteTasksFromView(TaskView taskView)
        {
            (_currentPage.Content as StackPanel).Children.RemoveAt(taskView.Position);
            for (int i = 0; i < (_currentPage.Content as StackPanel).Children.Count; i++)
                ((_currentPage.Content as StackPanel).Children[i] as TaskView).Position = i;
        }

        private Page _currentPage = new Page();

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public MainVM()
        {
            _currentPage.Content = new StackPanel();
            MainModel.DataBase.ParseTasks();
            ParseTasks();
        }
        /// <summary>
        /// This function parses tasks from database in View
        /// </summary>
        private void ParseTasks()
        {
            foreach (TaskView i in MainModel.DataBase.Tasks)
            {
                i.DeleteTaskCommand = DeleteTaskCommand;
                int index = (_currentPage.Content as StackPanel).Children.Add(i);
                ((_currentPage.Content as StackPanel).Children[index] as TaskView).Position = index;
            }
        }
    }
}