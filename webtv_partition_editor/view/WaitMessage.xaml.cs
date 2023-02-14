using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

namespace webtv_partition_editor
{
    /// <summary>
    /// Interaction logic for WaitMessage.xaml
    /// </summary>
    public partial class WaitMessage : Window
    {
        private delegate void void_call();

        public bool? ShowDialog(Action do_action, bool auto_close = true)
        {
            bool? result = true;

            this.Cursor = Cursors.Wait;

            Thread action_thread = new Thread(new ThreadStart(delegate()
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, do_action);

                if (auto_close)
                {
                    this.close_window();
                }
            }));
            action_thread.Start();

            if (action_thread.ThreadState != ThreadState.Stopped)
            {
                result = base.ShowDialog();
            }

            return result;
        }

        public void close_window()
        {
            if (this.Dispatcher.CheckAccess() == false)
            {
                var cb = new void_call(this.close_window);

                this.Dispatcher.Invoke(cb);
            }
            else
            {
                this.Close();
            }
        }

        public WaitMessage(string wait_message)
        {
            InitializeComponent();

            this.wait_message.Content = wait_message;
        }
    }
}
