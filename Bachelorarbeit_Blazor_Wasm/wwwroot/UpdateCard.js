function updateCardHeaderColorByGuid(orderGuid, suffix) {
    setHeader(orderGuid, suffix);
}

function updateCardHeadersColorByGuidsCollection(orderGuids, suffix) {
    orderGuids.forEach(guid => {
        setHeader(guid, suffix);
    });
}

function setHeader(guid, suffix){
    let card = document.getElementById(guid);

    if (card) {
        let header = card.querySelector(".card-header");
        let prefix = "bg-";

        header?.classList.forEach(cls => {
            if (cls.startsWith(prefix)) {
                header.classList.replace(cls, prefix + suffix);
            }
        });
    }
}