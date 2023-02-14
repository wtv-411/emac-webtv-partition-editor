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
    /// Interaction logic for InitializeDisk.xaml
    /// </summary>
    public partial class DiskInitialize : Window
    {
        public DiskInitialize(WebTVDisk disk)
        {
            InitializeComponent();

            this.DataContext = new DiskInitializeViewModel(this, disk);
        }
    }
}
