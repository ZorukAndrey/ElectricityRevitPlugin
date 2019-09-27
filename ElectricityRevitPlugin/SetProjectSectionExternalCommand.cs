﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ElectricityRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class SetProjectSectionExternalCommand : IExternalCommand
    {
        private Guid _projectSectionParameterGuid = new Guid("ffe3351b-555f-40fb-86fd-4e5a4a446d27");
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;
            var app = uiApp.Application;
            var result = Result.Succeeded;
            try
            {
                using (var tr = new Transaction(doc))
                {
                    tr.Start("Установка параметров раздел проектирования");
                    var sharedParameterApplicableRule = new SharedParameterApplicableRule("Раздел проектирования");
                    var elementParameterFilter = new ElementParameterFilter(sharedParameterApplicableRule);

                    var allElements = new FilteredElementCollector(doc)
                        .OfClass(typeof(FamilyInstance))
                        .WherePasses(elementParameterFilter)
                        .Cast<Element>();

                    foreach (var el in allElements)
                    {
                        var param = el.get_Parameter(_projectSectionParameterGuid);
                        if(param is null|| !param.HasValue || param.IsReadOnly)
                            continue;
                        var value = param.AsString();
                        if(value.EndsWith("@"))
                            continue;
                        param.Set(value + '@');
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
    }
}
