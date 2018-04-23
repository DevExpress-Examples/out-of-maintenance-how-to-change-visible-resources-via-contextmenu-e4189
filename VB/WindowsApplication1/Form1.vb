Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraScheduler

Namespace WindowsApplication1
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Dim dt As New DataTable()
            dt.Columns.Add("ID", GetType(Integer))
            dt.Columns.Add("Name", GetType(String))
            dt.Columns.Add("Color", GetType(Color))
            dt.Rows.Add(New Object() { 1, "John" })
            dt.Rows.Add(New Object() { 2, "Jane" })
            dt.Rows.Add(New Object() { 3, "Bob" })
            dt.Rows.Add(New Object() { 4, "Jack" })
            dt.Rows.Add(New Object() { 5, "Seth" })
            schedulerStorage1.Resources.Mappings.Id = "ID"
            schedulerStorage1.Resources.Mappings.Caption = "Name"
            schedulerStorage1.Resources.DataSource = dt
            Dim apt As Appointment = schedulerStorage1.CreateAppointment(AppointmentType.Normal)
            apt.Start = Date.Today
            apt.End = Date.Today
            apt.Subject = "test" & Environment.NewLine & "test test test" & Environment.NewLine & "Test teste teste"
            apt.ResourceId = 1
            schedulerStorage1.Appointments.Add(apt)
            schedulerStorage1.Appointments(0).AllDay = True

            schedulerControl1.Start = Date.Now
            schedulerControl1.TimelineView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Always

        End Sub

        Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
            schedulerControl1.ShowPrintOptionsForm()
        End Sub

        Private Sub schedulerControl1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles schedulerControl1.Click

        End Sub
        Private clickedResource As Resource
        Private Sub schedulerControl1_PopupMenuShowing(ByVal sender As Object, ByVal e As PopupMenuShowingEventArgs) Handles schedulerControl1.PopupMenuShowing
            clickedResource = schedulerControl1.ActiveView.ViewInfo.CalcHitInfo(schedulerControl1.PointToClient(Form.MousePosition), False).ViewInfo.Resource
            Dim subMenu As New SchedulerPopupMenu()
            subMenu.Caption = "Resources Collection"
            For i As Integer = 0 To schedulerStorage1.Resources.Count - 1
                If schedulerControl1.ActiveView.ViewInfo.VisibleResources.IndexOf(schedulerStorage1.Resources(i)) < 0 Then
                    Dim myMenu As New SchedulerMenuItem(schedulerStorage1.Resources(i).Caption, New EventHandler(AddressOf MyMenuItemClick))
                    myMenu.Tag = schedulerStorage1.Resources(i)
                    subMenu.Items.Add(myMenu)
                End If
            Next i
            e.Menu.Items.Add(subMenu)
        End Sub
        Private Sub MyMenuItemClick(ByVal sender As Object, ByVal e As EventArgs)
            schedulerStorage1.BeginUpdate()

            Dim clickedResourceIndex As Integer = schedulerStorage1.Resources.Items.IndexOf(clickedResource)
            Dim selectedResource As Resource = TryCast(DirectCast(sender, DevExpress.Utils.Menu.DXMenuItem).Tag, Resource)
            Dim selectedResourceIndex As Integer = schedulerStorage1.Resources.Items.IndexOf(selectedResource)


            schedulerStorage1.Resources.Items.Remove(selectedResource)
            schedulerStorage1.Resources.Items.Remove(clickedResource)
            If selectedResourceIndex > clickedResourceIndex Then
                schedulerStorage1.Resources.Items.Insert(clickedResourceIndex, selectedResource)
                schedulerStorage1.Resources.Items.Insert(selectedResourceIndex, clickedResource)
            Else
                schedulerStorage1.Resources.Items.Insert(selectedResourceIndex, clickedResource)
                schedulerStorage1.Resources.Items.Insert(clickedResourceIndex, selectedResource)
            End If
            schedulerStorage1.EndUpdate()
        End Sub
    End Class
End Namespace
