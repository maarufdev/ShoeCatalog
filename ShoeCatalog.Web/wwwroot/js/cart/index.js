import sweetAlertHelper from "../sweetAlertHelper.js";

let productCartList = [];


const MODEL = {
    id: "",
    shoeId: "",
    name: "",
    brand: "",
    price: 0.00,
    quantity: 0,
    total: 0.0
};

let updatePayload = MODEL;

const CART_URL = {
    productCart: "/Cart/ProductCart",
    updateQuantity: "/Cart/UpdateQuantity",
    checkoutUrl: "/Cart/Checkout"
}

const app = (function () {

    let component = {
        table: {
            tableEl: "#cart-table",
            tableBodyEl: "#table-cart-body",
            cartCheckAllItems: ".th-checkbox-col",
            cartItemCheckBox: ".cart-item-checkbox",
        },
        modal:
        {
            modalForm: "#modalCart",
            itemName: "#input-item-name",
            brand: "#input-item-brand",
            quantity: "#input-item-quantity",
            price: "#input-item-price",
            total: "#input-item-total",
            closeBtn: "#model-close-btn",
            saveBtn: "#update-cart-item",
        },
        cartTotal: "#cartTotal",
        btnUpdateQty: ".btn-updateQty",
        btnRemoveItem: ".btn-remove-item",
        btnCheckout: ".btn-checkoutUrl"
    };

    const helpers = {

        createTableRowData: function (data) {
            let tr = document.createElement("tr");
            tr.setAttribute("cart-id", data.id ?? "");

            let tdImg = document.createElement("td");
            let tdImgContainer = document.createElement("div");
            tdImgContainer.classList.add("td-img-container");
            let imgEl = document.createElement("img");
            imgEl.setAttribute("src", `/images/${data.imageFileName}`);

            tdImgContainer.appendChild(imgEl);
            tdImg.appendChild(tdImgContainer);
            tr.appendChild(tdImg);

            let tdItemEl = document.createElement("td");
            tdItemEl.classList.add("td-item-name");
            tdItemEl.appendChild(helpers.createLabel(data.name));
            tr.appendChild(tdItemEl);

            let tdPrice = document.createElement("td");
            tdPrice.classList.add("td-price");
            let itemPrice = parseFloat(data.price).toFixed(2);
            tdPrice.appendChild(helpers.createLabel(itemPrice));

            tr.appendChild(tdPrice);

            let tdQty = document.createElement("td");
            tdQty.classList.add("td-quantity");

            tdQty.appendChild(helpers.createLabel(data.quantity));

            tr.appendChild(tdQty);

            let tdTotal = document.createElement("td");
            tdTotal.classList.add("td-total");
            let itemTotal = parseFloat(data.total).toFixed(2);
            tdTotal.appendChild(helpers.createLabel(itemTotal));
            tr.appendChild(tdTotal);

            let tdAction = document.createElement("td");
            tdAction.classList.add("td-action");

            let updateQtyBtn = helpers.createButton("Update Quantity", "button", "btn btn-outline-success mx-1 btn-updateQty")
            updateQtyBtn.setAttribute("data-bs-toggle", "modal");
            updateQtyBtn.setAttribute("data-bs-target", component.modal.modalForm);

            let removeBtn = helpers.createButton("Remove", "button", "btn btn-outline-danger mx-1 btn-remove-item");
            let checkoutBtn = helpers.createButton("Checkout", "button", "btn btn-outline-dark px-3 text-uppercase fw-bold mx-1 btn-checkoutUrl");

            tdAction.appendChild(updateQtyBtn);
            tdAction.appendChild(removeBtn);
            tdAction.appendChild(checkoutBtn);

            tr.appendChild(tdAction);

            return tr;
        },
        createButton: function (text, type, btnClass) {
            let btn = document.createElement("button");
            btn.setAttribute("type", type ?? "button");
            btn.textContent = text;
            btn.classList = btnClass;

            return btn;
        },
        createLabel: function (text) {
            let labelEl = document.createElement("label");
            labelEl.textContent = text;
            return labelEl;
        },
        clearModal: function () {
            let modal = component.modal;
            $(modal.itemName).val("");
            $(modal.brand).val("");
            $(modal.quantity).val("");
            $(modal.price).val("");
            $(modal.total).val("");
        },
        setModal: function (data) {
            let modal = component.modal;
            $(modal.itemName).val(data.name);
            $(modal.brand).val(data.brand);
            $(modal.quantity).val(parseInt(data.quantity));
            $(modal.price).val(parseFloat(data.price).toFixed(2));
            $(modal.total).val(parseFloat(data.total).toFixed(2));
        }
    };

    let componentEvents = {
        quantityOnChange: function () {
            let modal = component.modal;
            registerEvent(modal.quantity, "input", function (e) {
                let currentQuantity = parseInt($(modal.quantity).val());
                let currentPrice = parseFloat($(modal.price).val()).toFixed(2);
                let currentTotal = currentPrice * currentQuantity;

                $(modal.total).val(currentTotal);
                updatePayload = {
                    ...updatePayload,
                    price: currentPrice,
                    quantity: currentQuantity,
                    total: currentTotal
                };

                helpers.setModal(updatePayload);
            })
        },
        updateItemEvent: function () {
            let updateButtons = document.querySelectorAll(component.btnUpdateQty);
            Array.from(updateButtons).forEach(item => {

                let tr = $(item).parent().parent();
                const cartId = $(tr).attr("cart-id");
                registerEvent(item, "click", function (e) {
                    const result = productCartList.find(i => i.id == cartId);
                    helpers.clearModal();

                    helpers.setModal(result);

                    updatePayload = {
                        ...updatePayload,
                        id: cartId,
                        shoeId: result.shoeId,
                        name: result.name,
                        brand: result.brand,
                        price: result.price,
                        quantity: result.quantity,
                        total: result.total
                    }
                });
            });
        },
        removeItemEvent: function () {
            let removeButtons = document.querySelectorAll(component.btnRemoveItem);
            Array.from(removeButtons).forEach(item => {

                let tr = $(item).parent().parent();
                const cartId = $(tr).attr("cart-id");

                registerEvent(item, "click", function (e) {
                    const result = productCartList.find(i => i.id == cartId);
                    helpers.setModal(result);

                    updatePayload = {
                        ...updatePayload,
                        id: cartId,
                        shoeId: result.shoeId,
                        name: result.name,
                        brand: result.brand,
                        price: result.price,
                        quantity: result.quantity,
                        total: result.total
                    };

                    sweetAlertHelper.warningDelete(result.name)
                        .then((resul) => {
                            if (resul.isConfirmed) {
                                apiService("Cart/RemoveCart/" + cartId, "PUT")
                                    .then(async () => {
                                        await initialize();
                                        sweetAlertHelper.success("Cart item was successfully removed.");
                                    })
                                    .catch((error) => {
                                        sweetAlertHelper.error("Something went wrong.");
                                    });
                            }
                        })

                    return;
                });
            });
        },
        checkoutItemEvent: function () {
            let checkoutButtons = document.querySelectorAll(component.btnCheckout);
            Array.from(checkoutButtons).forEach(item => {

                let tr = $(item).parent().parent();
                const cartId = $(tr).attr("cart-id");

                registerEvent(item, "click", async function (e) {
                    const result = productCartList.find(i => i.id == cartId);
                    helpers.setModal(result);

                    updatePayload = {
                        ...updatePayload,
                        id: cartId,
                        shoeId: result.shoeId,
                        name: result.name,
                        brand: result.brand,
                        price: result.price,
                        quantity: result.quantity,
                        total: result.total
                    };

                    window.location.href = `${CART_URL.checkoutUrl}/${cartId}`;

                    return;
                });
            });
        },
        modalCloseEvent: function () {
            let modalProps = component.modal; 
            registerEvent(modalProps.closeBtn, "click", function (e) {
                helpers.clearModal();
            });
        },
        cartSaveUpdateEvent: function () {
            let modalProps = component.modal; 
            registerEvent(modalProps.saveBtn, "click", async function (e) {
                try {
                    let quantity = $(modalProps.quantity).val();

                    if (quantity > 0) {
                        await apiService("Cart/SaveCart", "POST", updatePayload);

                        $(modalProps.modalForm).modal("hide");
                        helpers.clearModal();
                        await initialize();
                    }
                    
                } catch (e) {
                    return;
                }

                updatePayload = MODEL;
            });
        }
    }


    async function initialize() {
        productCartList = await apiService(CART_URL.productCart, "GET");
        $(component.table.tableBodyEl).empty();
        productCartList.forEach((item) => {
            const createdRow = helpers.createTableRowData(item);
            $(component.table.tableBodyEl).append(createdRow);
        });
        componentEvents.updateItemEvent();
        componentEvents.cartSaveUpdateEvent();
        componentEvents.removeItemEvent();
        componentEvents.quantityOnChange();
        componentEvents.checkoutItemEvent();
    }



    return {
        init: initialize,
    }

}());

$(document).ready(function () {
    app.init();
})