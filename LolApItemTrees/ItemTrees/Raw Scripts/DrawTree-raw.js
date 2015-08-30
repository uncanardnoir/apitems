var labelType, useGradients, nativeTextSupport, animate;

(function () {
    var ua = navigator.userAgent,
        iStuff = ua.match(/iPhone/i) || ua.match(/iPad/i),
        typeOfCanvas = typeof HTMLCanvasElement,
        nativeCanvasSupport = (typeOfCanvas == 'object' || typeOfCanvas == 'function'),
        textSupport = nativeCanvasSupport
          && (typeof document.createElement('canvas').getContext('2d').fillText == 'function');
    //I'm setting this based on the fact that ExCanvas provides text support for IE
    //and that as of today iPhone/iPad current text support is lame
    labelType = (!nativeCanvasSupport || (textSupport && !iStuff)) ? 'Native' : 'HTML';
    nativeTextSupport = labelType == 'Native';
    useGradients = nativeCanvasSupport;
    animate = !(iStuff || !nativeCanvasSupport);
})();

// Updates an existing space tree
function UpdateST(json, outputTree) {
    var st;
    if (outputTree === "left") {
        leftRootNode = json;
        st = leftSt;
    } else {
        rightRootNode = json;
        st = rightSt;
    }

    st.loadJSON(json);
    //compute node positions and layout
    st.compute();
    //emulate a click on the root node.
    st.onClick(st.root);
    st.geom.translate({ x: 0, y: -200 }, 'end');
    st.switchPosition("top", "replot", null);
}

// Creates a new space tree
function CreateNewST(json, outputTree) {
    if (outputTree === "left") {
        leftRootNode = json;
    } else {
        rightRootNode = json;
    }

    //init Spacetree
    //Create a new ST instance
    var st = new $jit.ST({
        constrained: false,
        levelsToShow:15,
        //id of viz container element
        injectInto: outputTree === "left" ? 'left-infovis' : 'right-infovis',
        //set distance between node and its children
        levelDistance: 50,
        //set an X offset
        offsetY: 200,
        Navigation: {
            enable:true,
            panning:true
        },
        //set node, edge and label styles
        Node: {
            overridable: true,
            type: 'rectangle',
            height: 40,
            width: 60,
            color: "#ff7f00"
        },
        Edge: {
            overridable: true,
            type: 'boxy',
            color: '#ff7f00',
            lineWidth: 10
        },
        Tips: {
            enable: true,
            // Create tooltips on item hover
            onShow: function (tip, node) {
                var rootNode;
                if (outputTree === "left") {
                    rootNode = leftRootNode;
                } else {
                    rootNode = rightRootNode;
                }
                if (node.id == rootNode.id) {
                    tip.innerHTML = "<div class=\"rg-box-tooltip\"><span style=\"font-weight:bold;color:orange\">" +
                        node.name +
                        "</span><br /><span style=\"font-style:italic\">" +
                        getChampTitle(node.data.itemId) +
                        "</span><br /><br />"
                        + node.data.numPoints + " games.</div>";
                } else {
                    tip.innerHTML = "<div class=\"rg-box-tooltip\"><span style=\"font-weight:bold;color:orange\">" +
                        node.name +
                        "</span><br />Purchased in " +
                        node.data.numPoints +
                        " out of " +
                        rootNode.data.numPoints +
                        " games (" +
                        (100 * node.data.numPoints / rootNode.data.numPoints).toFixed(1) +
                        "% of total, " +
                        (100 * node.data.parentPopularity).toFixed(1) +
                        "% of parent)<br /><br /><span style=\"font-style:italic\">Click for more information.</span></div>";
                }
            }
        },
        // This method is called on DOM label creation -- we use it to substitute an item image for the node
        onCreateLabel: function (label, node) {
            var truesize = node.data.width > 0 ? 20 + 3 * node.data.width : 20;
            label.innerHTML = "<img src=\"" + node.data["img"] + "\" style=\"width:" + truesize + "px;height:" + truesize + "px;\">";
            label.onclick = function () {
                // On node click -- create the green path and set the HTML of the top-right box
                var rootNode;
                if (outputTree === "left") {
                    rootNode = leftRootNode;
                } else {
                    rootNode = rightRootNode;
                }
                st.graph.eachNode(function (n) {
                    n.removeData('color');
                    n.removeData('height');
                    n.removeData('width');
                    n.eachAdjacency(function (adj) {
                        adj.removeData('color');
                        adj.removeData('topmost');
                    });
                });

                var nodePtr = node;
                var stack = [];
                while (nodePtr != null) {
                    stack.push(nodePtr);
                    nodePtr.setData('color', '#22b14c', 'end');
                    var nodeParent = nodePtr.getParents()[0];
                    if (nodeParent != null) {
                        nodePtr.getAdjacency(nodeParent.id).setData('color', '#22b14c', 'end');
                        nodePtr.getAdjacency(nodeParent.id).setData('topmost', true);
                    }
                    nodePtr = nodeParent;
                }

                // Reverse order: construct our build path. Start at -2 to skip the root (champion) node.
                if (node.id != "root") {
                    innerHtml = "";
                    for (var i = stack.length - 2; i >= 0; i--) {
                        innerHtml += "<img src=\"" + stack[i].data["img"] + "\" style=\"width:30px;height:30px\">";
                    }
                    innerHtml +=
                        "<table><tr><td>Popularity</td><td>" +
                            (100 * node.data.numPoints / rootNode.data.numPoints).toFixed(2) +
                        "%</td><td>(" +
                            node.data.numPoints +
                        "/" +
                            rootNode.data.numPoints +
                        ")</td></tr><tr><td>Last Node Popularity</td><td>" +
                            (100 * node.data.numPoints / node.getParents()[0].data.numPoints).toFixed(2) +
                        "%</td><td>(" +
                            node.data.numPoints +
                        "/" +
                            node.getParents()[0].data.numPoints +
                        ")</td></tr><tr><td>Win Percentage</td><td>" +
                            (100 * node.data.numWins / node.data.numPoints).toFixed(2) +
                        "%</td><td>(" +
                            node.data.numWins +
                        "/" +
                            node.data.numPoints +
                        ")</td></tr><tr><td>Average Build Time</td><td>" +
                            Math.floor(node.data.avgBuildTime / 60) +
                        ":" +
                            EnsureLeadingZero((node.data.avgBuildTime % 60).toFixed(0)) +
                        "</td></tr></table>";
                    if (outputTree === "left") {
                        $('#left-tree-details').html(innerHtml);
                    } else {
                        $('#right-tree-details').html(innerHtml);
                    }
                }

                // Re-compute and plot our new tree
                st.compute('end');
                st.geom.translate({ x: 0, y: -200 }, 'end');
                st.fx.animate({
                    modes: ['linear',
                            'node-property:color',
                            'edge-property:color'],
                    duration: 1,
                    onComplete: function () {
                    }
                });
            }
        },
        // Set width and height of node according to its popularity
        onBeforePlotNode: function (node) {
            var truesize = node.data.width > 0 ? 20 + 3 * node.data.width : 20;
            node.setData('width', truesize + 6);
            node.setData('height', truesize + 6);
        },
        // Set width and height of label according to its popularity
        onPlaceLabel: function (label, node) {
            var style = label.style;
            style.width = node.getData('width') + 'px';
            style.height = node.getData('height') + 'px';
            style.textAlign = 'center';
            style.paddingTop = '3px';
        },
        // Set width of line according to its popularity
        onBeforePlotLine: function (adj) {
            adj.data.$lineWidth = adj.nodeTo.data.width / 2;
        }
    });
    // Load json data
    st.loadJSON(json);
    // Compute node positions and layout
    st.compute();
    // Emulate a click on the root node.
    st.onClick(st.root);
    st.switchPosition("top", "replot", null);

    if (outputTree === "left") {
        leftSt = st;
    } else {
        rightSt = st;
    }
    
    //end
}