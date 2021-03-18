<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ejemplo.aspx.cs" Inherits="Fonade.RegistroUnico.Ejemplo" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="Styles/js/jquery-3.4.1.min.js"></script>
    <link href="Styles/css/bootstrap.min.css" rel="stylesheet" />
    <script src="Styles/js/bootstrap.min.js"></script>
    <title></title>
    <script>
        var code;
        function createCaptcha() {
            //clear the contents of captcha div first 
            document.getElementById('captcha').innerHTML = "";
            var charsArray =
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@!#$%^&*";
            var lengthOtp = 6;
            var captcha = [];
            for (var i = 0; i < lengthOtp; i++) {
                //below code will not allow Repetition of Characters
                var index = Math.floor(Math.random() * charsArray.length + 1); //get the next character from the array
                if (captcha.indexOf(charsArray[index]) == -1)
                    captcha.push(charsArray[index]);
                else i--;
            }
            var canv = document.createElement("canvas");
            canv.id = "captcha";
            canv.width = 100;
            canv.height = 50;
            var ctx = canv.getContext("2d");
            ctx.font = "25px Georgia";
            ctx.strokeText(captcha.join(""), 0, 30);
            //storing captcha so that can validate you can save it somewhere else according to your specific requirements
            code = captcha.join("");
            document.getElementById("captcha").appendChild(canv); // adds the canvas to the body element
        }
        function validateCaptcha() {
            event.preventDefault();
            debugger
            if (document.getElementById("cpatchaTextBox").value == code) {
                procesarDatos();
            } else {
                alert("Invalid Captcha. try Again");
                createCaptcha();
            }
        }
        function procesarDatos() {
            
            __doPostBack('GuardarDatos');
        } 

    </script>
    <style>
        .cpatchaTextBox {
            padding: 12px 20px;
            display: inline-block;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }

        .botonCaptcha {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 12px 30px;
            text-decoration: none;
            margin: 4px 2px;
            cursor: pointer;
        }

        canvas {
            /*prevent interaction with the canvas*/
            pointer-events: none;
        }
    </style>

</head>
<body onload="createCaptcha()">
    <form id="form1" runat="server">

        <!-- Button trigger modal -->
        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModalScrollable">
            Launch demo modal
        </button>
        <asp:Label ID="lblprueba" runat="server" Text="pruebasinarrancar"></asp:Label>

        <!-- Modal -->
        <div class="modal fade" id="exampleModalScrollable" tabindex="-1" role="dialog" aria-labelledby="exampleModalScrollableTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-scrollable" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalScrollableTitle">Modal title</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <div id="captcha">
                        </div>
                        <input type="text" placeholder="Captcha" id="cpatchaTextBox" />
                        <%--<button id="botonCaptcha" type="submit">Submit</button>--%>
                        
                        
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        <asp:Button ID="Button1" runat="server" Text="Button" OnClientClick="validateCaptcha()" OnClick="Button1_Click1"  />

                    </div>
                </div>
            </div>
        </div>
        

    </form>
</body>
</html>

