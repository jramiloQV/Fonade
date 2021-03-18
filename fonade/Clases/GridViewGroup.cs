//------------------------------------------------------------------------------------------
// Copyright © 2006 Agrinei Sousa [www.agrinei.com]
//
// Esse código fonte é fornecido sem garantia de qualquer tipo.
// Sinta-se livre para utilizá-lo, modificá-lo e distribuí-lo,
// inclusive em aplicações comerciais.
// É altamente desejável que essa mensagem não seja removida.
//------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// GroupEvent
/// </summary>
/// <param name="groupName">Name of the group.</param>
/// <param name="values">The values.</param>
/// <param name="row">The row.</param>
public delegate void GroupEvent(string groupName, object[] values, GridViewRow row);

/// <summary>
/// A class that represents a group consisting of a set of columns
/// </summary>
public class GridViewGroup
{
    #region Fields

    private string[] _columns;
    private object[] _actualValues;
    private int _quantity;
    private bool _automatic;
    private bool _hideGroupColumns;
    private bool _isSuppressGroup;
    private bool _generateAllCellsOnSummaryRow;
    private GridViewSummaryList mSummaries;

    #endregion

    #region Properties    
    /// <summary>
    /// Gets the columns.
    /// </summary>
    /// <value>
    /// The columns.
    /// </value>
    public string[] Columns
    {
        get { return _columns; }
    }

    /// <summary>
    /// Gets the actual values.
    /// </summary>
    /// <value>
    /// The actual values.
    /// </value>
    public object[] ActualValues
    {
        get { return _actualValues; }
    }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    /// <value>
    /// The quantity.
    /// </value>
    public int Quantity
    {
        get { return _quantity; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="GridViewGroup"/> is automatic.
    /// </summary>
    /// <value>
    ///   <c>true</c> if automatic; otherwise, <c>false</c>.
    /// </value>
    public bool Automatic
    {
        get { return _automatic; }
        set { _automatic = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [hide group columns].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [hide group columns]; otherwise, <c>false</c>.
    /// </value>
    public bool HideGroupColumns
    {
        get { return _hideGroupColumns; }
        set { _hideGroupColumns = value; }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is suppress group.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is suppress group; otherwise, <c>false</c>.
    /// </value>
    public bool IsSuppressGroup
    {
        get { return _isSuppressGroup; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [generate all cells on summary row].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [generate all cells on summary row]; otherwise, <c>false</c>.
    /// </value>
    public bool GenerateAllCellsOnSummaryRow
    {
        get { return _generateAllCellsOnSummaryRow; }
        set { _generateAllCellsOnSummaryRow = value; }
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name
    {
        get { return String.Join("+", this._columns); }
    }

    /// <summary>
    /// Gets the summaries.
    /// </summary>
    /// <value>
    /// The summaries.
    /// </value>
    public GridViewSummaryList Summaries
    {
        get { return mSummaries; }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GridViewGroup"/> class.
    /// </summary>
    /// <param name="cols">The cols.</param>
    /// <param name="isSuppressGroup">if set to <c>true</c> [is suppress group].</param>
    /// <param name="auto">if set to <c>true</c> [automatic].</param>
    /// <param name="hideGroupColumns">if set to <c>true</c> [hide group columns].</param>
    /// <param name="generateAllCellsOnSummaryRow">if set to <c>true</c> [generate all cells on summary row].</param>
    public GridViewGroup(string[] cols, bool isSuppressGroup, bool auto, bool hideGroupColumns, bool generateAllCellsOnSummaryRow)
    {
        this.mSummaries = new GridViewSummaryList();
        this._actualValues = null;
        this._quantity = 0;
        this._columns = cols;
        this._isSuppressGroup = isSuppressGroup;
        this._automatic = auto;
        this._hideGroupColumns = hideGroupColumns;
        this._generateAllCellsOnSummaryRow = generateAllCellsOnSummaryRow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridViewGroup"/> class.
    /// </summary>
    /// <param name="cols">The cols.</param>
    /// <param name="auto">if set to <c>true</c> [automatic].</param>
    /// <param name="hideGroupColumns">if set to <c>true</c> [hide group columns].</param>
    /// <param name="generateAllCellsOnSummaryRow">if set to <c>true</c> [generate all cells on summary row].</param>
    public GridViewGroup(string[] cols, bool auto, bool hideGroupColumns, bool generateAllCellsOnSummaryRow) : this( cols, false, auto, hideGroupColumns, generateAllCellsOnSummaryRow)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridViewGroup"/> class.
    /// </summary>
    /// <param name="cols">The cols.</param>
    /// <param name="auto">if set to <c>true</c> [automatic].</param>
    /// <param name="hideGroupColumns">if set to <c>true</c> [hide group columns].</param>
    public GridViewGroup(string[] cols, bool auto, bool hideGroupColumns) : this(cols, auto, hideGroupColumns, false)
    {
    }

    #endregion

    /// <summary>
    /// Sets the actual values.
    /// </summary>
    /// <param name="values">The values.</param>
    internal void SetActualValues(object[] values)
    {
        this._actualValues = values;
    }

    /// <summary>
    /// Determines whether the specified s contains summary.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>
    ///   <c>true</c> if the specified s contains summary; otherwise, <c>false</c>.
    /// </returns>
    public bool ContainsSummary(GridViewSummary s)
    {
        return mSummaries.Contains(s);
    }

    /// <summary>
    /// Adds the summary.
    /// </summary>
    /// <param name="s">The s.</param>
    public void AddSummary(GridViewSummary s)
    {
        if (this.ContainsSummary(s))
        {
            throw new Exception("Summary already exists in this group.");
        }

        if (!s.Validate())
        {
            throw new Exception("Invalid summary.");
        }

        //s._group = this;
        s.SetGroup(this);
        this.mSummaries.Add(s);
    }

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public void Reset()
    {
        this._quantity = 0;

        foreach (GridViewSummary s in mSummaries)
        {
            s.Reset();
        }
    }

    /// <summary>
    /// Adds the value to summaries.
    /// </summary>
    /// <param name="dataitem">The dataitem.</param>
    public void AddValueToSummaries(object dataitem)
    {
        this._quantity++;

        foreach (GridViewSummary s in mSummaries)
        {
            s.AddValue(DataBinder.Eval(dataitem, s.Column));
        }
    }

    /// <summary>
    /// Calculates the summaries.
    /// </summary>
    public void CalculateSummaries()
    {
        foreach (GridViewSummary s in mSummaries)
        {
            s.Calculate();
        }
    }
}
