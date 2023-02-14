using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace webtv_partition_editor
{

    public enum PartitionState
    {
        HEALTHY = 0,
        OVERLAP_PREVIOUS = 1,
        OVERLAP_NEXT = 2,
        BAD_SIZE = 3,
        BROKEN = 4
    };

    public class WebTVPartition : INotifyPropertyChanged
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public PartitionType type { get; set; }
        public PartitionState state { get; set; }
        public ulong sector_start { get; set; }
        public ulong sector_length { get; set; }
        public WebTVDisk disk { get; set; }

        private ImDiskNamedPipeServer _server { get; set; }
        public ImDiskNamedPipeServer server 
        {
            get
            {
                return this._server;
            }

            set
            {
                if (this._server != value)
                {
                    this._server = value;

                    RaisePropertyChanged("server");
                }
            }
        }

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

        public void mount(string mount_point, bool read_only = false)
        { 
            if(!this.has_device_attached())
            {
                this.server = new ImDiskNamedPipeServer(this, mount_point, read_only, this.on_umount);
            }
            else
            {
                throw new ApplicationException("Partition is already mounted!");
            }
        }

        public void on_umount()
        {
            this.server = null;
        }

        public void unmount()
        {
            if (this.has_device_attached())
            {
                this.server.unmount();
            }
        }

        public bool has_device_attached()
        {
            return (this.server != null);
        }

        public WebTVPartition()
        { 
            id = Guid.NewGuid();
        }
    }
}
