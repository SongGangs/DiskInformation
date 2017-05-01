using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiskInformation
{
    /// <summary>
    /// SG 2017/1/15
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //计算机系统的引用 关于光驱的
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

        /// <summary>
        /// 系统变量 计算机的一些指示符
        /// </summary>
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;    //如果m.Msg的值为0x8000那么表示有U盘插入
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0X8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;


        DataGridView dataGridView = null;


        /// <summary>
        /// 重载
        /// 监视Windows消息
        /// WndProc(ref Message m)是Control类中的方法，用来处理Windows消息
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case WM_DEVICECHANGE:
                            break;
                        case DBT_DEVICEARRIVAL:         //U盘插入
                            DriveInfo[] s= DriveInfo.GetDrives();
                            foreach (DriveInfo i in s)
                            {
                                if (i.DriveType == DriveType.Removable)
                                {
                                    Form1_Load(null, null);
                                    MessageBox.Show("发现U盘或移动硬盘,盘符为："+i.Name.Remove(2));
                                    break;
                                }
                            }
                            break;
                        case DBT_CONFIGCHANGECANCELED:
                            break;
                        case DBT_CONFIGCHANGED:
                            break;
                        case DBT_CUSTOMEVENT:
                            break;
                        case DBT_DEVICEQUERYREMOVE:
                            break;
                        case DBT_DEVICEQUERYREMOVEFAILED:
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:   //U盘卸载
                            Form1_Load(null, null);
                            MessageBox.Show("U盘已拔出");
                            break;
                        case DBT_DEVICEREMOVEPENDING:
                            break;
                        case DBT_DEVNODES_CHANGED:
                            break;
                        case DBT_QUERYCHANGECONFIG:
                            break;
                        case DBT_USERDEFINED:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            base.WndProc(ref m);//将系统消息传递自父类的WndProc
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "此电脑";
            this.Controls.Clear();
            DriveInfo[] s = DriveInfo.GetDrives();
            int k = 0;
            foreach (DriveInfo i in s)
            {
                CDiskInfo d=new CDiskInfo();
                //判断DriveType若是 CD 则不赋予大小 
                if (i.DriveType!=DriveType.CDRom)
                {
                    d.DiskFreeSize = i.TotalFreeSpace;
                    d.DiskSize = i.TotalSize;
                    d.DiskLabel = i.VolumeLabel;
                }else
                {
                    d.DiskLabel = "DVD RW驱动器";
                }
                d.DiskName = i.Name.Remove(2);
                d.DiskType = i.DriveType;
                if (string.IsNullOrEmpty(d.DiskLabel))
                {
                    d.DiskLabel = "本地磁盘";
                }
                CreatContorl(d,k);
                k++;
               
            }
        }

        /// <summary>
        /// 动态生成硬盘信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="flag"></param>
        private void CreatContorl(CDiskInfo info,int flag)
        {
            if (info.DiskType != DriveType.CDRom)
            {
                // 
                // pictureBox1
                // 
                PictureBox pictureBox = new PictureBox();
                string url = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\","Image\\Wen.jpg");
                Image image = new Bitmap(url);
                pictureBox.BackgroundImage = image;
                pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                pictureBox.Dock = System.Windows.Forms.DockStyle.Left;
                pictureBox.Location = new System.Drawing.Point(0, 0);
                pictureBox.Name = "pictureBox" + flag;
                pictureBox.Size = new System.Drawing.Size(65, 58);
                pictureBox.TabStop = false;
                pictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick);
                // 
                // label1
                // 
                Label label1 = new Label();
                label1.Dock = System.Windows.Forms.DockStyle.Top;
                label1.Location = new System.Drawing.Point(65, 0);
                label1.Name = "diskLabel" + flag;
                label1.Size = new System.Drawing.Size(183, 20);
                label1.Text = info.DiskLabel + "（" + info.DiskName + "）";
                label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                label1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick);
                // 
                // progressBar
                // 
                ProgressBar progressBar = new ProgressBar();
                progressBar.Dock = System.Windows.Forms.DockStyle.Top;
                progressBar.Location = new System.Drawing.Point(65, 20);
                progressBar.Name = "progressBar" + flag;
                progressBar.Size = new System.Drawing.Size(183, 20);
                progressBar.Value = (100 - int.Parse(((double) info.DiskFreeSize/(double) info.DiskSize*100).ToString("0")));
                progressBar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick);
                // 
                // label3
                // 
                Label label3 = new Label();
                label3.Dock = System.Windows.Forms.DockStyle.Top;
                label3.Location = new System.Drawing.Point(65, 40);
                label3.Name = "label" + flag;
                label3.Size = new System.Drawing.Size(183, 20);
                label3.Text = (info.DiskFreeSize/1024.0/1024/1024).ToString("0.0") + "GB可用    共" + (info.DiskSize/1024.0/1024/1024).ToString("0.0") + "GB";
                label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                label3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick);
                // 
                // panel
                // 
                Panel panel = new Panel();
                panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                panel.Controls.Add(label3);
                panel.Controls.Add(progressBar);
                panel.Controls.Add(label1);
                panel.Controls.Add(pictureBox);
                panel.Location = new System.Drawing.Point(0, 80*flag);
                panel.Name = "panel" + flag;
                panel.Size = new System.Drawing.Size(250, 60);
                panel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick);
                this.Controls.Add(panel);
            }
            else
            {
                // 
                // pictureBox1
                // 
                PictureBox pictureBox = new PictureBox();
                string url = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Image\\Wen.jpg");
                Image image = new Bitmap(url);
                pictureBox.BackgroundImage = image;
                pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                pictureBox.Dock = System.Windows.Forms.DockStyle.Left;
                pictureBox.Location = new System.Drawing.Point(0, 0);
                pictureBox.Name = "pictureBox" + flag;
                pictureBox.Size = new System.Drawing.Size(65, 58);
                pictureBox.TabStop = false;
                pictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick1);
                // 
                // label1
                // 
                Label label1 = new Label();
                label1.Location = new System.Drawing.Point(65, 20);
                label1.Name = "diskLabel" + flag;
                label1.Size = new System.Drawing.Size(183, 20);
                label1.Text = info.DiskLabel + "（" + info.DiskName + "）";
                label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                label1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick1);
                // 
                // panel
                // 
                Panel panel = new Panel();
                panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                panel.Controls.Add(label1);
                panel.Controls.Add(pictureBox);
                panel.Location = new System.Drawing.Point(0, 80 * flag);
                panel.Name = "panel"+flag;
                panel.Size = new System.Drawing.Size(250, 60);
                panel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseDoubleClick1);
                this.Controls.Add(panel);
            }
        }

        /// <summary>
        /// 硬盘鼠标双击
        /// 打开硬盘内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string flag="";
            //this.Controls.Clear();
            if (sender.GetType().Name == "Label")
            {
                Label l=sender as Label;
                flag=l.Name.Remove(0, l.Name.Count() - 1);
            }
            else if (sender.GetType().Name == "PictureBox")
            {
                PictureBox l = sender as PictureBox;
                flag = l.Name.Remove(0, l.Name.Count() - 1);
            }
            else if (sender.GetType().Name == "ProgressBar")
            {
                ProgressBar l = sender as ProgressBar;
                flag = l.Name.Remove(0, l.Name.Count() - 1);
            }
            else if (sender.GetType().Name == "Panel")
            {
                Panel l = sender as Panel;
                flag = l.Name.Remove(0, l.Name.Count() - 1);
            }
            foreach (Control c in Application.OpenForms[0].Controls.Find("diskLabel" + flag, true))
            {
                this.Text = c.Text;
                string f = c.Text.Remove(c.Text.Length - 1);
                f = f.Remove(0, f.Length - 2);
                DirectoryInfo di = new DirectoryInfo(f+"\\");
                FileInfo[] fileInfos = di.GetFiles();
                DirectoryInfo[] directoryInfos = di.GetDirectories();
                //FileSystemInfo[] file = di.GetFileSystemInfos();
                this.Controls.Clear();
                CreatContorl(directoryInfos,fileInfos);
            }

        }

        /// <summary>
        /// 光驱的打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseDoubleClick1(object sender, MouseEventArgs e)
        {
            mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);//打开光区
            MessageBox.Show("请插入光盘");
        }

        /// <summary>
        /// 根据文件夹数组和文件数组生成DataGridView
        /// </summary>
        /// <param name="directories"></param>
        /// <param name="files"></param>
        private void CreatContorl(DirectoryInfo[] directories, FileInfo[] files)
        {
            dataGridView = new System.Windows.Forms.DataGridView();
            DataGridViewTextBoxColumn DataGrid_FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn DataGrid_ChangeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn DataGrid_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn DataGrid_Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            // 
            // listView1
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            DataGrid_FileName,
            DataGrid_ChangeDate,
            DataGrid_Type,
            DataGrid_Size});
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(0, 169);
            dataGridView.Name = "dataGridView1";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowTemplate.Height = 27;
            dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(RowDoubleClick);


            // 
            // DataGrid_FileName
            // 
            DataGrid_FileName.HeaderText = "名称";
            DataGrid_FileName.Name = "FileName";
            DataGrid_FileName.Width = 270;
            DataGrid_FileName.ReadOnly = true;

            // 
            // DataGrid_ChangeDate
            // 
            DataGrid_ChangeDate.HeaderText = "修改日期";
            DataGrid_ChangeDate.Name = "ChangeDate";
            DataGrid_ChangeDate.Width = 150;
            DataGrid_ChangeDate.ReadOnly = true;

            // 
            // DataGrid_Type
            // 
            DataGrid_Type.HeaderText = "类型";
            DataGrid_Type.Name = "Type";
            DataGrid_Type.Width = 135;
            DataGrid_Type.ReadOnly = true;

            // 
            // DataGrid_Size
            // 
            DataGrid_Size.HeaderText = "大小";
            DataGrid_Size.Name = "Size";
            DataGrid_Size.Width = 100;
            DataGrid_Type.ReadOnly = true;


            for (int i = 0; i < directories.Count(); i++)
            {
                dataGridView.Rows.Add(1);
                dataGridView.Rows[dataGridView.RowCount - 1].Cells["FileName"].Value = directories[i].Name;
                dataGridView.Rows[dataGridView.RowCount - 1].Cells["ChangeDate"].Value = directories[i].LastWriteTime.ToString("yyyy/MM//dd hh:mm:ss");
                dataGridView.Rows[dataGridView.RowCount - 1].Cells["Type"].Value = "文件夹";
            }
            for (int i = 0; i < files.Count(); i++)
            {
                dataGridView.Rows.Add(1);
                dataGridView.Rows[dataGridView.RowCount - 1].Cells["FileName"].Value = files[i].Name;
                dataGridView.Rows[dataGridView.RowCount - 1].Cells["ChangeDate"].Value = files[i].LastWriteTime.ToString("yyyy/MM//dd hh:mm:ss");
                if (files[i].Extension == ".txt")
                {
                    dataGridView.Rows[dataGridView.RowCount - 1].Cells["Type"].Value = "文本文档";
                }
                else if (files[i].Extension == ".doc" || files[i].Extension == ".docx")
                {
                    dataGridView.Rows[dataGridView.RowCount - 1].Cells["Type"].Value = "Microsoft Office Word";
                }
                else
                {
                    dataGridView.Rows[dataGridView.RowCount - 1].Cells["Type"].Value = files[i].Extension.Replace(".", "") + "文件";
                }
                dataGridView.Rows[dataGridView.RowCount - 1].Cells["Size"].Value = (files[i].Length / 1024.0).ToString("0") + "KB";

            }
            this.Controls.Add(dataGridView);
            dataGridView.ClearSelection();
        }

        //DataGridView中行的鼠标双击时间
        private void RowDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView.Rows[e.RowIndex].Cells["Type"].Value.ToString()=="文件夹")
            {
                string m_hostfilename = this.Text.Remove(0, this.Text.IndexOf("（") + 1).Replace("）", "");
                string m_filename = dataGridView.Rows[e.RowIndex].Cells["FileName"].Value.ToString();
                DirectoryInfo di = new DirectoryInfo(m_hostfilename + "\\"+m_filename+"\\");
                this.Text = this.Text + @"\" + m_filename;//窗体标题根据文件深度改变
                FileInfo[] fileInfos = di.GetFiles();
                DirectoryInfo[] directoryInfos = di.GetDirectories();
                this.Controls.Clear();
                CreatContorl(directoryInfos,fileInfos);
            }
            else
            {
                MessageBox.Show("还没有完善文件打开方式！");
            }
        }

       

    }
}
