// Try to get the node with the specified ID from a given array
function TryGetNodeIdValue(array, id) {
    for (var i = 0; i < array.length; i++) {
        if (array[i].id === id) {
            return array[i];
        }
    }
    return null;
}

// Function to format a champion option for select2 box
function s2FormatChampOption(option) {
    if (!option.id) { return option.text; }
    return $('<span style="font-weight:bold; font-size:150%;"><img src="Icons/c' + option.id + '.png" style="width:30px;height:30px;padding-right:3px;" />' + option.text + '</span>');
}

// Function to format a patch option for select2 box
function s2FormatPatchOption(option) {
    if (!option.id) { return option.text; }
    return $('<span style="font-weight:bold; font-size:100%;">' + option.text + '</span>');
}

// Recursively build the item tree
function RecursiveBuildItemTree(parentNode, thisNode) {
    // The root node
    var nextNode = {
        id: parentNode.id + "-" + thisNode.itemId,
        name: itemIdToName(thisNode.itemId),
        data: {
            img: "Icons/" + thisNode.itemId.toString() + ".png",
            numPoints: thisNode.numberOfBuilds,
            width: Math.max(parentNode.data.width + 10 * Math.log(thisNode.numberOfBuilds / parentNode.data.numPoints) / Math.LN10, 1),
            itemId: thisNode.itemId,
            parentPopularity: thisNode.numberOfBuilds / parentNode.data.numPoints,
            numWins: thisNode.numberOfWins,
            thirtySecondIntervals: thisNode.thirtySecondIntervals,
            avgBuildTime: thisNode.totalBuildTime / thisNode.numberOfBuilds
        },
        children: []
    }

    // Sort the children
    thisNode.children.sort(function (nodeA, nodeB) {
        if (nodeA.numberOfBuilds < nodeB.numberOfBuilds) {
            return 1;
        } else if (nodeA.numberOfBuilds > nodeB.numberOfBuilds) {
            return -1;
        } else {
            return 0;
        }
    });

    // We want to represent the most common node in the center, and the other nodes fanning from left to right, so the outermost nodes are least common
    var i;
    for (i = thisNode.children.length - 1; i >= 0; i -= 2) {
        var childNode = RecursiveBuildItemTree(nextNode, thisNode.children[i]);
        nextNode.children.push(childNode);
    }

    for (var i = (i == -2) ? 1 : 0; i < thisNode.children.length; i += 2) {
        var childNode = RecursiveBuildItemTree(nextNode, thisNode.children[i]);
        nextNode.children.push(childNode);
    }

    return nextNode;
}

// Create the HTML for the Commonly Built Items table
function CreateCommonBuiltItemsTable(thisDictionary, otherDictionary, champDataPoints, otherDataPoints) {
    // Convert to sorted array
    var sArray = DictionaryToSortedArray(thisDictionary);
    var innerHtml = "Most Commonly Built Items<table><tr>";
    // First row -- item images
    for (var i = 0; i < 10; i++) {
        innerHtml += '<td class="align-center"><img src="Icons/' + sArray[i][0] + '.png" title="' + itemIdToName(parseInt(sArray[i][0])) + '" style="height:30px;width:30px;" /></td>';
    }
    innerHtml += "</tr><tr>";
    // Second row -- percentages
    for (var i = 0; i < 10; i++) {
        if (otherDictionary == null) {
            innerHtml += '<td class="align-center">' + (100 * sArray[i][1] / champDataPoints).toFixed(1) + '%</td>';
        } else {
            if (sArray[i][0] in otherDictionary) {
                var diff = sArray[i][1] / champDataPoints - otherDictionary[sArray[i][0]] / otherDataPoints;
            } else {
                var diff = sArray[i][1] / champDataPoints;
            }
            var color = diff >= 0.03 ? "#390" : diff <= -0.03 ? "#d90026" : "#ffffff";
            innerHtml += '<td class="align-center" style="color:' + color + '">' + (100 * sArray[i][1] / champDataPoints).toFixed(1) + '%</td>';
        }
    }
    // Third row -- percentage diffs
    if (otherDictionary != null) {
        innerHtml += "</tr><tr>";
        for (var i = 0; i < 10; i++) {
            if (sArray[i][0] in otherDictionary) {
                var diff = sArray[i][1] / champDataPoints - otherDictionary[sArray[i][0]] / otherDataPoints;
            } else {
                var diff = sArray[i][1] / champDataPoints;
            }
            var color = diff >= 0.03 ? "#390" : diff <= -0.03 ? "#d90026" : "#ffffff";
            var symbol = diff > 0 ? "▲" : diff < 0 ? "▼" : "⇔";
            if (diff != 0) {
                innerHtml += '<td class="align-center" style="color:' + color + '">' + symbol + (100 * diff).toFixed(1) + '%</td>';
            } else {
                innerHtml += '<td class="align-center">--</td>'
            }
        }
    }
    innerHtml += "</tr></table>";
    return innerHtml;
}

// Parse a raw JSON object into an object we can use for Javascript Infovis Toolkit tree
function ParseJsonObj(jsonObj, champId, drawTreeFn, patch, outputTree) {
    var champ = jsonObj.championStatistics[champId.toString()];

    var rootNode = {
        id: outputTree === "left" ? "left-root" : "right-root",
        name: champIdToName(parseInt(champId)),
        data: {
            img: "Icons/c" + champId + ".png",
            numPoints: champ.numberOfDataPoints,
            width: 20.0,
            itemId: parseInt(champId)
        }, children: []
    };
    champ.itemPaths.children.sort(function (nodeA, nodeB) {
        if (nodeA.numberOfBuilds < nodeB.numberOfBuilds) {
            return 1;
        } else if (nodeA.numberOfBuilds > nodeB.numberOfBuilds) {
            return -1;
        } else {
            return 0;
        }
    });

    // We want to represent the most common node in the center, and the other nodes fanning from left to right, so the outermost nodes are least common
    var i;
    for (i = champ.itemPaths.children.length - 1; i >= 0; i-=2) {
        rootNode.children.push(RecursiveBuildItemTree(rootNode, champ.itemPaths.children[i]));
    }
    for (i = (i == -2) ? 1 : 0; i < champ.itemPaths.children.length; i++) {
        rootNode.children.push(RecursiveBuildItemTree(rootNode, champ.itemPaths.children[i]));
    }

    // Set some global variables we'll need later
    if (outputTree === "left") {
        currentLeftChamp = champId;
        currentLeftPatch = patch;
        leftItemsBuiltDictionary = champ.totalItemsBuilt;
        leftChampDataPoints = champ.numberOfDataPoints;
        
        $('#left-bg').attr("src", "Icons/c" + champId + "b.jpg");
        $('#left-tree-details').html("");
        $('#left-tree-champion-title').html(getChampTitle(parseInt(champId)));
    } else {
        currentRightChamp = champId;
        currentRightPatch = patch;
        rightItemsBuiltDictionary = champ.totalItemsBuilt;
        rightChampDataPoints = champ.numberOfDataPoints;
        $('#right-bg').attr("src", "Icons/c" + champId + "b.jpg");
        $('#right-tree-details').html("");
        $('#right-tree-champion-title').html(getChampTitle(parseInt(champId)));
    }

    // Update both trees
    if (leftItemsBuiltDictionary != null) {
        var innerHtml = CreateCommonBuiltItemsTable(leftItemsBuiltDictionary, rightItemsBuiltDictionary, leftChampDataPoints, rightChampDataPoints);
        $('#left-tree-item-details').html(innerHtml);
        $('#left-tree-item-details').offset({ left: $(document).width() / 4 - $('#left-tree-item-details').width() / 2 });
    }

    if (rightItemsBuiltDictionary != null) {
        innerHtml = CreateCommonBuiltItemsTable(rightItemsBuiltDictionary, leftItemsBuiltDictionary, rightChampDataPoints, leftChampDataPoints);
        $('#right-tree-item-details').html(innerHtml);
        $('#right-tree-item-details').offset({ left: $(document).width() * 3 / 4 - $('#right-tree-item-details').width() / 2 });
    }

    if (leftItemsBuiltDictionary != null && rightItemsBuiltDictionary != null) {
        var similarityScore = calculateSimilarityScore(leftItemsBuiltDictionary, rightItemsBuiltDictionary);
        var r = similarityScore > 50 ? 217 - 3.32 * (similarityScore - 50) : 217;
        var g = similarityScore > 50 ? 3.06 * (similarityScore - 50) : 0;
        var b = similarityScore > 50 ? 38 - 0.76 * (similarityScore - 50) : 38;
        innerHtml = 'Similarity Score<sup>[<a href="#" rel="tooltip" title="See the main page to see how this is calculated." id="similarity-score-help">?</a>]</sup>:<br/><div style="color: rgb(' + r.toFixed(0) + ',' + g.toFixed(0) + ',' + b.toFixed(0) + '); font-weight:bold; text-align:center; font-size: 150%">' + similarityScore.toFixed(2) + '</div>';
        $('#similarity-score').html(innerHtml);
        $('#similarity-score').offset({ left: $(document).width() / 2 - $('#similarity-score').width() / 2 });
        $('#similarity-score-help').tooltip();
    }

    // Use the callback to draw the tree
    drawTreeFn(rootNode, outputTree);

    // Update the URL to set the hash parameters
    setHashParameters();
}

// Reload data
function ReloadData(id, patch, shouldReinit, outputTree) {
    // Make the call to get the JSON data, then call ParseJsonObject
    var xobj = new XMLHttpRequest();
    xobj.overrideMimeType("application/json");
    xobj.open('GET', 'JSON/output-' + patch + '.json', true);
    xobj.onreadystatechange = function () {
        if (xobj.readyState == 4) {
            var jsonObj = JSON.parse(xobj.responseText);
            if (shouldReinit) {
                ParseJsonObj(jsonObj, id, UpdateST, patch, outputTree);
            } else {
                ParseJsonObj(jsonObj, id, CreateNewST, patch, outputTree);
            }
        }
    }
    xobj.send(null);
}

// Initialize function
function InitializeTrees() {
    // Parse hash parameters, if we have any
    var paramsDict = getHashParameters();
    currentLeftChamp = ("leftId" in paramsDict) ? paramsDict["leftId"] : "999";
    currentRightChamp = ("rightId" in paramsDict) ? paramsDict["rightId"] : "999";
    currentLeftPatch = ("leftPatch" in paramsDict) ? paramsDict["leftPatch"] : "5.11";
    currentRightPatch = ("rightPatch" in paramsDict) ? paramsDict["rightPatch"] : "5.14";

    // select2 start-up code
    $('#left-champ-select').select2(
    {
        templateResult: s2FormatChampOption,
    }).select2('val', currentLeftChamp);
    $('#left-patch-select').select2(
    {
        templateResult: s2FormatPatchOption,
        minimumResultsForSearch: Infinity
    }).select2('val', currentLeftPatch);
    $('#right-champ-select').select2(
    {
        templateResult: s2FormatChampOption,
    }).select2('val', currentRightChamp);
    $('#right-patch-select').select2(
    {
        templateResult: s2FormatPatchOption,
        minimumResultsForSearch: Infinity
    }).select2('val', currentRightPatch);

    $('#left-champ-select').change(function () {
        if (currentLeftChamp !== $(this).val()) {
            ReloadData(parseInt($(this).val()), $('#left-patch-select').val(), true, "left");
        }
    });
    $('#left-patch-select').change(function () {
        if (currentLeftPatch !== $(this).val()) {
            ReloadData(parseInt($('#left-champ-select').val()), $(this).val(), true, "left");
        }
    });
    $('#right-champ-select').change(function () {
        if (currentRightChamp !== $(this).val()) {
            ReloadData(parseInt($(this).val()), $('#right-patch-select').val(), true, "right");
        }
    });
    $('#right-patch-select').change(function () {
        if (currentRightPatch !== $(this).val()) {
            ReloadData(parseInt($('#right-champ-select').val()), $(this).val(), true, "right");
        }
    });

    // Implement a custom edge type for JIT
    $jit.ST.Plot.EdgeTypes.implement({
        'boxy': {
            'render': function (adj, canvas) {
                var orn = this.getOrientation(adj),
                    nodeFrom = adj.nodeFrom,
                    nodeTo = adj.nodeTo,
                    rel = nodeFrom._depth < nodeTo._depth,
                    from = this.viz.geom.getEdge(rel ? nodeFrom : nodeTo, 'begin', orn),
                    to = this.viz.geom.getEdge(rel ? nodeTo : nodeFrom, 'end', orn),
                    midpoint1 = { x: from.x, y: (from.y + to.y) / 2 },
                    midpoint2 = { x: to.x, y: (from.y + to.y) / 2 };

                if (adj.getData('topmost') == true) {
                    canvas.getCtx().globalCompositeOperation = 'source-over';
                } else {
                    canvas.getCtx().globalCompositeOperation = 'destination-over';
                }

                this.edgeHelper.line.render(from, midpoint1, canvas);
                this.edgeHelper.line.render(midpoint1, midpoint2, canvas);
                this.edgeHelper.line.render(midpoint2, to, canvas);

                // round off the corners
                if (nodeTo.data.width / 4 > 1) {
                    this.nodeHelper.circle.render('fill', midpoint1, nodeTo.data.width / 4, canvas);
                    this.nodeHelper.circle.render('fill', midpoint2, nodeTo.data.width / 4, canvas);
                }
            }
        }
    })

    leftItemsBuiltDictionary = null;
    rightItemsBuiltDictionary = null;
    leftChampDataPoints = 0;
    rightChampDataPoints = 0;

    // Load our data
    ReloadData(currentLeftChamp, currentLeftPatch, false, "left");
    ReloadData(currentRightChamp, currentRightPatch, false, "right");
}