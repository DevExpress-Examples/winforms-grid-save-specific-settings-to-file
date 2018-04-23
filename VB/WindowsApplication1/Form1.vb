Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.IO
Imports System.Data

Namespace WindowsFormsApplication1
	Partial Public Class Form1
		Inherits Form
		Private Function CreateTable(ByVal RowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			tbl.Columns.Add("Name", GetType(String))
			tbl.Columns.Add("ID", GetType(Integer))
			tbl.Columns.Add("Number", GetType(Integer))
			tbl.Columns.Add("Date", GetType(DateTime))
			For i As Integer = 0 To RowCount - 1
				tbl.Rows.Add(New Object() { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) })
			Next i
			Return tbl
		End Function


		Public Sub New()
			InitializeComponent()
			gridControl1.DataSource = CreateTable(20)
		End Sub


		Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
			Dim fs As New FileStream("test.xml", FileMode.Create)
			GridLayoutSerializer.SaveLayout(gridView1, fs)
			fs.Close()
		End Sub

		Private Sub simpleButton2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton2.Click
			gridView1.RestoreLayoutFromXml("test.xml")
		End Sub
	End Class
End Namespace
