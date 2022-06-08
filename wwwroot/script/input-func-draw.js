const LARGE_Y = 100000000;

var containerHolder = document.getElementById("container-holder");
var chartContainer = document.getElementById("chart-container");
Plotly.newPlot(chartContainer, [], layout);

var analysisOptions = document.getElementById("analysis-options"); 

var listOfPoints = [];
var listOfStyles = [];

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
        autorange: false,
        showline: false
    },
};

function resetLists() {
    listOfPoints = [];
    listOfStyles = [];
}

function appendDataToLists(data, name) {
    if (Array.isArray(data) && data[0] !== undefined) {
        data[0].points.name = name;
        listOfPoints.push(data[0].points);
        listOfStyles.push(data[0].style);
    } else if (data.points !== undefined && data.style !== undefined) {
        data.points.name = name;
        listOfPoints.push(data.points);
        listOfStyles.push(data.style);
    } else if (typeof data === "boolean") {
        showInfoMessage(name + "? " + data);
    } else {
        throw "Uknown data format";
    }
}

function getResponseJsonOrError(response) {
    if (!response.ok)
        return response.text().then((text) => { throw text });
    else
        return response.json();
}

function assembleChartData() {
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

function parseChartModes() {
    var modes = [];
    for (var style of listOfStyles) {
        switch (style.type.value.toLowerCase()) {
            case "line": modes.push("lines"); break;
            case "dots": default: modes.push("markers"); break;
        }
    }
    return modes;
}

function parseChartColors() {
    var colors = [];
    for (var style of listOfStyles) {
        colors.push(style.color.value);
    }
    return colors;
}

function assembleStyleData() {
    return {
        "mode": parseChartModes(listOfStyles),
        "line.color": parseChartColors(listOfStyles),
        "line.width": 3,
    };
}

function hideErrorMessage() {
    var messageContainer = document.getElementById("error-container");
    messageContainer.style.display = "none";
}

function showErrorMessage(error) {
    console.error(error);
    hideFunc();
    hideInfoMessage();
    var messageContainer = document.getElementById("error-container");
    messageContainer.style.display = "block";
    var message = document.createElement("p");
    message.id = "error-message";
    message.innerText = "An error occured: " + error;
    messageContainer.replaceChildren(message);
}

function hideInfoMessage() {
    var messageContainer = document.getElementById("info-container");
    messageContainer.style.display = "none";
}

function showInfoMessage(info) {
    hideFunc();
    hideErrorMessage();
    var messageContainer = document.getElementById("info-container");
    messageContainer.style.display = "block";
    var message = document.createElement("p");
    message.id = "info-message";
    message.innerText = info;
    messageContainer.replaceChildren(message);
}

function hideFunc() {
    Plotly.react(chartContainer, []);
}

function drawFunction() {
    hideErrorMessage();
    hideInfoMessage();

    var chartData = assembleChartData();
    Plotly.react(chartContainer, chartData, layout);

    var styleData = assembleStyleData();
    Plotly.restyle(chartContainer, styleData);
}

function appendPointsToPlot() {
    var chartData = assembleChartData();
    Plotly.react(chartContainer, chartData, styleData);

    var styleData = assembleStyleData();
    Plotly.restyle(chartContainer, styleData);
}

function fetchAnalysisData(target, params) {
    for (var analysisOption of analysisOptions) {
        if (!analysisOption.checked) continue;

        var action = analysisOption.attributes.action.value;
        const analysisOptionName = analysisOption.name;
        fetch(target + "/" + action + "?" + params)
            .then((response) => getResponseJsonOrError(response))
            .then(
                (data) => {
                    if (data.Error !== undefined)
                        return showErrorMessage(data.Error);

                    appendDataToLists(data, analysisOptionName);
                    appendPointsToPlot();
                }
            )
            .catch((error) => showErrorMessage(error));

        console.log("fetching " + analysisOption.name + " ...");
    }
    return false;
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
                resetLists();
                appendDataToLists(data, inputFunc);
                drawFunction();
                fetchAnalysisData(event.target.action, params);
            }
        )
        .catch((error) => showErrorMessage(error));

    return false;
}