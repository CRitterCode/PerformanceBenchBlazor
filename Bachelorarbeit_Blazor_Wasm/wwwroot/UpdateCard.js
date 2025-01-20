function updateCardHeaderColorByGuid(orderGuid, color) {
    let card = document.getElementById(orderGuid);

    if (card) {
        let header = card.querySelector(".card-header");
        let prefix = "bg-";

        header?.classList.forEach(cls => {
            if (cls.startsWith(prefix)) {
                header.classList.replace(cls, prefix + color);
            }
        });
    }
}