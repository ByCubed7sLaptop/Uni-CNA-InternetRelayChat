using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
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

            //SendButton.MouseDown += SendButton_MouseDown;
            SendButton.Click += SendButton_Click;
            ExitButton.Click += ExitButton_Click;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Actions

        public void AddMessage(string str)
        {
            Dispatcher.Invoke(() => {
                MessageContainer.Items.Add(str);
            });

            // Scroll to bottom if near

            // Play sounds?
            Console.Beep(150, 100);
        }

        public void UpdateMembers(List<string> member)
        {
            Dispatcher.Invoke(() => {
                MemberContainer.Items.Clear();
                foreach (string str in member) 
                    MemberContainer.Items.Add(str);
            });
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private void InternalAddButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("InternalAddButton_Click");
            throw new NotImplementedException();
        }

        private void InternalDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("InternalDeleteButton_Click");
            throw new NotImplementedException();
        }

        private void InternalEditButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("InternalEditButton_Click");
            throw new NotImplementedException();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Interactions

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Get text from richtext box
            TextRange textRange = new TextRange(
                MessageInput.Document.ContentStart, // TextPointer to the start of content in the RichTextBox.
                MessageInput.Document.ContentEnd // TextPointer to the end of content in the RichTextBox.
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            string message = textRange.Text.Trim();

            // Clear the contents
            MessageInput.Document = new FlowDocument();

            InvokeSendMessage(message);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            InvokeLeave();

            // Change page to main
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Events


        // When the client attempts to send a message
        public event EventHandler<SendMessageEventArgs>? OnSendMessage;
        protected virtual void InvokeSendMessage(string message)
        {
            var handler = OnSendMessage;
            handler?.Invoke(this, new SendMessageEventArgs(message));
        }
        public class SendMessageEventArgs : EventArgs {
            public string Message { get; set; }
            public SendMessageEventArgs(string message)
            {
                Message = message;
            }
        }


        // When the client attempts to send a message
        public event EventHandler<LeaveEventArgs>? OnLeave;
        protected virtual void InvokeLeave()
        {
            var handler = OnLeave;
            handler?.Invoke(this, new LeaveEventArgs());
        }
        public class LeaveEventArgs : EventArgs { }
    }
}
