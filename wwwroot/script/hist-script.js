var funcInputField = document.getElementById("func-input-field");
var funcSubmitButton = document.getElementById("input-submit-button");
var historyList = document.getElementById("history-list");

function updateHistoryList() {
    var func = funcInputField.value;
    removeExistingFuncs(func);
    addInputFuncToHistory(func);
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
    funcInputField.value = historyList.value;
    funcSubmitButton.click();
}