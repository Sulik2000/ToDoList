using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Application.Views
{
    public partial class TaskView : UserControl
    {
        public int Position { get; set; }
        public int ID { get; set; }

        public ICommand DeleteTaskCommand { get; set; }

        public ICommand CompleteTaskCommand { get; set; }

        // Properties
        public bool IsCompletedTask
        {
            get { return (bool)GetValue(IsCompletedTaskProperty); }
            set { SetValue(IsCompletedTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCompletedTask.  This var which stores status of task
        public static readonly DependencyProperty IsCompletedTaskProperty =
            DependencyProperty.Register("IsCompletedTask", typeof(bool), typeof(TaskView), new PropertyMetadata(false));

        public string DeadlineText
        {
            get { return (string)GetValue(DeadlineTextProperty); }
            set { SetValue(DeadlineTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeadlineText.  This is var which stores date of deadline
        public static readonly DependencyProperty DeadlineTextProperty =
            DependencyProperty.Register("DeadlineText", typeof(string), typeof(TaskView), new PropertyMetadata(string.Empty));


        public string TaskName
        {
            get { return (string)GetValue(TaskNameProperty); }
            set { SetValue(TaskNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskName.  This is var which stores name of task
        public static readonly DependencyProperty TaskNameProperty =
            DependencyProperty.Register("TaskName", typeof(string), typeof(TaskView), new PropertyMetadata(string.Empty));


        public TaskView()
        {
            InitializeComponent();
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            if (!DeleteTaskCommand.CanExecute(this))
                return;
            DeleteTaskCommand.Execute(this);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!CompleteTaskCommand.CanExecute(this))
                return;
            CompleteTaskCommand.Execute(this);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!CompleteTaskCommand.CanExecute(this))
                return;
            CompleteTaskCommand.Execute(this);
        }
    }
}