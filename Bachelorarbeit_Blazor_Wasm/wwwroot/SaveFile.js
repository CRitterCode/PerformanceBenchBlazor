//https://www.reddit.com/r/Blazor/comments/je58n0/blazor_wasm_to_save_excel_or_csv_potentially/?rdt=49053
function SaveAsFile(filename, fileContent) {

    var link = document.createElement('a');

    link.download = filename;

    link.href = "data:text/plain;charset=utf-8," + encodeURIComponent(fileContent)

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);

}