//------------------------------------------------------------------------------------------
// Copyright � 2006 Agrinei Sousa [www.agrinei.com]
//
// Esse c�digo fonte � fornecido sem garantia de qualquer tipo.
// Sinta-se livre para utiliz�-lo, modific�-lo e distribu�-lo,
// inclusive em aplica��es comerciais.
// � altamente desej�vel que essa mensagem n�o seja removida.
//------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for GridViewSummaryList
/// </summary>
public class GridViewSummaryList : List<GridViewSummary>
{
    /// <summary>
    /// Gets the <see cref="GridViewSummary"/> with the specified name.
    /// </summary>
    /// <value>
    /// The <see cref="GridViewSummary"/>.
    /// </value>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public GridViewSummary this[string name]
    {
        get { return this.FindSummaryByColumn(name); }
    }

    /// <summary>
    /// Finds the summary by column.
    /// </summary>
    /// <param name="columnName">Name of the column.</param>
    /// <returns></returns>
    public GridViewSummary FindSummaryByColumn(string columnName)
    {
        foreach (GridViewSummary s in this)
        {
            if (s.Column.ToLower() == columnName.ToLower()) return s;
        }

        return null;
    }
}
