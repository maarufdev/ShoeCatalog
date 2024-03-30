/// <reference path="notificationalert.js" />
/// <reference path="notificationalert.js" />
/// <reference path="sweetAlertHelper.js" />

import alertNotification from "./notificationAlert.js";
import sweetAlertHelper from "./sweetAlertHelper.js";

const baseModel = (function () {

    const apiProps = {
        getAll: {
            url: '/Brand/GetAll',
            method: 'GET'
        },
        get: {
            url: '/Brand/GetById',
            method: 'GET'
        },
        post: {
            url: '/Brand/Create',
            method:'POST'
        },
        put: {
            url: '/Brand/Update',
            method: 'PUT'
        },
        delete: {
            url: '/Brand/Delete',
            method: 'DELETE'
        }
    }

    let brandModel = {
        id:'',
        name: '',
    }

    const inputs = {
        name: $("#brand-name")
    }

    const actionType = {
        CREATE: "CREATE",
        UPDATE: "UPDATE",
        DELETE: "DELETE",
        GET: "GET"
    }
    let currentAction = actionType.CREATE;

    const editButtonAttributes = {
        "type":"button", 
        "data-bs-toggle":"modal",
        "data-bs-target":"#modalBrand"
    }

    const upsertButton = $('#upsert-button');

    const modalCloseButton = $('#modal-close-button');

    const tableBody = $("#table-brand-body");
    const modalBrand = $("#modalBrand");
    const modalBrandLabel = $("#modalBrandLabel");
    const createBrandButton = $("#create-brand-button");


    const initialize = async function () {
        const response = await getAll();
        createTableRow(response);

        runEvents()
    }

    const runEvents = async function () {

        upsertButton.on('click', async function () {
            await performUpsert();
        });
        createBrandButton.on('click', function () {
            modalBrand.modal("show");
            modalBrandLabel.text('Create Brand');
            currentAction = actionType.CREATE;
        });

        // When the close button on modal is being clicked.
        modalCloseButton.on('click', function () {
            cleanProperties();
        })


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

    const upsert = async function (data) {
        if (data.id === null || data.id === '') {
            return await apiService(apiProps.post.url, apiProps.post.method, data)
        } 

        return await apiService(apiProps.put.url, apiProps.put.method, data);
    }

    const getAll = async function () {
        return await apiService(apiProps.getAll.url, apiProps.get.method);
    }

    const get = async function (id) {
        return await apiService(apiProps.get, 'GET');
    }

    const deleteBrand = async function (id) {
        return await apiService(`${apiProps.delete.url}/${id}`, apiProps.delete.method)
    }

    const brandName = $('#brand-name');


    const actionButton = function (id, name) {
        const actionContainer = $('<div>');
        actionContainer.addClass('d-flex gap-2');

        const updateButton = $('<button>')
            .addClass('btn btn-outline-success px-3 d-flex gap-2')
            .attr(editButtonAttributes)
            .append($("<i>").addClass("bi bi-pencil-square"))
            .append($("<span>").text("EDIT").addClass("text-uppercase fw-bold"));

        updateButton.on('click', function () {

            currentAction = actionType.UPDATE;

            brandModel.id = id;
            brandModel.name = name;
            inputs.name.val(name)
            modalBrandLabel.text("Update Brand");
        });


        const deleteButton = $('<button>')
            .addClass('btn btn-outline-danger px-2 d-flex gap-3')
            .attr('type', 'button')
            .append($("<i>").addClass("bi bi-archive"))
            .append($("<span>").text("REMOVE").addClass("text-uppercase fw-bold"));


        deleteButton.on('click', function () {
            currentAction = actionType.DELETE;

            sweetAlertHelper.warningDelete(name)
                .then((resul) => {
                    if (resul.isConfirmed) {
                        deleteBrand(id)
                            .then(async () => {
                                await loadTable();
                                sweetAlertHelper.success("Successfully deleted!");
                            })
                            .catch((error) => {
                                sweetAlertHelper.error("Delete failed.");
                            });
                    }

                    return;
                })

            return;
        });

        actionContainer.append(updateButton);
        actionContainer.append(deleteButton);

        return actionContainer;
    }

    const handleOnChange = function () {
        brandName.on('change', function (event) {
            console.log(event);
        })
    }
    const createTableRow = function (data) {
        if (data) {
            for (let item of data) {
                const newRow = $('<tr>')

                newRow.append($('<td>').text(item.name))
                newRow.append($('<td>').text(3))
                newRow.append($('<td>').append(actionButton(item.id, item.name)))

                tableBody.append(newRow)
            }
        }
    }

    const cleanProperties = function () {
        brandModel = { ...brandModel, id: '', name: '' }
        inputs.name.val('');
        
    }

    const loadTable = async function () {
        tableBody.empty();

        const fetchAll = await getAll();
        createTableRow(fetchAll);
    }

    const performUpsert = async function () {
        const data = { ...brandModel, name: inputs.name.val() }

        if (data.name == null || data.name == '') {
            sweetAlertHelper.error("No data to be saved.");
            return
        }

        if (currentAction == actionType.CREATE) {
            upsert(data)
                .then(async () => {
                    modalBrand.modal("hide");
                    sweetAlertHelper.success("Brand was successfully created.");
                    cleanProperties()

                    await loadTable();

                })
                .catch(error => {
                    alert('There is an error')
                })

        } else if (currentAction == actionType.UPDATE) {
            upsert({ ...brandModel, name: inputs.name.val() })
                .then(async () => {
                    modalBrand.modal("hide");
                    sweetAlertHelper.success("Brand was successfully updated.");
                    cleanProperties();
                    await loadTable();
                })
                .catch(error => {
                    alert('There is an error')
                })
        }

        return;
    }

    return {
        initialize: initialize
    }

})();

$(document).ready(async function () {
    await baseModel.initialize()
})