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
/// Summary description for GridViewGroupList
/// </summary>
public class GridViewGroupList : List<GridViewGroup>
{
    /// <summary>
    /// Gets the <see cref="GridViewGroup"/> with the specified name.
    /// </summary>
    /// <value>
    /// The <see cref="GridViewGroup"/>.
    /// </value>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public GridViewGroup this[string name]
    {
        get { return this.FindGroupByName(name); }
    }

    /// <summary>
    /// Finds the name of the group by.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public GridViewGroup FindGroupByName(string name)
    {
        foreach (GridViewGroup g in this)
        {
            if (g.Name.ToLower() == name.ToLower()) return g;
        }

        return null;
    }
}
