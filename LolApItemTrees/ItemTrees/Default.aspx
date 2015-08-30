<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ItemTrees._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="analysis-background">
        <div class="analysis-textbox"><div style="text-align: center; padding-top: 40px; padding-bottom: 40px;"><img src="Images/banner.gif" /><br /><br /><br /><a href="ItemTrees" target="LeagueOfLegendsApItemTrees"><img src="Images/preview.png" /></a></div>
<div style="text-align: center; font-size: 150%;"><a href="ItemTrees" target="LeagueOfLegendsApItemTrees">» Launch League of Legends: AP Item Trees «</a></div>
            <hr />
<span class="text-header">Overview</span><br />
This website was created in response to the Riot API Challenge 2.0. We analyzed 400,000 games of League of Legends, spanning all 10 servers, including both Ranked and Normal on Summoner’s Rift on patches 5.11 and 5.14. The goal was to determine differences in AP item uses between patch 5.11 and 5.14. To help us visualize the differences, we created League of Legends AP Item Trees, a web application which dynamically generates the most common build paths for each mage on both patches.<br /><br />
These trees include the following features:<br /><ul>
	<li>Popularity of each build path, visually displayed by node size/edge width and with text</li>
	<li>Win rate of each build path</li>
	<li>Average completion time for each build path</li>
	<li>List of most common items built for each champion</li>
	<li>Side-by-side comparison of two champions, for quick comparison</li>
	<li>Build percentage differentials between two champions</li>
	<li>A similarity score, between 0-100, representing how similar both champions build</li></ul>
When viewing the trees, use the drop-down menus to change champions/patches, and click on item nodes to view statistics about that path.<br />
            <hr />
<span class="text-header">How were the trees created?</span><br />
To build the trees, we first download and analyze the set of 400,000 games provided by Riot as part of the API challenge. We identified 45 champions as being primary users of AP items, and focused our analysis exclusively on these champions:<br /><br />
<div class="text-center"><img src="Icons\c1.png" class="medium-icon" /><img src="Icons\c10.png" class="medium-icon" /><img src="Icons\c101.png" class="medium-icon" /><img src="Icons\c103.png" class="medium-icon" /><img src="Icons\c105.png" class="medium-icon" /><img src="Icons\c112.png" class="medium-icon" /><img src="Icons\c115.png" class="medium-icon" /><br /><img src="Icons\c117.png" class="medium-icon" /><img src="Icons\c127.png" class="medium-icon" /><img src="Icons\c13.png" class="medium-icon" /><img src="Icons\c131.png" class="medium-icon" /><img src="Icons\c134.png" class="medium-icon" /><img src="Icons\c143.png" class="medium-icon" /><img src="Icons\c161.png" class="medium-icon" /><br /><img src="Icons\c245.png" class="medium-icon" /><img src="Icons\c25.png" class="medium-icon" /><img src="Icons\c26.png" class="medium-icon" /><img src="Icons\c268.png" class="medium-icon" /><img src="Icons\c3.png" class="medium-icon" /><img src="Icons\c30.png" class="medium-icon" /><img src="Icons\c31.png" class="medium-icon" /><br /><img src="Icons\c34.png" class="medium-icon" /><img src="Icons\c38.png" class="medium-icon" /><img src="Icons\c4.png" class="medium-icon" /><img src="Icons\c43.png" class="medium-icon" /><img src="Icons\c45.png" class="medium-icon" /><img src="Icons\c50.png" class="medium-icon" /><img src="Icons\c55.png" class="medium-icon" /><br /><img src="Icons\c60.png" class="medium-icon" /><img src="Icons\c61.png" class="medium-icon" /><img src="Icons\c63.png" class="medium-icon" /><img src="Icons\c68.png" class="medium-icon" /><img src="Icons\c69.png" class="medium-icon" /><img src="Icons\c7.png" class="medium-icon" /><img src="Icons\c74.png" class="medium-icon" /><br /><img src="Icons\c76.png" class="medium-icon" /><img src="Icons\c8.png" class="medium-icon" /><img src="Icons\c81.png" class="medium-icon" /><img src="Icons\c82.png" class="medium-icon" /><img src="Icons\c84.png" class="medium-icon" /><img src="Icons\c85.png" class="medium-icon" /><img src="Icons\c9.png" class="medium-icon" /><br /><img src="Icons\c90.png" class="medium-icon" /><img src="Icons\c96.png" class="medium-icon" /><img src="Icons\c99.png" class="medium-icon" /></div>
            <br /> Many of these champions could be played in different roles (ADC Ezreal, ADC Kog’maw, Support Annie, etc.) but we are primarily interested in the ones that build pure AP, so we filtered out champions identified by Riot in a game as a bot lane or duo support champion. The filtering is not perfect, and one can still see some ADC and support items sneak into the trees, but it was considered good enough.<br /><br />
We then took each champion’s five-item build path, excluding component items and boots and considering all Runeglaive jungle items as the same item, and constructed a tree data structure representing all build paths seen by each champion, as well as its completion time and win rate. For each champion, we prune the tree down to its six most common build paths, which is what is shown in the application.
            <hr />
<span class="text-header">How is the similarity score calculated?</span><br />
For each champion, we also track how often every item was built, as a percentage of games. This is displayed at the bottom of the screen for each champion. We can think of this data as an n-dimensional vector, where n is the total number of items in League of Legends and the values represent the build percentage. Given two champions, we calculate their similarity score by calculating the angle of the two vectors, expressed as:
<br /><br /><div class="text-center"><img src="Images\similarity.gif" /></div><br />
We then map this value to the 0 to 100 range. Two champions with identical builds would have an angle of 0, which we map to 100, and two champions with exactly opposite builds would have an angle of π, which we map to 0. In practice, most of the champions we track have a similarity score between 50 and 100.
            <hr />
<span class="text-header">How are the top 6 paths calculated? (Why do champions have different trees from 5.11 to 5.14, even though the build percentages are relatively equal?)</span><br />
The top 6 path calculation is a variation on classical tree search algorithms. Our trees by design have a maximum depth of five, so we can always show the full depth of the tree (a full five-item build) and are really optimizing for breadth.<br /><br /> We want to balance between showing the most popular builds, and showing a diversity of builds – if there are two popular build paths that have four items in common and only differ by the last item, there is a limited value-add to showing both paths, with just the last item different. At the same time, we do not want to eschew showing similar but highly popular paths in favor of wacky, fringe builds solely because they are different.
<br /><br />The solution is to use a greedy algorithm that constructs the pruned tree one build at a time. In each iteration, for every possible build path that we could add, we calculate the total number of occurrences for each node that does not yet appear in the search tree. For instance, if we have the build path:
<br /><br /><div class="text-center"><img src="Icons\3027.png" class="medium-icon" /> (500 builds) -> <img src="Icons\3089.png" class="medium-icon" />(400 builds) -> <img src="Icons\3135.png" class="medium-icon" /> (200 builds) -> <img src="Icons\3157.png" class="medium-icon" /> (100 builds) -> <img src="Icons\3116.png" class="medium-icon" /> (50 builds)</div>
<br />But the items <img src="Icons\3027.png" class="small-icon" /><img src="Icons\3089.png" class="small-icon" /><img src="Icons\3135.png" class="small-icon" /> already exist in our tree, then this path will have a score of 150. This rewards popular paths while negating nodes that already exist. The path with the highest score is added.
<br /><br />One may notice that across patches, even though a champion’s overall item build percentages does not change a lot, the displayed paths are quite different. For paths that differ from each other by only a few percentage points or tenths of a percentage point, small changes to these numbers can cause paths to add or drop from the tree. Also, many champions may change the order in which they buy items, which would change the displayed paths but may keep the overall percentage numbers constant.
            <hr />

<span class="text-header">What are some cool results we can see from these trees?</span><br />
Using our similarity score, we can compare each champion’s 5.11 builds to their 5.14 builds, and see which champions were the most affected by the AP item changes (Click for larger version).<br />
            <br /><br /><a href="Images/allchampssimilarityscore.png"><img src="Images/allchampssimilarityscore.png" class="full-size"/></a><br /><br />

Most champions have a similarity score between 90 and 100 between patches 5.11 and 5.14. The noticeable exception is <span class="champ-me" data-id="81"></span> (score of 85.09). The reason for this is due to <span class="tooltip-me" data-id="3720"></span>, which replaced 
<span class="tooltip-me" data-id="3716"></span> in patch 5.12 and made AP Ezreal mid much more popular. As seen in the Item Trees, Luden’s Echo (25.3%), Lich Bane (16.5%) and Runeglaive (8.7%)-first builds overtook the popularity of Manamune or Trinity Force-first builds in 5.11.
<br /><br />
	<span class="itemtree-me" data-leftid="81" data-leftpatch="5.11" data-rightid="81" data-rightpatch="5.14"></span>
<br /><br />Other champions such as <span class="champ-me" data-id="60"></span>, <span class="champ-me" data-id="9"></span>, <span class="champ-me" data-id="96"></span> and <span class="champ-me" data-id="268"></span> also saw larger than average changes to their item builds. These are champions that really benefitted from <span class="tooltip-me" data-id="3720"></span>, or benefitted from the buffs to control-type items such as <span class="tooltip-me" data-id="3116"></span> and <span class="tooltip-me" data-id="3115"></span>.
<br /><br />
    <span class="itemtree-me" data-leftid="60" data-leftpatch="5.11" data-rightid="60" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="9" data-leftpatch="5.11" data-rightid="9" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="96" data-leftpatch="5.11" data-rightid="96" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="268" data-leftpatch="5.11" data-rightid="268" data-rightpatch="5.14"></span><br />
<br />
<hr />
<span class="text-header">Similarity Scores Between Champions</span><br />
We can also compute the similarity score for every pair of champions within a single patch. The results are shown below for both patch versions (click for larger version):<br /><br />
  <div class="text-center" ><a href="Images/511champsimilarities.png"><img src="Images/511champsimilarities.png" style="width:45%;padding-right:5px;" /></a><a href="Images/514champsimilarities.png"><img src="Images/514champsimilarities.png" style="width: 45%;" /></a></div><br />
We can see across both patches that champions such as <span class="champ-me" data-id="76"></span>, <span class="champ-me" data-id="1"></span> and <span class="champ-me" data-id="55"></span> builds that align most closely with all other mages – their champions have what can be thought of as the prototypical mage build, which include the staple items of <span class="tooltip-me" data-id="3285"></span>, <span class="tooltip-me" data-id="3089"></span>, <span class="tooltip-me" data-id="3165"></span> and <span class="tooltip-me" data-id="3157"></span> in some order.
            <br /><br />
    <span class="itemtree-me" data-leftid="76" data-leftpatch="5.11" data-rightid="999" data-rightpatch="5.11"></span><br />
    <span class="itemtree-me" data-leftid="1" data-leftpatch="5.14" data-rightid="999" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="55" data-leftpatch="5.11" data-rightid="999" data-rightpatch="5.11"></span><br />
<br />At the other end of the spectrum, we can see champions such as <span class="champ-me" data-id="60"></span>, <span class="champ-me" data-id="13"></span> and <span class="champ-me" data-id="10"></span>, whose builds are very different from most other mages. These champions tend to have very specialized build paths that only work for their specific kits, although there are some similarities. We see that:
<br /><ul><li><span class="champ-me" data-id="60"></span> builds most similarly to <span class="champ-me" data-id="68"></span> in both patches (both use <span class="tooltip-me" data-id="3151"></span>, <img src="Icons/3001.png" class="small-icon">Abyssal Scepter, <span class="tooltip-me" data-id="3116"></span>)</li>
<li><span class="champ-me" data-id="13"></span> builds most similarly to <span class="champ-me" data-id="34"></span> in both patches (both build <span class="tooltip-me" data-id="3027"></span>, <span class="tooltip-me" data-id="3003"></span>)</li>
<li><span class="champ-me" data-id="10"></span> builds most similarly to <span class="champ-me" data-id="131"></span> in both patches (both build <span class="tooltip-me" data-id="3115"></span>)</li></ul>
<br />
    <span class="itemtree-me" data-leftid="60" data-leftpatch="5.14" data-rightid="999" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="60" data-leftpatch="5.14" data-rightid="68" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="13" data-leftpatch="5.14" data-rightid="999" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="13" data-leftpatch="5.14" data-rightid="34" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="10" data-leftpatch="5.11" data-rightid="999" data-rightpatch="5.11"></span><br />
    <span class="itemtree-me" data-leftid="10" data-leftpatch="5.11" data-rightid="131" data-rightpatch="5.11"></span><br />
<br />
As an interesting fact, the two mages that are the most dissimilar to each other are <span class="champ-me" data-id="10"></span> and <span class="champ-me" data-id="13"></span>, with a similarity score of 52.56:
<br /><br />	<span class="itemtree-me" data-leftid="10" data-leftpatch="5.11" data-rightid="13" data-rightpatch="5.11"></span><br />
<hr />
<span class="text-header">Item Popularity Changes Among All Mages</span><br />
It’s also worth taking a look at overall item popularity changes, across the board:
	<br /><br /><span class="itemtree-me" data-leftid="999" data-leftpatch="5.11" data-rightid="999" data-rightpatch="5.14"></span><br /><br />
We can see that <span class="tooltip-me" data-id="3285"></span> saw a 9.4 point drop between the two patches, which represents a 21.6% usage drop, most likely because of buffs to control and sustained damage mage items. On the other hand, <span class="tooltip-me" data-id="3116"></span> saw a 3.4 point increase, which represents a 50% usage increase. The full chart is shown below (click for larger version):
 <br /><br /><div class="text-center" ><a href="Images/allitempopularity.png"><img src="Images/allitempopularity.png" style="width:45%;padding-right:5px;" /></a><a href="Images/Itempercentchange.png"><img src="Images/Itempercentchange.png" style="width:45%;" /></a></div>
 <br/>
We will look at two items that received interesting changes: Nashor’s Tooth and Rylai’s Crystal Scepter.<br /><hr />
<span class="text-header">Nashor’s Tooth</span><br /><br />
            <a href="Images/nashortooth.png"><img src="Images/nashortooth.png" class="full-size"/></a><br /><br />
As seen in the above chart <span class="tooltip-me" data-id="3115"></span> saw a phenomenal rise in popularity between patches 5.11 and 5.14. Most of this was due to the rise in popularity of <span class="champ-me" data-id="10"></span>, who was played more than three times as frequently in 5.14 than in 5.11 in our data set. But Riot had also stated that one of the goals for Nashor’s Tooth was to make it more accessible to non-Kayle champions. It looks like they’ve succeeded in this goal -- the above graph shows that Nashor’s Tooth saw an increase in popularity across the board, particularly for its 2nd and 3rd biggest users, <span class="champ-me" data-id="131"></span> and <span class="champ-me" data-id="268"></span> (although it’s still a niche pick for everyone else). 
    <br /><br /><span class="itemtree-me" data-leftid="10" data-leftpatch="5.11" data-rightid="10" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="131" data-leftpatch="5.11" data-rightid="131" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="268" data-leftpatch="5.11" data-rightid="268" data-rightpatch="5.14"></span><br />
 <br /><hr />
<span class="text-header">Rylai's Crystal Scepter</span><br /><br />
            <a href="Images/rylai.png"><img src="Images/rylai.png" class="full-size"/></a>
<br /><br /><span class="tooltip-me" data-id="3116"></span> is an interesting item because its AP component was untouched, yet its slowing effect was buffed. We expect then that control, poking and sustained damage mages would see the biggest benefit from the item (as opposed to assassin mages). The above graph shows this is mostly true. Champions such as <span class="champ-me" data-id="68"></span>, <span class="champ-me" data-id="8"></span>, <span class="champ-me" data-id="268"></span> and <span class="champ-me" data-id="96"></span> saw a big increase in their Rylai’s build rates.
    <br /><br /><span class="itemtree-me" data-leftid="68" data-leftpatch="5.11" data-rightid="68" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="8" data-leftpatch="5.11" data-rightid="8" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="268" data-leftpatch="5.11" data-rightid="268" data-rightpatch="5.14"></span><br />
    <span class="itemtree-me" data-leftid="96" data-leftpatch="5.11" data-rightid="96" data-rightpatch="5.14"></span><br />
<hr />
<span class="text-header">What technologies were used to create the site?</span><br />
<ul><li>Data Download/Parsing: C# with <a href="http://json.codeplex.com/">JSON.Net</a></li>
<li>Website: ASP.Net using the <a href="http://philogb.github.io/jit/">Javascript Infovis Toolkit</a>, <a href="http://getbootstrap.com/">Bootstrap</a>, <a href="https://select2.github.io/">select2</a> and <a href="http://jquery.com/">jQuery</a> (all open-sourced).</li>
<li>Data Analysis/Graphing: Excel</li></ul><br />
LoL AP Item Trees is not affiliated with Riot Games. League of Legends and Riot Games are trademarks or registered trademarks of Riot Games, Inc. League of Legends © Riot Games, Inc.

<script src="Scripts/Utilities.js"></script>
<script>
    $('.tooltip-me').each(
        function () {
            $(this).html(tooltipItem($(this).data('id')));
        });
    $('.item-tooltip').each(
        function () {
            $(this).tooltip();
        });
    $('.champ-me').each(
        function() {
            $(this).html(champHtml($(this).data('id')));
        });
    $('.itemtree-me').each(
        function () {
            $(this).html(itemTree($(this).data('leftid'), $(this).data('leftpatch'), $(this).data('rightid'), $(this).data('rightpatch')));
        });
</script>
</div>
    </div>

</asp:Content>
