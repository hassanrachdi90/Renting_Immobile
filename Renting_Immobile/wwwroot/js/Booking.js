﻿var dataTable;
$(document).ready(function () {
    const utlParams = new URLSearchParams(window.location.search);
    const status = utlParams.get('status');

    loadDataTable(status);
});

function loadDataTable(status) {
    dataTable = $('#myTable').DataTable({
        "ajax": {
            url: '/Booking/getall?status=' + status
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'phone', "width": "10%" },
            { data: 'email', "width": "15%" },
            { data: 'status', "width": "10%" },
            { data: 'checkInDate', "width": "10%" },
            { data: 'nights', "width": "10%" },
            { data: 'totalCost', render: $.fn.dataTable.render.number(',','.',2), "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return ` <div class="w-75 btn-group" >
                    <a href="/booking/bookingDetails?bookingId=${data}" class="btn btn-outline-warning mx-2"> <i class="bi bi-pencil-square"></i>Details</a>

                    </div>` 
                }
            }
        ]
    });
}

