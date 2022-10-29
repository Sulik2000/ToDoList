using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ViewModels;
using Application.Views;
using Microsoft.Data.Sqlite;

namespace Application.Models
{
    public class DataBase
    {
        private SqliteConnection Connection;

        public List<TaskView> Tasks { get; set; }

        public DataBase(string connectionString)
        {
            Tasks = new List<TaskView>();
            Connection = new SqliteConnection(connectionString);
            Connection.Open();
            using (SqliteCommand command = Connection.CreateCommand())
            {
                command.CommandText =
                    $"CREATE TABLE IF NOT EXISTS Tasks(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, deadline TEXT, status INTEGER)";
                command.ExecuteNonQuery();
            }
            Connection.Close();
        }

        public void ChangeStatus(TaskView task)
        {
            Connection.Open();
            using (SqliteCommand command = Connection.CreateCommand())
            {
                command.CommandText =
                    $"UPDATE Tasks SET status = {(task.IsCompletedTask ? 1 : 0)} WHERE id = {task.ID}";
                command.ExecuteNonQuery();
            }
            Connection.Close();
        }

        public void ParseTasks()
        {
            Connection.Open();
            using (SqliteCommand command = Connection.CreateCommand())
            {
                command.CommandText =
                    @"SELECT * FROM Tasks";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Tasks.Add(new TaskView()
                    {
                        Height = 170,
                        TaskName = reader.GetString(1),
                        ID = reader.GetInt32(0),
                        DeadlineText = reader.GetString(2),
                        CompleteTaskCommand = new DelegateCommand(x => { return false; }, x => { }),
                        IsCompletedTask = reader.GetInt32(3) != 0,
                        Margin = new System.Windows.Thickness() { Bottom = 10, Left = 10, Right = 10, Top = 10 },
                        VerticalAlignment = System.Windows.VerticalAlignment.Top
                    });
                }
            }
            Connection.Close();
        }


        /// <summary>
        /// Add new task into database
        /// </summary>
        /// <param name="task">Instance of new task</param>
        /// <returns>ID in database of new task</returns>
        public int Insert(TaskView task)
        {
            Connection.Open();
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO Tasks(name, deadline, status) VALUES('{task.TaskName}', '{task.DeadlineText.Split(' ').First()}', {(task.IsCompletedTask ? 1 : 0)})";
            command.ExecuteNonQuery();
            command.CommandText =
                $"SELECT ID FROM Tasks WHERE name = '{task.TaskName}' AND deadline = '{task.DeadlineText.Split(' ').First()}' AND status = {(task.IsCompletedTask ? 1 : 0)}";
            SqliteDataReader read = command.ExecuteReader();
            read.Read();
            int ID = read.GetInt32(0);
            Connection.Close();
            return ID;
        }

        public void DeleteTask(TaskView task)
        {
            Connection.Open();
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText =
                $"DELETE FROM Tasks WHERE id = {task.ID}";
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}