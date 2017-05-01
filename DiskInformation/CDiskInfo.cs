using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskInformation
{
    class CDiskInfo
    {
        private string m_DiskName = "";
        private string m_DiskLabel = "";
        private long m_DiskSize = 0;
        private long m_DiskFreeSize = 0;
        private DriveType m_DiskType ;

        /// <summary>
        /// 磁盘盘符
        /// </summary>
        public string DiskName
        {
            get { return this.m_DiskName; }
            set { this.m_DiskName = value; }
        }
        /// <summary>
        /// 磁盘标签
        /// </summary>
        public string DiskLabel
        {
            get { return this.m_DiskLabel; }
            set { this.m_DiskLabel = value; }
        }
        /// <summary>
        /// 磁盘大小
        /// </summary>
        public long DiskSize
        {
            get { return this.m_DiskSize; }
            set { this.m_DiskSize = value; }
        }
        /// <summary>
        /// 磁盘剩余大小
        /// </summary>
        public long DiskFreeSize
        {
            get { return this.m_DiskFreeSize; }
            set { this.m_DiskFreeSize = value; }
        }
        /// <summary>
        /// 磁盘类型
        /// </summary>
        public DriveType DiskType
        {
            get { return this.m_DiskType; }
            set { this.m_DiskType = value; }
        }

    }
}
