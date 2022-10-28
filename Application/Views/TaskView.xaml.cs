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



        public bool IsCompletedTask
        {
            get { return (bool)GetValue(IsCompletedTaskProperty); }
            set { SetValue(IsCompletedTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCompletedTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCompletedTaskProperty =
            DependencyProperty.Register("IsCompletedTask", typeof(bool), typeof(TaskView), new PropertyMetadata(false));



        public string Reliable
        {
            get { return (string)GetValue(ReliableProperty); }
            set { SetValue(ReliableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Reliable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReliableProperty =
            DependencyProperty.Register("Reliable", typeof(string), typeof(TaskView), new PropertyMetadata(string.Empty));


        public string DeadlineText
        {
            get { return (string)GetValue(DeadlineTextProperty); }
            set { SetValue(DeadlineTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeadlineText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeadlineTextProperty =
            DependencyProperty.Register("DeadlineText", typeof(string), typeof(TaskView), new PropertyMetadata(string.Empty));


        public string TaskName
        {
            get { return (string)GetValue(TaskNameProperty); }
            set { SetValue(TaskNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskNameProperty =
            DependencyProperty.Register("TaskName", typeof(string), typeof(TaskView), new PropertyMetadata(string.Empty));


        public TaskView()
        {
            InitializeComponent();
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            
        }
    }
}