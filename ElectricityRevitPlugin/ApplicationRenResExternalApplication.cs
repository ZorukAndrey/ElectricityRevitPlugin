﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.UI;
using VCRevitRibbonUtilCustom;

namespace ElectricityRevitPlugin
{
    class ApplicationRenResExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication uicApp)
        {
            var result = Result.Succeeded;
            try
            {
                MyRibbon.GetApplicationRibbon(uicApp)
                    .Tab("ЭОМ")
                    .Panel("Листы")
                    .CreateSplitButton("SB Листы1", " SB Листы1",
                        sb =>
                        {
                            sb.CreateButton<SortSheets>("Сортировка листов", "Сортировка листов",
                                bt =>
                                    bt.SetLargeImage(Resource1.icons8_futurama_professor_farnsworth_32)
                                        .SetLongDescription<MyButton>("Сортировка листов")
                                        .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                            "https://www.revitapidocs.com/2019/"));
                            sb.CreateButton<SelectFramesFromSelectedSheets>("Выбрать рамки", "Выбрать рамки",
                                bt => bt.SetLargeImage(Resource1.icons8_futurama_fry_32)
                                    .SetLongDescription<MyButton>("Выбрать семейства основной надписи на листах")
                                    .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                        "https://www.revitapidocs.com/2019/"));

                        })
                    .CreateSplitButton("SB Листы2", " SB Листы2",
                        sb =>
                        {
                            sb.CreateButton<SelectSheetFormatAndAlignExternalCommand>("Подобрать рамки", "Подобрать рамки",
                                bt =>
                                    bt.SetLargeImage(Resource1.icons8_futurama_bender_32)
                                        .SetLongDescription<MyButton>("Подобрать рамки")
                                        .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                            "https://www.revitapidocs.com/2019/"));
                            sb.CreateButton<SelectSheetFormatAndAlignOnDiagramExternalCommand>("Подобрать рамки для схем", "Подобрать рамки для схем",
                                bt => bt.SetLargeImage(Resource1.icons8_futurama_zoidberg_32)
                                    .SetLongDescription<MyButton>("Подобрать рамки для схем")
                                    .SetContextualHelp<MyButton>(ContextualHelpType.Url,
                                        "https://www.revitapidocs.com/2019/"));

                        });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace);
                return Result.Failed;
            }

            return result;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
