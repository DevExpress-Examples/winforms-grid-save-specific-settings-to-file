using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.IO;
using DevExpress.Utils.Serializing;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils.Serializing.Helpers;

namespace WindowsFormsApplication1
{
    public class GridLayoutSerializer : XmlXtraSerializer
    {
        public static void SaveLayout(ColumnView view, Stream stream)
        {
            GridLayoutSerializer serializer = new GridLayoutSerializer();
            serializer.Serialize(stream, serializer.GetFilterProps(view), "View");
        }

        protected XtraPropertyInfoCollection GetFilterProps(ColumnView view)
        {
            XtraPropertyInfoCollection store = new XtraPropertyInfoCollection();
            ArrayList propList = new ArrayList();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(view);
            propList.Add(properties.Find("Columns", false));
            propList.Add(properties.Find("OptionsView", false));
            propList.Add(properties.Find("ActiveFilterEnabled", false));
            propList.Add(properties.Find("ActiveFilterString", false));
            propList.Add(properties.Find("MRUFilters", false));
            propList.Add(properties.Find("ActiveFilter", false));

            MethodInfo mi = typeof(SerializeHelper).GetMethod("SerializeProperty", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo miGetXtraSerializableProperty = typeof(SerializeHelper).GetMethod("GetXtraSerializableProperty", BindingFlags.NonPublic | BindingFlags.Instance);
            SerializeHelper helper = new SerializeHelper(view);
            (view as IXtraSerializable).OnStartSerializing();
            foreach (PropertyDescriptor prop in propList)
            {
                XtraSerializableProperty newXtraSerializableProperty = miGetXtraSerializableProperty.Invoke(helper, new object[] { view, prop }) as XtraSerializableProperty;
                SerializablePropertyDescriptorPair p = new SerializablePropertyDescriptorPair(prop, newXtraSerializableProperty);
                mi.Invoke(helper, new object[] { store, view, p, XtraSerializationFlags.None, null });
            }
            (view as IXtraSerializable).OnEndSerializing();
            return store;
        }
    }
}
