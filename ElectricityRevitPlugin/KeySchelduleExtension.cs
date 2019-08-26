﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ElectricityRevitPlugin
{
    public static class KeyScheduleExtension
    {
        public static void AddElement(this ViewSchedule schedule, Element el)
        {
            var td = schedule.GetTableData();
            var body = td.GetSectionData(SectionType.Body);
            var flag = body.CanInsertRow(body.FirstRowNumber);
            if(!flag)
                throw new Exception("Невозможно вставить строку в данную спецификацию");
            body.InsertRow(body.FirstRowNumber);
            

        }
    }
}
