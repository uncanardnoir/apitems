<%@ Page Title="Item Trees" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemTrees.aspx.cs" Inherits="ItemTrees.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jit-yc.js"></script>
    <link href="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/css/select2.min.css" rel="stylesheet" />
    <script src="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/js/select2.min.js"></script>
    <script src="Scripts/DrawTree.js"></script>
    <script src="Scripts/LoadData.js"></script>
    <script src="Scripts/Utilities.js"></script>

    <script>
        jQuery(document).ready(InitializeTrees);
    </script>

    <div id="left-tree" style="position:absolute; height: 100%; width: 100%;">
        <div id="left-infovis" style="position:absolute; height: 100%; width: 50%; left:0px; top:0px">
            <span id="left-tree-champion" style="background:rgba(51, 51, 51, 0.5);padding: 5px;position:absolute;left:3px;top:3px;color:#ffffff; text-shadow: 2px 2px #000; z-index:301">
                <select id="left-patch-select" style="width:200px">
                    <option value="5.11">Patch 5.11</option>
                    <option value="5.14">Patch 5.14</option>
                </select><br />
                <select id="left-champ-select" style="width:200px">
                    <option value="999">(All mages)</option>
                    <option value="103">Ahri</option>
                    <option value="84">Akali</option>
                    <option value="34">Anivia</option>
                    <option value="1">Annie</option>
                    <option value="268">Azir</option>
                    <option value="63">Brand</option>
                    <option value="69">Cassiopeia</option>
                    <option value="31">Cho'gath</option>
                    <option value="131">Diana</option>
                    <option value="245">Ekko</option>
                    <option value="60">Elise</option>
                    <option value="81">Ezreal</option>
                    <option value="9">Fiddlesticks</option>
                    <option value="105">Fizz</option>
                    <option value="3">Galio</option>
                    <option value="74">Heimerdinger</option>
                    <option value="43">Karma</option>
                    <option value="30">Karthus</option>
                    <option value="38">Kassadin</option>
                    <option value="55">Katarina</option>
                    <option value="10">Kayle</option>
                    <option value="85">Kennen</option>
                    <option value="96">Kog'Maw</option>
                    <option value="7">Leblanc</option>
                    <option value="127">Lissandra</option>
                    <option value="117">Lulu</option>
                    <option value="99">Lux</option>
                    <option value="90">Malzahar</option>
                    <option value="82">Mordekaiser</option>
                    <option value="25">Morgana</option>
                    <option value="76">Nidalee</option>
                    <option value="61">Orianna</option>
                    <option value="68">Rumble</option>
                    <option value="13">Ryze</option>
                    <option value="50">Swain</option>
                    <option value="134">Syndra</option>
                    <option value="4">Twisted Fate</option>
                    <option value="45">Veigar</option>
                    <option value="161">Vel'koz</option>
                    <option value="112">Viktor</option>
                    <option value="8">Vladimir</option>
                    <option value="101">Xerath</option>
                    <option value="115">Ziggs</option>
                    <option value="26">Zilean</option>
                    <option value="143">Zyra</option>

                </select><br />
            <span id="left-tree-champion-title" style="font-style:italic"></span></span>
            <span style="background:rgba(51, 51, 51, 0.5);padding: 5px;position:relative;left:-3px;top:3px;float:right; z-index:300">
                <span id="left-tree-details" style="color:#ffffff; text-shadow: 2px 2px #000;"></span>
            </span>
            <span id="left-tree-item-details" style="background:rgba(51, 51, 51, 0.5);padding: 5px;position:absolute;bottom:3px;z-index:300;color:#ffffff; text-shadow: 2px 2px #000">
            </span>
            <div style="position:absolute; height:100%; width:100%; z-index:-100; left:0px; top:0px">
                <img id="left-bg" style="height: 100%; width: 100%;"/>
            </div>
        </div>
        <div id="right-infovis" style="position:absolute; height: 100%; width: 50%; right:0px; top:0px">
            <span id="right-tree-champion" style="background:rgba(51, 51, 51, 0.5);padding: 5px;position:absolute;left:3px;top:3px;color:#ffffff; text-shadow: 2px 2px #000; z-index:301">
                <select id="right-patch-select" style="width:200px">
                    <option value="5.11">Patch 5.11</option>
                    <option value="5.14">Patch 5.14</option>
                </select><br />
                <select id="right-champ-select" style="width:200px">
                    <option value="999">(All mages)</option>
                    <option value="103">Ahri</option>
                    <option value="84">Akali</option>
                    <option value="34">Anivia</option>
                    <option value="1">Annie</option>
                    <option value="268">Azir</option>
                    <option value="63">Brand</option>
                    <option value="69">Cassiopeia</option>
                    <option value="31">Cho'gath</option>
                    <option value="131">Diana</option>
                    <option value="245">Ekko</option>
                    <option value="60">Elise</option>
                    <option value="81">Ezreal</option>
                    <option value="9">Fiddlesticks</option>
                    <option value="105">Fizz</option>
                    <option value="3">Galio</option>
                    <option value="74">Heimerdinger</option>
                    <option value="43">Karma</option>
                    <option value="30">Karthus</option>
                    <option value="38">Kassadin</option>
                    <option value="55">Katarina</option>
                    <option value="10">Kayle</option>
                    <option value="85">Kennen</option>
                    <option value="96">Kog'Maw</option>
                    <option value="7">Leblanc</option>
                    <option value="127">Lissandra</option>
                    <option value="117">Lulu</option>
                    <option value="99">Lux</option>
                    <option value="90">Malzahar</option>
                    <option value="82">Mordekaiser</option>
                    <option value="25">Morgana</option>
                    <option value="76">Nidalee</option>
                    <option value="61">Orianna</option>
                    <option value="68">Rumble</option>
                    <option value="13">Ryze</option>
                    <option value="50">Swain</option>
                    <option value="134">Syndra</option>
                    <option value="4">Twisted Fate</option>
                    <option value="45">Veigar</option>
                    <option value="161">Vel'koz</option>
                    <option value="112">Viktor</option>
                    <option value="8">Vladimir</option>
                    <option value="101">Xerath</option>
                    <option value="115">Ziggs</option>
                    <option value="26">Zilean</option>
                    <option value="143">Zyra</option>

                </select><br />
            <span id="right-tree-champion-title" style="font-style:italic"></span></span>
            <span style="background:rgba(51, 51, 51, 0.5);padding: 5px;position:relative;left:-3px;top:3px;float:right; z-index:300">
                <span id="right-tree-details" style="color:#ffffff; text-shadow: 2px 2px #000;"></span>
            </span>
            <span id="right-tree-item-details" style="background:rgba(51, 51, 51, 0.5);padding: 5px;position:absolute;bottom:3px;z-index:300;color:#ffffff; text-shadow: 2px 2px #000">
            </span>
            <div style="position:absolute; height:100%; width:100%; z-index:-100; left:0px; top:0px">
                <img id="right-bg" style="height: 100%; width: 100%;"/>
            </div>
        </div>
        <span id="similarity-score" style="background:rgba(51, 51, 51, 0.5);padding: 5px;position:absolute;bottom:3px;z-index:300;color:#ffffff; text-shadow: 2px 2px #000">
            </span>
    </div>
</asp:Content>
