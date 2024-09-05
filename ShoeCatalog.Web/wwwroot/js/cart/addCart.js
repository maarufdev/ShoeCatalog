$(document).ready(function () {

    let AddCartPayload = {
        shoeId: "",
        name: "",
        brand: "",
        price: 0.00,
        quantity: 0,
        total: 0.00
    }

    let shoeSummaryResult = {};

    const URL = {
        shoeSummaryDetail: function (id) {
            return `/Cart/GetShoeDetails/${id}`
        },
        saveCart: "/Cart/SaveCart",
    }

    let app = (function () {

        const component = {
            addCartButton: "#add-cart-btn",
            addCartForm: {
                modal: "#addCartModal",
                saveCartBtn: "#save-cart",
                itemName: "#cart-item-name",
                itemBrand: "#cart-item-brand",
                itemPrice: "#cart-item-price",
                itemQuantity: "#cart-item-quantity",
                itemTotal: "#cart-item-total-price"
            }
        };

        const helpers = {
            clearModal: function () {
                $(component.addCartForm.itemName).val("");
                $(component.addCartForm.itemBrand).val("");
                $(component.addCartForm.itemPrice).val("");
                $(component.addCartForm.itemQuantity).val("");
                $(component.addCartForm.itemTotal).val("");
            },
            getShoeId: function () {
                let url = window.location.href;
                let splitUrl = url.split("/");
                const shoeId = splitUrl[splitUrl.length - 1];

                return shoeId;
            },
            setModalValues: function (data) {
                $(component.addCartForm.itemName).val(data.name ?? "");
                $(component.addCartForm.itemBrand).val(data.brand ?? "");
                $(component.addCartForm.itemPrice).val(parseFloat(data.price).toFixed(2) ?? 0.00);
                $(component.addCartForm.itemQuantity).val(data.quantity ?? 0);
                $(component.addCartForm.itemTotal).val(parseFloat(data.total ?? 0.00).toFixed(2));
            },
            setAddCartPayload: function () {
                AddCartPayload = {
                    shoeId: helpers.getShoeId(),
                    name: $(component.addCartForm.itemName).val(),
                    brand: $(component.addCartForm.itemBrand).val(),
                    price: $(component.addCartForm.itemPrice).val(),
                    quantity: $(component.addCartForm.itemQuantity).val(),
                    total: $(component.addCartForm.itemTotal).val()
                }

                return AddCartPayload;
            }
        }

        const services = {
            getShoeSummary: async function () {
                let shoeId = helpers.getShoeId();
                return await apiService(URL.shoeSummaryDetail(shoeId), "GET");
            },
            saveCart: async function (payload) {
                return await apiService(URL.saveCart, "POST", payload)
            }
        }

        const componentEvents = {
            addCartEvent: function () {
                registerEvent(component.addCartButton, "click", async function (e) {
                    let result = await services.getShoeSummary();
                    helpers.clearModal();
                    if (result) {
                        shoeSummaryResult = result;
                        helpers.setModalValues(result);
                        helpers.setAddCartPayload();
                        $(component.addCartForm.modal).modal("show");
                    }
                });
            },
            quantityOnChangeEvent: function () {
                registerEvent(component.addCartForm.itemQuantity, "input", function (e) {
                    let cartEl = component.addCartForm;
                    let currentQuantity = $(cartEl.itemQuantity).val() ?? 0;
                    let currentPrice = parseFloat($(cartEl.itemPrice).val());
                    let newTotal = (currentPrice).toFixed(2) * parseInt(currentQuantity);
                    $(cartEl.itemTotal).val(parseFloat(newTotal).toFixed(2));
                    helpers.setAddCartPayload();
                });
            },
            saveCartEvent: async function () {
                registerEvent(component.addCartForm.saveCartBtn, "click", async function (e) {
                    try {
                        let result = await services.saveCart(AddCartPayload);
                        $(component.addCartForm.modal).modal("hide");
                    } catch (e) {

                    }
                    
                });
            }
        }

        async function initialize() {
            componentEvents.addCartEvent();
            componentEvents.quantityOnChangeEvent();
            componentEvents.saveCartEvent();
        }

        return {
            initialize: initialize
        }

    }());
    app.initialize();
})