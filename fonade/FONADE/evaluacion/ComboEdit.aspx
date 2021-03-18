<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComboEdit.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ComboEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css" media="screen">
	div.combobox	{font-family: Tahoma;font-size: 12px}
	div.combobox	{position: relative;zoom: 1}
	div.combobox	div.dropdownlist	{display: none;width: 200px;
		border: solid 1px #000;background-color: #fff;
		height: 200px;overflow: auto;position: absolute;
		top: 18px;left: 0px;}
	div.combobox	.dropdownlist	a	{display: block;text-decoration: none;
		color: #000;padding: 1px;height: 1em;cursor: default}
	div.combobox	.dropdownlist	a.light	{color: #fff;
		background-color: #007}
	div.combobox	.dropdownlist, input {font-family: Tahoma;font-size: 12px;}
	div.combobox	input {float: left;width: 182px;
		border: solid 1px #ccc;height: 15px}
	div.combobox	span	{border: solid 1px #ccc;background: #eee;
		width: 16px;height: 17px;
		float: left;text-align: center;border-left: none;cursor: default}
</style>
</head>
  <script type="text/javascript">
      function AsignaValorHidden()
      {
          //var hidCmb = document.getElementById("hidCombo")
          //hidCmb.value = valor
          document.forms["form1"].submit();
      }
 </script>
<body>
    <form id="form1" runat="server">
    <div><asp:ScriptManager ID="ScriptManager1"  runat="server"/>
    <script type="text/javascript">
             Sys.WebForms.PageRequestManager.instance.add_endRequest(Notify);
    </script>
    <div>Posición Arancelaria:<div class="combobox">
	<input type="text" name="comboboxfieldname" id="cb_identifier" style="width:60%" runat="server"/><span>▼</span>
	<div id="cmbArancelaria" class="dropdownlist" style="width:60%" runat="server">
        
    </div>
   </div>
        <script  type="text/javascript" charset="utf-8"src="../../Scripts/Fonade/combobox-min.js"></script>
        <script  type="text/javascript" charset="utf-8"src="../../Scripts/Fonade/combobox-min.js"></script>
        <script type="text/javascript" charset="utf-8">
            var no = new ComboBox('cb_identifier');
        </script>
    </div>
    </div>
      <input id="hidCombo" type="hidden"  value="0" runat="server" />
    </form>
        <p>
            <input id="guardar" type="button" onclick="AsignaValorHidden()" value="Guardar" /></p>
        </body>
</html>
