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
        showline: false
    },
};

function assembleChartData(points) {
    var chartData = {
        x: [],
        y: [],
    };

    for (var point of points) {
        chartData.x.push(point.x);
        chartData.y.push(point.y);
    }

    return [ chartData ];
}

function parseChartMode(type) {
    switch (type.toLowerCase()) {
        case "line": return "lines";
        default: return "markers";
    }
}

function assembleStyleData(styles) {
    return {
        "mode": parseChartMode(styles.type.value),
        "line.color": styles.Color,
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

function drawFunc(points, styles) {
    hideErrorMessage();

    var chartData = assembleChartData(points);
    Plotly.newPlot(chartContainer, chartData, layout);

    var update = assembleStyleData(styles);
    Plotly.restyle(chartContainer, update);
}

function submitInputFunc(event) {
    event.preventDefault();

    var inputFunc = event.target.elements.input.value;
    var from = event.target.elements.from.value;
    var to = event.target.elements.to.value;
    var params = { input: inputFunc.trim(), from: from.trim(), to: to.trim() };
    var target = event.target.action + "?" + new URLSearchParams(params);

    fetch(target)
        .then(
            (response) => {
                if (!response.ok) 
                    return response.text().then((text) => { throw text });
                else
                    return response.json();
            }
        )
        .then(
            (data) => {
                if (data.Error !== undefined)
                    return showErrorMessage(data.Error);

                updateHistoryList();
                return drawFunc(data[0].points, data[0].style);
            }
        )
        .catch((error) => showErrorMessage(error));

    return false;
}