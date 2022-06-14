var history = {};

var funcInputField = document.getElementById("func-input-field");
var funcFromField = document.getElementById("from-input-field");
var funcToField = document.getElementById("to-input-field");

var funcSubmitButton = document.getElementById("input-submit-button");
var historyList = document.getElementById("history-list");
loadUserHistory();

function loadUserHistory() {
    historyList.innerHTML = "";
    fetch(window.location.origin + "/History/Get")
        .then((response) => getResponseJsonOrError(response))
        .then(
            (data) => {
                if (data.Error !== undefined)
                    return showErrorMessage(data.Error);

                for (var historyEntry of data) {
                    var func = historyEntry.function;
                    history[func] = historyEntry;
                    removeExistingFuncs(func);
                    addInputFuncToHistory(func);
                }
            }
        )
        .catch((error) => showErrorMessage(error));
}

function updateHistoryList(event) {
    var func = funcInputField.value;
    removeExistingFuncs(func);
    addInputFuncToHistory(func);

    var inputFunc = event.target.elements.input.value;
    var from = event.target.elements.from.value;
    var to = event.target.elements.to.value;

    var params = new URLSearchParams({ input: inputFunc.trim(), from: from.trim(), to: to.trim() });
    var analysisData = fetchAnalysisData();
    history[inputFunc] = { function: inputFunc, from: from, to: to, analysisOptions: analysisData };

    fetch(window.location.origin + "/History/Add?" + params, {
        method: "PUT",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(analysisData)
    })
    .catch((error) => showErrorMessage(error));
}

function addInputFuncToHistory(func) {
    var funcInput = document.createElement("option");
    funcInput.innerHTML = func;
    funcInput.selected = "true";
    historyList.insertBefore(funcInput, historyList.firstChild);
}

function removeExistingFuncs(func) {
    for (var option of historyList.options) {
        if (option.innerHTML.trim() == func.trim())
            option.remove();
        else
            option.selected = undefined;
    }
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
    for (var analysisOption of historyEntry.analysisOptions) {
        if (analysisOption.name === "function") continue;
        document.getElementsByName(analysisOption.name)[0].checked = true;
    }

    //console.log(history[historyList.value]);
    funcSubmitButton.click();
}