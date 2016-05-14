<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="sample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Stock quote and chart from Yahoo in C#</title>

    <!-- Google News API -->
    <script type="text/javascript" src="Scripts/jquery-2.1.0.min.js"></script>

    <script>
        //For Enter Key Press Event
        function runScript(e) {
            if (e.keyCode == 13) {

                //Call GetNews Function
                GetNews();

                return false;
            }
        }

        //Function for GetNews Usinf Ajax Post Method
        function GetNews() {

            //Set FadeIn for Progressive Div
            $("#ProgressiveDiv").fadeIn();

            //Create Ajax Post Method
            $.ajax({
                type: "POST", // Ajax Mehod
                url: "Default.aspx/GetNewsContent",  //Page URL / Method name
                data: "{'SearchPhrase':'" + document.getElementById("txtSubject").value + "'}", // Pass Parameters
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (data) { // Function call on success

                    $("#DivNews").empty(); // Set Div Empty

                    for (var i = 0; i < data.d.length; i++)
                        $("#DivNews").append("<article><h3><a href=" + data.d[i].link + " target=_blank>" + data.d[i].title + "</a></h3><p>" + data.d[i].PubDate + "</p><div>" + data.d[i].Description + "</div></article>");

    // "<tr><td><B style='color:Red'>" + data.d[i].title + "</B></a></td><td>Date: " + data.d[i].PubDate + "</td></tr><tr><td>" + data.d[i].Description + "</td></tr>"

                    //set fadeOut for ProgressiveDiv
                    $("#ProgressiveDiv").fadeOut(500);
                },

                error: function (result) { // Function call on failure or error
                    alert(result.d);
                }


            });


        }
    </script>

    <style type="text/css">
        .classname
        {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
            background: -moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
            background-color: #ededed;
            -webkit-border-top-left-radius: 6px;
            -moz-border-radius-topleft: 6px;
            border-top-left-radius: 6px;
            -webkit-border-top-right-radius: 6px;
            -moz-border-radius-topright: 6px;
            border-top-right-radius: 6px;
            -webkit-border-bottom-right-radius: 6px;
            -moz-border-radius-bottomright: 6px;
            border-bottom-right-radius: 6px;
            -webkit-border-bottom-left-radius: 6px;
            -moz-border-radius-bottomleft: 6px;
            border-bottom-left-radius: 6px;
            text-indent: 0;
            border: 1px solid #dcdcdc;
            display: inline-block;
            color: #777777;
            font-family: arial;
            font-size: 15px;
            font-weight: bold;
            font-style: normal;
            height: 25px;
            line-height: 50px;
            width: 100px;
            text-decoration: none;
            text-align: center;
            text-shadow: 1px 1px 0px #ffffff;
        }
        .classname:hover
        {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
            background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
            background-color: #dfdfdf;
        }
        .classname:active
        {
            position: relative;
            top: 1px;
        }
        .textbox
        {
            background: #FFF url(http://html-generator.weebly.com/files/theme/input-text-9.png) no-repeat 4px 4px;
            border: 1px solid #999;
            outline: 0;
            padding-left: 25px;
            height: 25px;
            width: 275px;
        }
        .style1
        {
            height: 61px;
        }
        #ProgressiveDiv
        {
            width: 100%;
            height: 100%;
            display: none;
            opacity: 0.4;
            position: fixed;
            top: 0px;
            left: 0px;
            vertical-align: middle;
        }
        #ProgressiveDiv article
        {
            margin-bottom: 80px;
        }
    </style>
    
    <!-- Yahoo Finance API -->
    <script  type="text/javascript" language="JavaScript">
        /// <summary>
        /// This function will be called when user clicks the Get Quotes button.
        /// </summary>
        /// <returns>Always return false.</returns>
        function SendRequest()
        {
            var txtSymbol = document.getElementById("txtSymbol");
            // Refresh the page.
            window.location = "default.aspx?s=" + txtSymbol.value;
            return false;
        }

        /// <summary>
        /// The functyion will be called when a keyboard key is pressed in the textbox.
        /// </summary>
        /// <param name="e">Onkeypress event.</param>
        /// <returns>Return true if user presses Enter key; otherwise false.</returns>        
        function CheckEnter(e)
        {
            if ((e.keyCode && e.keyCode == 13) || (e.which && e.which == 13))
                // Enter is pressed in the textbox.
                return SendRequest();
            return true;
        }

        /// <summary>
        /// The function will be called when user changes the chart type to another type.
        /// </summary>
        /// <param name="type">Chart type.</param>
        /// <param name="num">Stock number.</param>
        /// <param name="symbol">Stock symobl.</param>
        function changeChart(type, num, symbol)
        {
            // All the DIVs are inside the main DIV and defined in the code-behind class.
            var div1d=document.getElementById("div1d_"+num);
            var div5d = document.getElementById("div5d_" + num);
            var div3m = document.getElementById("div3m_" + num);
            var div6m = document.getElementById("div6m_" + num);
            var div1y = document.getElementById("div1y_" + num);
            var div2y = document.getElementById("div2y_" + num);
            var div5y = document.getElementById("div5y_" + num);
            var divMax = document.getElementById("divMax_" + num);
            var divChart = document.getElementById("imgChart_" + num);
            // Set innerHTML property.
            div1d.innerHTML = "1d";
            div5d.innerHTML="5d";
            div3m.innerHTML="3m";
            div6m.innerHTML="6m";
            div1y.innerHTML="1y";
            div2y.innerHTML="2y";
            div5y.innerHTML="5y";
            divMax.innerHTML="Max";
            // Use a random number to defeat cache.
            var rand_no = Math.random();
            rand_no = rand_no * 100000000;
            // Display the stock chart.
            switch(type)
            {
            case 1: // 5 days
                div5d.innerHTML="<b>5d</b>";
                divChart.src = "http://ichart.finance.yahoo.com/w?s=" + symbol + "&" + rand_no;
                break;
            case 2: // 3 months 
                div3m.innerHTML="<b>3m</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/3m/" + symbol + "?" + rand_no;
                break;
            case 3: // 6 months 
                div6m.innerHTML = "<b>6m</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/6m/" + symbol + "?" + rand_no;
                break;
            case 4: // 1 year
                div1y.innerHTML = "<b>1y</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/1y/" + symbol + "?" + rand_no;
                break;
            case 5: // 2 years 
                div2y.innerHTML = "<b>2y</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/2y/" + symbol + "?" + rand_no;
                break;
            case 6: // 5 years
                div5y.innerHTML = "<b>5y</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/5y/" + symbol + "?" + rand_no;
                break;
            case 7: // Max
                divMax.innerHTML = "<b>msx</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/my/" + symbol + "?" + rand_no;
                break;
            case 0: // 1 day
            default:                
                div1d.innerHTML = "<b>1d</b>";
                divChart.src = "http://ichart.finance.yahoo.com/b?s=" + symbol + "&" + rand_no;
                break;
            }
        }
    </script>    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="0" cellpadding="0" style="width: 43%">
            <tr valign="top">                                            
                <td style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #000; text-decoration: none;">
                    <input type="text" value="" id="txtSymbol" runat="server" onkeypress="return CheckEnter(event);" />
                    <input type="button" value="Get Quotes" onclick="return SendRequest();" />
                    <br />
                    <span style="font-family: Arial, Helvetica, sans-serif; font-size: 11px;	color: #666;">
                        e.g. "YHOO or MSFT GOOG AAPL TSLA"
                    </span>
                    <%if (m_symbol != "") {%>                        
                        <div id="divService" runat="server">
                        <!-- Main DIV: this DIV contains contains text and DIVs that displays stock quotes and chart. -->
                        </div>
                    <%}%>                                                                                            
                </td>    
            </tr>
        </table>    
    </div>

        <!--Form for Google News API-->
        <div>
        <table>
            <tr>
                <td align="center" class="style1">
                    <h3>
                        Welcome to My news Portal</h3>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtSubject" CssClass="textbox" onkeypress="return runScript(event)" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <h6 style="height: 35px">
                        By: Ilia Valchenko</h6>
                </td>
            </tr>
        </table>
        <div id="DivNews">
        </div>

    </div>
    <%--This Div is For Binding News--%>
    <div id="ProgressiveDiv" style="padding-left: 500px">
        <img src="Image/loading.gif" />
    </div>

    <!-- End of news api's code--> 

    </form>
</body>
</html>
