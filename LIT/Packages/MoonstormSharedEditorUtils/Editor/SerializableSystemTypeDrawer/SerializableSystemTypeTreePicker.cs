using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using EntityStates;

namespace Moonstorm.EditorUtils.TreeDrawers
{
    public class SerializableSystemTypeTreePicker : EditorWindow
    {
        private static SerializableSystemTypeTreePicker typeTreePicker;

        private readonly SerializableSystemTypeTreeView treeView = new SerializableSystemTypeTreeView();
        private bool close;
        private SerializableSystemTypeTreeView.SystemTypeTreeInfo selectedSystemType;
        private SerializedProperty serializableSystemTypeReference;
        private SerializedObject serializedObject;

        public static EditorWindow LastFocusedWindow = null;

        private void Update()
        {
            if (close)
            {
                Close();

                if (LastFocusedWindow)
                {
                    EditorApplication.delayCall += LastFocusedWindow.Repaint;
                    LastFocusedWindow = null;
                }
            }
        }

        private void OnGUI()
        {
            using (new GUILayout.VerticalScope())
            {
                treeView.DisplayTreeView(TreeListControl.DisplayTypes.USE_SCROLL_VIEW);

                using (new GUILayout.HorizontalScope("box"))
                {
                    if (GUILayout.Button("Ok"))
                    {
                        //Get the selected item
                        var selectedItem = treeView.GetSelectedItem();
                        var data = (SerializableSystemTypeTreeView.SystemTypeTreeInfo)selectedItem?.DataContext;
                        if (selectedItem != null && data.itemType == SerializableSystemTypeTreeView.ItemType.SystemType)
                            SetSystemType(selectedItem);

                        //The window can now be closed
                        close = true;
                    }
                    else if (GUILayout.Button("Cancel"))
                        close = true;
                    else if (GUILayout.Button("Reset"))
                    {
                        ResetSystemType();
                        close = true;
                    }
                    else if (Event.current.type == EventType.Used && treeView.LastDoubleClickedItem != null)
                    {
                        //We must be in 'used' mode in order for this to work
                        SetSystemType(treeView.LastDoubleClickedItem);
                        close = true;
                    }
                }
            }
        }

        private void SetSystemType(TreeListItem in_item)
        {
            serializedObject.Update();

            selectedSystemType = in_item.DataContext as SerializableSystemTypeTreeView.SystemTypeTreeInfo;
            serializableSystemTypeReference.stringValue = selectedSystemType.fullName;
            serializedObject.ApplyModifiedProperties();
        }

        private void ResetSystemType()
        {
            serializedObject.Update();
            serializableSystemTypeReference.stringValue = null;
            selectedSystemType = null;
            serializedObject.ApplyModifiedProperties();
        }


        public class PickerCreator
        {
            public SerializedProperty systemTypeReference;
            public Rect pickerPosition;
            public SerializedObject serializedObject;

            internal PickerCreator()
            {
                EditorApplication.delayCall += DelayCall;
            }

            private void DelayCall()
            {
                if (typeTreePicker != null)
                    return;

                typeTreePicker = CreateInstance<SerializableSystemTypeTreePicker>();

                //position the window below the button
                var pos = new Rect(pickerPosition.x, pickerPosition.yMax, 0, 0);

                //If the window gets out of the screen, we place it on top of the button instead
                if (pickerPosition.yMax > Screen.currentResolution.height / 2)
                    pos.y = pickerPosition.y - Screen.currentResolution.height / 2;

                //We show a drop down window which is automatically destroyed when focus is lost
                typeTreePicker.ShowAsDropDown(pos,
                    new Vector2(pickerPosition.width >= 250 ? pickerPosition.width : 250,
                        Screen.currentResolution.height / 2));

                typeTreePicker.serializableSystemTypeReference = systemTypeReference;
                typeTreePicker.serializedObject = serializedObject;

                typeTreePicker.treeView.AssignDefaults();
                typeTreePicker.treeView.SetRootItem("System Types States");
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        assembly.GetTypes()
                            .Where(type => !type.IsAbstract)
                            .ToList()
                            .ForEach(type => typeTreePicker.treeView.PopulateItem(type));
                    }
                    catch { }
                }
            }
        }

    }


}
