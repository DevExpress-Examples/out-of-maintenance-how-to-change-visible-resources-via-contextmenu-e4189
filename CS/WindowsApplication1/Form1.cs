using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Color", typeof(Color));
            dt.Rows.Add(new object[] { 1, "John" });
            dt.Rows.Add(new object[] { 2, "Jane" });
            dt.Rows.Add(new object[] { 3, "Bob" });
            dt.Rows.Add(new object[] { 4, "Jack" });
            dt.Rows.Add(new object[] { 5, "Seth" });
            schedulerStorage1.Resources.Mappings.Id = "ID";
            schedulerStorage1.Resources.Mappings.Caption = "Name";
            schedulerStorage1.Resources.DataSource = dt;
            schedulerStorage1.Appointments.Add(new Appointment(AppointmentType.Normal, DateTime.Today, DateTime.Today, "test" + Environment.NewLine + "test test test" + Environment.NewLine + "Test teste teste") { ResourceId = 1 });
            schedulerStorage1.Appointments[0].AllDay = true;

            schedulerControl1.Start = DateTime.Now;
            schedulerControl1.TimelineView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Always;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            schedulerControl1.ShowPrintOptionsForm();
        }

        private void schedulerControl1_Click(object sender, EventArgs e)
        {

        }
        Resource clickedResource;
        private void schedulerControl1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            clickedResource = schedulerControl1.ActiveView.ViewInfo.CalcHitInfo(schedulerControl1.PointToClient(Form.MousePosition), false).ViewInfo.Resource;
            SchedulerPopupMenu subMenu = new SchedulerPopupMenu();
            subMenu.Caption = "Resources Collection";
            for (int i = 0; i < schedulerStorage1.Resources.Count; i++)
                if (schedulerControl1.ActiveView.ViewInfo.VisibleResources.IndexOf(schedulerStorage1.Resources[i]) < 0)
                {
                    SchedulerMenuItem myMenu = new SchedulerMenuItem(schedulerStorage1.Resources[i].Caption, new EventHandler(MyMenuItemClick));
                    myMenu.Tag = schedulerStorage1.Resources[i];
                    subMenu.Items.Add(myMenu);
                }
            e.Menu.Items.Add(subMenu);
        }
        private void MyMenuItemClick(object sender, EventArgs e)
        {
            schedulerStorage1.Resources.BeginUpdate();
            
            int clickedResourceIndex = schedulerStorage1.Resources.Items.IndexOf(clickedResource);
            Resource selectedResource = ((DevExpress.Utils.Menu.DXMenuItem)(sender)).Tag as Resource;
            int selectedResourceIndex = schedulerStorage1.Resources.Items.IndexOf(selectedResource);


            schedulerStorage1.Resources.Items.Remove(selectedResource);
            schedulerStorage1.Resources.Items.Remove(clickedResource);
            if (selectedResourceIndex > clickedResourceIndex)
            {
                schedulerStorage1.Resources.Items.Insert(clickedResourceIndex, selectedResource);
                schedulerStorage1.Resources.Items.Insert(selectedResourceIndex, clickedResource);
            }
            else
            {
                schedulerStorage1.Resources.Items.Insert(selectedResourceIndex, clickedResource);
                schedulerStorage1.Resources.Items.Insert(clickedResourceIndex, selectedResource);
            }
            schedulerStorage1.Resources.EndUpdate();
        }
    }
}
