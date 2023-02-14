using System;
using System.Runtime.Serialization;

namespace webtv_partition_editor
{

    [Serializable]
    public class ATAOperationException : Exception
    {
        public enum ATAError
        {
            NONE = 0,
            AMNF = 1,
            TKZNF = 2,
            ABRT = 4,
            MCR = 8,
            IDNF = 16,
            MC = 32,
            UN = 64,
            BBK = 128
        };

        ATAError ata_error_code;

        public ATAOperationException()
        {
            this.ata_error_code = ATAError.NONE;
        }

        public ATAOperationException(string message)
            : base(message)
        {
            this.ata_error_code = ATAError.NONE;
        }

        public ATAOperationException(string message, Exception inner)
            : base(message, inner)
        {
            this.ata_error_code = ATAError.NONE;
        }

        public ATAOperationException(ATAError ata_error_code)
        {
            this.ata_error_code = ata_error_code;
        }

        public ATAOperationException(ATAError ata_error_code, string message)
            : base(message)
        {
            this.ata_error_code = ata_error_code;
        }

        public ATAOperationException(ATAError ata_error_code, string message, Exception inner)
            : base(message, inner)
        {
            this.ata_error_code = ata_error_code;
        }

        public ATAOperationException(byte ata_error_code)
        {
            this.ata_error_code = (ATAError)ata_error_code;
        }

        public ATAOperationException(byte ata_error_code, string message)
            : base(message)
        {
            this.ata_error_code = (ATAError)ata_error_code;
        }

        public ATAOperationException(byte ata_error_code, string message, Exception inner)
            : base(message, inner)
        {
            this.ata_error_code = (ATAError)ata_error_code;
        }
    }
}