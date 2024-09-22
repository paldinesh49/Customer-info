$(document).ready(function () {
    // Load customers with DataTables integration
    function loadCustomers() {
        $.ajax({
            url: '/Home/GetCustomers',
            type: 'GET',
            success: function (customers) {
                var rows = "";
                customers.forEach(function (customer) {
                    var gender = customer.genderId == 1 ? 'Male' : 'Female';
                    rows += `<tr>
                                <td>${customer.id}</td>
                                <td>${customer.name}</td>
                                <td>${gender}</td>
                                <td>${customer.stateName}</td>
                                <td>${customer.districtName}</td>
                                <td>
                                    <button class="btn btn-primary btn-sm edit-btn" data-id="${customer.id}">Edit</button>
                                    <button class="btn btn-info btn-sm view-btn" data-id="${customer.id}">View</button>
                                    <button class="btn btn-danger btn-sm delete-btn" data-id="${customer.id}">Delete</button>
                                </td>
                             </tr>`;
                });

                $("#customerList").html(rows);

                // Initialize or destroy and reinitialize DataTables
                if ($.fn.DataTable.isDataTable("#customerTable")) {
                    $('#customerTable').DataTable().clear().destroy();
                }

                $('#customerTable').DataTable({
                    responsive: true,
                    order: [[0, 'asc']],
                    columnDefs: [
                        { orderable: false, targets: 5 }
                    ]
                });

                bindCustomerActionButtons();
            },
            error: function () {
                alert("Failed to load customers.");
            }
        });
    }

    function bindCustomerActionButtons() {
        $(".edit-btn").click(function () {
            var customerId = $(this).data('id');
            editCustomer(customerId);
        });

        $(".view-btn").click(function () {
            var customerId = $(this).data('id');
            viewCustomer(customerId);
        });

        $(".delete-btn").click(function () {
            var customerId = $(this).data('id');
            if (confirm("Are you sure you want to delete this customer?")) {
                deleteCustomer(customerId);
            }
        });
    }

    function editCustomer(customerId) {
        $.ajax({
            url: `/Home/EditCustomer/${customerId}`,
            type: 'GET',
            success: function (customer) {
                $("#editCustomerId").val(customer.id);
                $("#editName").val(customer.name);
                $("#editGenderId").val(customer.genderId);
                loadStates("#editStateId", customer.stateId);
                loadDistricts("#editDistrictId", customer.stateId, customer.districtId);
                $('#editCustomerModal').modal('show');
            },
            error: function () {
                alert("Failed to load customer data.");
            }
        });
    }

    function viewCustomer(customerId) {
        $.ajax({
            url: `/Home/ViewCustomer?id=${customerId}`,
            type: 'GET',
            success: function (customer) {
                $("#viewName").text(customer.name);
                $("#viewGender").text(customer.genderId == 1 ? 'Male' : 'Female');
                $("#viewState").text(customer.stateName);
                $("#viewDistrict").text(customer.districtName);
                $('#viewCustomerModal').modal('show');
            },
            error: function () {
                alert("Failed to load customer details.");
            }
        });
    }

    function deleteCustomer(customerId) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: `/Home/DeleteCustomer/${customerId}`,
            type: 'POST',
            headers: {
                'RequestVerificationToken': token
            },
            success: function () {
                alert("Customer deleted successfully.");
                // Destroy the current DataTable
                if ($.fn.DataTable.isDataTable("#customerTable")) {
                    $('#customerTable').DataTable().clear().destroy();
                }

                // Reload customer list and reinitialize DataTable
                loadCustomers();
            },
            error: function () {
                alert("Failed to delete customer.");
            }
        });
    }

    function loadStates(selector, selectedStateId = null) {
        $.ajax({
            url: '/Home/GetStates',
            type: 'GET',
            success: function (states) {
                var options = "";
                states.forEach(function (state) {
                    options += `<option value="${state.id}" ${selectedStateId == state.id ? 'selected' : ''}>${state.name}</option>`;
                });
                $(selector).html(options);
            },
            error: function () {
                alert("Failed to load states.");
            }
        });
    }

    function loadDistricts(selector, stateId, selectedDistrictId = null) {
        if (!stateId) return;
        $.ajax({
            url: '/Home/GetDistricts',
            type: 'GET',
            data: { stateId: stateId },
            success: function (districts) {
                var options = "";
                districts.forEach(function (district) {
                    options += `<option value="${district.id}" ${selectedDistrictId == district.id ? 'selected' : ''}>${district.name}</option>`;
                });
                $(selector).html(options);
            },
            error: function () {
                alert("Failed to load districts.");
            }
        });
    }

    $('#stateId, #editStateId').change(function () {
        var stateId = $(this).val();
        var targetDistrict = $(this).attr('id') === 'stateId' ? '#districtId' : '#editDistrictId';
        loadDistricts(targetDistrict, stateId);
    });
    // Load states into dropdown when the modal opens
    $('#createCustomerModal').on('show.bs.modal', function () {
        loadStates("#stateId"); // Load states
    });

    // Load districts when the state changes
    $('#stateId').change(function () {
        var stateId = $(this).val();
        loadDistricts("#districtId", stateId); // Load districts based on the selected state
    });
    $("#createCustomerForm").submit(function (e) {
        e.preventDefault();

        var selectedStateId = $("#stateId").val();
        var selectedDistrictId = $("#districtId").val();

        if (!selectedStateId) {
            alert("Please select a state.");
            return;
        }

        loadDistricts("#districtId", selectedStateId);

        var formData = {
            Name: $("#name").val(),
            GenderId: parseInt($("#genderId").val(), 10),
            StateId: parseInt(selectedStateId, 10),
            DistrictId: parseInt(selectedDistrictId, 10)
        };

        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '/Home/CreateCustomer',
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            headers: { 'RequestVerificationToken': token },
            data: JSON.stringify(formData),
            success: function () {
                $('#createCustomerModal').modal('hide');
                $("#createCustomerForm")[0].reset();
                // Destroy the current DataTable
                if ($.fn.DataTable.isDataTable("#customerTable")) {
                    $('#customerTable').DataTable().clear().destroy();
                }

                // Reload customer list and reinitialize DataTable
                loadCustomers();
            },
            error: function (xhr) {
                alert("Failed to create customer: " + xhr.responseText);
            }
        });
    });

    $("#editCustomerForm").submit(function (e) {
        e.preventDefault();
        var formData = {
            Id: $("#editCustomerId").val(),
            Name: $("#editName").val(),
            GenderId: parseInt($("#editGenderId").val(), 10),
            StateId: parseInt($("#editStateId").val(), 10),
            DistrictId: parseInt($("#editDistrictId").val(), 10)
        };
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '/Home/EditCustomer',
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            headers: {
                'RequestVerificationToken': token
            },
            data: JSON.stringify(formData),
            success: function () {
                $('#editCustomerModal').modal('hide');
                // Destroy the current DataTable
                if ($.fn.DataTable.isDataTable("#customerTable")) {
                    $('#customerTable').DataTable().clear().destroy();
                }

                // Reload customer list and reinitialize DataTable
                loadCustomers();
            },
            error: function (xhr) {
                alert("Failed to update customer: " + xhr.responseText);
            }
        });
    });

    loadCustomers(); // Load initial customer list
});
