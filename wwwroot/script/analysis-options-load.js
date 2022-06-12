var plotters = {}, plotStyles = {};
var analysisOptions = document.getElementById("analysis-options");
loadAnalysisOptions();

var colorIndex = 0;
var colors = [
    '#ff7f0e',
    '#1f77b4',
    '#2ca02c',
    '#d62728',
    '#9467bd',
    '#8c564b',
    '#e377c2',
    '#7f7f7f',
    '#bcbd22',
    '#17becf'
];

function getNextColor() {
    var color = colors[colorIndex];
    colorIndex = (colorIndex + 1) % colors.length;
    return color;
}

function createWhitespace() {
    return document.createTextNode("\u00A0");
}

function addAnalysisOption(analysisOption) {
    var optionCheckBox = document.createElement("input");
    optionCheckBox.type = "checkbox";
    optionCheckBox.name = analysisOption.name;

    analysisOptions.appendChild(optionCheckBox);

    if (analysisOption.name == "function") {
        optionCheckBox.checked = true;
        optionCheckBox.style.display = "none";
    } else {
        var optionLabel = document.createElement("label");
        optionLabel.setAttribute("for", analysisOption.name);
        optionLabel.innerHTML = analysisOption.name;
        analysisOptions.appendChild(optionLabel);
        analysisOptions.appendChild(createWhitespace());
    }
}

function loadAnalysisOptions() {
    //console.log(window.location.origin + "/Function/Plotters");
    fetch(window.location.origin + "/Function/Plotters")
        .then((response) => getResponseJsonOrError(response))
        .then(
            (data) => {
                if (data.Error !== undefined)
                    return showErrorMessage(data.Error);

                analysisOptions.innerHTML = "";
                for (var analysisOption of data) {
                    //console.log(analysisOption);
                    plotters[analysisOption.name] = analysisOption;
                    plotStyles[analysisOption.name] = {
                        color: getNextColor(),
                        type: analysisOption.drawType.value
                    };
                    addAnalysisOption(analysisOption);
                }
            }
        )
        .catch((error) => showErrorMessage(error));
}