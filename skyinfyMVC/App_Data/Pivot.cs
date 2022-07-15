using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


// Written by Anurag Gandhi.
// Url: http://www.gandhisoft.com
// Contact me at: soft.gandhi@gmail.com

/// <summary>
/// Pivots the data
/// </summary>
public class Pivot
{
    public static string MyGuid = "";
    private DataTable _SourceTable = new DataTable();
    private IEnumerable<DataRow> _Source = new List<DataRow>();
    //public string MyGuid = "";
    public Pivot(DataTable SourceTable, string Myguid)
    {
        _SourceTable = SourceTable;
        _Source = SourceTable.Rows.Cast<DataRow>();
        MyGuid = Myguid;
    }

    /// <summary>
    /// Pivots the DataTable based on provided RowField, DataField, Aggregate Function and ColumnFields.//
    /// </summary>
    /// <param name="rowField">The column name of the Source Table which you want to spread into rows</param>
    /// <param name="dataField">The column name of the Source Table which you want to spread into Data Part</param>
    /// <param name="aggregate">The Aggregate function which you want to apply in case matching data found more than once</param>
    /// <param name="columnFields">The List of column names which you want to spread as columns</param>
    /// <returns>A DataTable containing the Pivoted Data</returns>
    public DataTable PivotData(string rowField, string dataField, AggregateFunction aggregate, params string[] columnFields)
    {
        DataTable dt = new DataTable();
        string Separator = ".";
        List<string> rowList = _Source.Select(x => x[rowField].ToString()).Distinct().ToList();
        // Gets the list of columns .(dot) separated.
        var colList = _Source.Select(x =>(columnFields.Select(n => x[n]).Aggregate((a, b) => a += Separator + b.ToString())).ToString()).Distinct().OrderBy(m => m);
        
        dt.Columns.Add(rowField);
        foreach (var colName in colList)
            dt.Columns.Add(colName);  // Cretes the result columns.//

        foreach (string rowName in rowList)
        {
            DataRow row = dt.NewRow();
            row[rowField] = rowName;
            foreach (string colName in colList)
            {
                string strFilter = rowField + " = '" + rowName + "'";
                string[] strColValues = colName.Split(Separator.ToCharArray(), StringSplitOptions.None);
                for (int i = 0; i < columnFields.Length; i++)
                    strFilter += " and " + columnFields[i] + " = '" + strColValues[i] + "'";
                row[colName] = GetData(strFilter, dataField, aggregate);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    public DataTable PivotData(string rowField, string dataField, AggregateFunction aggregate, bool showSubTotal, params string[] columnFields)
    {
        DataTable dt = new DataTable();
        string Separator = ".";
        List<string> rowList = _Source.Select(x => x[rowField].ToString()).Distinct().ToList();
        // Gets the list of columns .(dot) separated.
        List<string> colList = _Source.Select(x => columnFields.Aggregate((a, b) => x[a].ToString() + Separator + x[b].ToString())).Distinct().OrderBy(m => m).ToList();

        if (showSubTotal && columnFields.Length > 1)
        {
            string totalField = string.Empty;
            for (int i = 0; i < columnFields.Length - 1; i++)
                totalField += columnFields[i] + "(Total)" + Separator;
            List<string> totalList = _Source.Select(x => totalField + x[columnFields.Last()].ToString()).Distinct().OrderBy(m => m).ToList();
            colList.InsertRange(0, totalList);
        }

        dt.Columns.Add(rowField);
        colList.ForEach(x => dt.Columns.Add(x));

        foreach (string rowName in rowList)
        {
            DataRow row = dt.NewRow();
            row[rowField] = rowName;
            foreach (string colName in colList)
            {
                string filter = rowField + " = '" + rowName + "'";
                string[] colValues = colName.Split(Separator.ToCharArray(), StringSplitOptions.None);
                for (int i = 0; i < columnFields.Length; i++)
                    if (!colValues[i].Contains("(Total)"))
                        filter += " and " + columnFields[i] + " = '" + colValues[i] + "'";
                row[colName] = GetData(filter, dataField, colName.Contains("(Total)") ? AggregateFunction.Sum : aggregate);
            }
            dt.Rows.Add(row);
        }
        return dt;
    } 

    public DataTable PivotData(string DataField, AggregateFunction Aggregate, string[] RowFields, string[] ColumnFields)
    {
        DataTable dt = new DataTable();
        string Separator = ".";
        var RowList = _SourceTable.DefaultView.ToTable(true, RowFields).AsEnumerable().ToList();
        for (int index = RowFields.Count() - 1; index >= 0; index--)
            RowList = RowList.OrderBy(x => x.Field<object>(RowFields[index])).ToList();
        // Gets the list of columns .(dot) separated.
        var ColList = (from x in _SourceTable.AsEnumerable()
                       select new
                       {
                           Name = ColumnFields.Select(n => x.Field<object>(n))
                               .Aggregate((a, b) => a += Separator + b.ToString())

                       })
                           .Distinct();
        //.OrderBy(m => m.Name);
        //.OrderBy(m => m.Name); 
        //dt.Columns.Add(RowFields);
        foreach (string s in RowFields)
            dt.Columns.Add(s, Type.GetType(_SourceTable.Columns[s].DataType.ToString()));

        foreach (var col in ColList)
        {
            if (col.Name != null)
                dt.Columns.Add(col.Name.ToString(), GetTypes(Aggregate));

        }
        // Cretes the result columns.//

        foreach (var RowName in RowList)
        {
            DataRow row = dt.NewRow();
            string strFilter = string.Empty;
            foreach (string Field in RowFields)
            {
                row[Field] = RowName[Field];
                strFilter += " and " + Field + " = '" + RowName[Field].ToString() + "'";
            }
            strFilter = strFilter.Substring(5);
            foreach (var col in ColList)
            {
                if (col.Name != null)
                {
                    string filter = strFilter;
                    string[] strColValues = col.Name.ToString().Split(Separator.ToCharArray(), StringSplitOptions.None);
                    for (int i = 0; i < ColumnFields.Length; i++)
                    {
                        filter += " and " + ColumnFields[i] + " = '" + strColValues[i] + "'";
                    }
                    var vall = GetData(filter, DataField, Aggregate);
                    if (vall == null) vall = "0";
                    row[col.Name.ToString()] = vall;
                }
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    public DataTable PivotData_Tot(string DataField, AggregateFunction Aggregate, string[] RowFields, string[] ColumnFields, string TotColName)

    {
        DataTable dt = new DataTable();
        sgenFun sgen = new sgenFun(MyGuid);
        string Separator = "!~!~!";
        var RowList = _SourceTable.DefaultView.ToTable(true, RowFields).AsEnumerable().ToList();
        for (int index = RowFields.Count() - 1; index >= 0; index--)
            RowList = RowList.OrderBy(x => x.Field<object>(RowFields[index])).ToList();
        // Gets the list of columns .(dot) separated.
        var ColList = (from x in _SourceTable.AsEnumerable()
                       select new
                       {
                           Name = ColumnFields.Select(n => x.Field<object>(n))
                               .Aggregate((a, b) => a += Separator + b.ToString())


                       })
                           .Distinct();
        //.OrderBy(m => m.Name);
        //.OrderBy(m => m.ordd);

        DataTable dtcols = new DataTable();

        dtcols.Columns.Add("name", typeof(string));
        dtcols.Columns.Add("ordd", typeof(decimal));
        int d = 1;
        foreach (var col in ColList)
        {
            DataRow drc = dtcols.NewRow();
            if (col.Name.ToString().Contains(Separator))
            {
                drc["name"] = col.Name.ToString().Trim().Split(new[] { Separator }, StringSplitOptions.None)[0];
                drc["ordd"] = col.Name.ToString().Trim().Split(new[] { Separator }, StringSplitOptions.None)[1];
            }
            else
            {
                drc["name"] = col.Name.ToString().Trim();
                drc["ordd"] = d;
            }
            dtcols.Rows.Add(drc);
            d++;

        }
        DataView dvc = dtcols.DefaultView;
        dvc.Sort = "ordd";
        dtcols = dvc.ToTable();


        //dt.Columns.Add(RowFields);
        foreach (string s in RowFields)
            dt.Columns.Add(s);

        foreach (DataRow col in dtcols.Rows)
            dt.Columns.Add(col[0].ToString());  // Cretes the result columns.//
        dt.Columns.Add(TotColName,typeof(decimal));
        foreach (var RowName in RowList)
        {
            DataRow row = dt.NewRow();
            string strFilter = string.Empty;

            foreach (string Field in RowFields)
            {
                row[Field] = RowName[Field];
                strFilter += " and " + Field + " = '" + RowName[Field].ToString() + "'";
            }
            strFilter = strFilter.Substring(5);
            decimal total = 0;
            foreach (DataRow col in dtcols.Rows)
            {
                string filter = strFilter;
                string[] strColValues = col[0].ToString().Split(Separator.ToCharArray(), StringSplitOptions.None);
                for (int i = 0; i < ColumnFields.Length; i++)
                    filter += " and " + ColumnFields[0] + " = '" + strColValues[0] + "'";
                var vall = GetData(filter, DataField, Aggregate);
                if (vall == null) vall = 0;             
                
                total = total + sgen.Make_decimal(vall.ToString());
                row[col[0].ToString()] = vall;
                row[TotColName] = total;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    public DataTable PivotData_Tot_dec(string DataField, AggregateFunction Aggregate, string[] RowFields, string[] ColumnFields, string TotColName)
    {
        DataTable dt = new DataTable();
        sgenFun sgen = new sgenFun(MyGuid);
        string Separator = "!~!~!";
        var RowList = _SourceTable.DefaultView.ToTable(true, RowFields).AsEnumerable().ToList();
        for (int index = RowFields.Count() - 1; index >= 0; index--)
            RowList = RowList.OrderBy(x => x.Field<object>(RowFields[index])).ToList();
        // Gets the list of columns .(dot) separated.
        var ColList = (from x in _SourceTable.AsEnumerable()
                       select new
                       {
                           Name = ColumnFields.Select(n => x.Field<object>(n))
                               .Aggregate((a, b) => a += Separator + b.ToString())


                       })
                           .Distinct();
        //.OrderBy(m => m.Name);
        //.OrderBy(m => m.ordd);

        DataTable dtcols = new DataTable();

        dtcols.Columns.Add("name", typeof(string));
        dtcols.Columns.Add("ordd", typeof(decimal));
        int d = 1;
        foreach (var col in ColList)
        {
            DataRow drc = dtcols.NewRow();
            if (col.Name.ToString().Contains(Separator))
            {
                drc["name"] = col.Name.ToString().Trim().Split(new[] { Separator }, StringSplitOptions.None)[0];
                drc["ordd"] = col.Name.ToString().Trim().Split(new[] { Separator }, StringSplitOptions.None)[1];
            }
            else
            {
                drc["name"] = col.Name.ToString().Trim();
                drc["ordd"] = d;
            }
            dtcols.Rows.Add(drc);
            d++;

        }
        DataView dvc = dtcols.DefaultView;
        dvc.Sort = "ordd";
        dtcols = dvc.ToTable();


        //dt.Columns.Add(RowFields);
        foreach (string s in RowFields)
            dt.Columns.Add(s, Type.GetType(_SourceTable.Columns[s].DataType.ToString()));

        foreach (DataRow col in dtcols.Rows)
        {
            try
            {
                dt.Columns.Add(col[0].ToString(), GetTypes(Aggregate));  // Cretes the result columns.//
            }
            catch (Exception err) { }
        }
        dt.Columns.Add(TotColName, GetTypes(Aggregate));
        foreach (var RowName in RowList)
        {
            DataRow row = dt.NewRow();
            string strFilter = string.Empty;

            foreach (string Field in RowFields)
            {
                row[Field] = RowName[Field];
                strFilter += " and " + Field + " = '" + RowName[Field].ToString() + "'";
            }
            strFilter = strFilter.Substring(5);
            decimal total = 0;
            foreach (DataRow col in dtcols.Rows)
            {
                string filter = strFilter;
                string[] strColValues = col[0].ToString().Split(Separator.ToCharArray(), StringSplitOptions.None);
                for (int i = 0; i < ColumnFields.Length; i++)
                    filter += " and " + ColumnFields[0] + " = '" + strColValues[0] + "'";
                var vall = GetData(filter, DataField, Aggregate);
                total = total + sgen.Make_decimal(vall.ToString());
                row[col[0].ToString()] = vall;
                row[TotColName] = total;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    
    public DataTable PivotData_Tot_multi(string[] DataFields, AggregateFunction Aggregate, string[] RowFields, string[] ColumnFields)
    {
        DataTable dt = new DataTable();
        sgenFun sgen = new sgenFun(MyGuid);
        string Separator = "!~!~!";
        var RowList = _SourceTable.DefaultView.ToTable(true, RowFields).AsEnumerable().ToList();
        for (int index = RowFields.Count() - 1; index >= 0; index--)
            RowList = RowList.OrderBy(x => x.Field<object>(RowFields[index])).ToList();
        // Gets the list of columns .(dot) separated.
        var ColList = (from x in _SourceTable.AsEnumerable()
                       select new
                       {
                           Name = ColumnFields.Select(n => x.Field<object>(n))
                               .Aggregate((a, b) => a += Separator + b.ToString())
                       }).Distinct();
        //.OrderBy(m => m.Name);
        //.OrderBy(m => m.ordd);

        DataTable dtcols = new DataTable();

        dtcols.Columns.Add("name", typeof(string));
        dtcols.Columns.Add("ordd", typeof(decimal));
        int d = 1;
        foreach (var col in ColList)
        {
            DataRow drc = dtcols.NewRow();
            if (col.Name.ToString().Contains(Separator))
            {
                drc["name"] = col.Name.ToString().Trim().Split(new[] { Separator }, StringSplitOptions.None)[0];
                drc["ordd"] = col.Name.ToString().Trim().Split(new[] { Separator }, StringSplitOptions.None)[1];
            }
            else
            {
                drc["name"] = col.Name.ToString().Trim();
                drc["ordd"] = d;
            }
            dtcols.Rows.Add(drc);
            d++;

        }
        DataView dvc = dtcols.DefaultView;
        dvc.Sort = "ordd";
        dtcols = dvc.ToTable();

        //dt.Columns.Add(RowFields);
        foreach (string s in RowFields)
            dt.Columns.Add(s, Type.GetType(_SourceTable.Columns[s].DataType.ToString()));

        foreach (DataRow col in dtcols.Rows)
        {
            foreach (var s in DataFields)
            {
                dt.Columns.Add(col[0].ToString() + "_" + s, GetTypes(Aggregate));
            }
        }
        // Cretes the result columns.//
        foreach (var s in DataFields)
        {
            dt.Columns.Add("Tot_" + s, GetTypes(Aggregate));
        }
        foreach (var RowName in RowList)
        {
            DataRow row = dt.NewRow();
            string strFilter = string.Empty;

            foreach (string Field in RowFields)
            {
                row[Field] = RowName[Field];
                strFilter += " and " + Field + " = '" + RowName[Field].ToString() + "'";
            }
            strFilter = strFilter.Substring(5);
            decimal total = 0;
            foreach (DataRow col in dtcols.Rows)
            {
                string filter = strFilter;
                string[] strColValues = col[0].ToString().Split(Separator.ToCharArray(), StringSplitOptions.None);
                for (int i = 0; i < ColumnFields.Length; i++)
                    filter += " and " + ColumnFields[0] + " = '" + strColValues[0] + "'";
                foreach (var s in DataFields)
                {
                    var vall = GetData(filter, s, Aggregate);
                    if (vall == null) vall = 0;
                    row[col[0].ToString() + "_" + s] = vall;
                    row["Tot_" + s] = sgen.Make_decimal(row["Tot_" + s].ToString()) + sgen.Make_decimal(vall.ToString());                    
                }
                
            }
            dt.Rows.Add(row);
        }
        return dt;
    }


    private Type GetTypes(AggregateFunction Aggregate)
    {
        switch (Aggregate)
        {
            case AggregateFunction.Average:
                return typeof(Decimal);
            case AggregateFunction.Count:
                return typeof(Decimal);
            case AggregateFunction.Exists:
                return typeof(string);
            case AggregateFunction.First:
                return typeof(string);
            case AggregateFunction.Last:
                return typeof(string);
            case AggregateFunction.Max:
                return typeof(string);
            case AggregateFunction.Min:
                return typeof(Decimal);
            case AggregateFunction.Sum:
                return typeof(Decimal);
            default:
                return null;
        }
    }
    /// <summary>
    /// Retrives the data for matching RowField value and ColumnFields values with Aggregate function applied on them.
    /// </summary>
    /// <param name="Filter">DataTable Filter condition as a string</param>
    /// <param name="DataField">The column name which needs to spread out in Data Part of the Pivoted table</param>
    /// <param name="Aggregate">Enumeration to determine which function to apply to aggregate the data</param>
    /// <returns></returns>
    private object GetData(string Filter, string DataField, AggregateFunction Aggregate)
    {
        try
        {
            DataRow[] FilteredRows = _SourceTable.Select(Filter);
            object[] objList = FilteredRows.Select(x => x.Field<object>(DataField)).ToArray();

            switch (Aggregate)
            {
                case AggregateFunction.Average:
                    return GetAverage(objList);
                case AggregateFunction.Count:
                    return objList.Count();
                case AggregateFunction.Exists:
                    return (objList.Count() == 0) ? "False" : "True";
                case AggregateFunction.First:
                    return GetFirst(objList);
                case AggregateFunction.Last:
                    return GetLast(objList);
                case AggregateFunction.Max:
                    return GetMax(objList);
                case AggregateFunction.Min:
                    return GetMin(objList);
                case AggregateFunction.Sum:
                    return GetSum(objList);
                default:
                    return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    private object GetAverage(object[] objList)
    {
        return objList.Count() == 0 ? null : (object)(Convert.ToDecimal(GetSum(objList)) / objList.Count());
    }
    private object GetSum(object[] objList)
    {
        return objList.Count() == 0 ? null : (object)(objList.Aggregate(new decimal(), (x, y) => x += Convert.ToDecimal(y)));
    }
    private object GetFirst(object[] objList)
    {
        return (objList.Count() == 0) ? null : objList.First();
    }
    private object GetLast(object[] objList)
    {
        return (objList.Count() == 0) ? null : objList.Last();
    }
    private object GetMax(object[] objList)
    {
        return (objList.Count() == 0) ? null : objList.Max();
    }
    private object GetMin(object[] objList)
    {
        return (objList.Count() == 0) ? null : objList.Min();
    }
}

public enum AggregateFunction
{
    Count = 1,
    Sum = 2,
    First = 3,
    Last = 4,
    Average = 5,
    Max = 6,
    Min = 7,
    Exists = 8
}
