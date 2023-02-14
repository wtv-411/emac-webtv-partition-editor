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

namespace webtv_partition_editor
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class NoReturnProgressWindow : Window
    {
        private delegate void progress_owner_call(float percent_value, string message = "");

        public void set_progress(float percent_value, string message = "")
        {
            if (this.Dispatcher.CheckAccess() == false)
            {
                var cb = new progress_owner_call(set_progress);

                this.Dispatcher.BeginInvoke(cb, percent_value, message);
            }
            else
            {
                this.progress_bar.Value = percent_value;
                this.progress_message.Text = message;
            }
        }

        public NoReturnProgressWindow(int initial_percent_value, string initial_message = "")
        {
            InitializeComponent();

            this.set_progress(initial_percent_value, initial_message);
        }
    }
}
