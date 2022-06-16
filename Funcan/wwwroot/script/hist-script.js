var history = {};

var funcInputField = document.getElementById("func-input-field");
var funcFromField = document.getElementById("from-input-field");
var funcToField = document.getElementById("to-input-field");

var funcSubmitButton = document.getElementById("input-submit-button");
var historyList = document.getElementById("history-list");
historyList.innerHTML = "";
loadUserHistory();

function loadUserHistory() {
    historyList.innerHTML = ""; 
    fetch(window.location.origin + "/Function/History")
        .then((response) => getResponseJsonOrError(response))
        .then(
            (data) => {
                if (data.Error !== undefined)
                    return showErrorMessage(data.Error);

                var sortedData = Object.entries(data)
                    .sort(([, value1], [, value2]) => new Date(value1.time) - new Date(value2.time));
                console.log(sortedData);
                for (var [index, historyEntry] of sortedData) {
                    var func = historyEntry.function;
                    history[func] = historyEntry;
                    addInputFuncToHistory(func);
                }
            }
        )
        .catch((error) => showErrorMessage(error));
}

function updateHistoryList(event) {
    loadUserHistory();
}

function addInputFuncToHistory(func) {
    var funcInput = document.createElement("option");
    funcInput.innerHTML = func;
    historyList.insertBefore(funcInput, historyList.firstChild);
}

function historyDoubleClick(event) {
    var historyEntry = history[historyList.value];
    funcInputField.value = historyEntry.function;
    funcFromField.value = historyEntry.from;
    funcToField.value = historyEntry.to;

    for (var analysisOption of analysisOptions) {
        if (analysisOption.name === "function") continue;
        analysisOption.checked = false;
    }
    for (var analysisOption of historyEntry.plotters) {
        if (analysisOption.name === "function") continue;
        document.getElementsByName(analysisOption)[0].checked = true;
    }

    funcSubmitButton.click();
}