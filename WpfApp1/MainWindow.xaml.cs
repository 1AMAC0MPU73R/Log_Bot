using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Time_Init(null, null);
            cblsLog_Directories = new ObservableCollection<CheckBoxListItem>();
            cblsLog_Files = new ObservableCollection<CheckBoxListItem>();
            cblsLog_Directories.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Log_Directories_Changed);
        }


        public ObservableCollection<CheckBoxListItem> cblsLog_Directories { get; set; }
        public ObservableCollection<CheckBoxListItem> cblsLog_Files { get; set; }

        private void Log_Directories_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            Log_Files_List_Update();
        }

        public class CheckBoxListItem : INotifyPropertyChanged
        {
            private string tstrContent;
            private bool boolIsChecked;

            public bool IsChecked
            {
                get { return boolIsChecked; }
                set
                {
                    boolIsChecked = value;
                    NotifyPropertyChanged("IsChecked");
                }
            }

            public string Content
            {
                get { return tstrContent; }
                set
                {
                    tstrContent = value;
                    NotifyPropertyChanged("Content");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void NotifyPropertyChanged(string strPropertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(strPropertyName));
            }
        }

        private void Time_Init(object sender, RoutedEventArgs e)
        {
            DateTime dtmeNow = DateTime.Now;

            dpicTime_Start.SelectedDate = dtmeNow.Date;
            cboxTime_Start_Hour.SelectedValue = dtmeNow.ToString("hh");
            cboxTime_Start_Minute.SelectedValue = dtmeNow.ToString("mm");
            cboxTime_Start_Second.SelectedValue = dtmeNow.ToString("ss");
            cboxTime_Start_Period.SelectedValue = dtmeNow.ToString("tt");
            dpicTime_End.SelectedDate = dtmeNow.AddSeconds(1).Date;
            cboxTime_End_Hour.SelectedValue = dtmeNow.AddSeconds(1).ToString("hh");
            cboxTime_End_Minute.SelectedValue = dtmeNow.AddSeconds(1).ToString("mm");
            cboxTime_End_Second.SelectedValue = dtmeNow.AddSeconds(1).ToString("ss");
            cboxTime_End_Period.SelectedValue = dtmeNow.AddSeconds(1).ToString("tt");
            Time_Modify(null, null);
        }

        private void Time_Copy(object sender, RoutedEventArgs e)
        {
            string tstrTime_Format = "yyyy-MM-dd hh:mm:ss tt";
            DateTime dtmeEnd = DateTime.Now;

            DateTime.TryParseExact((dpicTime_Start.SelectedDate.Value.ToString("yyyy-MM-dd")
                + " " + cboxTime_Start_Hour.Text + ":" + cboxTime_Start_Minute.Text + ":" + cboxTime_Start_Second.Text + " "
                + cboxTime_Start_Period.Text), tstrTime_Format, System.Globalization.CultureInfo.InvariantCulture
                , System.Globalization.DateTimeStyles.None, out dtmeEnd);

            dtmeEnd = dtmeEnd.AddSeconds(1);
            dpicTime_End.SelectedDate = dtmeEnd.Date;
            cboxTime_End_Hour.SelectedValue = dtmeEnd.ToString("hh");
            cboxTime_End_Minute.SelectedValue = dtmeEnd.ToString("mm");
            cboxTime_End_Second.SelectedValue = dtmeEnd.ToString("ss");
            cboxTime_End_Period.SelectedValue = dtmeEnd.ToString("tt");
            Time_Modify(null, null);
        }

        private void Time_Modify(object sender, EventArgs e)
        {
            string tstrTime_Format = "yyyy-MM-dd hh:mm:ss tt";
            DateTime dtmeStart = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 12:00:00 AM", tstrTime_Format, System.Globalization.CultureInfo.InvariantCulture);
            DateTime dtmeEnd = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " 12:00:01 AM", tstrTime_Format, System.Globalization.CultureInfo.InvariantCulture); ;

            DateTime.TryParseExact((dpicTime_Start.SelectedDate.Value.ToString("yyyy-MM-dd")
                + " " + cboxTime_Start_Hour.Text + ":" + cboxTime_Start_Minute.Text + ":" + cboxTime_Start_Second.Text + " "
                + cboxTime_Start_Period.Text), tstrTime_Format, System.Globalization.CultureInfo.InvariantCulture
                , System.Globalization.DateTimeStyles.None, out dtmeStart);
            DateTime.TryParseExact((dpicTime_End.SelectedDate.Value.ToString("yyyy-MM-dd")
                + " " + cboxTime_End_Hour.Text + ":" + cboxTime_End_Minute.Text + ":" + cboxTime_End_Second.Text + " "
                + cboxTime_End_Period.Text), tstrTime_Format, System.Globalization.CultureInfo.InvariantCulture
                , System.Globalization.DateTimeStyles.None, out dtmeEnd);

            if (dtmeEnd <= dtmeStart)
            {
                dtmeEnd = dtmeStart.AddSeconds(1);
                dpicTime_End.SelectedDate = dtmeEnd.Date;
                cboxTime_End_Hour.SelectedValue = dtmeEnd.ToString("hh");
                cboxTime_End_Minute.SelectedValue = dtmeEnd.ToString("mm");
                cboxTime_End_Second.SelectedValue = dtmeEnd.ToString("ss");
                cboxTime_End_Period.SelectedValue = dtmeEnd.ToString("tt");
            }
            tboxTime_Range.Text = "From [" + dtmeStart.ToString(tstrTime_Format) + "] to [" + dtmeEnd.ToString(tstrTime_Format) + "]";
        }

        private void Log_Directory_Select(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbdiLog_Directory_Select = new System.Windows.Forms.FolderBrowserDialog();
            if (fbdiLog_Directory_Select.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tboxLog_Directory_Select.Text = fbdiLog_Directory_Select.SelectedPath;
            }
        }

        private void Log_Directory_Select_Update(object sender, TextChangedEventArgs e)
        {
            if (tboxLog_Directory_Select.ToString() == "")
            {
                tboxLog_Directory_Select.Text = "[Drive]:{Log_Directory}";
                tboxLog_Directory_Select.SelectAll();
            }
        }

        public bool IsPathValidRootedLocal(String tstrPath)
        {
            Uri pathUri;

            Boolean isValidUri = Uri.TryCreate(tstrPath, UriKind.Absolute, out pathUri);
            return isValidUri && pathUri != null && pathUri.IsLoopback;
        }

        private void Log_Directory_Add(object sender, EventArgs e)
        {
            bool boolIsDuplicate = false;

            if (IsPathValidRootedLocal(tboxLog_Directory_Select.Text))
            {
                CheckBoxListItem cbliLog_Directory_New = new CheckBoxListItem();
                cbliLog_Directory_New.Content = tboxLog_Directory_Select.Text;
                cbliLog_Directory_New.IsChecked = true;
                foreach (CheckBoxListItem cbliLog_Directories_List_Entry in cblsLog_Directories)
                {
                    if (cbliLog_Directories_List_Entry.Content == cbliLog_Directory_New.Content)
                    {
                        boolIsDuplicate = true;
                    }
                }
                if (!boolIsDuplicate)
                {
                    cblsLog_Directories.Add(cbliLog_Directory_New);
                    lboxLog_Directory_List.ItemsSource = cblsLog_Directories;
                }
            }
        }

        private void Log_Directory_Remove(object sender, EventArgs e)
        {
            if (lboxLog_Directory_List.SelectedIndex > -1)
            {
                MessageBoxResult mresLog_Directory_Confirm_Removal = System.Windows.MessageBox.Show(
                        "Are you sure?", "Remove Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (mresLog_Directory_Confirm_Removal == MessageBoxResult.Yes)
                {
                    cblsLog_Directories.RemoveAt(lboxLog_Directory_List.SelectedIndex);
                    lboxLog_Directory_List.ItemsSource = cblsLog_Directories;
                }
            }

        }

        private void Log_Directory_ClearAll(object sender, RoutedEventArgs e)
        {
            cblsLog_Directories.Clear();
            lboxLog_Directory_List.ItemsSource = cblsLog_Directories;
        }

        private void Log_Directory_CheckAll(object sender, RoutedEventArgs e)
        {
            foreach(CheckBoxListItem cbliLog_Directory_ToCheck in cblsLog_Directories)
            {
                cbliLog_Directory_ToCheck.IsChecked=true;
            }
            lboxLog_Directory_List.ItemsSource = cblsLog_Directories;
        }

        private void Log_Directory_UncheckAll(object sender, RoutedEventArgs e)
        {
            foreach (CheckBoxListItem cbliLog_Directory_ToCheck in cblsLog_Directories)
            {
                cbliLog_Directory_ToCheck.IsChecked = false;
            }
            lboxLog_Directory_List.ItemsSource = cblsLog_Directories;
        }

        private void Log_File_CheckAll(object sender, RoutedEventArgs e)
        {
            foreach (CheckBoxListItem cbliLog_File_ToCheck in cblsLog_Files)
            {
                cbliLog_File_ToCheck.IsChecked = true;
            }
            lboxLog_File_List.ItemsSource = cblsLog_Files;
        }

        private void Log_File_UncheckAll(object sender, RoutedEventArgs e)
        {
            foreach (CheckBoxListItem cbliLog_File_ToCheck in cblsLog_Files)
            {
                cbliLog_File_ToCheck.IsChecked = false;
            }
            lboxLog_File_List.ItemsSource = cblsLog_Files;
        }

        private void Log_Files_List_Update(object sender, RoutedEventArgs e)
        {
            Log_Files_List_Update();
        }

        private void Log_Files_List_Update()
        {
            ObservableCollection<CheckBoxListItem> lcbiLog_File_List_New=new ObservableCollection<CheckBoxListItem>();
            CheckBoxListItem cbliLog_File_New;

            string[] astrLog_Directory_File_List;

            foreach (CheckBoxListItem cbliLog_Directory_Entry in cblsLog_Directories)
            {
                if (cbliLog_Directory_Entry.IsChecked)
                {
                    astrLog_Directory_File_List = Directory.GetFiles(cbliLog_Directory_Entry.Content.ToString());
                    foreach (string tstrLog_File_Current in astrLog_Directory_File_List)
                    {
                        cbliLog_File_New = new CheckBoxListItem();
                        cbliLog_File_New.IsChecked = true;
                        cbliLog_File_New.Content = tstrLog_File_Current;
                        foreach (CheckBoxListItem cbliLog_File_Entry in cblsLog_Files)
                        {
                            if (cbliLog_File_Entry.Content == cbliLog_File_New.Content)
                            {
                                cbliLog_File_New.IsChecked = cbliLog_File_Entry.IsChecked;
                            }
                        }
                        lcbiLog_File_List_New.Add(cbliLog_File_New);
                    }
                }
            }
            cblsLog_Files.Clear();
            cblsLog_Files = lcbiLog_File_List_New;
            lboxLog_File_List.ItemsSource = cblsLog_Files;
            Log_Output_Update();
        }

        private void Log_Output_Update(object sender, RoutedEventArgs e)
        {
            Log_Output_Update();
        }

        private void Log_Output_Update()
        {
            string tstrLogContent = "";

            foreach (CheckBoxListItem cbliLog_File_Current in cblsLog_Files)
            {
                if (cbliLog_File_Current.IsChecked)
                {
                    tstrLogContent = tstrLogContent + File.ReadAllText(cbliLog_File_Current.Content);
                }
            }
            if (tstrLogContent == "")
            {
                tboxLog_Output.Text = "< Nothing to display... >";
            }
            else
            {
                tboxLog_Output.Text = tstrLogContent;
            }
        }
    }
}
