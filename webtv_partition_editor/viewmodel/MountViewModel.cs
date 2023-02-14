using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows;

namespace webtv_partition_editor
{
    class MountViewModel : INotifyPropertyChanged
    {
        public MountPartition mount_dialog { get; set; }
        public WebTVPartition part { get; set; }
        public StringCollection available_drive_letters { get; set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // Raise PropertyChanged event
        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private RelayCommand _cancel_command;
        public ICommand cancel_command
        {
            get
            {
                if (_cancel_command == null)
                {
                    _cancel_command = new RelayCommand(x => on_cancel_click(), x => true);
                }

                return _cancel_command;
            }
        }

        private RelayCommand _mount_command;
        public ICommand mount_command
        {
            get
            {
                if (_mount_command == null)
                {
                    _mount_command = new RelayCommand(x => on_mount_click(), x => true);
                }

                return _mount_command;
            }
        }

        public void close_window()
        {
            this.mount_dialog.Close();
        }

        public void on_cancel_click()
        {
            close_window();
        }

        public void on_mount_click()
        {
            try
            {
                if(this.part.type == PartitionType.FAT16_DVR)
                {
                    MessageBox.Show("You are trying to mount a FAT16 'DVR' partition.  This partition is usually encrypted and this tool does NOT unencrypt the file stream.  If Windows doesn't properly detect the file system, then this partition is probably encrypted.");
                }

                this.part.mount(this.mount_dialog.mount_letter.SelectedItem.ToString() + ":", (bool)this.mount_dialog.mount_read_only.IsChecked);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message);
            }
            finally
            {
                close_window();
            }
        }

        public MountViewModel(MountPartition mount_dialog, WebTVPartition part)
        {
            this.mount_dialog = mount_dialog;
            this.part = part;
            this.available_drive_letters = (new AvailableDriveLetters()).get_available_drive_letters();
        }
    }
}
