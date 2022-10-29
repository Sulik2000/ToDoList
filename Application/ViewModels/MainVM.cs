using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Application.Models;
using Application.Views;

namespace Application.ViewModels
{
    public class MainVM
    {
        public ICommand SelectAllTasks
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    return true;
                }, x =>
                {
                    (_currentPage.Content as StackPanel).Children.Clear();
                    StackPanel newPage = new StackPanel();
                    allTasks.ForEach(x => { 
                        int index = newPage.Children.Add(x);
                        (newPage.Children[index] as TaskView).Position = index;
                    });
                    _currentPage.Content = newPage;
                    _currentTasks = Tasks.All;
                });
            }
        }

        public ICommand SelectCompletedTasks
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    return true;
                    //return _currentTasks != Tasks.Completed;
                }, x =>
                {
                    (_currentPage.Content as StackPanel).Children.Clear();
                    StackPanel newPage = new StackPanel();
                    completedTasks.ForEach(x =>
                    {
                        int index = newPage.Children.Add(x);
                        (newPage.Children[index] as TaskView).Position = index;
                    });
                    _currentPage.Content = newPage;
                    _currentTasks = Tasks.Completed;
                });
            }
        }

        private Tasks _currentTasks;

        public ICommand CompleteTaskCommand
        {
            get
            {
                return new DelegateCommand(x =>
                {
                    return true;
                }, x =>
                {
                    TaskView taskView = x as TaskView;
                    MainModel.DataBase.ChangeStatus(taskView);
                    if (taskView.IsCompletedTask)
                    {
                        completedTasks.Add(taskView);
                        return;
                    }
                    completedTasks.Remove(taskView);
                });
            }
        }
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
                DeadlineText = taskPage.DateCalendar.SelectedDate.ToString().Split(' ').First()
            };

            allTasks.Add(newTaskView);


            taskPage.Close();


            newTaskView.ID = MainModel.DataBase.Insert(newTaskView);

            newTaskView.DeleteTaskCommand = DeleteTaskCommand;
            newTaskView.CompleteTaskCommand = CompleteTaskCommand;
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
            if (taskView.IsCompletedTask)
                completedTasks.Remove(completedTasks.Where(x => { return x == taskView; }).First());
            allTasks.Remove(allTasks.Where(x => { return x == taskView; }).First());
            for (int i = 0; i < (_currentPage.Content as StackPanel).Children.Count; i++)
                ((_currentPage.Content as StackPanel).Children[i] as TaskView).Position = i;
        }

        private Page _currentPage = new Page();

        private List<TaskView> completedTasks = new List<TaskView>();
        private List<TaskView> allTasks = new List<TaskView>();

        public Page CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public MainVM()
        {
            MainModel.DataBase.ParseTasks();
            ParseTasks();
            StackPanel content = new StackPanel();
            allTasks.ForEach(x => { 
                int index = content.Children.Add(x);
                (content.Children[index] as TaskView).Position = index;
            });
            _currentPage.Content = content;
            _currentTasks = Tasks.All;
        }
        /// <summary>
        /// This function parses tasks from database in View
        /// </summary>
        private void ParseTasks()
        {
            foreach (TaskView i in MainModel.DataBase.Tasks)
            {
                i.DeleteTaskCommand = DeleteTaskCommand;
                i.CompleteTaskCommand = CompleteTaskCommand;
                if (i.IsCompletedTask)
                    completedTasks.Add(i);

                allTasks.Add(i);
            }
        }
    }
}