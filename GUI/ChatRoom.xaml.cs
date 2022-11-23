using System;
using System.Windows;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatRoom : Window
    {

        public ChatRoom()
        {
            InitializeComponent();

            SendButton.MouseDown += SendButton_MouseDown;
            ExitButton.MouseDown += ExitButton_MouseDown;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Actions

        public void AddMessage(string str)
        {
            MessageLabel.Content += '\n' + str;

            // Scroll to bottom if near

            // Play sounds?

        }
        


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Interactions

        private void SendButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Text;
            InvokeSendMessage(new SendMessageEventArgs());
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InvokeLeave(new LeaveEventArgs());
            
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events


        // When the client attempts to send a message
        public event EventHandler<SendMessageEventArgs> SendMessage;
        protected virtual void InvokeSendMessage(SendMessageEventArgs e)
        {
            var handler = SendMessage;
            handler?.Invoke(this, e);
        }
        public class SendMessageEventArgs : EventArgs { }


        // When the client attempts to send a message
        public event EventHandler<LeaveEventArgs> Leave;
        protected virtual void InvokeLeave(LeaveEventArgs e)
        {
            var handler = Leave;
            handler?.Invoke(this, e);
        }
        public class LeaveEventArgs : EventArgs { }
    }
}
