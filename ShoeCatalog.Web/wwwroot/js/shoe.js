import sweetAlertHelper from "./sweetAlertHelper.js";

const baseModel = (function () {

    const apiProps = {
        getAll: {
            url: "/Shoe/GetAll",
            method: "GET"
        },

        delete: {
            url: "/shoe/delete",
            method: "DELETE"
        }
    }

    const apiService = function (url, method, data) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: url,
                method: method,
                data: data,
                success: function (response) {
                    resolve(response);
                },
                error: function (xhr, status, error) {
                    reject(error);
                }
            })
        });
    }

    const getAll = async function() {
        return await apiService(apiProps.getAll.url, apiProps.getAll.method);
    }

    const deleteShoe = async function (id) {
        return await apiService(`${apiProps.delete.url}/${id}`, apiProps.delete.method);
    }

    const shoeIndexElements = {
        tableBody: $("#shoe-table-body"),

    }

    const createButton = function (id) {

        const actionButtonContainer = $("<div>");
        actionButtonContainer.addClass("d-flex gap-1")

        // create view details button
        const viewDetailsButton = $("<a>")
            .addClass("btn btn-outline-dark px-2 d-flex gap-1")
            .attr({ href: `/Shoe/Details/${id}` })
            .append($("<i>").addClass("bi bi-info-circle"))
            .append($("<span>").text("Details").addClass("text-uppercase fw-bold"))
        
        // create update button
        const updateButton = $("<a>")
            .addClass('btn btn-outline-success px-3 d-flex gap-1')
            .attr({ href: `/Shoe/Upsert/${id}` })
            .append($("<i>").addClass("bi bi-pencil-square"))
            .append($("<span>").text("EDIT").addClass("text-uppercase fw-bold"));

        // create delete button
        const deleteButton = $("<button>")
            .addClass('btn btn-outline-danger px-2 d-flex gap-1')
            .append($("<i>").addClass("bi bi-archive"))
            .append($("<span>").text("REMOVE").addClass("text-uppercase fw-bold"))
            .on("click", function () {
                sweetAlertHelper.warningDelete(id)
                    .then(function(result) {
                        if (result.isConfirmed) {
                            deleteShoe(id)
                                .then(async function () {
                                    await loadTable();
                                    sweetAlertHelper.success("Success Deleted");
                                })
                                .catch(function(error){
                                    sweetAlertHelper.error("Deleting a resource was failed");
                                });
                        }
                    });
            });

        actionButtonContainer.append(viewDetailsButton);
        actionButtonContainer.append(updateButton);
        actionButtonContainer.append(deleteButton);

        return actionButtonContainer;
    }

    const loadTable = async function() {
        shoeIndexElements.tableBody.empty();

        const response = await getAll();

        await createTableRow(response);
    }

    const createTableRow = function(data) {
        if (data) {
            for (let item of data) {

                const newRow = $("<tr>");

                newRow.append($("<td>").text(item.name));
                newRow.append($("<td>").text(item.price));
                newRow.append($("<td>").text(item.brand));
                newRow.append($("<td>").text(item.category));
                newRow.append($("<td>").append(createButton(item.id)));

                shoeIndexElements.tableBody.append(newRow);
            }
        }
    }

    return {
        initialize: async function() {
            await loadTable()
        }
    }
})();


$(document).ready(async function () {
    await baseModel.initialize();
});