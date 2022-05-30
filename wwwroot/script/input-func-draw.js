const LARGE_Y = 100000000;

var containerHolder = document.getElementById("container-holder");
var chartContainer = document.getElementById("chart-container");

var layout = {
    title: "Chart",
    width: 450,
    height: 350,
    xaxis: {
        title: "X Axis",
        zeroline: false
    },
    yaxis: {
        title: "Y Axis",
        //scaleanchor: "x",
        showline: false
    },
};

function getResponseJsonOrError(response) {
    if (!response.ok)
        return response.text().then((text) => { throw text });
    else
        return response.json();
}

function assembleChartData(listOfPoints) {
    var chartData = [];

    for (var points of listOfPoints) {
        chartData.push({ x: [], y: [], name: points.name });
        for (var point of points) {
            if (Math.abs(point.y) > LARGE_Y) continue;
            chartData[chartData.length-1].x.push(point.x);
            chartData[chartData.length-1].y.push(point.y);
        }
    }

    return chartData;
}

function parseChartModes(listOfStyles) {
    var modes = [];
    for (var style of listOfStyles) {
        switch (style.type.value.toLowerCase()) {
            case "line": modes.push("lines"); break;
            case "dots": default: modes.push("markers"); break;
        }
    }
    return modes;
}

function parseChartColors(listOfStyles) {
    var colors = [];
    for (var style of listOfStyles) {
        colors.push(style.color.value);
    }
    return colors;
}

function assembleStyleData(listOfStyles) {
    return {
        "mode": parseChartModes(listOfStyles),
        "line.color": parseChartColors(listOfStyles),
        "line.width": 3,
    };
}

function hideErrorMessage() {
    var messageContainer = document.getElementById("message-container");
    messageContainer.style.display = "none";
}

function showErrorMessage(error) {
    hideFunc();
    var messageContainer = document.getElementById("message-container");
    messageContainer.style.display = "block";
    var message = document.createElement("p");
    message.id = "error-message";
    message.innerText = "An error occured: " + error;
    messageContainer.replaceChildren(message);
}

function hideFunc() {
    chartContainer.innerHTML = "";
}

function drawFunction(points, styles) {
    hideErrorMessage();

    var chartData = assembleChartData([ points ]);
    Plotly.newPlot(chartContainer, chartData, layout);

    var styleData = assembleStyleData([ styles ]);
    Plotly.restyle(chartContainer, styleData);
}

function appendPointsToPlot(funcPoints, newPoints, funcStyles, newStyles) {
    var chartData = assembleChartData([ funcPoints, newPoints ]);
    console.log(chartData);
    Plotly.react(chartContainer, chartData, styleData);

    var styleData = assembleStyleData([ funcStyles, newStyles ]);
    Plotly.restyle(chartContainer, styleData);
}

function drawExtremes(target, params, funcPoints, funcStyles) {
    var extremes = document.getElementById("analysis-options").elements["Extremes"];
    if (!extremes.checked) return;

    var action = extremes.attributes.action.value;
    //console.log(target + "/" + action + "?" + params);
    fetch(target + "/" + action + "?" + params)
        .then((response) => getResponseJsonOrError(response))
        .then(
            (data) => {
                if (data.Error !== undefined)
                    return showErrorMessage(data.Error);

                data[0].points.name = "Extremes";
                appendPointsToPlot(funcPoints, data[0].points, funcStyles, data[0].style);
            }
        )
        .catch((error) => showErrorMessage(error));

    console.log("fetching extreme points ...");
}

function submitInputFunc(event) {
    event.preventDefault();

    var inputFunc = event.target.elements.input.value;
    var from = event.target.elements.from.value;
    var to = event.target.elements.to.value;
    var params = new URLSearchParams({ input: inputFunc.trim(), from: from.trim(), to: to.trim() });

    fetch(event.target.action + "?" + params)
        .then((response) => getResponseJsonOrError(response))
        .then(
            (data) => {
                if (data.Error !== undefined)
                    return showErrorMessage(data.Error);

                updateHistoryList();
                data[0].points.name = inputFunc;
                drawFunction(data[0].points, data[0].style);
                drawExtremes(event.target.action, params, data[0].points, data[0].style);
            }
        )
        .catch((error) => showErrorMessage(error));

    return false;
}