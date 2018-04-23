Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Reflection
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports DevExpress.Utils.Serializing
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.Utils.Serializing.Helpers

Namespace WindowsFormsApplication1
	Public Class GridLayoutSerializer
		Inherits XmlXtraSerializer
		Public Shared Sub SaveLayout(ByVal view As ColumnView, ByVal stream As Stream)
			Dim serializer As New GridLayoutSerializer()
			serializer.Serialize(stream, serializer.GetFilterProps(view), "View")
		End Sub

		Protected Function GetFilterProps(ByVal view As ColumnView) As XtraPropertyInfoCollection
			Dim store As New XtraPropertyInfoCollection()
			Dim propList As New ArrayList()
			Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(view)
			propList.Add(properties.Find("Columns", False))
			propList.Add(properties.Find("OptionsView", False))
			propList.Add(properties.Find("ActiveFilterEnabled", False))
			propList.Add(properties.Find("ActiveFilterString", False))
			propList.Add(properties.Find("MRUFilters", False))
			propList.Add(properties.Find("ActiveFilter", False))

			Dim mi As MethodInfo = GetType(SerializeHelper).GetMethod("SerializeProperty", BindingFlags.NonPublic Or BindingFlags.Instance)
			Dim miGetXtraSerializableProperty As MethodInfo = GetType(SerializeHelper).GetMethod("GetXtraSerializableProperty", BindingFlags.NonPublic Or BindingFlags.Instance)
			Dim helper As New SerializeHelper(view)
			TryCast(view, IXtraSerializable).OnStartSerializing()
			For Each prop As PropertyDescriptor In propList
				Dim newXtraSerializableProperty As XtraSerializableProperty = TryCast(miGetXtraSerializableProperty.Invoke(helper, New Object() { view, prop }), XtraSerializableProperty)
				Dim p As New SerializablePropertyDescriptorPair(prop, newXtraSerializableProperty)
				mi.Invoke(helper, New Object() { store, view, p, XtraSerializationFlags.None, Nothing })
			Next prop
			TryCast(view, IXtraSerializable).OnEndSerializing()
			Return store
		End Function
	End Class
End Namespace
