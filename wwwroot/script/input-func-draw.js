var containerHolder = document.getElementById("container-holder");
var chartContainer = document.getElementById("chart-container");

var layout = {
    title: "Chart",
    width: 500,
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
        chartData.x.push(point.X);
        chartData.y.push(point.Y);
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
        "mode": parseChartMode(styles.Type.Value),
        "line.color": styles.Color,
        "line.width": 3,
    };
}

function hideErrorMessage() {
    var messageContainer = document.getElementById("message-container");
    var container = document.createElement("div");
    container.id = messageContainer.id;
    messageContainer.replaceWith(container);
}

function showErrorMessage(error) {
    hideFunc();
    var messageContainer = document.getElementById("message-container");
    var container = document.createElement("div");
    container.id = messageContainer.id;
    container.classList.add("container");
    var message = document.createElement("p");
    message.classList.add("error-message");
    message.innerText = "An error occured: " + error;
    container.appendChild(message);
    messageContainer.replaceWith(container, " ");
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
    var params = { input: inputFunc };
    var target = event.target.action + "?" + new URLSearchParams(params);
    fetch(target)
        .then(
            (response) => response.json(),
            (error) => console.error(error)
        )
        .then(
            (data) => {
                if (data.Error !== undefined)
                    return showErrorMessage(data.Error);
                else
                    return drawFunc(data[0].Points, data[0].Style);
            }
        );

    return false;
}