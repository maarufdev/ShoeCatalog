
const apiEndpoint = {
    getShoeList: "/Shoe/ShoeListApi"
}

const apiService = (url, method = 'GET', data) => {

    return new Promise(function(resolve, reject){
        $.ajax({
            url: url,
            method: method,
            data: data,
            success: function (response) {
                resolve(response);
            },
            reject: function(xhr, status, error) {
                reject(error);
            }
        });
    })
}

const CONSTANT = {
    searchBox: $("#search-box"),
    cardRowContainer: $("#card-row-container"),
    btnSearch: $("#btn-search"),
    btnRefresh: $("#btn-refresh")
}

const shoeListFeature = (() => {

    let shoeList = [];

    const displayNoData = () => {

        const { cardRowContainer } = CONSTANT;

        cardRowContainer.append(
            $("<div>")
                .addClass("col p-5")
                .append($("<h1>").text("NO DATA!").addClass("text-center"))
        );
    }

    const convertToDecimalCurrency = (price) => {
        return price.toLocaleString('en-PH', {
            style: 'currency',
            currency: 'PHP',
            minimumFractionDigits: 2,
        });
    }

    const sliceString = (item) => {
        const maxLimit = 20;

        return (item.length > maxLimit) ? item.slice(0, 20) + "..." : item;
    }

    const filterList = (query) => {
        const filterResult = shoeList.filter(result =>
            result.name.toLowerCase().includes(query) ||
            result.brand.toLowerCase().includes(query)
        );

        displayCards(filterResult);
    }

    const createCardItem = (shoeItem) => {
        const cardContainer = $("<a>")
            .addClass("col-xs-12 col-sm-6 col-md-4 col-lg-3 card-container")
            .attr({href: `/Shoe/Details/${shoeItem.shoeId}`});

        const card = $("<div>").addClass("card");
        const cardImageContainer = $("<div>").addClass("card-img__container");
        cardImageContainer.append(
            $("<img>")
                .addClass("card-item-img")
                .attr({ src: `/images/${shoeItem.image}`})
        );

        card.append(cardImageContainer);

        const brandName = $("<p>").addClass("card-text shoe-brand").text(shoeItem.brand);
        card.append(brandName);

        const cardBody = $("<div>").addClass("card-body");
        const cardTitle = $("<h6>").addClass("card-title").text(sliceString(shoeItem.name));
        const cardPrice = $("<p>").addClass("card-text shoe-price").text(convertToDecimalCurrency(shoeItem.price));

        cardBody.append(cardTitle);
        cardBody.append(cardPrice);

        card.append(cardBody);

        cardContainer.append(card);

        return cardContainer;
    };

    const displayCards = (shoeData) => {
        const { cardRowContainer } = CONSTANT;
        cardRowContainer.empty();

        if (!shoeData.length) {
            displayNoData();
            return
        }

        shoeData.map((item, idx) => {
            const cardItem = createCardItem(item);

            cardRowContainer.append(cardItem);
        });
    }

    const getShoeList = async () => {
        shoeList = await apiService(apiEndpoint.getShoeList);

        if (shoeList == null || !shoeList.length) {
            return
        }

        displayCards(shoeList);
    };

    const initialize = async () => {
        const { searchBox, btnSearch, btnRefresh } = CONSTANT;

        await getShoeList();

        btnSearch.on("click", function () {
            let query = searchBox.val()
            if (query) {
                filterList(query);
                return
            }
        })

        btnRefresh.on("click", async function () {
            await getShoeList();
            searchBox.val('');
        })
    };

    return {
        initialize: initialize
    };

})();

$(document).ready(() => {

    shoeListFeature.initialize();

});