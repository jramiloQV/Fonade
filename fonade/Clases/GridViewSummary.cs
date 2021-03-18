//------------------------------------------------------------------------------------------
// Copyright © 2006 Agrinei Sousa [www.agrinei.com]
//
// Esse código fonte é fornecido sem garantia de qualquer tipo.
// Sinta-se livre para utilizá-lo, modificá-lo e distribuí-lo,
// inclusive em aplicações comerciais.
// É altamente desejável que essa mensagem não seja removida.
//------------------------------------------------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// SummaryOperation
/// </summary>
public enum SummaryOperation {
    /// <summary>
    /// The sum
    /// </summary>
    Sum,
    /// <summary>
    /// The average
    /// </summary>
    Avg,
    /// <summary>
    /// The count
    /// </summary>
    Count,
    /// <summary>
    /// The custom
    /// </summary>
    Custom
}
/// <summary>
/// CustomSummaryOperation
/// </summary>
/// <param name="column">The column.</param>
/// <param name="groupName">Name of the group.</param>
/// <param name="value">The value.</param>
public delegate void CustomSummaryOperation(string column, string groupName, object value);
/// <summary>
/// SummaryResultMethod
/// </summary>
/// <param name="column">The column.</param>
/// <param name="groupName">Name of the group.</param>
/// <returns></returns>
public delegate object SummaryResultMethod(string column, string groupName);

/// <summary>
/// A class that represents a summary operation defined to a column
/// </summary>
public class GridViewSummary
{
    #region Fields

    private string _column;
    private SummaryOperation _operation;
    private CustomSummaryOperation _customOperation;
    private SummaryResultMethod _getSummaryMethod;
    private GridViewGroup _group;
    private object _value;
    private string _formatString;
    private int _quantity;
    private bool _automatic;
    private bool _treatNullAsZero;

    #endregion

    #region Properties    
    /// <summary>
    /// Gets the column.
    /// </summary>
    /// <value>
    /// The column.
    /// </value>
    public string Column
    {
        get { return _column; }
    }

    /// <summary>
    /// Gets the operation.
    /// </summary>
    /// <value>
    /// The operation.
    /// </value>
    public SummaryOperation Operation
    {
        get { return _operation; }
    }

    /// <summary>
    /// Gets the custom operation.
    /// </summary>
    /// <value>
    /// The custom operation.
    /// </value>
    public CustomSummaryOperation CustomOperation
    {
        get { return _customOperation; }
    }
    /// <summary>
    /// Gets the get summary method.
    /// </summary>
    /// <value>
    /// The get summary method.
    /// </value>
    public SummaryResultMethod GetSummaryMethod
    {
        get { return _getSummaryMethod; }
    }

    /// <summary>
    /// Gets the group.
    /// </summary>
    /// <value>
    /// The group.
    /// </value>
    public GridViewGroup Group
    {
        get { return _group; }
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public object Value
    {
        get { return _value; }
    }

    /// <summary>
    /// Gets or sets the format string.
    /// </summary>
    /// <value>
    /// The format string.
    /// </value>
    public string FormatString
    {
        get { return _formatString; }
        set { _formatString = value; }
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
    /// Gets or sets a value indicating whether this <see cref="GridViewSummary"/> is automatic.
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
    /// Gets or sets a value indicating whether [treat null as zero].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [treat null as zero]; otherwise, <c>false</c>.
    /// </value>
    public bool TreatNullAsZero
    {
        get { return _treatNullAsZero; }
        set { _treatNullAsZero = value; }
    }

    #endregion

    #region Constructors

    private GridViewSummary(string col, GridViewGroup grp)
    {
        this._column = col;
        this._group = grp;
        this._value = null;
        this._quantity = 0;
        this._automatic = true;
        this._treatNullAsZero = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridViewSummary"/> class.
    /// </summary>
    /// <param name="col">The col.</param>
    /// <param name="formatString">The format string.</param>
    /// <param name="op">The op.</param>
    /// <param name="grp">The GRP.</param>
    public GridViewSummary(string col, string formatString, SummaryOperation op, GridViewGroup grp) : this(col, grp)
    {
        this._formatString = formatString;
        this._operation = op;
        this._customOperation = null;
        this._getSummaryMethod = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridViewSummary"/> class.
    /// </summary>
    /// <param name="col">The col.</param>
    /// <param name="op">The op.</param>
    /// <param name="grp">The GRP.</param>
    public GridViewSummary(string col, SummaryOperation op, GridViewGroup grp) : this(col, String.Empty, op, grp)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridViewSummary"/> class.
    /// </summary>
    /// <param name="col">The col.</param>
    /// <param name="formatString">The format string.</param>
    /// <param name="op">The op.</param>
    /// <param name="getResult">The get result.</param>
    /// <param name="grp">The GRP.</param>
    public GridViewSummary(string col, string formatString, CustomSummaryOperation op, SummaryResultMethod getResult, GridViewGroup grp) : this(col, grp)
    {
        this._formatString = formatString;
        this._operation = SummaryOperation.Custom;
        this._customOperation = op;
        this._getSummaryMethod = getResult;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GridViewSummary"/> class.
    /// </summary>
    /// <param name="col">The col.</param>
    /// <param name="op">The op.</param>
    /// <param name="getResult">The get result.</param>
    /// <param name="grp">The GRP.</param>
    public GridViewSummary(string col, CustomSummaryOperation op, SummaryResultMethod getResult, GridViewGroup grp) : this(col, String.Empty, op, getResult, grp)
    {
    }

    #endregion

    internal void SetGroup(GridViewGroup g)
    {
        this._group = g;
    }

    /// <summary>
    /// Validates this instance.
    /// </summary>
    /// <returns></returns>
    public bool Validate()
    {
        if (this._operation == SummaryOperation.Custom)
        {
            return (this._customOperation != null && this._getSummaryMethod != null);
        }
        else
        {
            return (this._customOperation == null && this._getSummaryMethod == null);
        }
    }

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public void Reset()
    {
        this._quantity = 0;
        this._value = null;
    }

    /// <summary>
    /// Adds the value.
    /// </summary>
    /// <param name="newValue">The new value.</param>
    public void AddValue(object newValue)
    {
        // Increment to (later) calc the Avg or for other calcs
        this._quantity++;

        // Built-in operations
        if (this._operation == SummaryOperation.Sum || this._operation == SummaryOperation.Avg)
        {
            if (this._value == null)
                this._value = newValue;
            else
                this._value = PerformSum(this._value, newValue);
        }
        else
        {
            // Custom operation
            if (this._customOperation != null)
            {
                // Call the custom operation
                if ( this._group != null )
                    this._customOperation(this._column, this._group.Name, newValue);
                else
                    this._customOperation(this._column, null, newValue);
            }
        }
    }

    /// <summary>
    /// Calculates this instance.
    /// </summary>
    public void Calculate()
    {
        if (this._operation == SummaryOperation.Avg)
        {
            this._value = PerformDiv(this._value, this._quantity);
        }
        if (this._operation == SummaryOperation.Count)
        {
            this._value = this._quantity;
        }
        else if (this._operation == SummaryOperation.Custom)
        {
            if (this._getSummaryMethod != null)
            {
                this._value = this._getSummaryMethod(this._column, null);
            }
        }
        // if this.Operation == SummaryOperation.Avg
        // this.Value already contains the correct value
    }
    
    #region Built-in Summary Operations

    private object PerformSum(object a, object b)
    {
        object zero = 0;

        if (a == null)
        {
            if (_treatNullAsZero)
                a = 0;
            else
                return null;
        }

        if (b == null)
        {
            if (_treatNullAsZero)
                b = 0;
            else
                return null;
        }

        // Convert to proper type before add
        switch (a.GetType().FullName)
        {
            case "System.Int16": return Convert.ToInt16(a) + Convert.ToInt16(b);
            case "System.Int32": return Convert.ToInt32(a) + Convert.ToInt32(b);
            case "System.Int64": return Convert.ToInt64(a) + Convert.ToInt64(b);
            case "System.UInt16": return Convert.ToUInt16(a) + Convert.ToUInt16(b);
            case "System.UInt32": return Convert.ToUInt32(a) + Convert.ToUInt32(b);
            case "System.UInt64": return Convert.ToUInt64(a) + Convert.ToUInt64(b);
            case "System.Single": return Convert.ToSingle(a) + Convert.ToSingle(b);
            case "System.Double": return Convert.ToDouble(a) + Convert.ToDouble(b);
            case "System.Decimal": return Convert.ToDecimal(a) + Convert.ToDecimal(b);
            case "System.Byte": return Convert.ToByte(a) + Convert.ToByte(b);
            case "System.String": return a.ToString() + b.ToString();
        }

        return null;
    }

    private object PerformDiv(object a, int b)
    {
        object zero = 0;

        if (a == null)
        {
            return (_treatNullAsZero ? zero : null);
        }

        // Don't raise an exception, just return null
        if (b == 0)
        {
            return null;
        }

        // Convert to proper type before div
        switch (a.GetType().FullName)
        {
            case "System.Int16": return Convert.ToInt16(a) / b;
            case "System.Int32": return Convert.ToInt32(a) / b;
            case "System.Int64": return Convert.ToInt64(a) / b;
            case "System.UInt16": return Convert.ToUInt16(a) / b;
            case "System.UInt32": return Convert.ToUInt32(a) / b;
            case "System.Single": return Convert.ToSingle(a) / b;
            case "System.Double": return Convert.ToDouble(a) / b;
            case "System.Decimal": return Convert.ToDecimal(a) / b;
            case "System.Byte": return Convert.ToByte(a) / b;
            // Operator '/' cannot be applied to operands of type 'ulong' and 'int'
            //case "System.UInt64": return Convert.ToUInt64(a) / b;
        }

        return null;
    }

    #endregion

}
