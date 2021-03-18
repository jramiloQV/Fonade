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
