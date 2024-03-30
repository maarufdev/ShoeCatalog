import sweetAlertHelper from "./sweetAlertHelper.js";


const apiProps = {
    getAll: {
        url: '/Category/GetAll',
        method: 'GET'
    },
    get: {
        url: '/Category/GetById',
        method: 'GET'
    },
    post: {
        url: '/Category/Create',
        method: 'POST'
    },
    put: {
        url: '/Category/Update',
        method: 'PUT'
    },
    delete: {
        url: '/Category/Delete',
        method: 'DELETE'
    }
}

const apiService = function (url, method, data) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url,
            method: method,
            data: data,
            success: function (response) {
                resolve(response)
            },
            error: function (xhr, status, error) {
                reject(error)
            }
        })
    })
}

let categoryModel = {
    id: '',
    name: ''
};

const texts = {
    modalCategoryLabel: $("#modalCategoryLabel"),
}


const modalProperties = {
    modalForm: $("#modalCategory"),
    button: $("#create-category-button"),
    headerTitle: $("#modalCategoryLabel"),
    input: $("#category-name"),
    buttonText: "Save",
    btnClose: $("#modalCloseButton")
}

const actions = {
    create: "CREATE",
    update: "UPDATE"
}

const baseModel = (function () {

    const upsertBtn = $("#upsert-button");
    const createCategoryBtn = $("#create-category-button");
    const tableBody = $("#table-category-body");

    let currentAction = actions.create;

    const runEventListerners = function () {
        createCategoryBtn.on("click", function () {
            modalProperties.modalForm.modal("show")
            texts.modalCategoryLabel.text("Create Category");
            currentAction = actions.create;
        })

        upsertBtn.on("click", async function () {

            const reponse = await upsert();
            
        });

        modalProperties.btnClose.on("click", function () {
            cleanProps();
        });


    }

    const getAll = async function () {
        return apiService(apiProps.getAll.url, apiProps.getAll.method)
    }

    const loadData = async function () {
        const response = await getAll();

        createTableRow(response);
    }

    const upsert = async function () {
        const data = { ...categoryModel, name: modalProperties.input.val() }

        if (data.name == null || data.name == '') {
            sweetAlertHelper.error("No data to be saved.");
            return
        }

        if (currentAction === actions.create) {
            // perform create operation here

            apiService(
                apiProps.post.url,
                apiProps.post.method,
                data
                )
                .then(async () => {
                    modalProperties.modalForm.modal("hide");
                    sweetAlertHelper.success("Category was successfully created.");

                    cleanProps();

                    await loadData();
                })
                .catch(error => {
                    sweetAlertHelper.error("Unable to create.");
                })

            return;
        }

        if (currentAction === actions.update) {
            if (!data.id) {
                sweetAlertHelper.error("No ID Found")

                return;
            }

            apiService(apiProps.put.url, apiProps.put.method, data)
                .then(async (response) => {
                    modalProperties.modalForm.modal("hide");
                    sweetAlertHelper.success("Category was successfully updated.");

                    cleanProps()

                    await loadData();
                })
                .catch(error => {
                    sweetAlertHelper.error("Unable to Update.")
                })

            return;
        }
    }

    const deletCategory = async function (id) {
        return await apiService(`${apiProps.delete.url}/${id}`, apiProps.delete.method)
    }

    const createActionButton = function (data) {
        const { id, name } = data;
        
        const divContainer = $("<div>");
        divContainer.addClass("d-flex gap-2");

        // update button
        const btnEdit = $("<button>")
            .addClass('btn btn-outline-success px-3 d-flex gap-2')
            .attr("type", "button")
            .append($("<i>").addClass("bi bi-pencil-square"))
            .append($("<span>").text("EDIT").addClass("text-uppercase fw-bold"));

        btnEdit.on("click", function () {
            modalProperties.modalForm.modal("show")
            modalProperties.headerTitle.text("Update Category")
            currentAction = actions.update;

            modalProperties.input.val(name)

            categoryModel = { ...categoryModel, id: id, name: name }
        })

        const btnDelete = $("<button>")
            .addClass('btn btn-outline-danger px-2 d-flex gap-3')
            .attr("type", "button")
            .append($("<i>").addClass("bi bi-archive"))
            .append($("<span>").text("REMOVE").addClass("text-uppercase fw-bold"));

        btnDelete.on("click", function () {
            categoryModel = { ...categoryModel, id: id, name: name }

            sweetAlertHelper.warningDelete(categoryModel.name)
                .then((result) => {
                    if (result.isConfirmed) {
                        deletCategory(id)
                            .then(async () => {
                                sweetAlertHelper.success("Category was successfully deleted.");
                                await loadData();
                            })
                    }
                })
                .catch(error => {
                    sweetAlertHelper.error("Unable to delete.");
                })
        })

        divContainer.append(btnEdit);
        divContainer.append(btnDelete);

        return divContainer;

    }
    
    const createTableRow = function (data) {
        tableBody.empty();
        if (data) {
            data.map((item) => {
                const newRow = $("<tr>");

                newRow.append($("<td>").text(item.name));
                newRow.append($("<td>").text(23));
                newRow.append($("<td>").append(createActionButton(item)));

                tableBody.append(newRow);

            })
        }
    }
    
    const cleanProps = function () {
        modalProperties.input.val("");
        currentAction = actions.create;
        categoryModel = { id: "", name: "" };
    }

    const initialize = async function () {
        const response = await getAll()
        createTableRow(response);
        runEventListerners();
    }


    return {
        initialize: initialize
    }


})();


$(document).ready(function () {
    baseModel.initialize();
})



