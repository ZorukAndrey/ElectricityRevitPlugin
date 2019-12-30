﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SetHeightForSpaceExternalCommand : IExternalCommand
    {
        IList<Room> _rooms;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var app = uiApp.Application;
            var doc = uiDoc.Document;
            var result = Result.Succeeded;

            try
            {
                var spaces = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_MEPSpaces)
                    .OfType<Space>();

                //var spaceWithRooms = spaces.Where(s => s.Room != null)
                //    .Select(x=>x.Name)
                //    .ToArray();
                SelectLink(uiDoc);

              

                using (var tr = new Transaction(doc))
                {
                    tr.Start("Установка высоты пространств");


                    foreach (var space in spaces)
                    {
                        SetHeightOfSpace(space);
                    }
                    tr.Commit();
                }



            }
            catch (Exception e)
            {
                message += e.Message + '\n' + e.StackTrace;
                result = Result.Failed;
            }
            finally
            {

            }
            return result;

        }

        private void SelectLink(UIDocument uiDoc)
        {
            var doc = uiDoc.Document;
            var selection = uiDoc.Selection;
            var reference = selection.PickObject(ObjectType.Element, new RevitLinkSelectionFilter());
            var linkInstance = doc.GetElement(reference.ElementId) as RevitLinkInstance;
            var linkedDoc = linkInstance.GetLinkDocument();
            var roomsInLinkDoc = new FilteredElementCollector(linkedDoc)
                .OfCategory(BuiltInCategory.OST_Rooms)
                .OfType<Room>();
            _rooms = roomsInLinkDoc.ToArray();

        }
        private void SetHeightOfSpace(Space space)
        {
            var spaceNumber = space.Number;
            var spaceName = space.Name;
            if(string.IsNullOrEmpty(spaceName) || string.IsNullOrEmpty(spaceNumber))
                return;
            var spaceLevel = space.Level;
            var room = _rooms.FirstOrDefault(r =>
            {
                if (string.IsNullOrEmpty(r.Name) || string.IsNullOrEmpty(r.Number))
                    return false;
                return r.Name == spaceName && spaceNumber.StartsWith(r.Number);
            });
            if(room is null)
                return;
            var sb = space.BaseOffset;
            var su = space.LimitOffset;

            var result = space.get_Parameter(BuiltInParameter.ROOM_LOWER_OFFSET).Set(room.BaseOffset) &&
            space.get_Parameter(BuiltInParameter.ROOM_UPPER_OFFSET).Set( room.LimitOffset);
        }
    }
}
