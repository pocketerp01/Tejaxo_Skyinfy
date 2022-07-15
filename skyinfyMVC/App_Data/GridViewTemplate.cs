using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for GridViewTemplate
/// </summary>
public  class GridViewTemplate : ITemplate
{
    private DataControlRowType templateType;
    private string columnName;
    private string Control;


    public GridViewTemplate(DataControlRowType type, string colname, string CtrlName)
    {
        templateType = type;
        columnName = colname;
        Control = CtrlName;
        
    }

    public void InstantiateIn(System.Web.UI.Control container)
    {

        switch (templateType)
        {
            case DataControlRowType.DataRow:
                switch (Control.Trim().ToUpper())
                {
                    case "TEXTBOX":
                        TextBox txt = new TextBox();
                        txt.ID = columnName;                        
                        container.Controls.Add(txt);
                        break;
                    case "LABEL":
                        Label lbl = new Label();
                        lbl.ID = columnName;
                        container.Controls.Add(lbl);
                        break;
                    case "DROPDOWN":
                        DropDownList list = new DropDownList();
                        list.ID = columnName;
                        container.Controls.Add(list);
                        break;
                }
                break;


            default:
                break;
        }

    }
}