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

function drawFunc(points, styles) {
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
            (data) => drawFunc(data[0].Points, data[0].Style)
        );

    return false;
}